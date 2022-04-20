using System;
using System.Collections.Generic;
using CinemaluxAPI.Auth;
using CinemaluxAPI.Common.Extensions;

#nullable disable

namespace CinemaluxAPI.DAL.CinemaluxCatalogue.Models
{
    public partial class Employee : Identity, ICreatableEntity, IModifiableEntity, IArchivableEntity
    {
        public Employee()
        {
            InverseManager = new HashSet<Employee>();
            Orders = new HashSet<Order>();
        }

        public int? ManagerId { get; set; }
        public string Key { get; set; }
        public string Password { get; set; }
        public DateTime BornAt { get; set; }
        public string Address { get; set; }
        public double? Salary { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ArchivedAt { get; set; }
        public string ArchivedBy { get; set; }

        public virtual Employee Manager { get; set; }
        public virtual ICollection<Employee> InverseManager { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
