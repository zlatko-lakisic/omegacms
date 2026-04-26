namespace MD.Tools.BaseDataAccess.Plugins.Core.Mapping.TaxonomyContent
{
    public enum Methods : int
    {
        Insert = 1,
        DeleteTaxonomy = 2,
        SaveTaxonomyContent = 3,
        GetByTaxonomyId = 4,
        Delete = 5,
        DeleteContentTaxonomy = 6,
        Update = 7,
        GetByContent = 8,
        DeleteAllByTaxonomyId = 9,
        GetByTaxonomyIdCount = 10,
        GetByTaxonomyIdWithPagination = 11,
        TaxonomyContentGetTaxonomyByContent=12,
        Search = 13
    }
}