using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace MediaCenter.Plugin.Interface
{

    class Program
    {

        static void Main(string[] args)
        {
            var plugin = new MainInterface();
            var mediaCenterReference = new MediaCenter.MCAutomation();

            plugin.Init(mediaCenterReference);

            if (plugin.Enabled)
                MessageBox.Show("Test initialization of the MediaCenter plugin succeeded.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("Test initialization of the MediaCenter plugin failed.", "Test error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

    }

}
