using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sunrise;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            SunriseArray numbers = new SunriseArray(new List<SunriseToken>() 
            { 
                new SunriseValue(BitConverter.GetBytes(420)),
                new SunriseValue(BitConverter.GetBytes(69))
            });

            SunriseObject container = new SunriseObject();
            container["numbers"] = numbers;

            byte[] output = container.Serialize();
        }
    }
}
