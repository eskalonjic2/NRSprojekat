using System;
using System.Collections.Generic;

#nullable disable

namespace CinemaluxAPI.DAL.OrganizationDbContext.Models
{
    public partial class City 
    {
        public City()
        {
            Cinemas = new HashSet<Cinema>();
        }

        public byte Id { get; set; }
        public string Name { get; set; }
        public string PostalCode { get; set; }
        public string TelephonePrefix { get; set; }
        public int Population { get; set; }

        public virtual ICollection<Cinema> Cinemas { get; set; }
    }
}
