﻿namespace Levshits.Data.Common
{
    public class RequestBase
    {
        public object Value { get; set; }
        public string Id => GetType().Name;
    }
}
