namespace Alpha.BetLists.Models
{
    using System;

    public class Product
    {
        public Product(int id, string name) : this(id, name, null)
        {
        }

        public Product(int id, string name, DateTime? endOfSupportDate)
        {
            Id = id;
            Name = name;
            EndOfSupportDate = endOfSupportDate;
        }

        public Product(int id, string name, DateTime? endOfSupportDate, bool isEnable)
        {
            Id = id;
            Name = name;
            EndOfSupportDate = endOfSupportDate;
            IsEnable = isEnable;
        }

        public Product(byte id, string name, bool isEnable)
            : this(id, name, null)
        {
            IsEnable = isEnable;
        }

        internal Product(int id, string name, DateTime? endOfSupportDate, Func<int, string> getProductDisplayName)
            : this(id, name, endOfSupportDate)
        {
            DisplayName = getProductDisplayName == null ? name : getProductDisplayName(id);
        }

        internal Product(int id, string name, bool isEnable, Func<int, string> getProductDisplayName)
            : this(id, name, null, getProductDisplayName)
        {
            IsEnable = isEnable;
        }

        public int Id { get; }

        public string Name { get; }

        public DateTime? EndOfSupportDate { get; }

        public string DisplayName { get; }

        public bool IsEnable { get; }
    }
}