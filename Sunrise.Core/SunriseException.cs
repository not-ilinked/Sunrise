using System;

namespace Sunrise
{
    // rebranded AggregateException to make it easier to catch Sunrise specific errors :)
    public class SunriseException : AggregateException
    {
        public SunriseException(Exception ex) : base(ex)
        { }
    }
}
