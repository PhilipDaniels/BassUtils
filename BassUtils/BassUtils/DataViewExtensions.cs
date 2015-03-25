﻿using System.Collections.Generic;
using System.Data;

namespace BassUtils
{
    /// <summary>
    /// Extensions to the <code>System.Data.DataView</code> class.
    /// </summary>
    public static class DataViewExtensions
    {
        /// <summary>
        /// Return a list of all the column names in the view. This is the same as the underlying table, I think.
        /// </summary>
        /// <param name="dataView">The data view that you want the column names from.</param>
        /// <returns>List of column names.</returns>
        public static IEnumerable<string> ColumnNames(this DataView dataView)
        {
            dataView.ThrowIfNull("dataView");

            return dataView.Table.ColumnNames();
        }
    }
}
