using pnj_generator.Models.Rules;
using System.Text.Json.Serialization;

namespace pnj_generator.DTOs
{
    public class UniverseCreateDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Era { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string DiceRule { get; set; } = string.Empty;
        public bool HasModifiers { get; set; } = false;
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ModifierType? ModifierType { get; set; } = null;
    }
}
