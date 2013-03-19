﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Dev2.Data.Storage;

namespace Dev2.Data.Binary_Objects
{
    public class Dev2PersistantDictionary<T> where T : class
    {
        #region Fields

        private const string _ext = ".r2d2";
        private readonly string _completeFilename = Path.GetTempFileName();
        private FileStream _file;
        private BinaryDataListIndexStorage _lstIndexes;
        private readonly object _opsLock = new object();
        private const long _compactThresholdSize = 500 * 1024 * 1024;
        private long _lastCompactSize;
        private bool _hasBeenRemoveSinceLastCompact = false;

        private static readonly string _rootPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        private const string _savePath = @"Dev2\DataListServer\";
        private static readonly string _dataListPersistPath = Path.Combine(_rootPath, _savePath); 

        #endregion Fields

        #region Constructors

        public Dev2PersistantDictionary(string filename)
        {
            if (!string.IsNullOrEmpty(filename))
            {
                var fileName = Path.GetFileName(filename);
                if (fileName != null)
                {
                    string fName = fileName.Replace(".tmp", _ext);
                    _completeFilename = _dataListPersistPath + fName;

                    _lstIndexes = new BinaryDataListIndexStorage(fName);
                }
            }

            _file = new FileStream(_completeFilename, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        }

        public Dev2PersistantDictionary(string dataPath, string indexPath)
        {

            if (string.IsNullOrEmpty(dataPath) || string.IsNullOrEmpty(indexPath))
            {
                throw new Exception("Null Data and/or Index Path");
            }

            _lstIndexes = new BinaryDataListIndexStorage(indexPath);

            _completeFilename = dataPath;

            _file = new FileStream(dataPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        }

        #endregion Constructors

        #region Indexers

        public T this[string key]
        {
            get
            {
                return Read(key);
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value", "Cannot add null to dictionary");
                }
                Add(key, value);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public int Count
        {
            get
            {
                return _lstIndexes.Count;
            }
        }

        /// <summary>
        /// Gets the keys.
        /// </summary>
        /// <value>
        /// The keys.
        /// </value>
        public ICollection<string> Keys
        {
            get
            {
                return _lstIndexes.Keys;
            }
        }

        /// <summary>
        /// Gets the index file path.
        /// </summary>
        /// <value>
        /// The index file path.
        /// </value>
        public string IndexFilePath { get { return _lstIndexes.IndexFilePath; } }

        /// <summary>
        /// Gets the data file path.
        /// </summary>
        /// <value>
        /// The data file path.
        /// </value>
        public string DataFilePath { get { return _completeFilename; } }

        #endregion

        #region Private Methods

        private T Read(string key)
        {
            lock (_opsLock)
            {
                byte[] rawData = ReadBytes(key);

                if (rawData == null || rawData.Length == 0)
                {
                    return null;
                }

                var fromBytes = ConvertFromBytes(rawData);

                return fromBytes;
            }
        }

        private byte[] ReadBytes(string key)
        {
            lock (_opsLock)
            {
                if (!_lstIndexes.ContainsKey(key))
                {
                    return null;
                }

                long pos;
                int len;
                GetPositionLength(key, out pos, out len);

                _file.Seek(pos, SeekOrigin.Begin);
                var bytesRead = new byte[len];
                _file.Read(bytesRead, 0, len);

                return bytesRead;
            }
        }

        public void Compact()
        {
            if (!_hasBeenRemoveSinceLastCompact)
            {
                return;
            }

            // Get tmp file path
            string directory = Path.GetDirectoryName(_completeFilename);
            if (directory == null)
            {
                throw new Exception(string.Format("Unable to create compact path. '{0}' doesn't contain a valid directory name.", _completeFilename));
            }

            string tempFile = string.Format("{0}.tmp", Guid.NewGuid());
            string tempPath = Path.Combine(directory, tempFile);

            // Open temp file to write entries to
            try
            {
                using (FileStream tmpFileStream = new FileStream(tempPath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    // Write entries sequentially into the tmp file, this will exclude any removed entries.
                    foreach (var key in Keys.ToList())
                    {
                        byte[] data = ReadBytes(key);
                        if (data != null && data.Length > 0)
                        {
                            _lstIndexes.AddIndex(key, tmpFileStream.Position, data.Length);
                            tmpFileStream.Write(data, 0, data.Length);
                        }
                        else
                        {
                            _lstIndexes.RemoveIndex(key);
                        }
                    }

                    tmpFileStream.Close();
                }
            }
            catch (Exception e)
            {
                throw new Exception("Compacting data to the temp file failed.", e);
            }

            // Swap files
            string backupPath = string.Format("{0}{1}", _completeFilename, ".bak");

            try
            {
                _file.Close();
                _file.Dispose();
            }
            catch (Exception e)
            {
                throw new Exception("Unable to close current file for compacting.", e);
            }

            try
            {
                File.Move(_completeFilename, backupPath);
            }
            catch (Exception e)
            {
                throw new Exception("Unable to backup current file for compacting.", e);
            }

            try
            {
                File.Move(tempPath, _completeFilename);
            }
            catch (Exception e)
            {
                File.Move(backupPath, _completeFilename);
                File.Delete(tempPath);
                throw new Exception("Unable to swap compacted file with the old file.", e);
            }

            // Remove old file
            File.Delete(backupPath);

            // Open file again
            _file = new FileStream(_completeFilename, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            _lastCompactSize = _file.Length;
            _hasBeenRemoveSinceLastCompact = false;
        }

        private T ConvertFromBytes(byte[] payload)
        {
            BinaryFormatter bf = new BinaryFormatter();
            T convertFromBytes = null;
            lock (_opsLock)
            {
                using (MemoryStream ms = new MemoryStream(payload))
                {

                    ms.Position = 0;
                    try
                    {
                        convertFromBytes = (T)bf.Deserialize(ms);
                    }
                    catch (SerializationException e)
                    {
                        Console.WriteLine(e);
                    }
                }

            }
            return convertFromBytes;
        }

        private MemoryStream ConvertToStream(T payload)
        {
            // TODO : Fix this, it keeps bombing out ?!
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();

            try
            {
                bf.Serialize(ms, payload);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            ms.Position = 0;
            return ms;
        }

        private void GetPositionLength(string key, out long position, out int length)
        {
            //string tmp;

            if (!_lstIndexes.GetPositionLength(key, out position, out length))
            {
                throw new Exception(string.Format("Key '{0}' doesn't exist in index.", key));
            }
        }

        #endregion

        #region Public Methods

        public void Add(string key, T objToAdd)
        {
            lock (_opsLock)
            {
                if (_file.Length - _lastCompactSize > _compactThresholdSize)
                {
                    Compact();
                }

                //var convertToJson = ConvertToJson(objToAdd);
                byte[] data = null;
                using (MemoryStream ms = ConvertToStream(objToAdd))
                {
                    data = new byte[ms.Length];
                    ms.Read(data, 0, (int)ms.Length);
                    ms.Close();
                }

                _lstIndexes.AddIndex(key, _file.Position, data.Length);

                // ensure we write to the end of the log ;)
                _file.Seek(0, SeekOrigin.End);
                _file.Write(data, 0, data.Length);
            }
        }

        public void Remove(string key)
        {
            lock (_opsLock)
            {
                _lstIndexes.RemoveIndex(key);
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Dev2PersistantDictionary()
        {
            Dispose(false);
        }

        void Dispose(bool disposing)
        {
            if (!disposing) return;
            _file.Close();
            _file.Dispose();
            // clean up ;)
            File.Delete(_completeFilename);
            //_lstIndexes.Dispose();
        }


        #endregion
    }
}