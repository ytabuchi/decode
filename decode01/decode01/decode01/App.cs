using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace decode01
{
    public class App : Application
    {
        public App()
        {
            // The root page of your application
            //MainPage = new ThermoMeterPage();
            MainPage = new NavigationPage(new StartPage());
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

    public class StartPage : ContentPage
    {
        public StartPage()
        {
            Title = "de:code Demo App";
            Padding = 10;
            Content = new StackLayout
            {
                Children = {
                    new Button {
                        Text = "ThermoMeter",
                        Command = new Command(async () => 
                            await Navigation.PushAsync(new ThermoMeterPage()))
                    },
                    new Button {
                        Text = "ListView",
                        Command = new Command(async () => 
                            await Navigation.PushAsync(new ListViewPage()))
                    },
                    //new Button{
                    //    Text = "Infragistics Control!",
                    //    Command = new Command(async () => 
                    //        await Navigation.PushAsync(new Page2()))
                    //},
                    //new Button{
                    //    Text = "Infragistics Gauge!",
                    //    Command = new Command(async () => 
                    //        await Navigation.PushAsync(new ThermoMeterPage2()))
                    //},

                }
            };
        }

    }
}
