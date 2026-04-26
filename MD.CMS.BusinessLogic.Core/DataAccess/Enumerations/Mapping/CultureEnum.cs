using MD.Tools.Helpers.Core.TypeAttributes;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Enumerations.Mapping
{
  
        internal enum CultureEnum
        {
            [StringValue("LCID")]
            LCID,
            [StringValue("Name")]
            Name,
            [StringValue("Code")]
            Code,
            [StringValue("IsoCode")]
            IsoCode,
            [StringValue("IsApproved")]
            IsApproved
        }

        internal enum CultureParametersEnum
        {
            [StringValue("_LCID")]
            LCID,
            [StringValue("_Name")]
            Name,
            [StringValue("_Code")]
            Code,
            [StringValue("_IsoCode")]
            IsoCode
        }

        internal enum CultureSPEnum
        {
            [StringValue("Cultures_Update")]
            Update,
            [StringValue("Cultures_SelectByLCID")]
            SelectByLCID,
            [StringValue("Cultures_SelectByCode")]
            SelectByCode,
            [StringValue("Cultures_SelectAll")]
            SelectAll,
            [StringValue("Cultures_SelectAvailableForContent")]
            SelectAvailableForContent,
            [StringValue("Cultures_Insert")]
            Insert,
            [StringValue("Cultures_Delete")]
            Delete
        }
    
}
