using System;
using CinemaluxAPI.Common;

namespace CinemaluxAPI.Services.Screenings.DTO
{
    #nullable enable
    public class ScreeningGridParams : GridParams
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string DateFormat { get; } = "dd/MM/yyyy";
    }
}