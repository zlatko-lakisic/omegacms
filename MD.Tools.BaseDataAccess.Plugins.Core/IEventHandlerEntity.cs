using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.Tools.BaseDataAccess.Plugins.Core
{
    public interface IEventHandlerEntity
    {
        #region Properties
        bool HasOnBeforeInsert { get; }
        bool HasOnAfterInsert { get; }
        bool HasOnBeforeUpdate { get; }
        bool HasOnAfterUpdate { get; }
        bool HasOnBeforeDelete { get; }
        bool HasOnAfterDelete { get; }
        bool HasOnBeforeSelect { get; }
        bool HasOnAfterSelect { get; }
        #endregion

        #region Methods
        void OnBeforeInsert(string args);
        void OnAfterInsert(string args);
        void OnBeforeUpdate(string args);
        void OnAfterUpdate(string args);
        void OnBeforeDelete(string args);
        void OnAfterDelete(string args);
        void OnBeforeSelect(string args);
        void OnAfterSelect(string args);
        #endregion
    }
}
