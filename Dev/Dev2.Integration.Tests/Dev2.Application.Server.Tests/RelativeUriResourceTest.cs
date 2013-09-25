﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dev2.Integration.Tests.Helpers;

namespace Dev2.Integration.Tests.Dev2.Application.Server.Tests
{
    [TestClass][Ignore]//Ashley: round 2 hunting the evil test
    public class RelativeUriResourceTest
    {
        private const string _workflowName = "RelativeUriResourceTest";

        public RelativeUriResourceTest()
        {
        }

        private TestContext _context;

        public TestContext TestContext { get { return _context; } set { _context = value; } }

        [TestMethod]
        public void RelativeUriResource_TestMethod()
        {
            string postData = String.Format("{0}{1}", ServerSettings.WebserverURI, _workflowName);
            string actualResult = TestHelper.PostDataToWebserver(postData);
            int result = actualResult.IndexOf("http://localhost", StringComparison.OrdinalIgnoreCase);
            Assert.AreEqual(result, -1);
        }
    }
}
