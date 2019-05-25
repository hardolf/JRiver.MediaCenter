using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MediaCenter.LyricsFinder.Model.Helpers
{

    /// <summary>
    /// Add-in custom resolve helper type.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    /// <remarks>
    /// <para>
    /// </para>
    /// Source: https://stackoverflow.com/questions/1682681/custom-config-section-could-not-load-file-or-assembly
    /// <para>
    /// Courtesy https://stackoverflow.com/users/27457/aj and https://stackoverflow.com/users/1600964/rileywhite
    /// </para>
    /// </remarks>
    public sealed class AddinCustomConfigResolveHelper : IDisposable
    {

        private Assembly AddinAssemblyContainingConfigSectionDefinition { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="AddinCustomConfigResolveHelper"/> class.
        /// </summary>
        /// <param name="addinAssemblyContainingConfigSectionDefinition">The addin assembly containing configuration section definition.</param>
        public AddinCustomConfigResolveHelper(
            Assembly addinAssemblyContainingConfigSectionDefinition)
        {
            Contract.Assert(addinAssemblyContainingConfigSectionDefinition != null);

            this.AddinAssemblyContainingConfigSectionDefinition =
                addinAssemblyContainingConfigSectionDefinition;

            AppDomain.CurrentDomain.AssemblyResolve +=
                this.ConfigResolveEventHandler;
        }


        /// <summary>
        /// Finalizes an instance of the <see cref="AddinCustomConfigResolveHelper"/> class.
        /// </summary>
        ~AddinCustomConfigResolveHelper()
        {
            this.Dispose(false);
        }


        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }


        private void Dispose(bool isDisposing)
        {
            if (isDisposing)
                AppDomain.CurrentDomain.AssemblyResolve -= this.ConfigResolveEventHandler;
        }


        private Assembly ConfigResolveEventHandler(object sender, ResolveEventArgs args)
        {
            // Often the name provided is partial...this will match full or partial naming
            if (this.AddinAssemblyContainingConfigSectionDefinition.FullName.Contains(args.Name))
            {
                return this.AddinAssemblyContainingConfigSectionDefinition;
            }

            return null;
        }

    }

}
