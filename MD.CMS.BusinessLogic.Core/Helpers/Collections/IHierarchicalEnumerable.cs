using System;
using System.Collections;

namespace MD.CMS.BusinessLogic.Core.Helpers.Collections
{
    //
    // Summary:
    //     Represents a hierarchical collection that can be enumerated with an System.Collections.IEnumerator
    //     interface. Collections that implement the System.Web.UI.IHierarchicalEnumerable
    //     interface are used by ASP.NET site navigation and data source controls.
    public interface IHierarchicalEnumerable : IEnumerable
    {
        //
        // Summary:
        //     Returns a hierarchical data item for the specified enumerated item.
        //
        // Parameters:
        //   enumeratedItem:
        //     The System.Object for which to return an System.Web.UI.IHierarchyData.
        //
        // Returns:
        //     An System.Web.UI.IHierarchyData instance that represents the System.Object passed
        //     to the System.Web.UI.IHierarchicalEnumerable.GetHierarchyData(System.Object)
        //     method.
        IHierarchyData GetHierarchyData(object enumeratedItem);
    }
}
