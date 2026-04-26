using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace MD.Tools.Helpers.Core
{

    /// <summary>
    /// Arguments class used to parse command line arguments
    /// </summary>
    public class Arguments
    {
        // Variables

        private StringDictionary _parameters;

        // Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Arguments"/> class.
        /// </summary>
        /// <param name="args">The args.</param>
        public Arguments(string[] args)
        {
            if (args is null)
            {
                throw new System.ArgumentNullException(nameof(args));
            }

            _parameters = new StringDictionary();
            Regex Spliter = new Regex(@"^-{1,2}|^/|=|:",
                RegexOptions.IgnoreCase | RegexOptions.Compiled);

            Regex Remover = new Regex(@"^['""]?(.*?)['""]?$",
                RegexOptions.IgnoreCase | RegexOptions.Compiled);

            string Parameter = null;
            string[] Parts;

            // Valid parameters forms:

            // {-,/,--}param{ ,=,:}((",')value(",'))

            // Examples: 

            // -param1 value1 --param2 /param3:"Test-:-work" 

            //   /param4=happy -param5 '--=nice=--'

            foreach (string Txt in args)
            {
                // Look for new parameters (-,/ or --) and a

                // possible enclosed value (=,:)

                Parts = Spliter.Split(Txt, 3);

                switch (Parts.Length)
                {
                    // Found a value (for the last parameter 

                    // found (space separator))

                    case 1:
                        if (Parameter != null)
                        {
                            if (!_parameters.ContainsKey(Parameter))
                            {
                                Parts[0] =
                                    Remover.Replace(Parts[0], "$1");

                                _parameters.Add(Parameter, Parts[0]);
                            }
                            Parameter = null;
                        }
                        // else Error: no parameter waiting for a value (skipped)

                        break;

                    // Found just a parameter

                    case 2:
                        // The last parameter is still waiting. 

                        // With no value, set it to true.

                        if (Parameter != null)
                        {
                            if (!_parameters.ContainsKey(Parameter))
                                _parameters.Add(Parameter, "true");
                        }
                        Parameter = Parts[1];
                        break;

                    // Parameter with enclosed value

                    case 3:
                        // The last parameter is still waiting. 

                        // With no value, set it to true.

                        if (Parameter != null)
                        {
                            if (!_parameters.ContainsKey(Parameter))
                                _parameters.Add(Parameter, "true");
                        }

                        Parameter = Parts[1];

                        // Remove possible enclosing characters (",')

                        if (!_parameters.ContainsKey(Parameter))
                        {
                            Parts[2] = Remover.Replace(Parts[2], "$1");
                            _parameters.Add(Parameter, Parts[2]);
                        }

                        Parameter = null;
                        break;
                }
            }
            // In case a parameter is still waiting

            if (Parameter != null)
            {
                if (!_parameters.ContainsKey(Parameter))
                    _parameters.Add(Parameter, "true");
            }
        }

        // Retrieve a parameter value if it exists 

        // (overriding C# indexer property)

        /// <summary>
        /// Gets the <see cref="System.String"/> with the specified param.
        /// </summary>
        /// <value></value>
        public string this[string key]
        {
            get
            {
                if (!_parameters.ContainsKey(key)) return string.Empty;
                return (_parameters[key]);
            }
        }
    }
}
