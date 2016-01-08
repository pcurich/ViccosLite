using System;
using System.Collections.Generic;
using ViccosLite.Core.Domain.Stores;

namespace ViccosLite.Core.Domain.Users
{
    public class User : BaseEntity
    {
        public int UserParentId { get; set; }
        public string UserParentNombre { get; set; }

        public Guid UserGuid { get; set; }

        public string UserName { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompleteName { get; set; }
        public string Dni { get; set; }       
        public string Address { get; set; }
        public string Department { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string CelPhone { get; set; }
        public string Email { get; set; }
        
        public string LastIpAddress { get; set; }
        public DateTime LastActivityDateUtc { get; set; }
        public bool IsSystemAccount { get; set; }
        public string SystemName { get; set; }
        public DateTime LastLoginDateUtc { get; set; }

        private List<Store> _stores;

        public virtual List<Store> Stores
        {
            get { return _stores ?? (_stores = new List<Store>()); }
            set { _stores = value; }
        }

        private List<UserRole> _userRoles;

        public virtual List<UserRole> UserRoles
        {
            get { return _userRoles ?? (_userRoles = new List<UserRole>()); }
            set { _userRoles = value; }
        } 
    }
}