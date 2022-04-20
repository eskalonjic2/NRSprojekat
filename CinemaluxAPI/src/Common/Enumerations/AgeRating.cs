using System.ComponentModel;

namespace CinemaluxAPI.Common.Enumerations
{
    public enum AgeRating
    {
        [Description("Adults Only")]
        NC17 = 4,
        [Description("Restricted")]
        R = 3,
        [Description("Parents Strongly Cautioned")]
        PG13 = 2,
        [Description("PG – Parental Guidance Suggested")]
        PG = 1,
        [Description("General Audiences")]
        G = 0
    }
}