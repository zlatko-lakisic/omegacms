namespace MD.Tools.BaseDataAccess.Plugins.Core.Mapping.ContentTypeDefinitionFolderDataBoundSorting
{
    public class ContentTypeDefinitionFolderDataBoundSorting
    {
        #region Attributes
        private SortType _sorter;
        private string _leftField;
        #endregion

        #region Properties
        public string LeftField { get => _leftField; set => _leftField = value; }
        public SortType Sorter { get => _sorter; set => _sorter = value; }
        #endregion
    }
}
