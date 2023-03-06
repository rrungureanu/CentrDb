using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

namespace CentricaWPF.Windows
{
    /// <summary>
    /// Interaction logic for AddSalespersonNewWindow.xaml
    /// </summary>
    public partial class AddSalespersonNewWindow : Window
    {
        private int m_districtId;

        public AddSalespersonNewWindow(int districtId)
        {
            m_districtId = districtId;
            InitializeComponent();
        }

        private async Task AddNewSalespersonToDistrict()
        {
            using (HttpClient client = new HttpClient())
            {
                var jObject = "";
                HttpContent httpContent = new StringContent(jObject.ToString());
                var response = await client.PostAsync("https://localhost:7028/api/centrica/AddNewSalespersonToDistrict/" +
                    TBFamilyName.Text + "/" + TBFirstName.Text + "/" + TBPhone.Text + "/" +
                    TBEmail.Text + "/" + m_districtId.ToString() + "/" + (CheckBoxIsPrimary.IsChecked == true).ToString(), httpContent);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Success");
                    // Refresh main salesperson list after adding salesperson
                    ((MainWindow)this.Owner.Owner).PopulateLists();
                }
                else
                {
                    Console.WriteLine("Fail");
                }
            }
        }

        private async void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(TBFamilyName.Text) && !string.IsNullOrEmpty(TBFirstName.Text) &&
                !string.IsNullOrEmpty(TBPhone.Text) && !string.IsNullOrEmpty(TBEmail.Text))
            {
                await AddNewSalespersonToDistrict();
                Close();
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
