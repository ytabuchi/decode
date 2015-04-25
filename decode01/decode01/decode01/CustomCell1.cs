using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace decode01
{
    /// <summary>
    /// ViewCell を使用した CustomCell です。
    /// </summary>
    public class CustomCell1 : ViewCell
    {
        public CustomCell1()
        {
            var header = new Label()
            {
                Style = Device.Styles.SubtitleStyle, // 各PF標準の Style を使用します。
                TextColor = Color.FromHex("#75c465")
            };
            header.SetBinding(Label.TextProperty, "date"); // Binding を指定します。

            // List<int> temp をそれぞれ取得して Binding を指定します。
            var time0 = new Label();
            time0.SetBinding(Label.TextProperty, new Binding("temp[0]", stringFormat: "{0}℃"));
            var time1 = new Label();
            time1.SetBinding(Label.TextProperty, new Binding("temp[1]", stringFormat: "{0}℃"));
            var time2 = new Label();
            time2.SetBinding(Label.TextProperty, new Binding("temp[2]", stringFormat: "{0}℃"));
            var time3 = new Label();
            time3.SetBinding(Label.TextProperty, new Binding("temp[3]", stringFormat: "{0}℃"));
            var time4 = new Label();
            time4.SetBinding(Label.TextProperty, new Binding("temp[4]", stringFormat: "{0}℃"));
            var time5 = new Label();
            time5.SetBinding(Label.TextProperty, new Binding("temp[5]", stringFormat: "{0}℃"));
            var time6 = new Label();
            time6.SetBinding(Label.TextProperty, new Binding("temp[6]", stringFormat: "{0}℃"));
            var time7 = new Label();
            time7.SetBinding(Label.TextProperty, new Binding("temp[7]", stringFormat: "{0}℃"));
            var time8 = new Label();
            time8.SetBinding(Label.TextProperty, new Binding("temp[8]", stringFormat: "{0}℃"));
            var time9 = new Label();
            time9.SetBinding(Label.TextProperty, new Binding("temp[9]", stringFormat: "{0}℃"));
            var time10 = new Label();
            time10.SetBinding(Label.TextProperty, new Binding("temp[10]", stringFormat: "{0}℃"));
            var time11 = new Label();
            time11.SetBinding(Label.TextProperty, new Binding("temp[11]", stringFormat: "{0}℃"));
            var time12 = new Label();
            time12.SetBinding(Label.TextProperty, new Binding("temp[12]", stringFormat: "{0}℃"));
            var time13 = new Label();
            time13.SetBinding(Label.TextProperty, new Binding("temp[13]", stringFormat: "{0}℃"));
            var time14 = new Label();
            time14.SetBinding(Label.TextProperty, new Binding("temp[14]", stringFormat: "{0}℃"));
            var time15 = new Label();
            time15.SetBinding(Label.TextProperty, new Binding("temp[15]", stringFormat: "{0}℃"));
            var time16 = new Label();
            time16.SetBinding(Label.TextProperty, new Binding("temp[16]", stringFormat: "{0}℃"));
            var time17 = new Label();
            time17.SetBinding(Label.TextProperty, new Binding("temp[17]", stringFormat: "{0}℃"));
            var time18 = new Label();
            time18.SetBinding(Label.TextProperty, new Binding("temp[18]", stringFormat: "{0}℃"));
            var time19 = new Label();
            time19.SetBinding(Label.TextProperty, new Binding("temp[19]", stringFormat: "{0}℃"));
            var time20 = new Label();
            time20.SetBinding(Label.TextProperty, new Binding("temp[20]", stringFormat: "{0}℃"));
            var time21 = new Label();
            time21.SetBinding(Label.TextProperty, new Binding("temp[21]", stringFormat: "{0}℃"));
            var time22 = new Label();
            time22.SetBinding(Label.TextProperty, new Binding("temp[22]", stringFormat: "{0}℃"));
            var time23 = new Label();
            time23.SetBinding(Label.TextProperty, new Binding("temp[23]", stringFormat: "{0}℃"));

            // 室温データのセルレイアウトです。 6x4 で配置してみました。
            var tempcells = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Children = {
                    new StackLayout {
                        Orientation = StackOrientation.Horizontal,
                        Children = {
                            time0,time1,time2,time3,time4,time5,
                        }
                    },
                    new StackLayout {
                        Orientation = StackOrientation.Horizontal,
                        Children = {
                            time6,time7,time8,time9,time10,time11,
                        }
                    },
                    new StackLayout {
                        Orientation = StackOrientation.Horizontal,
                        Children = {
                            time12,time13,time14,time15,time16,time17,
                        }
                    },
                    new StackLayout {
                        Orientation = StackOrientation.Horizontal,
                        Children = {
                            time18,time19,time20,time21,time22,time23,
                        }
                    },
                }
            };

            // セルのレイアウトです。
            var CellLayout = new StackLayout
            {
                Children = {
                    header,
                    tempcells
                }
            };

            this.View = CellLayout;
        }
    }
}
