namespace Odyssey.GrainInterfaces.Core.Services
{
    public class NameGenerator : INameGenerator
    {
        private static readonly string[] _adjectives =
        [
            "Wiggly", "Blobby", "Chunky", "Dizzy", "Fluffy", "Goofy",
            "Grumpy", "Jiggly", "Lumpy", "Mushy", "Noodly", "Plump",
            "Pudgy", "Quirky", "Scruffy", "Sleepy", "Slimy", "Sloppy",
            "Squishy", "Stumpy", "Tubby", "Wacky", "Wheezy", "Wobbly",
            "Zany", "Cranky", "Dopey", "Frumpy", "Soggy", "Groggy"
        ];

        private static readonly string[] _nouns =
        [
            "Biscuit", "Blob", "Bonker", "Boop", "Cabbage", "Clonker",
            "Dingus", "Donut", "Doodle", "Flop", "Fungus", "Giblet",
            "Goblin", "Gopher", "Gremlin", "Gumdrop", "Muffin", "Noodle",
            "Nugget", "Pickle", "Plonker", "Pudding", "Rascal", "Sausage",
            "Schnozzle", "Snorkel", "Splodge", "Tater", "Waffle", "Wobble"
        ];

        private readonly Random _random = Random.Shared;

        public string Generate()
        {
            var adjective = _adjectives[_random.Next(_adjectives.Length)];
            var noun = _nouns[_random.Next(_nouns.Length)];
            return $"{adjective} {noun}";
        }
    }
}
