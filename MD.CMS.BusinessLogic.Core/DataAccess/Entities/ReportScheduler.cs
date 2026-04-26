using MD.Tools.BaseDataAccess.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities
{
  public  class ReportScheduler:BaseEntity<long>
  {
      #region Attributes
      private string _name;
      private string _authorId;
      private DateTime _dateCreated;
      private DateTime _dateEdited;
      private bool _isRecurring;
      private TimeSpan _interval;
      private DateTime _start;
      private DateTime? _end;
      private int _reportId;
      private bool _isActive;
      private bool _isDeleted;
      private List<ReportSchedulerAction> _actions;
      private User _author;
      #endregion

      #region Properties
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
      public bool IsRecurring
      {
          get { return _isRecurring; }
          set { _isRecurring = value; }
      }
      public TimeSpan Interval 
      {          
          get { return _interval; }
          set { _interval = value; }
      }
      public DateTime Start
      {
          get { return _start; }
          set { _start = value; }
      }
      public DateTime? End
      {
          get { return _end; }
          set { _end = value; }
      }
      public int ReportId
      {
          get { return _reportId; }
          set { _reportId = value; }
      }
      public bool IsActive
      {
          get { return _isActive; }
          set { _isActive = value; }
      }
      public bool IsDeleted
      {
          get { return _isDeleted; }
          set { _isDeleted = value; }
      }
      public List<ReportSchedulerAction> Actions 
      {
          get { return _actions; }
          set { _actions = value; }
      }
      public User Author
      {
          get { return _author; }
          set { _author = value; }
      }
      #endregion
  }
}
