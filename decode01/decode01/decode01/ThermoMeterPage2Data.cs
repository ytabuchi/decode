using decode01.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace decode01
{
    public class ThermoMeterPage2Data
        : BindableBase
    {
        private DateTime startDateTime;

        public DateTime StartDateTime
        {
            get { return startDateTime; }
            set
            {
                if (startDateTime != value)
                {
                    startDateTime = value;
                    OnPropertyChanged();
                }
            }
        }

        private TimeSpan selectedTime;

        public TimeSpan SelectedTime
        {
            get { return selectedTime; }
            set
            {
                if (selectedTime != value)
                {
                    selectedTime = value;
                    OnPropertyChanged();
                }
            }
        }


        private float temperatureValue;

        public float TemperatureValue
        {
            get { return temperatureValue; }
            set
            {
                if (temperatureValue != value)
                {
                    temperatureValue = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand GetTemperatureCommand { protected set; get; }


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

        public ThermoMeterPage2Data()
        {
            this.StartDateTime = new DateTime(2015, 4, 1);
            this.SelectedTime = TimeSpan.FromHours(1);
            this.TemperatureValue = 0;
            this.GetTemperatureCommand = new Command(
           async () =>
           {
               var list = await GetTemperatureAsync(
                   this.startDateTime.Date, this.StartDateTime.Date.AddDays(1));
               
               this.TemperatureValue = list[this.SelectedTime.Hours].Value;
               DependencyService.Get<ITextToSpeech>().Speak(string.Format("{0:f1} ド です", 
                   this.TemperatureValue.ToString()));
           });
        }

    }
}
