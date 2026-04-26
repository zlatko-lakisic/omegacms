using MD.Tools.Helpers.Core.TypeAttributes;

namespace MD.Tools.BaseDataAccess.Plugins.Core.Mapping
{
	public enum DatabaseTypes : int
	{
		[StringValue("MySQL")]
		MySQL = 1,
		[StringValue("Microsoft SQL")]
		MSSQL = 2,
		[StringValue("Oracle")]
		Oracle = 3,
		[StringValue("CouchBase")]
		CouchBase = 4,
		[StringValue("SOLR")]
		SOLR = 5
	}
}