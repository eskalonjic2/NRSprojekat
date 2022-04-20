using System;
using System.Collections.Generic;
using CinemaluxAPI.Common.Extensions;

#nullable disable

namespace CinemaluxAPI.DAL.CinemaluxCatalogue.Models
{
    public partial class Hall: ICreatableEntity,IArchivableEntity
    {
        public Hall()
        {
            Screenings = new HashSet<Screening>();
        }

        public byte Id { get; set; }
        public string Name { get; set; }        
        public string SeatValidityRegex { get; set; }
        public byte Capacity { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ArchivedAt { get; set; }
        public string ArchivedBy { get; set; }
        public virtual ICollection<Screening> Screenings { get; set; }
    }
}