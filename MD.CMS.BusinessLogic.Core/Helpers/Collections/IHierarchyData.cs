namespace MD.CMS.BusinessLogic.Core.Helpers.Collections
{
    //
    // Summary:
    //     Exposes a node of a hierarchical data structure, including the node object and
    //     some properties that describe characteristics of the node. Objects that implement
    //     the System.Web.UI.IHierarchyData interface can be contained in System.Web.UI.IHierarchicalEnumerable
    //     collections, and are used by ASP.NET site navigation and data source controls.
    public interface IHierarchyData
    {
        //
        // Summary:
        //     Indicates whether the hierarchical data node that the System.Web.UI.IHierarchyData
        //     object represents has any child nodes.
        //
        // Returns:
        //     true if the current node has child nodes; otherwise, false.
        bool HasChildren { get; }
        //
        // Summary:
        //     Gets the hierarchical path of the node.
        //
        // Returns:
        //     A System.String that identifies the hierarchical path relative to the current
        //     node.
        string Path { get; }
        //
        // Summary:
        //     Gets the hierarchical data node that the System.Web.UI.IHierarchyData object
        //     represents.
        //
        // Returns:
        //     An System.Object hierarchical data node object.
        object Item { get; }
        //
        // Summary:
        //     Gets the name of the type of System.Object contained in the System.Web.UI.IHierarchyData.Item
        //     property.
        //
        // Returns:
        //     The name of the type of object that the System.Web.UI.IHierarchyData object represents.
        string Type { get; }

        //
        // Summary:
        //     Gets an enumeration object that represents all the child nodes of the current
        //     hierarchical node.
        //
        // Returns:
        //     An System.Web.UI.IHierarchicalEnumerable collection of child nodes of the current
        //     hierarchical node.
        IHierarchicalEnumerable GetChildren();
        //
        // Summary:
        //     Gets an System.Web.UI.IHierarchyData object that represents the parent node of
        //     the current hierarchical node.
        //
        // Returns:
        //     An System.Web.UI.IHierarchyData object that represents the parent node of the
        //     current hierarchical node.
        IHierarchyData GetParent();
    }
}
