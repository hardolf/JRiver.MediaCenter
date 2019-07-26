using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using MediaCenter.SharedComponents;


namespace MediaCenter.LyricsFinder.Model.LyricServices
{

    /// <summary>
    /// Service property to be displayed.
    /// </summary>
    [ComVisible(false)]
    [Serializable]
    public class DisplayProperty
    {

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Caption { get; set; }

        /// <summary>
        /// Gets or sets the property name.
        /// </summary>
        /// <value>
        /// The property name.
        /// </value>
        public string PropertyName { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public object Value { get; set; }

        /// <summary>
        /// Gets or sets the tool tips.
        /// </summary>
        /// <value>
        /// The tool tips.
        /// </value>
        public string ToolTips { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is readonly.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is readonly; otherwise, <c>false</c>.
        /// </value>
        public bool IsEditAllowed { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="DisplayProperty" /> class.
        /// </summary>
        /// <param name="caption">The caption.</param>
        /// <param name="value">The value.</param>
        /// <param name="toolTips">The tool tips.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="isEditAllowed">if set to <c>false</c>, the property is readonly; else <c>true</c>.</param>
        /// <exception cref="System.ArgumentNullException">caption
        /// or
        /// property
        /// or
        /// value</exception>
        public DisplayProperty(string caption, string value, string toolTips = null, string propertyName = null, bool isEditAllowed = false)
        {
            Caption = caption;
            Value = value?.LfToCrLf();
            ToolTips = toolTips;
            PropertyName = propertyName;
            IsEditAllowed = isEditAllowed;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="DisplayProperty" /> class.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="toolTips">The tool tips.</param>
        /// <param name="isEditAllowed">if set to <c>true</c> [is edit allowed].</param>
        /// <exception cref="ArgumentNullException">property</exception>
        public DisplayProperty(string propertyName, object value, string caption = null, string toolTips = null, bool isEditAllowed = false)
        {
            if (propertyName is null) throw new ArgumentNullException(nameof(propertyName));

            PropertyName = propertyName;
            Caption = caption ?? PropertyName;
            Value = value?.ToString().LfToCrLf() ?? string.Empty;
            ToolTips = toolTips;
            IsEditAllowed = isEditAllowed;
        }


        /// <summary>
        /// Gets the property value from the target object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="ignorePropertyNameError">if set to <c>true</c> ignore property name error; else throw an exception on this error.</param>
        /// <returns>String value of the property from the target object.</returns>
        /// <exception cref="System.ArgumentNullException">obj</exception>
        /// <exception cref="System.Exception">PropertyName</exception>
        public object GetPropertyValue(object obj, bool ignorePropertyNameError = false)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            if (!ignorePropertyNameError && PropertyName.IsNullOrEmptyTrimmed()) throw new Exception(nameof(PropertyName) + " is not initialized.");

            object ret = null;
            var isOk = false;
            var props = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in props)
            {
                if (prop.Name.Equals(PropertyName, StringComparison.InvariantCultureIgnoreCase))
                {
                    ret = prop.GetValue(obj);
                    isOk = true;
                    break;
                }
            }

            if (!ignorePropertyNameError && !isOk)
                throw new Exception($"The object has no \"{PropertyName}\" property.");

            return ret;
        }


        /// <summary>
        /// Sets the property value on the target object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="ignorePropertyNameError">if set to <c>true</c> ignore property name error; else throw an exception on this error.</param>
        /// <exception cref="System.ArgumentNullException">obj</exception>
        /// <exception cref="System.Exception">
        /// PropertyName
        /// or
        /// The object has no \"{PropertyName}\" property.
        /// </exception>
        public void SetPropertyValue(object obj, bool ignorePropertyNameError = false)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            if (!ignorePropertyNameError && PropertyName.IsNullOrEmptyTrimmed()) throw new Exception(nameof(PropertyName) + " is not initialized.");

            var isOk = false;
            var props = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in props)
            {
                if (prop.Name.Equals(PropertyName, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (prop.PropertyType == typeof(Uri))
                        prop.SetValue(obj, new UriBuilder(Value.ToString()).Uri);
                    else if ((prop.PropertyType == typeof(int))
                        && (int.TryParse(Value.ToString(), out var intVar)))
                        prop.SetValue(obj, intVar);
                    else
                        prop.SetValue(obj, Value);

                    isOk = true;
                    break;
                }
            }

            if (!ignorePropertyNameError && !isOk)
                throw new Exception($"The object has no \"{PropertyName}\" property.");
        }

    }



    /// <summary>
    /// Display properties extensions.
    /// </summary>
    public static class DisplayPropertiesExtensions
    {

        /// <summary>
        /// Adds the specified property to the display properties dictionary.
        /// </summary>
        /// <param name="displayProperties">The display properties dictionary.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="toolTips">The tool tips.</param>
        /// <param name="isEditAllowed">if set to <c>true</c> [is edit allowed].</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">displayProperties</exception>
        public static DisplayProperty Add(this Dictionary<string, DisplayProperty> displayProperties, string propertyName, object value, string caption = null, string toolTips = null, bool isEditAllowed = false)
        {
            if (displayProperties is null) throw new ArgumentNullException(nameof(displayProperties));

            var ret = new DisplayProperty(propertyName, value, caption, toolTips, isEditAllowed);

            displayProperties.Add(ret.PropertyName, ret);

            return ret;
        }

    }

}
