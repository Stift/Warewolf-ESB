﻿using System;
using System.Activities.Presentation.Model;
using Dev2.Studio.Core.Interfaces;

namespace Dev2.Studio.Core.Messages
{
    public class ConfigureCaseExpressionMessage : IMessage
    {
        public ConfigureCaseExpressionMessage(Tuple<ModelItem, IEnvironmentModel> model)
        {
            Model = model;
        }

        public Tuple<ModelItem, IEnvironmentModel> Model { get; set; }
    }
}