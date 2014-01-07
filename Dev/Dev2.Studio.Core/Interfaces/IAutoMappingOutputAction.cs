﻿using Dev2.Data.Interfaces;

namespace Dev2.Studio.Core.Interfaces
{
    public interface IAutoMappingOutputAction
    {
        IInputOutputViewModel LoadOutputAutoMapping(IInputOutputViewModel item);
    }
}
