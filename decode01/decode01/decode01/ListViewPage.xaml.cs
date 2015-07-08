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
    /// <summary>
    /// 透過度を追加
    /// </summary>
    public class NewTemperature : Temperature
    {
        public double OpacityValue { get; set; }
    }

    public partial class ListViewPage : ContentPage
    {
        List<Temperature> tempdata;

        public ListViewPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// ボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        async void ButtonClicked(object sender, EventArgs args)
        {
            // 日付チェック
            double d;
            if (double.TryParse(EndDateEntry.Text, out d))
            {
                var StartDate = StartDatePicker.Date;
                var EndDate = StartDatePicker.Date.AddDays(d);
                tempdata = await GetTemperatureAsync(StartDate, EndDate);

                if (sender == GetButton)
                {
                    var tempList = (from x in tempdata
                                    select new NewTemperature
                                    {
                                        RegisterDate = x.RegisterDate,
                                        Value = x.Value,
                                        OpacityValue = x.Value * 3.5 / 100,
                                    }).ToList();

                    // ItemsSource と Template を指定して先頭にスクロール
                    list.ItemsSource = tempList;
                    list.ItemTemplate = this.Resources["DefaultTemplate"] as DataTemplate;
                    list.ScrollTo(tempList[0], ScrollToPosition.Start, false);
                }
                else
                {
                    // Linq で日付でグループ化して各日にちの平均温度を取得します。
                    var tempList = (from x in tempdata
                                    group x by x.RegisterDate.Date into newgroup
                                    orderby newgroup.Key
                                    select new NewTemperature
                                    {
                                        RegisterDate = newgroup.Key,
                                        Value = (from q in newgroup
                                                 select q.Value).Average(),
                                        OpacityValue = (from q in newgroup
                                                        select q.Value).Average() * 3.5 / 100,
                                    }).ToList();

                    // ItemsSource と Template を指定して先頭にスクロール
                    list.ItemsSource = tempList;
                    list.ItemTemplate = this.Resources["AverageTemplate"] as DataTemplate;
                    list.ScrollTo(tempList[0], ScrollToPosition.Start, false);
                }
            }
            else
            {
                await DisplayAlert("入力エラー", "日数を入力してください", "OK");
            }
            
        }

        ///// <summary>
        ///// 平均ボタンクリック
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //async void AverageClicked(object sender, EventArgs e)
        //{
        //    var StartDate = StartDatePicker.Date;
        //    var EndDate = StartDatePicker.Date.AddDays(Double.Parse(EndDateEntry.Text));
        //    tempdata = await GetTemperatureAsync(StartDate, EndDate);

        //    // Linq で日付でグループ化して温度の平均値を取得します。
        //    var tempList = (from q in tempdata
        //                    group q by q.RegisterDate.Date into newgroup
        //                    orderby newgroup.Key
        //                    select new NewTemperature
        //                    {
        //                        RegisterDate = newgroup.Key,
        //                        Value = (from q2 in newgroup
        //                                 select q2.Value).Average(),
        //                        OpacityValue = (from q2 in newgroup
        //                                        select q2.Value).Average() * 3.5 / 100,
        //                    }).ToList();

        //    list.ItemsSource = tempList;
        //    list.ItemTemplate = this.Resources["AverageTemplate"] as DataTemplate;

        //}

        /// <summary>
        /// ListItem タップ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void ListItemTapped(object sender, ItemTappedEventArgs e)
        {
            list.SelectedItem = null;
            var data = (NewTemperature)e.Item;
            await DisplayAlert("Item Tapped",
                string.Format("日時: {0:yyyy/MM/dd  H}時\n温度: {1} ℃",
                data.RegisterDate, data.Value), "OK");
        }

        private async Task<List<Temperature>> GetTemperatureAsync(DateTime from, DateTime to)
        {
            var uri = new Uri(
                string.Format(
                "http://azuretemperature.azurewebsites.net/api/temp?from={0}&to={1}", from, to));
            var client = new HttpClient();
            var response = await client.GetAsync(uri);
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Temperature>>(json);
        }

    }

    /// <summary>
    /// Entry チェックです。数値以外で文字色を赤にします。
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

}
