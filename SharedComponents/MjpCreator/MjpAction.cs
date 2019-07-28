using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MediaCenter.SharedComponents
{

    /// <summary>
    /// MJP Package element type.
    /// </summary>
    [Serializable]
    public class MjpAction
    {

        /// <summary>
        /// Gets or sets the command string.
        /// </summary>
        /// <value>
        /// The command.
        /// </value>
        [XmlElement(IsNullable = false, Order = 0)]
        public string Command { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="MjpAction"/> class.
        /// </summary>
        public MjpAction()
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="MjpAction"/> class.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <exception cref="System.ArgumentNullException">command</exception>
        public MjpAction(string command)
            : this()
        {
            Command = command ?? throw new ArgumentNullException(nameof(command));
        }


        /// <summary>
        /// Serializes this object INI style.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>
        /// INI style string representation of this object.
        /// </returns>
        public string SerializeIniStyle(int index)
        {
            var ret = new StringBuilder();

            ret.AppendLine($"Action{index + 1}={Command}");

            return ret.ToString();
        }

    }

}
