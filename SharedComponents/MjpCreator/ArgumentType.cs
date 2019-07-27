using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace MediaCenter.SharedComponents
{

    /// <summary>
    /// Argument for the DbPortTable program.
    /// </summary>
    public class ArgumentType
    {

        private string _name;
        private string _nameAbbriv;
        private string _value;

        private IList<string> _allowedValues = new List<string>();
        private IList<string> _otherNamesForbiddenWithThis = new List<string>();
        private IList<string> _otherNamesForbiddenWithoutThis = new List<string>();
        private IList<string> _otherNamesRequiredWithThis = new List<string>();
        private IList<string> _otherNamesRequiredWithoutThis = new List<string>();
        private IList<string> _values = new List<string>();


        /// <summary>
        /// Gets or sets the argument flag character list.
        /// </summary>
        /// <value>
        /// The argument flag character list.
        /// </value>
        public char[] FlagChars { get; private set; }

        /// <summary>
        /// Gets or sets the help for this argument.
        /// </summary>
        /// <value>
        /// The help for this argument.
        /// </value>
        public string Help { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether the argument is an option.
        /// </summary>
        /// <value>
        /// <c>false</c> if the argument is option; <c>true</c> if the argument is a command.
        /// Default: <c>false</c>.
        /// </value>
        public bool IsCommand { get; set; }

        /// <summary>
        /// Gets or sets the name of the argument.
        /// </summary>
        /// <value>
        /// The name of the argument.
        /// </value>
        public string Name
        {
            get { return _name; }
            set { _name = value.Trim(); }
        }

        /// <summary>
        /// Gets the normalized name used for comparisons.
        /// </summary>
        /// <value>
        /// The normalized name.
        /// </value>
        protected internal string NameNorm { get { return Name.Trim().ToLowerInvariant(); } }

        /// <summary>
        /// Gets or sets the abbriviated argument name.
        /// </summary>
        /// <value>
        /// The abbriviated argument name.
        /// </value>
        public string NameAbbriv
        {
            get { return _nameAbbriv; }
            set { _nameAbbriv = value.Trim(); }
        }

        /// <summary>
        /// Gets the name abbriv norm.
        /// </summary>
        /// <value>
        /// The name abbriv norm.
        /// </value>
        protected internal string NameAbbrivNorm { get { return NameAbbriv?.Trim().ToLowerInvariant(); } }

        /// <summary>
        /// Gets or sets the argument value.
        /// </summary>
        /// <value>
        /// The argument value.
        /// </value>
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }


        /// <summary>
        /// Gets or sets the allowed values.
        /// </summary>
        /// <value>
        /// The allowed values.
        /// </value>
        public IList<string> AllowedValues
        {
            get { return _allowedValues; }
            set
            {
                _allowedValues = value;
                for (var i = 0; i < _allowedValues.Count; i++)
                {
                    _allowedValues[i] = _allowedValues[i].Trim();
                }
            }
        }


        /// <summary>
        /// Gets or sets the list of other argument names forbidden when this argument is specified.
        /// </summary>
        /// <value>
        /// The list of other argument names forbidden.
        /// </value>
        public IList<string> OtherNamesForbiddenWithThis
        {
            get { return _otherNamesForbiddenWithThis; }
            set
            {
                _otherNamesForbiddenWithThis = value;
                for (var i = 0; i < _otherNamesForbiddenWithThis.Count; i++)
                {
                    _otherNamesForbiddenWithThis[i] = _otherNamesForbiddenWithThis[i].Trim();
                }
            }
        }

        /// <summary>
        /// Gets or sets the list of other argument names forbidden when this argument is not specified.
        /// </summary>
        /// <value>
        /// The list of other argument names forbidden.
        /// </value>
        public IList<string> OtherNamesForbiddenWithoutThis
        {
            get { return _otherNamesForbiddenWithoutThis; }
            set
            {
                _otherNamesForbiddenWithoutThis = value;
                for (var i = 0; i < _otherNamesForbiddenWithoutThis.Count; i++)
                {
                    _otherNamesForbiddenWithoutThis[i] = _otherNamesForbiddenWithoutThis[i].Trim();
                }
            }
        }

        /// <summary>
        /// Gets or sets the list of other argument names required when this argument is specified.
        /// </summary>
        /// <value>
        /// The list of other argument names required.
        /// </value>
        public IList<string> OtherNamesRequiredWithThis
        {
            get { return _otherNamesRequiredWithThis; }
            set
            {
                _otherNamesRequiredWithThis = value;
                for (var i = 0; i < _otherNamesRequiredWithThis.Count; i++)
                {
                    _otherNamesRequiredWithThis[i] = _otherNamesRequiredWithThis[i].Trim();
                }
            }
        }

        /// <summary>
        /// Gets or sets the list of other argument names required when this argument is not specified.
        /// </summary>
        /// <value>
        /// The list of other argument names required.
        /// </value>
        public IList<string> OtherNamesRequiredWithoutThis
        {
            get { return _otherNamesRequiredWithoutThis; }
            set
            {
                _otherNamesRequiredWithoutThis = value;
                for (var i = 0; i < _otherNamesRequiredWithoutThis.Count; i++)
                {
                    _otherNamesRequiredWithoutThis[i] = _otherNamesRequiredWithoutThis[i].Trim();
                }
            }
        }

        /// <summary>
        /// Gets or sets the argument value list.
        /// </summary>
        /// <value>
        /// The argument value list.
        /// </value>
        public IList<string> Values
        {
            get { return _values; }
            set { _values = value; }
        }

        /// <summary>
        /// Gets the argument value list as an appended string.
        /// </summary>
        /// <value>
        /// The argument value list string.
        /// </value>
        public string ValuesToString
        {
            get { return _values.ToStringAppended(); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether multiple values are allowed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if multiple values are allowed; otherwise, <c>false</c>.
        /// </value>
        public bool MultiValuesAllowed { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="Value"/> property value is required].
        /// </summary>
        /// <value>
        ///   <c>true</c> if the <see cref="Value"/> property value is required; otherwise, <c>false</c>.
        /// </value>
        public bool ValueRequired { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ArgumentType"/> is optional.
        /// </summary>
        /// <value>
        ///   <c>true</c> if optional; otherwise, <c>false</c>.
        /// </value>
        public bool Optional { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ArgumentType"/> is specified.
        /// </summary>
        /// <value>
        ///   <c>true</c> if specified; otherwise, <c>false</c>.
        /// </value>
        public bool Specified { get; private set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class. Default constructor.
        /// </summary>
        public ArgumentType()
        {
            FlagChars = new[] { '-', '/' };

            IsCommand = false;
            MultiValuesAllowed = false;
            Optional = false;
            Specified = false;
            ValueRequired = false;
        } // Default constructor


        /// <summary>
        /// Clones this argument instance.
        /// </summary>
        /// <returns>Copy of this argument.</returns>
        public ArgumentType Clone()
        {
            return Clone(this);
        } // Clone


        /// <summary>
        /// Clones the specified argument.
        /// </summary>
        /// <param name="arg">The argument.</param>
        /// <returns>Copy of argument.</returns>
        public static ArgumentType Clone(
            ArgumentType arg)
        {
            var ret = new ArgumentType()
                {
                    MultiValuesAllowed = arg.MultiValuesAllowed,
                    Help = arg.Help,
                    FlagChars = arg.FlagChars,
                    Name = arg.Name,
                    NameAbbriv = arg.NameAbbriv,
                    Optional = arg.Optional,
                    OtherNamesForbiddenWithThis = arg.OtherNamesForbiddenWithThis.Clone(),
                    OtherNamesForbiddenWithoutThis = arg.OtherNamesForbiddenWithoutThis.Clone(),
                    OtherNamesRequiredWithThis = arg.OtherNamesRequiredWithThis.Clone(),
                    OtherNamesRequiredWithoutThis = arg.OtherNamesRequiredWithoutThis.Clone(),
                    Specified = arg.Specified,
                    Value = arg.Value,
                    ValueRequired = arg.ValueRequired,
                    Values = (IList<string>)arg.Values.Clone()
                };

            return ret;
        } // Clone


        /// <summary>
        /// Parses an array of arguments and adjusts the argument list.
        /// </summary>
        /// <param name="args">The argument array.</param>
        /// <param name="argumentList">The input and result argument list.</param>
        /// <param name="checkRules">if set to <c>true</c> [check rules].</param>
        /// <exception cref="ArgumentNullException">argumentList - The input list should be initialized with fully populated with all possible arguments for the current application. The \"Specified\" properties are set to false initially.</exception>
        /// <exception cref="ArgumentException">Argument value is required but is empty.
        /// or
        /// Argument already used. MultiValuesAllowed not set.
        /// or
        /// Mandatory argument not specified.
        /// or
        /// Empty argument value not allowed.
        /// or
        /// or
        /// or
        /// or</exception>
        /// <exception cref="System.ArgumentNullException">argumentList - The input list should be initialized fully populated with all possible arguments for the current application. The \"Specified\" properties are set to false initially.</exception>
        /// <exception cref="System.ArgumentException">Argument value is required but is empty.
        /// or
        /// Argument already used. MultiValuesAllowed not set.
        /// or
        /// Mandatory argument not specified.
        /// or
        /// Empty argument value not allowed.
        /// or
        /// or
        /// or
        /// or</exception>
        /// <exception cref="T:System.ArgumentException"></exception>
        /// <remarks>
        /// If <see cref="MultiValuesAllowed" /> is <c>true</c>, <see cref="Value" /> is set with the first value.
        /// </remarks>
        public static void ParseArgumentList(
            string[] args,
            IList<ArgumentType> argumentList,
            bool checkRules = false)
        {
            if ((argumentList == null) || (!argumentList.Any()))
                throw new ArgumentNullException(
                    "argumentList",
                    "The input list should be initialized fully populated with all possible arguments for the current application. The \"Specified\" properties are set to false initially.");

            var argLen = args.Length;
            var argsLower = (string[]) args.Clone();

            argsLower = argsLower.Select(arg => arg.Trim().ToLower()).ToArray();

            // Check each input argument / value pair
            for (var i = 0; i < args.Length; i++)
            {
                var opt = argsLower[i];
                var val = (i < args.Length - 1) ? args[i + 1] : string.Empty;
                var valIsOption = ValueIsOption(val, argumentList[0].FlagChars);
                if (valIsOption)
                    val = string.Empty;

                // Only process this iteration when this argument is an option start, i.e. starts with a flag char - e.g. '-'
                if (ValueIsOption(opt, argumentList[0].FlagChars))
                    opt = opt.Substring(1); // Strip the flag char
                else
                    continue; // Skip the rest of this iteration

                // Find, check and fill arguments with values
                foreach (var arg in argumentList.Where(arg => (opt == arg.NameNorm) || (opt == arg.NameAbbrivNorm)))
                {
                    arg.Specified = true;

                    // Check for option as an option value
                    if (valIsOption && arg.ValueRequired && checkRules)
                        throw new ArgumentException(
                            string.Format("Argument value starts with a flag character: '{0}'.", val[0]),
                            arg.NameAbbriv + " | " + arg.Name);

                    // Check and set Value
                    if ((arg.ValueRequired) && (val.IsNullOrEmptyTrimmed()) && checkRules)
                        throw new ArgumentException("Argument value is required but is empty.", arg.NameAbbriv + " | " + arg.Name);

                    if (arg.Values.Count < 1)
                        arg.Value = val;

                    if (val.Length > 0)
                        arg.Values.Add(val);

                    // Set additional values after the same option start
                    if (!valIsOption)
                    {
                        var j = i + 1;
                        val = (j < args.Length - 1) ? args[j + 1] : string.Empty;
                        valIsOption = ValueIsOption(val, argumentList[0].FlagChars);
                        while (!valIsOption && (val.Length > 0))
                        {
                            arg.Values.Add(val);
                            j++;
                            val = (j < args.Length - 1) ? args[j + 1] : string.Empty;
                            valIsOption = ValueIsOption(val, argumentList[0].FlagChars);
                        }
                    }

                    // Check for illegal multiple values
                    if (!checkRules && !arg.MultiValuesAllowed && (arg.Values.Count > 1))
                        throw new ArgumentException("Argument already used. MultiValuesAllowed not set.", arg.NameAbbriv + " | " + arg.Name);
                }
            }

            if (!checkRules)
                return;

            foreach (var arg in argumentList)
            {
                // Check for all mandatory arguments present
                if (!arg.Optional && !arg.Specified)
                    throw new ArgumentException("Mandatory argument not specified.", arg.NameAbbriv + " | " + arg.Name);

                if (!arg.Optional && arg.ValueRequired && arg.Value.IsNullOrEmptyTrimmed())
                    throw new ArgumentException("Empty argument value not allowed.", arg.NameAbbriv + " | " + arg.Name);

                // Check for forbidden dependency arguments not present
                if (arg.Specified && !arg.OtherNamesForbiddenWithThis.IsNullOrEmpty())
                {
                    foreach (var oa in arg.OtherNamesForbiddenWithThis.Where(argumentList.ContainsSpecifiedName))
                    {
                        throw new ArgumentException(
                            string.Format("Forbidden dependency argument for \"{0}\" found: \"{1}\".", arg.Name, oa),
                            oa);
                    }
                }

                if (!arg.Specified && !arg.OtherNamesForbiddenWithoutThis.IsNullOrEmpty())
                {
                    foreach (var oa in arg.OtherNamesForbiddenWithoutThis.Where(argumentList.ContainsSpecifiedName))
                    {
                        throw new ArgumentException(
                            string.Format("Forbidden dependency argument for \"{0}\" found: \"{1}\".", arg.Name, oa),
                            oa);
                    }
                }

                // Check for required dependency arguments present
                if (arg.Specified && !arg.OtherNamesRequiredWithThis.IsNullOrEmpty())
                {
                    foreach (var oa in arg.OtherNamesRequiredWithThis.Where(oa => !argumentList.ContainsSpecifiedName(oa)))
                    {
                        throw new ArgumentException(
                            string.Format("Required dependency argument for \"{0}\" missing: \"{1}\".", arg.Name, oa),
                            oa);
                    }
                }

                if (!arg.Specified && !arg.OtherNamesRequiredWithoutThis.IsNullOrEmpty())
                {
                    foreach (var oa in arg.OtherNamesRequiredWithoutThis.Where(oa => !argumentList.ContainsSpecifiedName(oa)))
                    {
                        throw new ArgumentException(
                            string.Format("Required dependency argument for \"{0}\" missing: \"{1}\".", arg.Name, oa),
                            oa);
                    }
                }
            }
        } // ParseArgumentList


        /// <summary>
        /// Writes the argument syntax.
        /// </summary>
        /// <param name="argumentList">The argument list.</param>
        /// <param name="abbriviatedOptions">if set to <c>true</c>, use abbriviated option names; otherwise use the full names.</param>
        /// <returns></returns>
        public static string SyntaxText(
            IEnumerable<ArgumentType> argumentList,
            bool abbriviatedOptions)
        {
            const string fmt = "{0}{1}{2}";
            var ret = new StringBuilder();

            foreach (var arg in argumentList)
            {
                if (ret.Length > 0)
                    ret.Append("  ");

                if (arg.Optional)
                    ret.Append("[");

                var value = (arg.AllowedValues.IsNullOrEmpty())
                    ? " value"
                    : string.Format(" {0}", string.Join("|", arg.AllowedValues));

                if (abbriviatedOptions)
                {
                    ret.AppendFormat(
                        fmt,
                        ((arg.IsCommand) ? string.Empty : arg.FlagChars[0].ToString()), 
                        arg.NameAbbriv, 
                        (arg.ValueRequired) ? value : "");
                }
                else
                {
                    ret.AppendFormat(
                        fmt,
                        ((arg.IsCommand) ? string.Empty : arg.FlagChars[0].ToString()), 
                        arg.Name, 
                        (arg.ValueRequired) ? value : "");
                }

                if (arg.MultiValuesAllowed)
                    ret.Append(" [value ...]");

                if (arg.Optional)
                    ret.Append("]");
            }

            return ret.ToString();
        } // SyntaxText


        /// <summary>
        /// Writes the argument syntax details, including help.
        /// </summary>
        /// <param name="argumentList">The argument list.</param>
        public static string SyntaxTextDetails(
            IList<ArgumentType> argumentList)
        {
            var ret = new StringBuilder();
            var maxLen = argumentList.Select(x => x.Name?.Length ?? 0).Concat(new[] { 0 }).Max();
            var maxLenAbbriv = argumentList.Select(x => x.NameAbbriv?.Length ?? 0).Concat(new[] { 0 }).Max();
            var fmt = "{0," + maxLenAbbriv.ToString(CultureInfo.InvariantCulture) + "} | {1,-" + maxLen.ToString(CultureInfo.InvariantCulture) + "}  {2} : {3} {4} {5}";

            foreach (var arg in argumentList)
            {
                ret.AppendFormat(fmt,
                    arg.NameAbbriv,
                    arg.Name,
                    ((arg.ValueRequired) ? "value" : "     "),
                    arg.Help,
                    ((arg.Optional) ? "Optional." : string.Empty),
                    ((arg.MultiValuesAllowed) ? "Multiple values allowed." : string.Empty));
                ret.AppendLine();
            }

            return ret.ToString();
        } // SyntaxTextDetails


        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var ret = new StringBuilder();

            ret.AppendFormat(
                "Name=\"{0}|{1}\"; values=\"{2}\".",
                NameAbbriv, Name, Values.ToStringAppended());

            return ret.ToString();
        } // ToString


        /// <summary>
        /// Check if a string value is an option, i.e. starts with a flag character, e.g. '-', '/'.
        /// </summary>
        /// <param name="value">The string value.</param>
        /// <returns><c>true</c>, if the string value is an option; otherwise <c>false</c>.</returns>
        private bool ValueIsOption(
            string value)
        {
            return ValueIsOption(value, FlagChars);
        } // valueIsOption


        /// <summary>
        /// Check if a string value is an option, i.e. starts with a flag character, e.g. '-', '/'.
        /// </summary>
        /// <param name="value">The string value.</param>
        /// <param name="flagChars">The flag chars, e.g. '-', '/'.</param>
        /// <returns>
        ///   <c>true</c>, if the string value is an option; otherwise <c>false</c>.
        /// </returns>
        private static bool ValueIsOption(
            string value,
            IEnumerable<char> flagChars)
        {
            var ret = ((value.Length > 0) && flagChars.Contains(value[0]));

            return ret;
        } // valueIsOption char[] FlagChars


        #region Sorting

        /// <summary>
        /// Sort by the full argument name, case insensitively.
        /// </summary>
        public class SortByArgumentName : IComparer<ArgumentType>
        {

            /// <summary>
            /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
            /// </summary>
            /// <param name="x">The first object to compare.</param>
            /// <param name="y">The second object to compare.</param>
            /// <returns>
            /// Value
            /// Condition
            /// Less than zero
            /// <paramref name="x"/> is less than <paramref name="y"/>.
            /// Zero
            /// <paramref name="x"/> equals <paramref name="y"/>.
            /// Greater than zero
            /// <paramref name="x"/> is greater than <paramref name="y"/>.
            /// </returns>
            public int Compare(ArgumentType x,
                               ArgumentType y)
            {
                return string.CompareOrdinal(x.NameNorm, y.NameNorm);
            } // Compare

        } // class SortByArgumentName


        /// <summary>
        /// Sort by the abbriviated argument name, case insensitively.
        /// </summary>
        public class SortByArgumentNameAbbriv : IComparer<ArgumentType>
        {

            /// <summary>
            /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
            /// </summary>
            /// <param name="x">The first object to compare.</param>
            /// <param name="y">The second object to compare.</param>
            /// <returns>
            /// Value
            /// Condition
            /// Less than zero
            /// <paramref name="x"/> is less than <paramref name="y"/>.
            /// Zero
            /// <paramref name="x"/> equals <paramref name="y"/>.
            /// Greater than zero
            /// <paramref name="x"/> is greater than <paramref name="y"/>.
            /// </returns>
            public int Compare(ArgumentType x,
                               ArgumentType y)
            {
                return string.CompareOrdinal(x.NameAbbrivNorm, y.NameAbbrivNorm);
            } // Compare

        } // class SortByArgumentNameAbbriv

        #endregion Sorting

    }



    #region Extensions

    /// <summary>
    /// Extension methods to ArgumentType.
    /// </summary>
    public static class ArgumentExtensionType
    {

        /// <summary>
        /// Clones the specified string list.
        /// </summary>
        /// <param name="stringList">The string list.</param>
        /// <returns></returns>
        public static IList<string> Clone(
            this IEnumerable<string> stringList)
        {
            var ret = stringList.ToList();

            return ret;
        } // Clone


        /// <summary>
        /// Determines whether the specified arguments contains the name or the abbriviated name.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        /// <param name="name">The name or the abbriviated name.</param>
        /// <returns>
        ///   <c>true</c> if the specified arguments contains the name or the abbriviated name; otherwise, <c>false</c>.
        /// </returns>
        public static bool ContainsName(
            this IEnumerable<ArgumentType> arguments,
            string name)
        {
            var ret = false;

            name = name.Trim().ToLowerInvariant();

            ret = arguments.Any(arg => (arg.NameNorm == name) || (arg.NameAbbrivNorm == name));

            return ret;
        }


        /// <summary>
        /// Determines whether the specified arguments contains the name or the abbriviated name.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        /// <param name="name">The name or the abbriviated name.</param>
        /// <returns>
        ///   <c>true</c> if the specified arguments contains the name or the abbriviated name; otherwise, <c>false</c>.
        /// </returns>
        public static bool ContainsSpecifiedName(
            this IEnumerable<ArgumentType> arguments,
            string name)
        {
            var ret = false;

            name = name.Trim().ToLowerInvariant();

            ret = arguments.Any(arg => ((arg.NameNorm == name) || (arg.NameAbbrivNorm == name)) && arg.Specified);

            return ret;
        }


        /// <summary>
        /// Gets the argument by its name or its abbriviated name.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        /// <param name="name">The name or abbriviated name.</param>
        /// <returns>
        /// Argument found.
        /// </returns>
        public static ArgumentType GetByName(
            this IEnumerable<ArgumentType> arguments,
            string name)
        {
            name = name.Trim().ToLowerInvariant();

            var ret = arguments.FirstOrDefault(arg => (arg.NameNorm == name) || (arg.NameAbbrivNorm == name));

            return ret;
        } // GetByName


        /// <summary>
        /// Gets the value of the argument by its name or its abbriviated name.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        /// <param name="name">The name or abbriviated name.</param>
        /// <returns>
        /// Argument found.
        /// </returns>
        public static string GetValueOf(
            this IEnumerable<ArgumentType> arguments,
            string name)
        {
            var arg = GetByName(arguments, name);
            var ret = arg?.Value;

            return ret;
        }


        /// <summary>
        /// Gets the value list of the argument by its name or its abbriviated name.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        /// <param name="name">The name or abbriviated name.</param>
        /// <returns>
        /// Argument found.
        /// </returns>
        public static IList<string> GetValuesOf(
            this IEnumerable<ArgumentType> arguments,
            string name)
        {
            var arg = GetByName(arguments, name);
            var ret = arg?.Values;

            return ret;
        }


        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance. Appended list of values.
        /// </summary>
        /// <param name="strings">The strings.</param>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public static string ToStringAppended(
            this IEnumerable<string> strings)
        {
            var ret = new StringBuilder();

            foreach (var s in strings)
            {
                if (ret.Length > 0)
                    ret.Append(", ");

                ret.Append("'");
                ret.Append(s);
                ret.Append("'");
            }

            return ret.ToString();
        } // ToStringAppended

    } // class ArgumentExtensionType

    #endregion Extensions

}
