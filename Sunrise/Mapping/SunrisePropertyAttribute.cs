using System;

namespace Sunrise.Mapping
{
    public class SunrisePropertyAttribute : Attribute
    {
        public string Name { get; private set; }

        public SunrisePropertyAttribute(string name)
        {
            Name = name;
        }
    }
}
