using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class FormMain : Form
    {

        bool showAllPorts = true;
        string[] ShimmerComPorts = null;

        public FormMain()
        {
            InitializeComponent();
            populateCOMPortComboBox();
        }


        private void populateCOMPortComboBox()
        {
            comboBoxComPorts.Items.Clear();
            comboBoxComPorts.ResetText();
            //            comboBoxComPorts.Text = "";

            // Set COM port list filter option: BSL, UART, ALL SHIMMER, ALL
            ShimmerDevices.PortFilterOption portFilterOption;
            if (showAllPorts)
            {
                portFilterOption = ShimmerDevices.PortFilterOption.All;
            }
            else
            {
                portFilterOption = ShimmerDevices.PortFilterOption.ShimmerAllDocks;
            }

            // Get list of COM ports
            ShimmerComPorts = ShimmerDevices.GetComPorts(portFilterOption);

            // Local filter to pick out BSL port. Include here so that the ShimmerComPorts function will return all 
            // Shimmer Dock ports for future functionality - not implemented yet.
            if (showAllPorts)
            {
                comboBoxComPorts.Items.AddRange(ShimmerComPorts);
            }
            else
            {
                comboBoxComPorts.Items.AddRange(ShimmerComPorts.Where(val => val.Contains("BSL")).ToArray());
            }

            comboBoxComPorts.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBoxComPorts.AutoCompleteSource = AutoCompleteSource.ListItems;

            // if combobox contains BSL, set index to first occuring value. 
            // if not, set to first item index. 
            int BslIndex = -1;
            bool BslFound = false;
            for (BslIndex = 0; BslIndex < comboBoxComPorts.Items.Count; BslIndex++)
            {
                if (comboBoxComPorts.Items[BslIndex].ToString().Contains("BSL"))
                {
                    BslFound = true;
                    break;
                }
            }

            if (BslFound.Equals(true))
            {
                comboBoxComPorts.SelectedIndex = BslIndex;
            }
            else
            {
                if (comboBoxComPorts.Items.Count > 0)
                {
                    comboBoxComPorts.SelectedIndex = 0;
                }
            }

            string selectedComPort = comboBoxComPorts.Text.Split(new string[] { " - " }, StringSplitOptions.None)[0];


            //if ((comboBoxComPorts.SelectedItem.ToString().Contains("Shimmer Dock")) && (comboBoxComPorts.SelectedItem.ToString().Contains("BSL")))
            //{
            //    //initiateDockUI(true);
            //    initiateTimerDockPing(true);
            //    this.picbxShimmerDock.Image = BootstrapLoader.Properties.Resources.Shimmer_Programming_Dock_Connected_68x55;
            //}
            //else
            //{
            //    //initiateDockUI(false);
            //    initiateTimerDockPing(false);
            //    this.picbxShimmerDock.Image = BootstrapLoader.Properties.Resources.Shimmer_Programming_Dock_Not_Connected_68x55;
            //}


            //tbScripter1.Text = selectedComPort;

            // Not working in Windows 8
            //comboBoxComPorts.Items.Add("Reload COM Ports...");

            //            tbScripter1.Text = "";
            //            string[] ShimmerDrives = ShimmerDevices.GetShimmerDrives();
        }
    }
}
