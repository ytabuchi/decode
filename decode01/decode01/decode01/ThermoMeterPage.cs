using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace decode01
{
    public class ThermoMeterPage : ContentPage
    {
        BoxView thermoBar;
        Image bgImage;
        DatePicker startDate;
        TimePicker startTime;
        StackLayout dateLayout;

        public ThermoMeterPage()
        {
            // AbsoluteLayout を用意して背景画像に "埋め込まれたリソース" で配置した画像を読み込みます。
            AbsoluteLayout abs = new AbsoluteLayout
            {
                BackgroundColor = Color.White,
            };
            bgImage = new Image { Aspect = Aspect.AspectFit };
            bgImage.Source = ImageSource.FromResource("decode01.Thermo.png");

            // 各種コントロール設定＆配置
            startDate = new DatePicker
            {
                Format = "yyyy/MM/dd",
                Date = new DateTime(2015, 4, 1),
            };
            startTime = new TimePicker
            {
                Format = "H時",
                Time = new TimeSpan(1, 0, 0),
                WidthRequest = 60,
            };
            dateLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children = {
                    startDate,
                    startTime,
                    new Button {
                        Text = "温度取得",
                        // thermoBar を回転させるメソッド
                        Command = new Command(() => RotateThermoBarAsync(
                            startDate.Date, 
                            startDate.Date.AddDays(1), 
                            startTime.Time))
                    }
                }
            };
            thermoBar = new BoxView
            {
                Color = Color.FromHex("#bd3f3f"),
                Rotation = -120,
                Opacity = 0.7,
            };

            abs.Children.Add(bgImage);
            abs.Children.Add(thermoBar);
            abs.Children.Add(dateLayout);

            Content = abs;
            Title = "温度計";

            SizeChanged += OnPageSizeChanged;
        }

        /// <summary>
        /// 画面サイズ変更時に呼び出されます。各コントロールの場所を指定します。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void OnPageSizeChanged(object sender, EventArgs args)
        {
            // 画面サイズを元に中心位置、半径、大きさなどを決めます。
            Point center = new Point(this.Width / 2, this.Height / 2);
            double radius = 0.35 * Math.Min(this.Width, this.Height);
            double width = 0.08 * radius;
            double height = 1.0 * radius;
            double offset = 0.9;

            // AbsoluteLayout に配置していきます。
            AbsoluteLayout.SetLayoutFlags(bgImage,
                AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(bgImage,
                new Rectangle(0.5, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            AbsoluteLayout.SetLayoutFlags(dateLayout,
                AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(dateLayout,
                new Rectangle(0.5, 0, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            AbsoluteLayout.SetLayoutBounds(thermoBar,
                new Rectangle(center.X - 0.5 * width, center.Y - offset * height, width, height));
            thermoBar.AnchorX = 0.51;
            thermoBar.AnchorY = offset;
        }

        /// <summary>
        /// thermoBar を取得した日時の温度まで回転させます。
        /// </summary>
        /// <param name="from">開始日</param>
        /// <param name="to">終了日(開始日+1を指定)</param>
        /// <param name="time">時間</param>
        private async void RotateThermoBarAsync(DateTime from, DateTime to, TimeSpan time)
        {
            var tempdata = await GetTemperatureAsync(from, to);
            var thermo = tempdata[time.Hours].Value;
            var angle = (thermo - 10) * 3;
            
            await thermoBar.RotateTo(angle, 3000, Easing.CubicOut); // 回転

            // Dependency Service で温度を読み上げます。
            DependencyService.Get<ITextToSpeech>().Speak(string.Format("{0:f1} ド です", thermo));
        }

        /// <summary>
        /// 温度の json データを取得します。
        /// </summary>
        /// <param name="from">開始日</param>
        /// <param name="to">終了日</param>
        /// <returns></returns>
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
}
