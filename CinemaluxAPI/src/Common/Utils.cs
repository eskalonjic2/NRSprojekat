using System;
using System.Linq;
using CinemaluxAPI.Common.Extensions;

namespace CinemaluxAPI.Common
{
    public class Utils
    {
        public static DateTime DateIdToDate(int date)
        {
            return new DateTime(date / 10000, (date / 100) % 100, date % 100);
        }
    }
}