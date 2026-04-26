using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ReportSchedulerAction
{
   public class Data
    {
//      
       #region Columns
       public class Columns
       {
           #region Properties
           public static string ReportSchedulerActionId
           {
               get { return "ReportSchedulerActionId"; }
           }
           public static string SchedulerId
           {
               get { return "SchedulerId"; }
           }
           public static string Name
           {
               get { return "Name"; }
           }
           public static string AuthorId
           {
               get { return "AuthorId"; }
           }
           public static string DateCreated
           {
               get { return "DateCreated"; }
           }
           public static string DateEdited
           {
               get { return "DateEdited"; }
           }
           public static string ActionType
           {
               get { return "ActionType"; }
           }
           public static string Options
           {
               get { return "Options"; }
           }
           public static string IsActive
           {
               get { return "IsActive"; }
           }
           #endregion
       }
       #endregion
       #region Methods
       public static DataTable GetTable()
       {
           DataTable table = new DataTable();
           table.Columns.Add(new DataColumn(Columns.ReportSchedulerActionId));
           table.Columns.Add(new DataColumn(Columns.SchedulerId));
           table.Columns.Add(new DataColumn(Columns.Name));
           table.Columns.Add(new DataColumn(Columns.AuthorId));
           table.Columns.Add(new DataColumn(Columns.DateCreated));
           table.Columns.Add(new DataColumn(Columns.DateEdited));
           table.Columns.Add(new DataColumn(Columns.ActionType));
           table.Columns.Add(new DataColumn(Columns.Options));
           table.Columns.Add(new DataColumn(Columns.IsActive));

           return table;
       }
       #endregion
    }
}
