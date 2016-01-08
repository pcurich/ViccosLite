using System;
using System.Collections.Generic;
using ViccosLite.Core.Domain.Users;

namespace ViccosLite.Core.Domain.Sales
{
    public class InStore : BaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime DateTime { get; set; }

        public virtual List<DetailInStore> DetailInStores { get; set; }
    }
}