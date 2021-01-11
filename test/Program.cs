using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sunrise;
using Sunrise.Mapping;

namespace test
{
    class Program
    {
        class ChildObject
        {
            [SunriseProperty("arr")]
            public string[] Arr { get; set; }
        }

        class TestObject
        {
            [SunriseProperty("bruh")]
            public int Bruh { get; set; }

            [SunriseProperty("kek")]
            public string Kek { get; set; }

            [SunriseProperty("obj")]
            public ChildObject Obj { get; set; }
        }

        static void Main(string[] args)
        {
            var obj = new TestObject()
            {
                Bruh = 5,
                Kek = "alright",
                Obj = new ChildObject()
                {
                    Arr = new string[] { "kek", "chief" }
                }
            };

            var serialized = SunriseSerializer.Serialize(obj);

            var deserialized = SunriseSerializer.Deserialize<TestObject>(serialized);
        }
    }
}