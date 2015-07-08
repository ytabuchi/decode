using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using decodeDemo.WinPhone;
using Windows.Phone.Speech.Synthesis;

[assembly: Xamarin.Forms.Dependency(typeof(TextToSpeech_WinPhone))]

namespace decodeDemo.WinPhone
{
    public class TextToSpeech_WinPhone : ITextToSpeech
    {
        public TextToSpeech_WinPhone() { }

        public async void Speak(string text)
        {
            SpeechSynthesizer synth = new SpeechSynthesizer();
            await synth.SpeakTextAsync(text);
        }
    }
}
