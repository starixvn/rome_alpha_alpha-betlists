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

        public int TypeId { get; set; }

        public int EventType { get; set; }

        public string FirstId { get; set; }

        public string InfoId { get; set; }
    }
}