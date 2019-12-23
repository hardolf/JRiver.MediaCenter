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
        /// Gets or sets the name of the alternative object.
        /// </summary>
        /// <value>
        /// The name of the alternative object.
        /// </value>
        public string AlternativeObjectName { get; set; }

        /// <summary>
        /// Gets or sets the name of the alternative property.
        /// </summary>
        /// <value>
        /// The name of the alternative property.
        /// </value>
        public string AlternativePropertyName { get; set; }

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
        /// Gets or sets the value of the property.
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
        /// <param name="propertyName">Name of the property in the configuration file and in the LyricsFinderData.</param>
        /// <param name="value">The value of the property.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="toolTips">The tool tips.</param>
        /// <param name="isEditAllowed">if set to <c>true</c> [is edit allowed].</param>
        /// <param name="defaultValue">The default value of the property. Used if the <c>value</c> assignment fails.</param>
        /// <exception cref="ArgumentNullException">property</exception>
        public DisplayProperty(string propertyName, object value, string caption = null, string toolTips = null, bool isEditAllowed = false, object defaultValue = null)
        {
            if (propertyName is null) throw new ArgumentNullException(nameof(propertyName));

            PropertyName = propertyName;
            Caption = caption ?? PropertyName;
            ToolTips = toolTips;
            IsEditAllowed = isEditAllowed;

            try
            {
                Value = value?.ToString().LfToCrLf() ?? string.Empty;
            }
            catch
            {
                Value = defaultValue?.ToString().LfToCrLf() ?? string.Empty;
            }
        }


        /// <summary>
        /// Gets the property value from the target object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="ignorePropertyNameError">if set to <c>true</c> ignore property name error; else throw an exception on this error.</param>
        /// <returns>String value of the property from the target object.</returns>
        /// <exception cref="System.ArgumentNullException">obj</exception>
        /// <exception cref="System.Exception">PropertyName</exception>
        public virtual object GetPropertyValue(object obj, bool ignorePropertyNameError = false)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            if (!ignorePropertyNameError && PropertyName.IsNullOrEmptyTrimmed()) throw new Exception(nameof(PropertyName) + " is not initialized.");
            if (AlternativeObjectName.IsNullOrEmptyTrimmed() ^ AlternativePropertyName.IsNullOrEmptyTrimmed()) throw new Exception($"None or both of {nameof(AlternativeObjectName)} and {nameof(AlternativePropertyName)} must be initialized.");

            object ret = null;
            var isOk = false;
            var props = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            if (AlternativeObjectName.IsNullOrEmptyTrimmed() || AlternativePropertyName.IsNullOrEmptyTrimmed())
            {
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
            }
            else
            {
                object altObj = null;

                foreach (var prop in props)
                {
                    if (prop.Name.Equals(AlternativeObjectName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        altObj = prop.GetValue(obj);
                        isOk = true;
                        break;
                    }
                }

                if ((!ignorePropertyNameError && !isOk)
                    || (altObj == null))
                    throw new Exception($"The object has no \"{AlternativeObjectName}\" property as alternative object.");

                isOk = false;
                props = altObj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (var prop in props)
                {
                    if (prop.Name.Equals(AlternativePropertyName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        ret = prop.GetValue(altObj);
                        isOk = true;
                        break;
                    }
                }

                if (!ignorePropertyNameError && !isOk)
                    throw new Exception($"The \"{AlternativeObjectName}\" property object has no \"{AlternativePropertyName}\" property.");
            }

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
        public virtual void SetPropertyValue(object obj, bool ignorePropertyNameError = false)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            if (!ignorePropertyNameError && PropertyName.IsNullOrEmptyTrimmed()) throw new Exception(nameof(PropertyName) + " is not initialized.");
            if (AlternativeObjectName.IsNullOrEmptyTrimmed() ^ AlternativePropertyName.IsNullOrEmptyTrimmed()) throw new Exception($"None or both of {nameof(AlternativeObjectName)} and {nameof(AlternativePropertyName)} must be initialized.");

            var isOk = false;
            var props = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            if (AlternativeObjectName.IsNullOrEmptyTrimmed() || AlternativePropertyName.IsNullOrEmptyTrimmed())
            {
                foreach (var prop in props)
                {
                    if (prop.Name.Equals(PropertyName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (prop.PropertyType == typeof(Uri))
                            prop.SetValue(obj, new UriBuilder(Value.ToString()).Uri);
                        else if ((prop.PropertyType == typeof(int))
                            && (int.TryParse(Value.ToString(), out var intVar)))
                            prop.SetValue(obj, intVar);
                        else if (prop.PropertyType == typeof(DateTime))
                        {
                            var s = Value.ToString().Replace("-", string.Empty).Replace("  ", " ");
                            var i = s.LastIndexOf(':');
                            var j = s.LastIndexOf('.');

                            if (j < 0)
                                j = s.LastIndexOf(',');

                            if (j > i)
                                s = s.Substring(j);

                            if (DateTime.TryParse(s, out var dtVar))
                                prop.SetValue(obj, dtVar);
                        }
                        else
                            prop.SetValue(obj, Value);

                        isOk = true;
                        break;
                    }
                }

                if (!ignorePropertyNameError && !isOk)
                    throw new Exception($"The object has no \"{PropertyName}\" property.");
            }
            else
            {
                object altObj = null;

                foreach (var prop in props)
                {
                    if (prop.Name.Equals(AlternativeObjectName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        altObj = prop.GetValue(obj);
                        isOk = true;
                        break;
                    }
                }

                if ((!ignorePropertyNameError && !isOk)
                    || (altObj == null))
                    throw new Exception($"The object has no \"{AlternativeObjectName}\" property as alternative object.");

                isOk = false;
                props = altObj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (var prop in props)
                {
                    if (prop.Name.Equals(AlternativePropertyName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (prop.PropertyType == typeof(Uri))
                            prop.SetValue(altObj, new UriBuilder(Value.ToString()).Uri);
                        else if ((prop.PropertyType == typeof(int))
                            && (int.TryParse(Value.ToString(), out var intVar)))
                            prop.SetValue(altObj, intVar);
                        else if (prop.PropertyType == typeof(DateTime))
                        {
                            var s = Value.ToString().Replace("-", string.Empty).Replace("  ", " ");

                            if (DateTime.TryParse(s, out var dtVar))
                                prop.SetValue(altObj, dtVar);
                        }
                        else
                            prop.SetValue(altObj, Value);

                        isOk = true;
                        break;
                    }
                }

                if (!ignorePropertyNameError && !isOk)
                    throw new Exception($"The \"{AlternativeObjectName}\" property object has no \"{AlternativePropertyName}\" property.");
            }

            // Check if the setting may be read again
            GetPropertyValue(obj, ignorePropertyNameError);
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
        /// <param name="propertyName">Name of the property in the configuration file and in the LyricsFinderData.</param>
        /// <param name="value">The value of the property.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="toolTips">The tool tips.</param>
        /// <param name="isEditAllowed">if set to <c>true</c> [is edit allowed].</param>
        /// <param name="defaultValue">The default value of the property. Used if the <c>value</c> assignment fails.</param>
        /// <returns>The added <c>DisplayProperty</c> object.</returns>
        /// <exception cref="ArgumentNullException">displayProperties</exception>
        public static DisplayProperty Add(this Dictionary<string, DisplayProperty> displayProperties,
            string propertyName, object value, string caption = null, string toolTips = null, bool isEditAllowed = false, object defaultValue = null)
        {
            if (displayProperties is null) throw new ArgumentNullException(nameof(displayProperties));

            var ret = new DisplayProperty(propertyName, value, caption, toolTips, isEditAllowed, defaultValue);

            displayProperties.Add(ret.PropertyName, ret);

            return ret;
        }


        /// <summary>
        /// Adds the specified property to the display properties dictionary.
        /// </summary>
        /// <param name="displayProperties">The display properties dictionary.</param>
        /// <param name="propertyName">Name of the property in the configuration file and in the LyricsFinderData.</param>
        /// <param name="alternativeObjectName">Name of the alternative object.</param>
        /// <param name="alternativePropertyName">Name of the alternative property.</param>
        /// <param name="value">The value of the property.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="toolTips">The tool tips.</param>
        /// <param name="isEditAllowed">if set to <c>true</c> [is edit allowed].</param>
        /// <param name="defaultValue">The default value of the property. Used if the <c>value</c> assignment fails.</param>
        /// <returns>The added <c>DisplayProperty</c> object.</returns>
        /// <exception cref="ArgumentNullException">displayProperties</exception>
        public static DisplayProperty Add(this Dictionary<string, DisplayProperty> displayProperties,
            string propertyName, string alternativeObjectName, string alternativePropertyName, object value, string caption = null, string toolTips = null, bool isEditAllowed = false, object defaultValue = null)
        {
            if (displayProperties is null) throw new ArgumentNullException(nameof(displayProperties));

            var ret = new DisplayProperty(propertyName, value, caption, toolTips, isEditAllowed, defaultValue)
            {
                AlternativeObjectName = alternativeObjectName,
                AlternativePropertyName = alternativePropertyName
            };

            displayProperties.Add(ret.PropertyName, ret);

            return ret;
        }

    }

}
