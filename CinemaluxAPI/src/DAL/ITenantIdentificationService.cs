using System;
using System.Collections.Generic;

namespace CinemaluxAPI.DAL.CinemaluxCatalogue.DAL.OrganizationCatalogue.DAL
{
    public interface ITenantIdentificationService
    {
        public class TenantMapping
        {
            public string Default { get; set; }
            public Dictionary<string, string> Tenants { get; } = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
        }
    }
}