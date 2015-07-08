
using decode01.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace decode01
{
    public class Page2Data : BindableBase
    {
       
        public Page2Data()
        {
            this.StartDate = new DateTime(2015, 4, 1);
            this.EndDate = DateTime.Today;
            this.IsInLoop = false;
            this.TempCollection = new ObservableCollection<Temperature>();

            // Azure からデータ取得
            this.LoadDataCommand = new Command(async () =>
            {
                var list = await GetTemperatureAsync(this.StartDate, this.EndDate);
                TempCollection = 
                    new ObservableCollection<Temperature>(
                        list.AsEnumerable<Temperature>());

                (this.StartFeedCommand as Command).ChangeCanExecute();
             });

            // 擬似的にデータフィードを開始
            this.StartFeedCommand = new Command(() =>
            {
                this.IsInLoop = true;
                Xamarin.Forms.Device.StartTimer(TimeSpan.FromMilliseconds(50),
                    () =>
                    {
                        Random rand = new Random(DateTime.Now.Millisecond);
                        this.TempCollection.Add(new Temperature
                        {
                            RegisterDate = this.TempCollection.Last().RegisterDate.AddHours(1),
                            Value = this.TempCollection.Last().Value +
                            (float)((double)rand.Next(-1, 2) * rand.NextDouble())
                        });
                        (this.StopFeedCommand as Command).ChangeCanExecute();
                        return IsInLoop;
                    });
               
            }, () => {
                return this.TempCollection.Count > 0;
            });

            // データフィードを停止
            this.StopFeedCommand = new Command(() =>
            {
                this.IsInLoop = false;
            }, () =>
            {
                return this.IsInLoop;
            });
          }
       


        private static async Task<List<Temperature>> GetTemperatureAsync(DateTime from, DateTime to)
        {
 
            var uri = new Uri(
                string.Format(
                "http://azuretemperature.azurewebsites.net/api/temp?from={0}&to={1}", from, to));
            var client = new HttpClient();
            var response = await client.GetAsync(uri);
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Temperature>>(json);
        }


        #region プロパティ

        public ICommand LoadDataCommand { protected set; get; }
        public ICommand StartFeedCommand { protected set; get; }
        public ICommand StopFeedCommand { protected set; get; }



        private DateTime startDate;
        public DateTime StartDate
        {
            get { return startDate; }
            set
            {
                if (startDate != value)
                {
                    startDate = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime endDate;
        public DateTime EndDate
        {
            get { return endDate; }
            set
            {
                if (endDate != value)
                {
                    endDate = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool isInLoop;
        public bool IsInLoop
        {
            get { return isInLoop; }
            set
            {
                if (isInLoop != value)
                {
                    isInLoop = value;
                    OnPropertyChanged();
                }
            }
        }
       
        private ObservableCollection<Temperature> tempCollection;
        public ObservableCollection<Temperature> TempCollection
        {
            get { return tempCollection; }
            set
            {
                if (tempCollection != value)
                {
                    tempCollection = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion
    }
}
