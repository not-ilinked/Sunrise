using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Sunrise.Mapping
{
    public static class SunriseMapper
    {
        private static bool IsNumber(object data)
        {
            return data is sbyte
                    || data is byte
                    || data is short
                    || data is ushort
                    || data is int
                    || data is uint
                    || data is long
                    || data is ulong
                    || data is float
                    || data is double
                    || data is decimal;
        }

        private static Dictionary<string, PropertyInfo> GetProperties(Type t)
        {
            var dict = new Dictionary<string, PropertyInfo>();

            foreach (var property in t.GetProperties())
            {
                foreach (var attr in property.GetCustomAttributes())
                {
                    if (attr.GetType() == typeof(SunrisePropertyAttribute))
                    {
                        dict[((SunrisePropertyAttribute)attr).Name] = property;
                        break;
                    }
                }
            }

            return dict;
        }

        public static SunriseToken Convert(object data)
        {
            var type = data.GetType();

            if (type.BaseType == typeof(SunriseToken))
                return (SunriseToken)data;
            else if (type.IsArray)
            {
                var arr = new SunriseArray();

                foreach (var item in (Array)data)
                    arr.Items.Add(Convert(item));

                return arr;
            }
            else if (type.GetProperties().Any(p => p.GetCustomAttributes().Any(a => a.GetType() == typeof(SunrisePropertyAttribute))))
            {
                var obj = new SunriseObject();

                foreach (var pair in GetProperties(type))
                {
                    var value = pair.Value.GetValue(data);

                    if (value != null)
                        obj.Children[pair.Key] = Convert(value);
                }

                return obj;
            }
            else if (type.BaseType == typeof(Enum))
                return Convert((int)data);
            else
            {
                if (IsNumber(data)) return new SunriseValue(BitConverter.GetBytes((dynamic)data));
                else return new SunriseValue(Encoding.UTF8.GetBytes(data.ToString()));
            }
        }

        public static object Map(SunriseToken token, Type t)
        {
            if (t == typeof(SunriseToken))
                return token;
            else if (t == typeof(SunriseValue))
                return (SunriseValue)token;
            else if (t == typeof(SunriseObject))
                return (SunriseObject)token;
            else if (t == typeof(SunriseArray))
                return (SunriseArray)token;

            switch (token.Type)
            {
                case SunriseType.Array:
                    if (!t.IsArray)
                        throw new ArgumentException("Type mismatch on array");

                    var items = ((SunriseArray)token).Items;

                    var arr = (object[])Activator.CreateInstance(t, new object[] { items.Count });
                    for (int i = 0; i < items.Count; i++)
                        arr[i] = Map(items[i], t.GetElementType());

                    return arr;
                case SunriseType.Object:
                    var sunriseObject = (SunriseObject)token;
                    object obj = Activator.CreateInstance(t);

                    foreach (var pair in GetProperties(t))
                    {
                        if (sunriseObject.Children.TryGetValue(pair.Key, out SunriseToken value))
                            pair.Value.SetValue(obj, Map(value, pair.Value.PropertyType));
                    }

                    return obj;
                case SunriseType.Value:
                    var sunriseValue = (SunriseValue)token;

                    if (t == typeof(string))
                        return Encoding.UTF8.GetString(sunriseValue.Data);

                    if (t.BaseType == typeof(Enum))
                    {
                        var values = Enum.GetValues(t);
                        return values.GetValue(BitConverter.ToInt32(sunriseValue.Data, 0)); // this might cause issues
                    }

                    foreach (var method in typeof(BitConverter).GetMethods())
                    {
                        if (method.Name == "To" + t.Name)
                        {
                            if (method.GetParameters().Length == 2)
                                return method.Invoke(null, new object[] { sunriseValue.Data, 0 });
                        }
                    }

                    throw new TypeAccessException("Could not find a method to convert value");
                default:
                    throw new ArgumentException("Unexpected Sunrise type");
            }
        }
    }
}
