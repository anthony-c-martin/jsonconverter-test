using Newtonsoft.Json;

var entity = new MyEntity {
    Compressed = "This is a compressed value!",
    Uncompressed = "This is an uncompressed value!",
    Enum = MyTestEnum.Foo,
};

var serialized = JsonConvert.SerializeObject(entity);

// validate compression works as expected
Console.WriteLine($"Serialized: {serialized}");

entity = JsonConvert.DeserializeObject<MyEntity>(serialized);

var test = new {
    entity.Compressed,
    entity.Uncompressed,
    entity.Enum
};

// validate serializing + deserializing gets back the original values
Console.WriteLine($"Values: {test}");