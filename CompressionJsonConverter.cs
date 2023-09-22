using Microsoft.WindowsAzure.ResourceStack.Common.Json;
using Microsoft.WindowsAzure.ResourceStack.Common.Utilities;
using Newtonsoft.Json;

public class CompressionJsonConverter : JsonConverter
{
    public override bool CanRead => true;

    public override bool CanWrite => true;

    public override bool CanConvert(Type objectType) => !objectType.IsValueType && objectType != typeof(string);

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var compressionUtility = DeflateCompressionUtility.Instance;
        if (reader.TokenType == JsonToken.String)
        {
            try
            {
                return JsonConvert.DeserializeObject(
                    value: compressionUtility.InflateString(new MemoryStream(Convert.FromBase64String((string)reader.Value))),
                    type: objectType,
                    settings: JsonExtensions.ObjectSerializationSettings);
            }
            catch (FormatException ex)
            {
                throw new JsonReaderException("Invalid encoded string value.", innerException: ex);
            }
            catch (JsonSerializationException ex)
            {
                throw new JsonReaderException("Invalid schema after decoding.", innerException: ex);
            }
        }
        else if (reader.TokenType == JsonToken.StartObject || reader.TokenType == JsonToken.StartArray)
        {
            // NOTE(wayan): Backward compatibility.
            return serializer.Deserialize(reader, objectType);
        }
        else if (reader.TokenType == JsonToken.Null)
        {
            return null;
        }

        throw new JsonReaderException("Unexpected value with compression.");
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        var compressionUtility = DeflateCompressionUtility.Instance;
        if (value != null)
        {
            writer.WriteValue(compressionUtility
                .DeflateString(JsonConvert.SerializeObject(
                    value: value,
                    settings: JsonExtensions.ObjectSerializationSettings))
                .ToArray());

            return;
        }

        writer.WriteNull();
    }
}