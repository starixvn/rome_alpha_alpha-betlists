namespace Alpha.BetLists.Models
{
    using System;

    public class CosolidateProduct
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime? EndOfSupportDate { get; set; }

        public string DisplayName { get; set; }

        public bool IsEnable { get; set; }

        public int Type { get; set; }

        public string TypeName { get; set; }

        public int OrderNo { get; set; }
    }

    public class Product
    {
        public Product()
        {
        }

        public Product(int id, string name, DateTime? endOfSupportDate, bool isEnable, int type, int orderNo)
            : this(id, name, endOfSupportDate, type, orderNo)
        {
            IsEnable = isEnable;
        }

        public Product(int id, string name, DateTime? endOfSupportDate, int type, int orderNo)
        {
            Id = id;
            Name = name;
            EndOfSupportDate = endOfSupportDate;
            Type = type;
            OrderNo = orderNo;
        }

        public Product(byte id, string name, bool isEnable)
            : this(id, name, null, 0, 0)
        {
            IsEnable = isEnable;
        }

        internal Product(int id, string name, DateTime? endOfSupportDate, Func<int, string> getProductDisplayName, int type, int orderNo)
            : this(id, name, endOfSupportDate, type, orderNo)
        {
            DisplayName = getProductDisplayName == null ? name : getProductDisplayName(id);
            TypeName = type == 0 ? string.Empty : (getProductDisplayName == null ? name : getProductDisplayName(type));
        }

        internal Product(int id, string name, bool isEnable, Func<int, string> getProductDisplayName, int type, int orderNo)
            : this(id, name, null, getProductDisplayName, type, orderNo)
        {
            IsEnable = isEnable;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime? EndOfSupportDate { get; set; }

        public string DisplayName { get; set; }

        public bool IsEnable { get; set; }

        public int Type { get; set; }

        public string TypeName { get; set; }

        public int OrderNo { get; set; }

        public bool IsHide { get; set; }
    }
}