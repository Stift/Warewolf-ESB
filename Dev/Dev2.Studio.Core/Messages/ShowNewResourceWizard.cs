﻿using Dev2.Studio.Interfaces.Messages;

namespace Dev2.Studio.Core.Messages
{
    public class ShowNewResourceWizard : IMessage
    {
        public string ResourceType { get; set; }

        public ShowNewResourceWizard(string resourceType)
        {
            ResourceType = resourceType;
        }
    }
}
