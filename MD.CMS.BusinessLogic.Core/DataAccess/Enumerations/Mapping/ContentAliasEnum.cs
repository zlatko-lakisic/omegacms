using MD.Tools.Helpers.Core.TypeAttributes;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Enumerations.Mapping
{
    internal enum ContentAliasEnum
    {
       
        [StringValue("ContentId")]
        ContentId,
        [StringValue("Alias")]
        Alias,
        [StringValue("LCID")]
        LCID,
        [StringValue("DateCreated")]
        DateCreated
    }
    internal enum ContentAliasParamatersEnum
    {
        
        [StringValue("_ContentId")]
        ContentId,
        [StringValue("_Alias")]
        Alias,
        [StringValue("_LCID")]
        LCID,
        [StringValue("_DateCreated")]
        DateCreated

    }

    internal enum ContentAliasSPEnum
    {
        [StringValue("ContentAlias_Select")]
        Select,
        [StringValue("ContentAlias_Delete")]
        Delete,
        [StringValue("ContentAlias_Insert")]
        Insert,
        [StringValue("ContentAlias_Update")]
        Update,
        [StringValue("ContentAlias_SelectAll")]
        SelectAll,
        [StringValue("ContentAlias_getByContent")]
        GetByContent
    }
}
