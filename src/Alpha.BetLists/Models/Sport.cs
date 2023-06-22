namespace Alpha.BetLists.Models
{
    using System.Collections.Generic;

    public class Sport
    {
        public Sport()
        {
        }

        public Sport(int id, string name, IEnumerable<League> leagues)
        {
            Id = id;
            Name = name;
            Leagues = leagues;
        }

        public int Id { get; }

        public string Name { get; }

        public IEnumerable<League> Leagues { get; }
    }
}
