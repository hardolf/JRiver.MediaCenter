using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

using MediaCenter.SharedComponents;


namespace MediaCenter.LyricsFinder
{

    /// <summary>
    /// Form to show the about box.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Form" />
    public partial class AboutBox : Form
    {

        Assembly _assembly = null;


        /// <summary>
        /// Initializes a new instance of the <see cref="AboutBox"/> class.
        /// </summary>
        public AboutBox(Assembly entryAssembly)
        {
            InitializeComponent();

            AllowTransparency = false;

            _assembly = entryAssembly ?? throw new ArgumentNullException(nameof(entryAssembly));

            Text = $"About {AssemblyTitle}";
            ProductNameLabel.Text = AssemblyProduct;
            VersionLabel.Text = $"Version {AssemblyVersion}";
            CopyrightLabel.Text = AssemblyCopyright;
            CompanyNameLabel.Text = AssemblyCompany;
            DescriptionTextBox.Text = AssemblyDescription;
        }


        /// <summary>
        /// Gets the assembly title.
        /// </summary>
        /// <value>
        /// The assembly title.
        /// </value>
        private string AssemblyTitle
        {
            get
            {
                object[] attributes = _assembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false);

                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];

                    if (!titleAttribute.Title.IsNullOrEmptyTrimmed())
                        return titleAttribute.Title;
                }

                return System.IO.Path.GetFileNameWithoutExtension(_assembly.CodeBase);
            }
        }


        /// <summary>
        /// Gets the assembly version.
        /// </summary>
        /// <value>
        /// The assembly version.
        /// </value>
        private string AssemblyVersion
        {
            get
            {
                return _assembly.GetName().Version.ToString();
            }
        }


        /// <summary>
        /// Gets the assembly description.
        /// </summary>
        /// <value>
        /// The assembly description.
        /// </value>
        private string AssemblyDescription
        {
            get
            {
                object[] attributes = _assembly.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);

                if (attributes.Length == 0)
                    return "";

                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }


        /// <summary>
        /// Gets the assembly product.
        /// </summary>
        /// <value>
        /// The assembly product.
        /// </value>
        private string AssemblyProduct
        {
            get
            {
                object[] attributes = _assembly.GetCustomAttributes(typeof(AssemblyProductAttribute), false);

                if (attributes.Length == 0)
                    return "";

                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }


        /// <summary>
        /// Gets the assembly copyright.
        /// </summary>
        /// <value>
        /// The assembly copyright.
        /// </value>
        private string AssemblyCopyright
        {
            get
            {
                object[] attributes = _assembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);

                if (attributes.Length == 0)
                    return "";

                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        /// <summary>
        /// Gets the assembly company.
        /// </summary>
        /// <value>
        /// The assembly company.
        /// </value>
        private string AssemblyCompany
        {
            get
            {
                object[] attributes = _assembly.GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);

                if (attributes.Length == 0)
                    return "";

                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }


        /// <summary>
        /// Handles the Click event of the CloseButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Handles the LinkClicked event of the ReleaseNotesLinkLabel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="LinkLabelLinkClickedEventArgs"/> instance containing the event data.</param>
        private void ReleaseNotesLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Navigate to a URL.
            System.Diagnostics.Process.Start("ReleaseNotes.html");
        }

    }

}
