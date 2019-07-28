using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MediaCenter.SharedComponents
{

    /// <summary>
    /// MJP Package file type.
    /// </summary>
    [Serializable]
    public class MjpFileEntry
    {

        /// <summary>
        /// Gets or sets the file.
        /// </summary>
        /// <value>
        /// The file.
        /// </value>
        [XmlElement(IsNullable = false, Order = 0)]
        public string File { get; set; }

        /// <summary>
        /// Gets or sets the actions.
        /// </summary>
        /// <value>
        /// The actions.
        /// </value>
        [XmlElement("Action", IsNullable = false, Order = 1)]
        public List<MjpAction> Actions { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="MjpFileEntry"/> class.
        /// </summary>
        public MjpFileEntry()
        {
            Actions = new List<MjpAction>();
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="MjpFileEntry"/> class.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <exception cref="System.ArgumentNullException">file</exception>
        public MjpFileEntry(string file)
            : this()
        {
            File = file ?? throw new ArgumentNullException(nameof(file));
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

            ret.AppendLine($"File{index + 1}={File}");

            for (int i = 0; i < Actions.Count; i++)
            {
                var action = Actions[i];

                ret.Append($"File{index + 1}{action.SerializeIniStyle(i)}"); 
            }

            return ret.ToString();
        }

    }

}
