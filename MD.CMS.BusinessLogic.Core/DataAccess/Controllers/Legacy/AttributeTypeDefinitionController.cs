using MD.CMS.BusinessLogic.Core.DataAccess.Entities;
using System;
using System.Collections.Generic;
using MD.CMS.BusinessLogic.Core.DataAccess.Controllers.Base;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Controllers
{
    public partial class AttributeTypeDefinitionController : BaseController<AttributeTypeDefinitionController>
    {
        [Obsolete("Deprecated", true)]
        public AttributeTypeDefinition GetByInputTypeId(long id)
        {
            return GetByInputTypeIdAsync(id).Result;
        }

        [Obsolete("Deprecated", true)]
        public AttributeTypeDefinition GetById(long id)
        {
            return GetByIdAsync(id).Result;
        }

        [Obsolete("Deprecated", true)]
        public List<AttributeTypeDefinition> GetAll()
        {
            return GetAllAsync().Result;
        }
    }
}
