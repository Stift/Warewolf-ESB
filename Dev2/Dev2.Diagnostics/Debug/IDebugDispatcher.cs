﻿using System;
using System.Collections.Generic;

namespace Dev2.Diagnostics
{
    /// <summary>
    /// Defines the requirements for a dispatcher of <see cref="IDebugState"/> messages.
    /// </summary>
    public interface IDebugDispatcher
    {
        /// <summary>
        /// Gets the number of writers.
        /// </summary>
        int Count
        {
            get;
        }

        /// <summary>
        /// Adds the specified writer to the dispatcher.
        /// </summary>
        /// <param name="workspaceID">The ID of the workspace to which the writer belongs.</param>
        /// <param name="writer">The writer to be added.</param>
        void Add(Guid workspaceID, IDebugWriter writer);

        /// <summary>
        /// Removes the specified workspace from the dispatcher.
        /// </summary>
        /// <param name="workspaceID">The ID of workspace to be removed.</param>
        void Remove(Guid workspaceID);

        /// <summary>
        /// Gets the writer for the given workspace ID.
        /// </summary>
        /// <param name="workspaceID">The workspace ID to be queried.</param>
        /// <returns>The <see cref="IDebugWriter"/> with the specified ID, or <code>null</code> if not found.</returns>
        IDebugWriter Get(Guid workspaceID);

        /// <summary>
        /// Writes the given state to any registered <see cref="IDebugWriter" />'s.
        /// <remarks>
        /// This must implement the one-way (fire and forget) message exchange pattern.
        /// </remarks>
        /// </summary>
        /// <param name="debugState">The state to be written.</param>
        /// <param name="isRemoteInvoke"><code>true</code> if this is a remote invoke; <code>false</code> otherwise.</param>
        /// <param name="remoteInvokerID">The remote invoker ID.</param>
        /// <param name="parentInstanceID">The parent instance ID.</param>
        /// <param name="remoteDebugItems">The remote debug items.</param>       
        // BUG 9706 - 2013.06.22 - TWR : added remote invoke parameters
        void Write(IDebugState debugState, bool isRemoteInvoke = false, string remoteInvokerID = null, string parentInstanceID = null, IList<DebugState> remoteDebugItems = null);
    }
}
