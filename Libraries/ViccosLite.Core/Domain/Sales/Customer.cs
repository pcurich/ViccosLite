namespace ViccosLite.Core.Domain.Sales
{
    public class Customer:BaseEntity
    {
        public string BusinessName { set; get; }
        public string DocumentNumber { set; get; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}