using MD.CMS.BusinessLogic.Core.DataAccess.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MD.Tools.Helpers.Core.Extensions.EnumExt;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities
{
    public class GenericResponse : GenericResponse<bool>
    {
        public GenericResponse(GenericResponseStatusText status = GenericResponseStatusText.Ok) : base(status)
        {
        }
    }

    public class GenericResponse<T>
    {
        private bool _success;
        private GenericResponseStatusText _status;
        private T _value;

        public string Status
        {
            get { return _status.GetStringValue(); }
        }

        public T Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public bool Success { get => _success; set => _success = value; }

        public GenericResponse(GenericResponseStatusText status = GenericResponseStatusText.Ok)
        {
            _status = status;
        }
    }
}
