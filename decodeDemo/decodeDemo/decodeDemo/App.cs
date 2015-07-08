using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using decodeDemo.Views;
using decodeDemo.Models;

namespace decodeDemo
{
    public class App : Application
    {
        public App()
        {
            #region Application全体で適用、参照できるスタイルを定義します。
            Application.Current.Resources = new ResourceDictionary();

            // アプリ全体のLabelに適用されます。
            var allLabelStyle = new Style(typeof(Label))
            {
                Setters = {
                    new Setter { Property = Label.FontSizeProperty, Value = 16 },
                    new Setter { Property = Label.TextColorProperty, Value = Color.FromHex("444444") },
                }
            };
            Application.Current.Resources.Add(allLabelStyle);

            // アプリ全体のButtonに適用されます。
            var allButtonStyle = new Style(typeof(Button))
            {
                Setters = {
                    new Setter { Property = Button.BorderRadiusProperty, Value = 5 },
                }
            };
            Application.Current.Resources.Add(allButtonStyle);

            // gButtonStyleでスタイルを参照できます。
            var g_ButtonStyle = new Style(typeof(Button))
            {
                Setters = {
                    new Setter { Property = Button.TextColorProperty, Value = Color.White },
                    new Setter { Property = Button.BackgroundColorProperty, Value = Color.FromHex("49d849") }, // 緑 #49d849 赤 d64848
                    new Setter { Property = Button.BorderRadiusProperty, Value = 5 },
                    new Setter { Property = Button.WidthRequestProperty, Value = Device.OnPlatform(70, 70, 100) }
                }
            };
            Application.Current.Resources.Add("gButtonStyle", g_ButtonStyle);
            #endregion

            // CarouselPageを定義します。
            List<ContentPage> pages = new List<ContentPage>();
            pages.Add(new ThemoGauge());
            pages.Add(new ListViewCS());
            pages.Add(new ListViewXaml());
            var pg = new CarouselPage
            {
                Title = "温度計 (C#)",
                Children = {
                    pages[0],pages[1],pages[2]
                }
            };
            pg.CurrentPageChanged += (object sender, EventArgs e) =>
            {
                pg.Title = pg.CurrentPage.Title;
            };

            // Navigationpageを背景色、テキスト色などを指定して読み込みます。
            var nav = new NavigationPage(pg);
            nav.BarBackgroundColor = Color.FromHex("3498DB"); // 水色 #3498DB 緑 #75C465 紫 #B765B8
            // Windows 8.1でLightThemeを使用しているため暫定的にBarTextColorを黒に…
            if (Device.OS == TargetPlatform.Windows)
            {
                nav.BarTextColor = Color.Black;
            }
            else
            {
                nav.BarTextColor = Color.White;
            }
            MainPage = nav;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
