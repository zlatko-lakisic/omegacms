using MD.Tools.Helpers.Core.TypeAttributes;

namespace MD.Tools.BaseDataAccess.Core.Enumerations
{
    public enum RequestBodyType
    {
        [StringValue("JSON")]
        JSON,
        [StringValue("UrlEncoded")]
        UrlEncoded
    }
}
