namespace MD.Tools.BaseDataAccess.Plugins.Core.Mapping.Taxonomy
{
    public enum Methods : int
    {
        GetById = 1,
        GetTaxonomyByPath = 2,
        GetByParentId = 3,
        GetByContent = 4,
        GetAll = 5,
        Insert = 6,
        Update = 7,
        Delete = 8,
        TaxonomySearchByName = 9,
        AssignContentToTaxonomy = 10,
        GetByParentIdCount = 11,
        GetByParentIdWithPagination = 12,
        TaxonomyContentGetTaxonomyByContent=13,
        Search = 14
    }
}