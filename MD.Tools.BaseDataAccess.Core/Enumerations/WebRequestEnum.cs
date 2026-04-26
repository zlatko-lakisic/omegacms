using MD.Tools.Helpers.Core.TypeAttributes;

namespace MD.Tools.BaseDataAccess.Core.Enumerations
{
    public enum WebRequestEnum
    {
        [StringValue("GET")]
        Get,
        [StringValue("POST")]
        Post,
        [StringValue("Put")]
        Put,
        [StringValue("Put")]
        Delete
    }
}
