using System;
using System.Collections.Generic;
using System.Text;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Entities.GenericContent
{
    public interface IGenericContentFieldConvert<F> where F : Entities.ContentTypeDefinitionField
    {
        T Convert<T>(F input) where T : GenericContentField, new();
    }
}
