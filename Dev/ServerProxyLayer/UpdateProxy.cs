﻿using System;
using System.Text;
using Dev2.Common;
using Dev2.Common.Interfaces.Data;
using Dev2.Common.Interfaces.ErrorHandling;
using Dev2.Common.Interfaces.Infrastructure.Communication;
using Dev2.Common.Interfaces.ServerProxyLayer;
using Dev2.Common.Interfaces.Studio.Core;
using Dev2.Common.Interfaces.Studio.Core.Controller;
using Dev2.Communication;

namespace Warewolf.Studio.ServerProxyLayer
{
    public class UpdateProxy : ProxyBase, IUpdateManager
    {
        #region Implementation of IUpdateManager


        public UpdateProxy(ICommunicationControllerFactory communicationControllerFactory, IEnvironmentConnection connection)
            : base(communicationControllerFactory, connection)
        {

        }

        /// <summary>
        /// Deploy a resource. order of execution is gauranteed
        /// </summary>
        /// <param name="resource"></param>
        public void DeployItem(IResource resource)
        {
            if (resource == null)
            {
                throw new ArgumentNullException("resource");
            }
            var comsController = CommunicationControllerFactory.CreateController("DeployResourceService");
            //todo: this is different and need to use new method
            // comsController.AddPayloadArgument("ResourceDefinition", resource.ToServiceDefinition());
            comsController.AddPayloadArgument("Roles", "*");

            var con = Connection;
            comsController.ExecuteCommand<IExecuteMessage>(con, GlobalConstants.ServerWorkspaceID);
        }

        /// <summary>
        /// Save a resource to the server
        /// </summary>
        /// <param name="resource">resource to save</param>
        /// <param name="workspaceId">workspace to save to </param>
        public void SaveResource(StringBuilder resource, Guid workspaceId)
        {
            var con = Connection;
            var comsController = CommunicationControllerFactory.CreateController("SaveResourceService");
            comsController.AddPayloadArgument("ResourceXml", resource);
            comsController.ExecuteCommand<IExecuteMessage>(con, workspaceId);
        }

        /// <summary>
        /// Save a resource to the server
        /// </summary>
        /// <param name="resource">resource to save</param>
        /// <param name="workspaceId">the workspace to save to</param>
        public void SaveResource(IResource resource, Guid workspaceId)
        {
            var con = Connection;
            var comsController = CommunicationControllerFactory.CreateController("SaveResourceService");
            Dev2JsonSerializer serialiser = new Dev2JsonSerializer();
            comsController.AddPayloadArgument("ResourceXml", serialiser.SerializeToBuilder(resource));
            var output = comsController.ExecuteCommand<IExecuteMessage>(con, workspaceId);
            if (output.HasError)
                throw new WarewolfSaveException(output.Message.ToString(), null);

        }

        /// <summary>
        /// Tests if a valid connection to a server can be made
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        public string TestConnection(IResource resource)
        {
            var con = Connection;
            var comsController = CommunicationControllerFactory.CreateController("TestConnectionService");
            Dev2JsonSerializer serialiser = new Dev2JsonSerializer();
            comsController.AddPayloadArgument("ResourceXml", serialiser.SerializeToBuilder(resource));
            var output = comsController.ExecuteCommand<IExecuteMessage>(con, GlobalConstants.ServerWorkspaceID);
            if (output == null)
                return "Unable to contact server";
            if (output.HasError)
                return output.Message.ToString();

            return "Success";


        }

        #endregion
    }

    public class WarewolfSaveException : WarewolfException
    {
        public WarewolfSaveException(string message, Exception innerException)
            : base(message, innerException, ExceptionType.Execution, ExceptionSeverity.Error)
        {
        }
    }
}
