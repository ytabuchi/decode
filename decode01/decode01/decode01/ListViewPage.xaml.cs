using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace decode01
{
    public partial class ListViewPage : ContentPage
    {
        DateTime StartDate = new DateTime(2015, 4, 1);
        DateTime EndDate = new DateTime(2015, 4, 4);

        List<NewTemperature> tempList;
        const string url = "http://xmdemo1.azurewebsites.net/thermos.png";

        public ListViewPage()
        {
            InitializeComponent();
        }

        private async void OnButtonClicked(object sender, EventArgs args)
        {

            var tempdata = await GetTemperatureAsync(StartDate, EndDate);

            tempList = (from x in tempdata
                        select new NewTemperature
                        {
                            RegisterDate = x.RegisterDate,
                            Value = x.Value,
                            Image = url,
                            OpacityValue = x.Value * 3.5 / 100,
                        }).ToList();

            list.ItemsSource = tempList;
            
        }

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

        //private DateTime startDate;
        //public DateTime StartDate
        //{
        //    get { return startDate; }
        //    set
        //    {
        //        if (startDate != value)
        //        {
        //            startDate = value;
        //            OnPropertyChanged();
        //        }
        //    }
        //}

        //private DateTime endDate;
        //public DateTime EndDate
        //{
        //    get { return endDate; }
        //    set
        //    {
        //        if (endDate != value)
        //        {
        //            endDate = value;
        //            OnPropertyChanged();
        //        }
        //    }
        //}
    }

    public class NewTemperature : Temperature
    {
        public string Image { get; set; }
        public double OpacityValue { get; set; }
    }
}
