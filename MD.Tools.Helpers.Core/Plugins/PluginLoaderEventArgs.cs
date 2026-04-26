using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MD.Tools.Helpers.Core.Plugins
{
    /// <summary>
    /// Allows extensible loading of advanced plugin Loading
    /// </summary>
    public class PluginLoaderEventArgs<T> : EventArgs
    {
        /// <summary>
        /// Gets or sets the type of the category.
        /// </summary>
        /// <value>The type of the category.</value>
        public Type PluginType { get; set; }

        private IList<T> _plugins = new List<T>();

        /// <summary>
        /// Gets the rules.
        /// </summary>
        /// <value>The rules.</value>
        public IList<T> Plugins
        {
            get { return _plugins; }
        }


    }
}
