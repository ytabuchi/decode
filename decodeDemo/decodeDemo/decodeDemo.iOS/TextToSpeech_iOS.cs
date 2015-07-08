using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foundation;
using UIKit;
using AVFoundation;
using Xamarin.Forms;
using decodeDemo.iOS;

[assembly: Dependency(typeof(TextToSpeech_iOS))]

namespace decodeDemo.iOS
{
    public class TextToSpeech_iOS : ITextToSpeech
    {
        public TextToSpeech_iOS() { }

        public void Speak(string text)
        {
            var speechSynthesizer = new AVSpeechSynthesizer();

            var speechUtterance = new AVSpeechUtterance(text)
            {
                Rate = AVSpeechUtterance.MaximumSpeechRate / 6,
                Voice = AVSpeechSynthesisVoice.FromLanguage("ja-JP"),
                Volume = 1.0f,
                PitchMultiplier = 1.0f
            };

            speechSynthesizer.SpeakUtterance(speechUtterance);
        }
    }
}