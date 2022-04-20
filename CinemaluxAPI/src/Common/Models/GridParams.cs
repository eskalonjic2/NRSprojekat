using System.ComponentModel;
using JetBrains.Annotations;

namespace CinemaluxAPI.Common
{
    public class GridParams
    {
        [Description("Current Page")] 
        public int CP { get; set; }
        
        [Description("Rows Per Page")] 
        public int? RPP { get; set; }
        
        [Description("Search Query")] 
        [CanBeNull] 
        public string SQ { get; set; }
    }
}