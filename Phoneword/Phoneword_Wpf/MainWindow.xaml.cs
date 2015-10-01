using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Phoneword_Wpf
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        string translatedNumber = string.Empty;
        List<string> phoneNumbers = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void translateButton_Click(object sender, RoutedEventArgs e)
        {
            translatedNumber = Core.PhonewordTranslator.ToNumber(phoneNumberText.Text);

            if (String.IsNullOrWhiteSpace(translatedNumber))
            {
                callButton.Content = "Call";
                callButton.IsEnabled = false;
            }
            else
            {
                callButton.Content = "Call " + translatedNumber;
                callButton.IsEnabled = true;
            }
        }

        private void callButton_Click(object sender, RoutedEventArgs e)
        {
            // MessageBox の内容
            var messageBoxText = "Call?";
            var caption = string.Format("Call {0} ?", translatedNumber);
            MessageBoxButton button = MessageBoxButton.YesNoCancel;

            // MessageBox 表示と結果取得
            var result = MessageBox.Show(messageBoxText, caption, button);

            if (result == MessageBoxResult.Yes)
            {
                phoneNumbers.Add(translatedNumber);
                callHistoryButton.IsEnabled = true;
                //電話はかけられないのでスキップ
            }
        }

        private void callHistoryButton_Click(object sender, RoutedEventArgs e)
        {
            // スキップ
        }
    }
}
