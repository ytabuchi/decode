using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace XF_Phoneword
{
    public class DetailPage : ContentPage
    {
        public DetailPage(List<string> phoneNumbers)
        {
            var listView = new ListView();
            listView.ItemsSource = phoneNumbers;

            listView.ItemTapped += async (sender, e) =>
            {
                ((ListView)sender).SelectedItem = null; // 選択解除
                await DisplayAlert("Tapped: ", e.Item.ToString(), "OK");
            };

            Title = "Call History";
            Content = listView;
        }
    }
}