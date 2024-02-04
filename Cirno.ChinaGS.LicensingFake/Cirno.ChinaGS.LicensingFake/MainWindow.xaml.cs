using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Cirno.ChinaGS.LicensingFake
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Encrypt_Click(object sender, RoutedEventArgs e)
        {
            CryptoHelper helper = new CryptoHelper(License.Text);
            this.TextOutput.Text = helper.Encrypt(this.TextInput.Text);
        }

        private void LicensingSelect_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "License files (.lic)|*.lic|All files (*.*)|*.*"
            };

            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                License.Text = dialog.FileName;
            }
        }

        private void Verify_Click(object sender, RoutedEventArgs e)
        {
            CryptoHelper helper = new CryptoHelper(License.Text);
            bool result = helper.Verify(this.TextInput.Text, this.TextOutput.Text);
            if (result)
            {
                MessageBox.Show("Success!\n\n=>AddonSymbolicName: " + this.TextInput.Text +
                    "\n\n=> SignedAddonSymbolicName: " + this.TextOutput.Text, "Verify");
            }
            else
            {
                MessageBox.Show("Failed!\n\n=> AddonSymbolicName: " + this.TextInput.Text +
                    "\n\n=> SignedAddonSymbolicName: " + this.TextOutput.Text, "Verify");
            }
        }
    }
}
