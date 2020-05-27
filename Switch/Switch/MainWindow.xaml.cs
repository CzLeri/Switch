using IPSwitch;
using System.Collections;
using System.Windows;
using System.Windows.Controls;


namespace Switch
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string ipAddress;
        private string subnet;
        bool checkboxDHCP;
        bool Running = false;

        public MainWindow()
        {
            InitializeComponent();
            afterStartApp();
        }

        private void btnActivate_Click(object sender, RoutedEventArgs e)
        {
            if (chkDHCP.IsChecked == true)
            {
                NetAdapt.SetDHCP(cboNIC.SelectedItem.ToString());
            }
            else
            {
                if(checkSyntax(txIP.Text) && checkSyntax(txSubnet.Text))
                    NetAdapt.SetIP(txIP.Text, txSubnet.Text, cboNIC.SelectedItem.ToString());
            }
            ReadData();
        }
        private void afterStartApp()
        {
            

            ArrayList nicNames = NetAdapt.GetNICNames();
            cboNIC.Items.Clear();
            foreach (string nic in nicNames)
            {
                cboNIC.Items.Add(nic);
            }
            if (cboNIC.Items.Count > 0)
            {
                cboNIC.SelectedIndex = 0;
                ipAddress = NetAdapt.GetIp(cboNIC.Text);
                txIP.Text = ipAddress;
                subnet = NetAdapt.GetMask(cboNIC.Text);
                txSubnet.Text = subnet;
                checkboxDHCP = NetAdapt.CheckDHCP(cboNIC.Text);
                chkDHCP.IsChecked = checkboxDHCP;

                
                Running = true;
            }

               
        }

        private void cboNIC_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(Running)
            {
                ReadData();
            }
            
        }

        private void chkDHCP_Click(object sender, RoutedEventArgs e)
        {
            if(chkDHCP.IsChecked==true)
            {
                txIP.IsEnabled = false;
                txSubnet.IsEnabled = false;
            }
            else
            {
                txIP.IsEnabled = true;
                txSubnet.IsEnabled = true;
            }
            
        }
        private bool checkSyntax (string text)
        {
            bool okFormat = false;
            string[] data = text.Split('.');
            int[] pomoc = new int[4];
            for(int i = 0; i < 4;i++)
            {
                
                try
                {
                    pomoc[i] = int.Parse(data[i]);
                    if (pomoc[i] >= 0 && pomoc[i] <= 255)
                        okFormat = true;
                    else
                    {
                        okFormat = false;
                        break;
                    }
                }
                catch
                {
                    okFormat = false;
                }                                         
           }
            return okFormat;
        }
        public void ReadData()
        {
            ipAddress = NetAdapt.GetIp(cboNIC.SelectedItem.ToString());
            txIP.Text = ipAddress;
            subnet = NetAdapt.GetMask(cboNIC.SelectedItem.ToString());
            txSubnet.Text = subnet;
            checkboxDHCP = NetAdapt.CheckDHCP(cboNIC.SelectedItem.ToString());
            chkDHCP.IsChecked = checkboxDHCP;
            if (checkboxDHCP)
            {
                txIP.IsEnabled = false;
                txSubnet.IsEnabled = false;
            }
            else
            {
                txIP.IsEnabled = true;
                txSubnet.IsEnabled = true;
            }
        }
    }
}
