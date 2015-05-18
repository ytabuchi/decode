using decode01.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace decode01
{

    public partial class ListViewPage : ContentPage
    {
        List<Temperature> tempdata;

        public ListViewPage()
        {
            InitializeComponent();
            list.ItemTapped += (sender, e) =>
            {
                list.SelectedItem = null;

                DisplayAlert("Item Tapped", e.Item.ToString(), "OK");
            };
        }

        async void GetClicked(object sender, EventArgs args)
        {
            var StartDate = StartDatePicker.Date;
            var EndDate = StartDatePicker.Date.AddDays(Double.Parse(EndDateEntry.Text));
            tempdata = await GetTemperatureAsync(StartDate, EndDate);

            var tempList = (from x in tempdata
                            select new NewTemperature
                            {
                                RegisterDate = x.RegisterDate,
                                Value = x.Value,
                                OpacityValue = x.Value * 3.5 / 100,
                            }).ToList();

            list.ItemsSource = tempList;
            list.ItemTemplate = this.Resources["DefaultTemplate"] as DataTemplate;

        }

        async void AverageClicked(object sender, EventArgs e)
        {
            var StartDate = StartDatePicker.Date;
            var EndDate = StartDatePicker.Date.AddDays(Double.Parse(EndDateEntry.Text));
            tempdata = await GetTemperatureAsync(StartDate, EndDate);

            var tempList = (from q in tempdata
                            group q by q.RegisterDate.Date into newgroup
                            orderby newgroup.Key
                            select new NewTemperature
                            {
                                RegisterDate = newgroup.Key,
                                Value = (from q2 in newgroup
                                         select q2.Value).Average(),
                                OpacityValue = (from q2 in newgroup
                                                select q2.Value).Average() * 3.5 / 100,
                            }).ToList();

            list.ItemsSource = tempList;
            list.ItemTemplate = this.Resources["AverageTemplate"] as DataTemplate;

            //Debug.WriteLine("Grouping");
            ////this.list.GroupDisplayBinding = 
            ////    new Binding("RegisterDate", 
            ////        BindingMode.OneWay, new MonthConveter());
            ////this.list.IsGroupingEnabled = true;
            ////this.list.GroupDisplayBinding = new Binding("Value",,new OpacityConverter());
            //this.list.ItemTemplate =
            //    this.Resources["MonthTemplate"] as DataTemplate;
        }

        private async Task<List<Temperature>> GetTemperatureAsync(DateTime from, DateTime to)
        {
            var uri = new Uri(
                string.Format(
                "http://decodexamarin.azurewebsites.net/api/temp?from={0}&to={1}", from, to));
            var client = new HttpClient();
            var response = await client.GetAsync(uri);
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Temperature>>(json);
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public class EntryValidation : TriggerAction<Entry>
    {
        protected override void Invoke(Entry sender)
        {
            double result;
            bool isValid = Double.TryParse(sender.Text, out result);
            sender.TextColor = isValid ? Color.Default : Color.Red;
        }
    }

    public class NewTemperature : Temperature
    {
        public double OpacityValue { get; set; }
    }
}
