# About
Sunrise is a simple and efficient object -> byte serializer/deserializer.<br>
Format wise it's somewhat similar to the MP4 container but is also inspired a lot from JSON.<br>

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
