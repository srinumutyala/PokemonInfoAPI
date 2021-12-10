using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace PokemonInfo.Services
{
    public class PokemonDeserializer
    {
        public static T DeserializeStream<T>(System.IO.Stream stream)
        {
            using var sr = new System.IO.StreamReader(stream);
            using JsonReader reader = new JsonTextReader(sr);
            var serializer = JsonSerializer.Create();
            return serializer.Deserialize<T>(reader);
        }

        internal static async Task<string> StreamToStringAsync(Stream stream)
        {
            string content = null;

            if (stream != null)
                using (var sr = new StreamReader(stream))
                    content = await sr.ReadToEndAsync();

            return content;
        }
    }
}
