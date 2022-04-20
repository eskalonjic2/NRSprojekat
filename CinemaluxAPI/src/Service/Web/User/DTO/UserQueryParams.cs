using CinemaluxAPI.Common;
using JetBrains.Annotations;

namespace CinemaluxAPI.Service.Web.DTO
{
    public class UserQueryParams : GridParams
    {
        [CanBeNull] public string NameQuery { get; set; }
    }
}