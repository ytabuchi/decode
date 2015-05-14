using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.SpeechSynthesis;
using Xamarin.Forms;
using decode01.Windows81;
using Windows.UI.Xaml.Controls;

[assembly: Dependency(typeof(TextToSpeech_Windows81))]

namespace decode01.Windows81
{
    class TextToSpeech_Windows81 : ITextToSpeech
    {
        public TextToSpeech_Windows81() { }

        public async void Speak(string text)
        {
            SpeechSynthesisStream stream;
            // MediaElement をインスタンス化
            var media = new MediaElement();

            // SpeechSynthesizerオブジェクトを作る
            using (var synth = new Windows.Media.SpeechSynthesis.SpeechSynthesizer())
            {
                // プレーン・テキストから合成音声ストリームを生成する
                stream = await synth.SynthesizeTextToStreamAsync(text);
            }

            // ストリームをMediaElement のインスタンスに渡して再生させる
            media.SetSource(stream, stream.ContentType);
            media.Play();

        }
    }
}
