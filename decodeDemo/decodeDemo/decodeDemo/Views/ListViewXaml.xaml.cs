using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using decodeDemo.Models;

namespace decodeDemo.Views
{
    public partial class ListViewXaml : ContentPage
    {
        GetTemperature gt = new GetTemperature();
        List<Temperature> tempDataXaml;
        public ListViewXaml()
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
                tempDataXaml = await gt.GetTemperatureAsync(StartDate, EndDate);

                if (sender == GetButton)
                {
                    // ItemsSource と Template を指定して先頭にスクロール
                    //listXaml.RowHeight = 85;
                    listXaml.ItemsSource = tempDataXaml;
                    listXaml.ItemTemplate = this.Resources["DefaultTemplate"] as DataTemplate;
                    listXaml.ScrollTo(tempDataXaml[0], ScrollToPosition.Start, false);
                }
                else
                {
                    // Linq で日付でグループ化して各日にちの平均温度を取得します。
                    var tempListXaml = (from x in tempDataXaml
                                        group x by x.RegisterDate.Date into newgroup
                                        orderby newgroup.Key
                                        select new Temperature
                                        {
                                            RegisterDate = newgroup.Key,
                                            Value = (from q in newgroup
                                                     select q.Value).Average(),
                                        }).ToList();

                    // ItemsSource と Template を指定して先頭にスクロール
                    //listXaml.RowHeight = 60;
                    listXaml.ItemsSource = tempListXaml;
                    listXaml.ItemTemplate = this.Resources["AverageTemplate"] as DataTemplate;
                    listXaml.ScrollTo(tempListXaml[0], ScrollToPosition.Start, false);
                }
            }
            else
            {
                await DisplayAlert("入力エラー", "日数を入力してください", "OK");
            }
        }

        /// <summary>
        /// ListItem タップ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void ListItemTapped(object sender, ItemTappedEventArgs e)
        {
            listXaml.SelectedItem = null;
            var data = (Temperature)e.Item;
            await DisplayAlert("Item Tapped",
                string.Format("日時: {0:yyyy/MM/dd  H}時\n温度: {1:f1} ℃",
                data.RegisterDate, data.Value), "OK");
        }

    }


}
