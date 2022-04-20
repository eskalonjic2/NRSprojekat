using System;
using System.Collections.Generic;

#nullable disable

namespace CinemaluxAPI.DAL.OrganizationDbContext.Models
{
    public partial class MovieGenre
    {
        public int Id { get; set; }
        public string GenreCode { get; set; }
        public short MovieId { get; set; }

        public virtual Genre Genre { get; set; }
        public virtual Movie Movie { get; set; }
    }
}
