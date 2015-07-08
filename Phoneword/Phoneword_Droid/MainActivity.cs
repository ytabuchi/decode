using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;


namespace Phoneword_Droid
{
    [Activity(Label = "@string/AppName" , MainLauncher = true)]
    public class MainActivity : Activity
    {
        static readonly List<string> phoneNumbers = new List<string>();
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // ロードしたレイアウトからコントロールを取得します。
            var phoneNumberText = FindViewById<EditText>(Resource.Id.PhoneNumberText);
            var translateButton = FindViewById<Button>(Resource.Id.TranslateButton);
            var callButton = FindViewById<Button>(Resource.Id.CallButton);
            var callHistoryButton = FindViewById<Button>(Resource.Id.CallHistoryButton);

            // "Call" ボタン利用不可
            callButton.Enabled = false;

            // 番号変換のコードを追加します。
            var translatedNumber = string.Empty;
            translateButton.Click += (object sender, EventArgs e) =>
            {
                // ユーザーのアルファベットの番号を数字の番号に変換します。
                translatedNumber = Core.PhonewordTranslator.ToNumber(phoneNumberText.Text);

                if (String.IsNullOrWhiteSpace(translatedNumber))
                {
                    callButton.Text = "Call";
                    callButton.Enabled = false;
                }
                else
                {
                    callButton.Text = "Call " + translatedNumber;
                    callButton.Enabled = true;
                }
            };

            callButton.Click += (object sender, EventArgs e) =>
            {
                // "Call" ボタンクリックで電話をします。
                var callDialog = new AlertDialog.Builder(this);
                callDialog.SetMessage("Call " + translatedNumber + "?");
                callDialog.SetNeutralButton("Call", delegate
                {
                    // 電話番号をリストに追加します。
                    phoneNumbers.Add(translatedNumber);
                    // Call History ボタンを利用可能にします。
                    callHistoryButton.Enabled = true;
                    // 電話に intent を作成します。
                    var callIntent = new Intent(Intent.ActionCall);
                    callIntent.SetData(Android.Net.Uri.Parse("tel:" + translatedNumber));
                    StartActivity(callIntent);
                });
                callDialog.SetNegativeButton("Cancel", delegate { });
                // レスポンスを待つアラートダイアログを表示します。
                callDialog.Show();
            };

            callHistoryButton.Click += (sender, e) =>
            {
                var intent = new Intent(this, typeof(CallHistoryActivity));
                intent.PutStringArrayListExtra("phone_numbers", phoneNumbers);
                StartActivity(intent);
            };

        }
    }
}

