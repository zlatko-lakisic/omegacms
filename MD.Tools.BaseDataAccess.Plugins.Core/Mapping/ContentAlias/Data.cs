using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ContentAlias
{
  public  class Data
  {
      #region Columns
      public class Columns
      {
          #region Properties
          public static string LCID
          {
              get { return "lcid"; }
          }
          public static string ContentId
          {
              get { return "contentId"; }
          }
          public static string DateCreated
          {
              get { return "dateCreated"; }
          }
          public static string Alias
          {
              get { return "alias"; }
          }

          #endregion
      }
      #endregion
      #region Methods
      public static DataTable GetTable()
      {
          DataTable table = new DataTable();
          table.Columns.Add(new DataColumn(Columns.LCID));
          table.Columns.Add(new DataColumn(Columns.ContentId));
          table.Columns.Add(new DataColumn(Columns.DateCreated));
          table.Columns.Add(new DataColumn(Columns.Alias));
          return table;
      }
      #endregion
  }
}
