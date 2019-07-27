﻿using System;
using System.Collections.Generic;
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
        ///     Initializes a new instance of the <see cref="T:System.Object" /> class.
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
                            Name = "DestinationFilePath",
                            NameAbbriv = "dp",
                            Help = "Full path to the destination file.",
                            Optional = false,
                            ValueRequired = true,
                        },
                    new ArgumentType()
                        {
                            Name = "SourceFileDirectories",
                            NameAbbriv = "sd",
                            Help = "Comma or semicolon separated list of source file directories.",
                            Optional = false,
                            ValueRequired = true,
                        },
                    new ArgumentType()
                        {
                            Name = "ComRegisterFiles",
                            NameAbbriv = "com",
                            Help = "Comma or semicolon separated list of source files that must be registered in COM.",
                            Optional = false,
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
            IsHelpRequired = (args.Length == 0)
                || (args.Any(arg => "?|help".Contains(arg.ToLower().Substring(1))));

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
                var ret = tmp.Split(',', ';').Select(x => x.Trim()).ToList();

                return ret;
            }
        }


        /// <summary>
        ///     Gets the operation.
        /// </summary>
        /// <value>
        ///     The operation.
        /// </value>
        public string DestinationFilePath
        {
            get
            {
                var ret = ArgList.GetValueOf("DestinationFilePath");

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
                var ret = tmp.Split(',', ';').Select(x => x.Trim()).ToList();

                return ret;
            }
        }

    }

}