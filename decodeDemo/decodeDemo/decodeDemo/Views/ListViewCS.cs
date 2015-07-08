using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;
using decodeDemo.Models;

namespace decodeDemo.Views
{
    public class ListViewCS : ContentPage
    {
        private DatePicker startDatePicker;
        private Entry endDateEntry;
        private Button getButton;
        private Button averageButton;
        private ListView listCS;
        private List<Temperature> tempDataCS;


        public ListViewCS()
        {
            // ページ内のスタイル設定です。
            var localButtonStyle = new Style(typeof(Button))
            {
                Setters = {
                    new Setter { Property = Button.BorderRadiusProperty, Value = 5 },
                    new Setter { Property = Button.WidthRequestProperty, Value = Device.OnPlatform(70, 70, 100) }
                }
            };

            startDatePicker = new DatePicker
            {
                Date = new DateTime(2015, 4, 4),
                Format = "yyyy/MM/dd",
            };
            endDateEntry = new Entry
            {
                Text = "",
                Placeholder = "日",
                HorizontalOptions = LayoutOptions.Center,
                WidthRequest = Device.OnPlatform(60, 50, 80),
            };
            endDateEntry.TextChanged += endDateEntry_TextChanged;
            getButton = new Button
            {
                Text = "取得",
                Style = Application.Current.Resources["gButtonStyle"] as Style,
            };
            getButton.Clicked += ButtonClicked;
            averageButton = new Button
            {
                Text = "平均",
                Style = localButtonStyle,
                IsEnabled = false,
            };
            averageButton.Clicked += ButtonClicked;



            var hStack = new StackLayout
            {
                Padding = 5,
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Children = {
					startDatePicker,
                    endDateEntry,
                    getButton,
                    averageButton
                    }
            };

            listCS = new ListView
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HasUnevenRows = true,
            };
            listCS.ItemTapped += ListItemTapped;

            Title = "ListView (C#)";
            Content = new StackLayout
            {
                Children = {
                    hStack,
                    listCS
				},
            };
        }

        /// <summary>
        /// ボタンクリック時のメソッド
        /// </summary>
        /// <param name="sender">ボタンインスタンス名</param>
        /// <param name="e"></param>
        async void ButtonClicked(object sender, EventArgs e)
        {
            // 日付チェック
            int i;
            var gt = new GetTemperature();
            if (int.TryParse(endDateEntry.Text, out i))
            {
                var StartDate = startDatePicker.Date;
                var EndDate = startDatePicker.Date.AddDays(i);
                tempDataCS = await gt.GetTemperatureAsync(StartDate, EndDate);

                if (sender == getButton)
                {
                    // ItemsSource と Template を指定して先頭にスクロール
                    listCS.ItemsSource = tempDataCS;
                    listCS.ItemTemplate = new DataTemplate(typeof(defaultCell));
                    listCS.ScrollTo(tempDataCS[0], ScrollToPosition.Start, false);
                }
                else
                {
                    // Linq で日付でグループ化して各日にちの平均温度を取得します。
                    var tempListCS = (from x in tempDataCS
                                    group x by x.RegisterDate.Date into newgroup
                                    orderby newgroup.Key
                                    select new Temperature
                                    {
                                        RegisterDate = newgroup.Key,
                                        Value = (from y in newgroup
                                                     select y.Value).Average()
                                    }).ToList();

                    // ItemsSource と Template を指定して先頭にスクロール
                    listCS.ItemsSource = tempListCS;
                    listCS.ItemTemplate = new DataTemplate(typeof(averageCell));
                    listCS.ScrollTo(tempListCS[0], ScrollToPosition.Start, false);
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
            listCS.SelectedItem = null;
            var data = (Temperature)e.Item;
            await DisplayAlert("Item Tapped",
                string.Format("日時: {0:yyyy/MM/dd  H}時\n温度: {1:f1} ℃",
                data.RegisterDate, data.Value), "OK");
        }

        /// <summary>
        /// Entry Validatiion メソッド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void endDateEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            double result;
            bool isValid = Double.TryParse(e.NewTextValue, out result);
            if (!isValid)
            {
                endDateEntry.TextColor = Color.Red;
                getButton.IsEnabled = false;
                averageButton.IsEnabled = false;
            }
            else
            {
                endDateEntry.TextColor = Color.Default;
                getButton.IsEnabled = true;
                averageButton.IsEnabled = true;
            }
        }

    }

    /// <summary>
    /// ViewCell を継承した Custom DataTemplate です。
    /// </summary>
    public class defaultCell : ViewCell
    {
        public defaultCell()
        {
            // CustomCell の各セルを定義します。
            var image = new Image { Source = ImageSource.FromFile("thermos.png") };
            image.SetBinding(Image.OpacityProperty, new Binding("Value", converter: new Converters.TempToOpacityConverter()));

            var dateLabel = new Label { YAlign = TextAlignment.Center };
            dateLabel.SetBinding(Label.TextProperty, new Binding("RegisterDate", stringFormat: "{0:yyyy/MM/dd  H時}"));

            var valueLabel = new Label { TextColor = Color.FromHex("3E7FC2"), YAlign = TextAlignment.Center };
            valueLabel.SetBinding(Label.TextProperty, new Binding("Value", stringFormat: "{0:f1} ℃"));

            // CustomCell のレイアウトを定義します。
            View = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Padding = 5,
                Children =
                {
                    image,
                    new StackLayout {
                        Orientation = StackOrientation.Vertical,
                        Children = {
                            dateLabel,
                            valueLabel
                            }
                    }
                },
            };
        }
    }

    /// <summary>
    /// ViewCell を継承した Custom DataTemplate です。
    /// </summary>
    public class averageCell : ViewCell
    {
        public averageCell()
        {
            // CustomCell の各セルを定義します。
            var image = new Image { Source = ImageSource.FromFile("thermos.png") };
            image.SetBinding(Image.OpacityProperty, new Binding("Value", converter: new Converters.TempToOpacityConverter()));

            var dateLabel = new Label { FontSize = 20, YAlign = TextAlignment.Center };
            dateLabel.SetBinding(Label.TextProperty, new Binding("RegisterDate", stringFormat: "{0:yyyy/MM/dd}"));

            var valueLabel = new Label { FontSize = 20, TextColor = Color.FromHex("3E7FC2"), YAlign = TextAlignment.Center };
            valueLabel.SetBinding(Label.TextProperty, new Binding("Value", stringFormat: "{0:f1} ℃"));

            // CustomCell のレイアウトを定義します。
            View = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Padding = 5,
                Spacing = 20,
                Children =
                {
                    image,
                    dateLabel,
                    valueLabel,
                },
            };
        }
    }
}
