using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public enum MyTestEnum {
    None,
    Foo,
    Bar,
}

public class MyEntity
{
    [JsonConverter(typeof(CompressionJsonConverter))]
    public string Compressed { get; set; }

    public string Uncompressed { get; set; }


    [JsonConverter(typeof(StringEnumConverter))]
    public MyTestEnum Enum { get; set; }
}