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
            callButton.Clicked += async (sender, e) =>
            {
                if (await DisplayAlert("Call?", $"Call {translatedNumber} ?", "Yes, Call", "Cancel"))
                {
                    phoneNumbers.Add(translatedNumber);
                    Device.OpenUri(new Uri("tel:{translatedNumber}"));
                    callHistoryButton.IsEnabled = true;
                }
            };
            callHistoryButton.Clicked += (sender, e) =>
            {
                Navigation.PushAsync(new DetailPage(phoneNumbers));
            };
        }
    }
}