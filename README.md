# About
Sunrise is an efficient and simple object -> byte serializer/deserializer.<br>
Format wise it's somewhat similar to the MP4 container but is also inspired a lot from JSON.<br>

Currently the only way of serializing would be to create the Sunrise objects yourself, tho i can see myself writing something like Json.NET's "JsonProperty" attribute.

# Usage
### Serializing
```cs
SunriseArray numbers = new SunriseArray(new List<SunriseToken>() 
{ 
    new SunriseValue(BitConverter.GetBytes(420)),
    new SunriseValue(BitConverter.GetBytes(69))
});

SunriseObject container = new SunriseObject();
container["numbers"] = numbers;

byte[] output = container.Serialize();
```

### Deserializing
```cs
SunriseObject deserialized = (SunriseObject)SunriseDeserializer.Deserialize(output);
```

# Sunrise standard specification
TODO
