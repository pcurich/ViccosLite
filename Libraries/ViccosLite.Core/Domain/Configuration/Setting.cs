namespace ViccosLite.Core.Domain.Configuration
{
    public class Setting : BaseEntity
    {
        public Setting()
        {
        }
        public Setting(string name, string value, int storeId = 0)
        {
            Name = name;
            Value = value;
            StoreId = storeId;
        }

        public string Name { get; set; }
        public string Value { get; set; }
        public int StoreId { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}