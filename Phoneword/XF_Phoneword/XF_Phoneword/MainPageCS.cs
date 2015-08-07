using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace XF_Phoneword
{
    public class MainPageCS : ContentPage
    {

        string translatedNumber = string.Empty;
        List<string> phoneNumbers = new List<string>();
        Entry phoneNumberText;
        Button callButton;
        Button callHistoryButton;

        public MainPageCS()
        {
            var headerLabel = new Label
            {
                Text = "Enter a Phoneword:",
                FontSize = 24,
            };

            phoneNumberText = new Entry
            {
                Text = "1-855-Xamarin",
            };
            var translateButton = new Button
            {
                Text = "Translate",
            };
            translateButton.Clicked += TranslateButton_Clicked;
            callButton = new Button
            {
                Text = "Call",
                IsEnabled = false,
            };
            callButton.Clicked += CallButton_Clicked;
            callHistoryButton = new Button
            {
                Text = "Call History",
                IsEnabled = false,
            };
            callHistoryButton.Clicked += CallHistoryButton_Clicked;

            Title = "Phoneword";
            Content = new StackLayout
            {
                Padding = 15,
                Spacing = 5,
                Children = {
                    headerLabel,
                    phoneNumberText,
                    translateButton,
                    callButton,
                    callHistoryButton,
                }
            };
        }

        private async void CallHistoryButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DetailPage(phoneNumbers));
        }

        private async void CallButton_Clicked(object sender, EventArgs e)
        {
            if (await DisplayAlert("Call?", $"Call {translatedNumber} ?", "Yes, Call", "Cancel"))
            {
                phoneNumbers.Add(translatedNumber);
                callHistoryButton.IsEnabled = true;
                Device.OpenUri(new Uri($"tel:{translatedNumber}"));
            }
        }

        private void TranslateButton_Clicked(object sender, EventArgs e)
        {
            translatedNumber = Core.PhonewordTranslator.ToNumber(phoneNumberText.Text);

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
        }
    }
}
