using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.Tools.BaseDataAccess.Core.Exceptions
{
    public class InadequatePermissionException: Exception
    {
        public InadequatePermissionException(string message) : base(message)
        {

        }

        public InadequatePermissionException(string message, InadequatePermissionException innerException) : base(message, innerException)
        {

        }
    }
}
