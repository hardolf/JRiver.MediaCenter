using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace MediaCenter.SharedComponents
{

    /// <summary>
    ///     Argument class.
    /// </summary>
    public class Argument
    {

#pragma warning disable 1591

        protected List<ArgumentType> ArgList { get; }

#pragma warning restore 1591


        /// <summary>
        ///     Initializes a new instance of the <see cref="System.Object" /> class.
        /// </summary>
        public Argument()
        {
            ArgList = new List<ArgumentType>()
                {
                    new ArgumentType()
                        {
                            Name = "Help",
                            NameAbbriv = "?",
                            Help = "Show the syntax message.",
                            Optional = true,
                        },
                    new ArgumentType()
                        {
                            Name = "Name",
                            NameAbbriv = "n",
                            Help = "Product name.",
                            Optional = false,
                            ValueRequired = true,
                        },
                    new ArgumentType()
                        {
                            Name = "Version",
                            NameAbbriv = "v",
                            Help = "Product version, numbers only (e.g. 1.2.2).",
                            Optional = false,
                            ValueRequired = true,
                        },
                    new ArgumentType()
                        {
                            Name = "URL",
                            NameAbbriv = "url",
                            Help = "URL to the source package file.",
                            Optional = false,
                            ValueRequired = true,
                        },
                    new ArgumentType()
                        {
                            Name = "DestinationFileDirectory",
                            NameAbbriv = "dd",
                            Help = "Full path to the MJP file destination directory.",
                            Optional = false,
                            ValueRequired = true,
                        },
                    new ArgumentType()
                        {
                            Name = "SourceFileDirectories",
                            NameAbbriv = "sd",
                            Help = "Comma or semicolon separated list of source file directories.",
                            Optional = true,
                            ValueRequired = true,
                        },
                    new ArgumentType()
                        {
                            Name = "ComRegisterFiles",
                            NameAbbriv = "com",
                            Help = "Comma or semicolon separated list of source files that must be registered in COM.",
                            Optional = true,
                            ValueRequired = true,
                        },
                };
        }


        /// <summary>
        ///     Gets or sets a value indicating whether [help required].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [help required]; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsHelpRequired { get; protected set; }

        /// <summary>
        ///     Checks the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns><c>false</c> if help is showed and execution therefore should stop; else <c>true</c>.</returns>
        /// <exception cref="SyntaxException"></exception>
        public virtual void Check(
            string[] args)
        {
            if (args is null) throw new ArgumentNullException(nameof(args));

            IsHelpRequired = (args.Length == 0)
                || (args.Any(arg => "?|HELP".Contains(arg.ToUpperInvariant().Substring(1))));

            try
            {
                ArgumentType.ParseArgumentList(args, ArgList, !IsHelpRequired);
            }
            catch (ArgumentException ex)
            {
                throw new SyntaxException(ex.Message);
            }
        }


        /// <summary>
        /// Gets the COM registered files.
        /// </summary>
        /// <value>
        /// The COM registered files.
        /// </value>
        public List<string> ComRegisterFiles
        {
            get
            {
                var tmp = ArgList.GetValueOf("ComRegisterFiles");
                var ret = tmp?.Split(',', ';').Select(x => x.Trim()).ToList() ?? new List<string>();

                return ret;
            }
        }


        /// <summary>
        ///     Gets the operation.
        /// </summary>
        /// <value>
        ///     The operation.
        /// </value>
        public string DestinationFileDirectory
        {
            get
            {
                var ret = ArgList.GetValueOf("DestinationFileDirectory");

                return ret;
            }
        }


        /// <summary>
        ///     Gets the command string.
        /// </summary>
        /// <returns></returns>
        public static string GetCommandString(
            string[] args)
        {
            if (args is null) throw new ArgumentNullException(nameof(args));

            var ret = new StringBuilder();

            ret.Append(AppDomain.CurrentDomain.FriendlyName);

            foreach (var arg in args)
            {
                ret.Append(" ");
                ret.Append(arg);
            }

            return ret.ToString();
        }


        /// <summary>
        ///     Gets the syntax.
        /// </summary>
        /// <returns></returns>
        public virtual string GetSyntax()
        {
            var ret = new StringBuilder();

            ret.AppendLine("Syntax:");
            ret.Append(AppDomain.CurrentDomain.FriendlyName);
            ret.AppendLine(" " + ArgumentType.SyntaxText(ArgList, true));
            ret.Append(AppDomain.CurrentDomain.FriendlyName);
            ret.AppendLine(" " + ArgumentType.SyntaxText(ArgList, false));
            ret.AppendLine();
            ret.AppendLine("Option names and values are case insensitive.");
            ret.AppendLine();
            ret.AppendLine("Argument details:");
            ret.Append(ArgumentType.SyntaxTextDetails(ArgList));

            return ret.ToString();
        }


        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get
            {
                var ret = ArgList.GetValueOf("Name");

                return ret;
            }
        }


        /// <summary>
        /// Gets the source file directories.
        /// </summary>
        /// <value>
        /// The source file directories.
        /// </value>
        public List<string> SourceFileDirectories
        {
            get
            {
                var tmp = ArgList.GetValueOf("SourceFileDirectories");
                var ret = tmp?.Split(',', ';').Select(x => x.Trim()).ToList() ?? new List<string>();

                return ret;
            }
        }


        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        public string Url
        {
            get
            {
                var ret = ArgList.GetValueOf("URL");

                return ret;
            }
        }


        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public string Version
        {
            get
            {
                var ret = ArgList.GetValueOf("Version");

                return ret;
            }
        }

    }

}