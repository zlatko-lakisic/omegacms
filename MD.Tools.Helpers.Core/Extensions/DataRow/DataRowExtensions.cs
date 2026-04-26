using System;
using System.Data;
using System.Globalization;
using System.Linq;

namespace MD.Tools.Helpers.Core.Extensions.DataRow
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static class DataRowExtensions
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        #region Public Methods
        /// <summary>
        /// Get a value from a colum within a DataRow
        /// </summary>
        /// <typeparam name="T">Paramater type to get</typeparam>
        /// <param name="row"></param>
        /// <param name="columnName">Name of the desired column</param>
        /// <returns>The default column value</returns>
        public static T GetValue<T>(this System.Data.DataRow row, string columnName)
        {
            return GetValue<T>(row, columnName, default(T));
        }

        /// <summary>
        /// Get a value from a colum within a DataRow
        /// </summary>
        /// <typeparam name="T">Paramater type to get</typeparam>
        /// <param name="row"></param>
        /// <param name="columnName">Name of the desired column</param>
        /// <param name="defaultValue">Default value to return</param>
        /// <returns>The default column value</returns>
        public static T GetValue<T>(this System.Data.DataRow row, string columnName, T defaultValue)
        {
            T value = defaultValue;

            try
            {
                if (row != null && row.Table.Columns.Cast<DataColumn>().Any(c => string.Equals(c.ColumnName, columnName, StringComparison.InvariantCultureIgnoreCase)))
                {
                    DataColumn column = row.Table.Columns.Cast<DataColumn>().FirstOrDefault(c => string.Equals(c.ColumnName, columnName, StringComparison.InvariantCultureIgnoreCase));
                    value = Parse<T>(row[column.ColumnName], defaultValue);
                }
            }
            catch (Exception)
            {
                value = defaultValue;
            }

            return value;
        }
        /// <summary>
        /// Get a value from a colum within a DataRow
        /// </summary>
        /// <typeparam name="T">Paramater type to get</typeparam>
        /// <param name="row"></param>
        /// <param name="columnIndex">Index of the desired column</param>
        /// <returns>The default column value</returns>
        public static T GetValue<T>(this System.Data.DataRow row, int columnIndex)
        {
            return GetValue<T>(row, columnIndex, default(T));
        }

        /// <summary>
        /// Get a value from a colum within a DataRow
        /// </summary>
        /// <typeparam name="T">Paramater type to get</typeparam>
        /// <param name="row"></param>
        /// <param name="columnIndex">Index of the desired column</param>
        /// <param name="defaultValue">Default value to return</param>
        /// <returns>The default column value</returns>
        public static T GetValue<T>(this System.Data.DataRow row, int columnIndex, T defaultValue)
        {
            T value = defaultValue;

            try
            {
                if (row != null && row.Table.Columns.Count <= columnIndex + 1)
                {
                    //value = (T)Convert.ChangeType(row[columnIndex].ToString(), typeof(T));
                    //value = xHelpers.Helpers.Parser<T>.Parse(row[columnIndex].ToString());
                    value = Parse<T>(row[columnIndex], defaultValue);
                }
            }
            catch (Exception)
            {
                value = defaultValue;
            }

            return value;
        }

        /// <summary>
        /// Get a value from a colum within a DataRow
        /// </summary>
        /// <param name="row"></param>
        /// <param name="columnIndex">Index of the desired column</param>
        /// <returns>The default column value</returns>
        public static string GetStringValue(this System.Data.DataRow row, int columnIndex)
        {
            return GetStringValue(row, columnIndex, default(string));
        }

        /// <summary>
        /// Get a value from a colum within a DataRow
        /// </summary>
        /// <param name="row"></param>
        /// <param name="columnIndex">Index of the desired column</param>
        /// <param name="defaultValue">Default value to return</param>
        /// <returns>The default column value</returns>
        public static string GetStringValue(this System.Data.DataRow row, int columnIndex, string defaultValue)
        {
            string value = defaultValue;

            try
            {
                if (row != null && row.Table.Columns.Count >= columnIndex + 1)
                {
                    //value = (T)Convert.ChangeType(row[columnIndex].ToString(), typeof(T));
                    //value = xHelpers.Helpers.Parser<T>.Parse(row[columnIndex].ToString());
                    value = row[columnIndex].ToString();
                }
            }
            catch (Exception)
            {
                value = defaultValue;
            }

            return value;
        }

        /// <summary>
        /// Get a value from a colum within a DataRow
        /// </summary>
        /// <param name="row"></param>
        /// <param name="columnName">Name of the desired column</param>
        /// <returns>The default column value</returns>
        public static string GetStringValue(this System.Data.DataRow row, string columnName)
        {
            return GetStringValue(row, columnName, default(string));
        }

        /// <summary>
        /// Get a value from a colum within a DataRow
        /// </summary>
        /// <param name="row"></param>
        /// <param name="columnName">Name of the desired column</param>
        /// <param name="defaultValue">Default value to return</param>
        /// <returns>The default column value</returns>
        public static string GetStringValue(this System.Data.DataRow row, string columnName, string defaultValue)
        {
            string value = defaultValue;

            try
            {
                if (row != null && row.Table.Columns.Contains(columnName))
                {
                    //value = (T)Convert.ChangeType(row[columnIndex].ToString(), typeof(T));
                    //value = xHelpers.Helpers.Parser<T>.Parse(row[columnIndex].ToString());
                    value = row[columnName].ToString();
                }
            }
            catch (Exception)
            {
                value = defaultValue;
            }

            return value;
        }

        private static T Parse<T>(object value, T defaultValue)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            T returnValue = defaultValue;
            //returnValue = xHelpers.Helpers.Parser<T>.Parse(value);
            //returnValue = (T)Convert.ChangeType(value, typeof(T));

            switch (typeof(T).ToString())
            {
                case "System.Int16":
                case "System.Int32":
                case "System.Int64":
                case "System.Decimal":
                    returnValue = MD.Tools.Helpers.Core.Helpers.Parser<T>.Parse(value.ToString());
                    break;
                case "System.Boolean":
                    int wholeNumberValue = 0;
                    if(int.TryParse(value.ToString(), out wholeNumberValue))
                    {
                        returnValue = (T)Convert.ChangeType(wholeNumberValue != 0, typeof(T), CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        returnValue = (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
                    }
                    break;
                default:
                    returnValue = (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
                    break;
            }

            return returnValue;
        }
        #endregion
    }
}
