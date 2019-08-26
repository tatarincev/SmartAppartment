using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Assessment1.Models
{
    /// <summary>
    /// Represents a pet in a pet store
    /// </summary>
    public class Pet
    {
        public long Id { get; set; }
        public string Name { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public PetStatus Status { get; set; }
    }
}
