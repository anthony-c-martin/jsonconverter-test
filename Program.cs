using Newtonsoft.Json;

var entity = new MyEntity {
    Compressed = "This is a compressed value!",
    Uncompressed = "This is an uncompressed value!",
    Enum = MyTestEnum.Foo,
};

var serialized = JsonConvert.SerializeObject(entity);

Console.WriteLine($"Serialized: {serialized}");