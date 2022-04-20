using System;
using System.Collections.Generic;

#nullable disable

namespace CinemaluxAPI.DAL.OrganizationDbContext.Models
{
    public partial class Genre
    {
        public Genre()
        {
            MovieGenres = new HashSet<MovieGenre>();
        }

        public string Code { get; set; }
        public string Name { get; set; }

        public virtual ICollection<MovieGenre> MovieGenres { get; set; }
    }
}
