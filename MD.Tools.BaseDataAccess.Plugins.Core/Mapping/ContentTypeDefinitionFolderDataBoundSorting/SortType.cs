using MD.Tools.Helpers.Core.TypeAttributes;

namespace MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolderDataBoundSorting
{
	public enum SortType : int
	{
        [StringValue("ASC")]
		ASC = 1,
        [StringValue("DESC")]
        DESC = 2
	}
}
