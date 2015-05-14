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
        List<NewTemperature> tempList;
        List<Temperature> tempdata;

        public ListViewPage()
        {
            InitializeComponent();
        }

        async void GetClicked(object sender, EventArgs args)
        {
            var StartDate = StartDatePicker.Date;
            var EndDate = StartDatePicker.Date.AddDays(Double.Parse(EndDateEntry.Text));
            tempdata = await GetTemperatureAsync(StartDate, EndDate);
            
            tempList = (from x in tempdata
                        select new NewTemperature
                        {
                            RegisterDate = x.RegisterDate,
                            Value = x.Value,
                            OpacityValue = x.Value * 3.5 / 100,
                        }).ToList();

            list.ItemsSource = tempList;
            
        }

        #region 並べ替えは不使用
        /*
        void AscendingClicked(object sender, EventArgs e)
        {
            var list1 = tempList.OrderBy(a => a.Value).ToList();
            list.ItemsSource = list1;
        }
        void DescendingClicked(object sender, EventArgs e)
        {
            var list1 = tempList.OrderByDescending(a => a.Value).ToList();
            list.ItemsSource = list1;
        }
        */
        #endregion

        async void AverageClicked(object sender, EventArgs e)
        {
            if (tempList == null)
            {
                var StartDate = StartDatePicker.Date;
                var EndDate = StartDatePicker.Date.AddDays(Double.Parse(EndDateEntry.Text));
                tempdata = await GetTemperatureAsync(StartDate, EndDate);
            }

            

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
