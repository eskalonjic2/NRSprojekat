using System;

namespace CinemaluxAPI.Common.Extensions
{
   public abstract class ArchivableEntity
   {
      public DateTime? ArchivedAt { get; set; }
   }
}