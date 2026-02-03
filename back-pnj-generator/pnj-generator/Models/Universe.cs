namespace pnj_generator.Models
{
    public class Universe
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Era { get; set; } = string.Empty;
        public string DiceRule { get; set; } = string.Empty;

        public ICollection<Weapons> Weapons { get; set; } = new List<Weapons>();
    }
}
