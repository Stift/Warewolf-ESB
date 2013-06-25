﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dev2.Converters.DateAndTime.Interfaces
{
    public interface IDateTimeFormatter
    {
        bool TryFormat(IDateTimeOperationTO dateTimeTO, out string result, out string error);
    }
}
