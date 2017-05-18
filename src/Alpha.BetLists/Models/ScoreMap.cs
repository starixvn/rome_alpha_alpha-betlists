namespace Alpha.BetLists.Models
{
    public class ScoreMap
    {
        public ScoreMap()
        {
        }

        public ScoreMap(
            int typeId,
            int eventType,
            string firstId,
            string infoId)
        {
            TypeId = typeId;
            EventType = eventType;
            FirstId = firstId;
            InfoId = infoId;
        }

        public int TypeId { get; }

        public int EventType { get; }

        public string FirstId { get; }

        public string InfoId { get; }
    }
}