using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace decode01
{
    public class Page1 : ContentPage
    {
        ListView list;

        public Page1()
        {
            // ページコンテンツを指定します。
            var header = new Label
            {
                Text = "Json Data",
                Style = Device.Styles.TitleStyle, // 各PF標準の Style を使用します。
            };

            list = new ListView
            {
                HasUnevenRows = true, // 行の高さを自動設定します。
                VerticalOptions = LayoutOptions.FillAndExpand,
                ItemTemplate = new DataTemplate(typeof(CustomCell))
            };

            Title = "Json";
            Content = new StackLayout
            {
                Padding = 10,
                Children = {
                    header,
                    list,
                },
            };
        }

        /// <summary>
        /// 画面表示時
        /// </summary>
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Json を取得して ListView の ItemsSource に指定します。
            //var root = await Json.GetJsonAsync();
            //list.ItemsSource = root.tempdata;
        }
    }
}
