using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

using MediaCenter.LyricsFinder.Model.Helpers;
using MediaCenter.SharedComponents;


namespace MediaCenter.LyricsFinder
{

    /// <summary>
    /// Form to show the about box.
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Form" />
    public partial class AboutBox : Form
    {

        private Assembly _assembly = null;

        private string _extraDescription = "\r\nThe LyricsFinder looks for lyrics in public lyric web services.\r\n"
            + "\r\nLookup is done for all - or just one at a time - of the current songs in the \"Playing Now\" list in the JRiver Media Center.\r\n"
            + "\r\nThe LyricsFinder can be used as a standalone program and/or as a plug-in for the JRiver Media Center.\r\n"
            + "\r\nThe found lyrics are saved in the songs' tags.";


        /// <summary>
        /// Initializes a new instance of the <see cref="AboutBox" /> class.
        /// </summary>
        /// <param name="entryAssembly">The entry assembly.</param>
        /// <exception cref="ArgumentNullException">entryAssembly</exception>
        public AboutBox(Assembly entryAssembly)
        {
            InitializeComponent();

            AllowTransparency = false;

            _assembly = entryAssembly ?? throw new ArgumentNullException(nameof(entryAssembly));
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
        /// Gets the assembly build date.
        /// </summary>
        /// <value>
        /// The assembly build date.
        /// </value>
        private string AssemblyBuildDate
        {
            get
            {
                var buildTime = _assembly.GetLinkerTime();

                return buildTime.ToLongDateString();
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
                var version = _assembly.GetName().Version;

                return $"{version.Major}.{version.Minor}.{version.Build}";
            }
        }


        /// <summary>
        /// Handles the Load event of the AboutBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void AboutBox_Load(object sender, EventArgs e)
        {
            try
            {
                Text = $"About {AssemblyTitle}";
                ProductNameLabel.Text = AssemblyProduct;
                VersionLabel.Text = $"Version {AssemblyVersion}";
                CopyrightLabel.Text = AssemblyCopyright;
                CompanyNameLabel.Text = AssemblyCompany;
                BuildDateLabel.Text = $"Build date {AssemblyBuildDate}";
                DescriptionTextBox.Text = AssemblyDescription + Environment.NewLine + _extraDescription;
            }
            catch (Exception ex)
            {
                ErrorHandling.ShowAndLogErrorHandler($"Error in {MethodBase.GetCurrentMethod().Name} event.", ex);
            }
        }


        /// <summary>
        /// Handles the Click event of the CloseButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void CloseButton_Click(object sender, EventArgs e)
        {
            try
            {
                Close();
            }
            catch (Exception ex)
            {
                ErrorHandling.ShowAndLogErrorHandler($"Error in {MethodBase.GetCurrentMethod().Name} event.", ex);
            }
        }


        /// <summary>
        /// Handles the LinkClicked event of the ReleaseNotesLinkLabel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="LinkLabelLinkClickedEventArgs"/> instance containing the event data.</param>
        private void LinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                var path = string.Empty;

                if (sender == ProjectLinkLabel)
                    path = Model.Helpers.Utility.RepositoryUrl.ToString();
                else if (sender == ReleaseNotesLinkLabel)
                    path = Path.Combine(Path.GetDirectoryName(_assembly.Location), "ReleaseNotes.html");

                // Navigate to a URL.
                System.Diagnostics.Process.Start(path);
            }
            catch (Exception ex)
            {
                ErrorHandling.ShowAndLogErrorHandler($"Error in {MethodBase.GetCurrentMethod().Name} event.", ex);
            }
        }


        /// <summary>
        /// Handles the Click event of the UpdateCheckButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void UpdateCheckButton_Click(object sender, EventArgs e)
        {
            try
            {
                Model.Helpers.Utility.UpdateCheckWithRetries(_assembly.GetName().Version, true);
            }
            catch (Exception ex)
            {
                ErrorHandling.ShowAndLogErrorHandler($"Error in {MethodBase.GetCurrentMethod().Name} event.", ex);
            }
        }

    }

}
