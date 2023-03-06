using CentricaAPI.Entities;
using Newtonsoft.Json.Linq;
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
    /// Interaction logic for AddSalespersonExistingWindow.xaml
    /// </summary>
    public partial class AddSalespersonExistingWindow : Window
    {
        private int m_districtId;

        public AddSalespersonExistingWindow(int districtId)
        {
            m_districtId = districtId; 
            PopulateSalespersonsList(m_districtId);
            InitializeComponent();
        }

        private async void PopulateSalespersonsList(int districtId)
        {
            using (HttpClient client = new HttpClient())
            {

                var response = await client.GetAsync("https://localhost:7028/api/centrica/GetSalespersonsNotInDistrict/" + districtId.ToString());

                if (response.IsSuccessStatusCode)
                {
                    List<Salesperson> salespersons = await response.Content.ReadAsAsync<List<Salesperson>>();
                    salespersons.ForEach(salesperson => ListAvailableSalespersons.Items.Add(salesperson));
                }
                else
                {
                    // TODO add warning msg for returned null
                }
            }
        }
        private async Task AppendSalespersonToDistrict()
        {
            using (HttpClient client = new HttpClient())
            {
                Salesperson salespersonSelected = (Salesperson)ListAvailableSalespersons.SelectedItem;
                var jObject = "";
                HttpContent httpContent = new StringContent(jObject.ToString());
                var response = await client.PutAsync("https://localhost:7028/api/centrica/AppendSalespersonToDistrict/" +
                    salespersonSelected.SalespersonId.ToString() + "/" + m_districtId.ToString() + "/" + (CheckBoxIsPrimary.IsChecked == true).ToString(), httpContent);

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
            if (ListAvailableSalespersons.SelectedItems.Count > 0)
            {
                
                await AppendSalespersonToDistrict();
                Close();
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
