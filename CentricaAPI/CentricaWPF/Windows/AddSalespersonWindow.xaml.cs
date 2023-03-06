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
using System.Windows.Shapes;
using CentricaWPF.Windows;

namespace CentricaWPF.Windows
{
    /// <summary>
    /// Interaction logic for AddSalespersonWindow.xaml
    /// </summary>
    public partial class AddSalespersonWindow : Window
    {
        private int m_districtId;

        public AddSalespersonWindow(int districtId)
        {
            m_districtId = districtId;
            InitializeComponent();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonAddExisting_Click(object sender, RoutedEventArgs e)
        {
            AddSalespersonExistingWindow addSalespersonExistingWindow = new AddSalespersonExistingWindow(m_districtId);
            addSalespersonExistingWindow.Owner = this;
            addSalespersonExistingWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            addSalespersonExistingWindow.ShowDialog();
            this.Close();
        }

        private void ButtonAddNew_Click(object sender, RoutedEventArgs e)
        {
            AddSalespersonNewWindow addSalespersonNewWindow = new AddSalespersonNewWindow(m_districtId);
            addSalespersonNewWindow.Owner = this;
            addSalespersonNewWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            addSalespersonNewWindow.ShowDialog();
            this.Close();
        }

    }
}
