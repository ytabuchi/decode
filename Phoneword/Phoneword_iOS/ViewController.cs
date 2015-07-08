using System;

using UIKit;
using Foundation;
using System.Collections.Generic;

namespace Phoneword_iOS
{
    public partial class ViewController : UIViewController
    {
        string translatedNumber = "";
        public List<string> PhoneNumbers { get; set; }

        public ViewController(IntPtr handle) : base(handle)
        {
            //Call History 画面で呼ばれる電話番号のリストを初期化します。
            PhoneNumbers = new List<string> ();
        }

        public override void ViewDidLoad() {
            base.ViewDidLoad();

            // Translate ボタンタップ時の処理
            TranslateButton.TouchUpInside += (object sender, EventArgs e) => {
                // ユーザーのアルファベットの番号を数字の番号に変換します。
                translatedNumber = Core.PhonewordTranslator.ToNumber(PhoneNumberText.Text);   
                // Text Field 以外がタップされたらキーボードを非表示 
                PhoneNumberText.ResignFirstResponder ();
                if (translatedNumber == "") {
                    CallButton.SetTitle ("Call", UIControlState.Normal);
                    CallButton.Enabled = false;
                } 
                else {
                    CallButton.SetTitle ("Call " + translatedNumber, UIControlState.Normal);
                    CallButton.Enabled = true;
                }
            };

            // Call ボタンタッチ時の処理
            CallButton.TouchUpInside += (object sender, EventArgs e) => {
                // ダイヤルした電話番号を保存します。
                PhoneNumbers.Add (translatedNumber);
                var url = new NSUrl ("tel:" + translatedNumber);
                // tel: で始まる URL ハンドラーを使用して Phone App を起動し、
                // 起動出来なければ alert dialog を表示します。                
                if (!UIApplication.SharedApplication.OpenUrl (url)) {
                    var alert = UIAlertController.Create  ("Not supported",
                        "Scheme 'tel:' is not supported on this device", 
                        UIAlertControllerStyle.Alert);
                    alert.AddAction (UIAlertAction.Create ("OK", UIAlertActionStyle.Default, null));
                    PresentViewController (alert, true, null);
                }
            };

        }

        public override void DidReceiveMemoryWarning() {
            base.DidReceiveMemoryWarning();
        }

        /// <summary>
        /// Segue 用のデータを用意します。
        /// </summary>
        /// <param name="segue"></param>
        /// <param name="sender"></param>
        public override void PrepareForSegue (UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue (segue, sender);
            // 移行先の画面の View Controller を設定します。
            var callHistoryContoller = segue.DestinationViewController as CallHistoryController;
            // Table View Controller のリスト PhoneNumbers 電話番号を設定します。
            if (callHistoryContoller != null) {
                callHistoryContoller.PhoneNumbers = PhoneNumbers;
            }
        }

    }
}

