using System.Text;
using ClausaComm.Network_Communication.Objects;
using Newtonsoft.Json;

namespace ClausaComm;

// Universal serialization.
public static class Serialization
{
    /*private static readonly JsonSerializerSettings SerializerSettings = new()
    {
        TypeNameHandling = TypeNameHandling.Auto,
        Formatting = Formatting.Indented,
    };

    public static byte[] SerializeToUtf8Bytes<T>(this T obj) => Encoding.UTF8.GetBytes(Serialize(obj));
    public static string Serialize<T>(this T obj) => JsonConvert.SerializeObject(obj, typeof(T), SerializerSettings);

    public static T Deserialize<T>(byte[] obj) => Deserialize<T>(Encoding.UTF8.GetString(obj));
    public static T Deserialize<T>(string obj) => JsonConvert.DeserializeObject<T>(obj, SerializerSettings);*/
}