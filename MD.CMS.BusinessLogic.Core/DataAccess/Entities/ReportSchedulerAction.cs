using MD.Tools.BaseDataAccess.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities
{
   public class ReportSchedulerAction:BaseEntity<long>
   {
       #region Attributes
       private long _schedulerId;
       private string _name;
       private string _authorId;
       private DateTime _dateCreated;
       private DateTime _dateEdited;
       private EnumAction _actionType;
       private string _options;
       private bool _isActive;
       #endregion

       #region Properties
       public long SchedulerId
       {
           get { return _schedulerId; }
           set { _schedulerId = value; }
       }
       public string Name
       {
           get { return _name; }
           set { _name = value; }
       }

       public string AuthorId
       {
           get { return _authorId; }
           set { _authorId = value; }
       }
       public DateTime DateCreated
       {
           get { return _dateCreated; }
           set { _dateCreated = value; }
       }
       public DateTime DateEdited
       {
           get { return _dateEdited; }
           set { _dateEdited = value; }
       }
       public EnumAction ActionType
       {
           get { return _actionType; }
           set { _actionType = value; }
       }
       public string Options
       {
           get { return _options; }
           set { _options = value; }
       }
       public bool IsActive
       {
           get { return _isActive; }
           set { _isActive = value; }
       }
       #endregion

       #region Enums
       public enum EnumAction : int
       {
           SaveToDisk = 1,
           Email = 2,
       }
     
       #endregion
    }
}
