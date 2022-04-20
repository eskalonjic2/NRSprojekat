using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using JetBrains.Annotations;

namespace CinemaluxAPI.Common.Extensions
{
    public interface ICreatableEntity
    {
        public DateTime CreatedAt { get; set; }
        [CanBeNull] public string CreatedBy { get; set; }
    }
    
    public interface IModifiableEntity
    {
        public DateTime ModifiedAt { get; set; }
        [CanBeNull] public string ModifiedBy { get; set; }
    }
    
    public interface IArchivableEntity
    {
        public DateTime? ArchivedAt { get; set; }
        [CanBeNull] public string ArchivedBy { get; set; }
    }
    public static class Extension
    {
        public static T EnsureNotNull<T>(this T targetObject , string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            if (targetObject == null)
            {
                throw new HttpResponseException(statusCode, message);
            }
            
            return targetObject;
        }
        
        public static T EnsureNotNull<T>(this T targetObject , string targetProp, string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            if (targetObject.GetType().GetProperty(targetProp) == null)
            {
                throw new HttpResponseException(statusCode, message);
            }

            return targetObject;
        }
        
        public static T Archive<T>(this T targetObject, IArchivableEntity entity, string archivedBy = "SYSTEM")
        {
            entity.ArchivedAt = DateTime.Now;
            entity.ArchivedBy = archivedBy;

            return targetObject;
        }

        public static bool NotContains<T>(this ICollection<T> targetObject, T target)
        {
            return !targetObject.Contains(target);
        }

        public static bool IsNotNull(this string targetString)
        {
            return targetString != null;
        }
        
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    } 
}