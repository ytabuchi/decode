using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XF_Phoneword
{
    public partial class MainPageXaml : ContentPage
    {
        private List<string> phoneNumbers = new List<string>();

        public MainPageXaml()
        {
            InitializeComponent();

            var translatedNumber = string.Empty;

            this.translateButton.Clicked += (sender, e) =>
            {
                // ユーザーのアルファベットの番号を数字の番号に変換します。
                translatedNumber = Core.PhonewordTranslator.ToNumber(PhoneNumberText.Text);
                if (String.IsNullOrWhiteSpace(translatedNumber))
                {
                    callButton.Text = "Call";
                    callButton.IsEnabled = false;
                }
                else
                {
                    callButton.Text = "Call " + translatedNumber;
                    callButton.IsEnabled = true;
                }
            };
            this.callButton.Clicked += (sender, e) =>
            {
                // ダイヤルする電話番号を保存します。
                phoneNumbers.Add(translatedNumber);
                var url = "tel:" + translatedNumber;
                // 電話出来なければ alert を表示します。
                Device.OpenUri(new Uri(url));
                //await DisplayAlert("Not supported", "'tel' is not supported on this device.", "OK");
                callHistoryButton.IsEnabled = true;
            };
            this.callHistoryButton.Clicked += async (sender, e) =>
            {
                await Navigation.PushAsync(new DetailPage(phoneNumbers));
            };
        }
    }
}