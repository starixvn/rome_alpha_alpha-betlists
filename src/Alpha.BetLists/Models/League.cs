namespace Alpha.BetLists.Models
{
    using System.Collections.Generic;

    public class League
    {
        public League(int id, string name, IEnumerable<Match> matches)
        {
            Id = id;
            Name = name;
            Matches = matches;
        }

        public int Id { get; }

        public string Name { get; }

        public IEnumerable<Match> Matches { get; }
    }
}
