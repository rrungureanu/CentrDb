using CentricaAPI.Entities;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.Json;
using System.Text.Json.Serialization;
using CentricaWPF.Windows;

namespace CentricaWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            PopulateDistrictsList();
            InitializeComponent();
        }

        private async void PopulateDistrictsList()
        {
            using (HttpClient client = new HttpClient())
            {

                var response = await client.GetAsync("https://localhost:7028/api/centrica/");

                if (response.IsSuccessStatusCode)
                {
                    List<District> districts = await response.Content.ReadAsAsync<List<District>>();
                    districts.ForEach(district => ListDistricts.Items.Add(district));
                }
                else
                {
                    // TODO add warning msg for returned null
                }
            }
        }

        private async Task<District> GetSalespersonsAndStoresByDistrictId(int districtId)
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync("https://localhost:7028/api/centrica/GetSalespersonsAndStoresByDistrictId/" + districtId.ToString());

                if (response.IsSuccessStatusCode)
                {
                    var district = await response.Content.ReadAsAsync<District>();
                    return district;
                }
                else
                {
                    // TODO add warning msg for returned null
                    return null;
                }
            }
        }

        public async void PopulateLists()
        {
            ListStores.Items.Clear();
            ListSalespersons.Items.Clear();

            District districtSelected = (District)ListDistricts.SelectedItem;
            District district = await GetSalespersonsAndStoresByDistrictId(districtSelected.DistrictId);
            if (district != null)
            {
                district.Stores.ForEach(store => ListStores.Items.Add(store));
                district.Salespersons.ForEach(salesperson => ListSalespersons.Items.Add(salesperson));
            }
        }

        private async void ListDistricts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PopulateLists();
        }

        private async Task RemoveSalespersonFromDistrict(int salespersonId, int districtId)
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.DeleteAsync("https://localhost:7028/api/centrica/RemoveSalespersonFromDistrict/" +
                    salespersonId.ToString() + "/" + districtId.ToString());

                if (response.IsSuccessStatusCode)
                {
                    PopulateLists();
                    Console.WriteLine("Success");
                }
                else
                {
                    Console.WriteLine("Fail");
                }
            }
        }

        private async void ButtonRemoveSalesperson_Click(object sender, RoutedEventArgs e)
        {
            if (ListSalespersons.SelectedItems.Count > 0 && ListDistricts.SelectedItems.Count > 0)
            {
                Salesperson salespersonSelected = (Salesperson)ListSalespersons.SelectedItem;
                District districtSelected = (District)ListDistricts.SelectedItem;

                await RemoveSalespersonFromDistrict(salespersonSelected.SalespersonId, districtSelected.DistrictId);
            }
        }

        private void Button_AddSalespersonClick(object sender, RoutedEventArgs e)
        {
            District districtSelected = (District)ListDistricts.SelectedItem;

            if (districtSelected != null)
            {
                AddSalespersonWindow addSalespersonWindow = new AddSalespersonWindow(districtSelected.DistrictId);
                addSalespersonWindow.Owner = this;
                addSalespersonWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                addSalespersonWindow.ShowDialog();
            }
        }
    }
}
