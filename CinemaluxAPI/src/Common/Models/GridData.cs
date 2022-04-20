using System;
using System.Linq;

namespace CinemaluxAPI.Common
{
    public class GridData<T>
    {
        public int TotalItems { get; }
        public int RowsPerPage { get; }
        public int CurrentPage { get; }
        public T[] Rows { get; }
        
        public GridData(IQueryable<T> rawRows, GridParams gridParams)
        {
            T[] rowArray = rawRows.ToArray();
            
            TotalItems = rowArray.Length;
            CurrentPage = gridParams.CP;
            RowsPerPage = gridParams.RPP ?? rowArray.Length;
            Rows = rowArray.Skip((CurrentPage - 1) * RowsPerPage).Take(RowsPerPage).ToArray();
        }
    }
}