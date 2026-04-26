using MD.Tools.Helpers.Core.TypeAttributes;

namespace MD.CMS.BusinessLogic.Core.DataAccess.Enumerations.Mapping
{

    internal enum GalleriesEnum
    {
        [StringValue("_GalleriesId")]
        GalleriesId,
        [StringValue("_DateCreated")]
        DateCreated,
        [StringValue("_Name")]
        Name,
        [StringValue("_Path")]
        Path
    }

    internal enum GalleriesParameterEnum
    {
        [StringValue("GalleriesId")]
        GalleriesId,
        [StringValue("DateCreated")]
        DateCreated,
        [StringValue("Name")]
        Name,
        [StringValue("Path")]
        Path
    }

    internal enum GalleriesSPEnum
    {

        [StringValue("Galleries_Delete")]
        Delete,
        [StringValue("Galleries_Insert")]
        Insert,
        [StringValue("Galleries_Update")]
        Update,
        [StringValue("Galleries_SelectByGalleriesId")]
        SelectByGalleries,
        [StringValue("Galleries_SelectAll")]
        SelectAll
    }
}
