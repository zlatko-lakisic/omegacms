using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MD.Tools.BaseDataAccess.Core.Interfaces
{
    public interface IBaseController<T>
        where T : MD.Tools.BaseDataAccess.Core.Entities.BaseEntity
    {
        T Create(DataRow row);
    }
}
