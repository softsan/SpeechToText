using System;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Speech;
using SpeechToText.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(SpeechToTextService))]
namespace SpeechToText.Droid
{
	public class SpeechToTextService :Java.Lang.Object, ISpeechToTextService,IRecognitionListener
	{
		private SpeechRecognizer speechRecognizer;

		private Intent speechIntent;
		private Action<EventArgsVoiceRecognition> _callback;


		public SpeechToTextService()
		{
		}

		public void OnBeginningOfSpeech()
		{
			
		}

		public void OnBufferReceived(byte[] buffer)
		{
			
		}

		public void OnEndOfSpeech()
		{
		}

		public void OnError([GeneratedEnum] SpeechRecognizerError error)
		{
			
		}

		public void OnEvent(int eventType, Bundle @params)
		{
			
		}

		public void OnPartialResults(Bundle partialResults)
		{
			
		}

		public void OnReadyForSpeech(Bundle @params)
		{

		}

		public void OnResults(Bundle results)
		{
			var matches = results.GetStringArrayList(Android.Speech.SpeechRecognizer.ResultsRecognition);
            if (matches == null)
            {
				Console.WriteLine("Matches value is null in bundle");
            }
            else
            {
				Console.WriteLine("Matches found: " + matches.Count);
				if (matches.Count > 0)
				{
					var result = matches[0];
					if (_callback != null)
                            _callback(new EventArgsVoiceRecognition(result, true));
				}
				   
			}
		}

		public void OnRmsChanged(float rmsdB)
		{

		}

		public void Start(Action<EventArgsVoiceRecognition> handler, string Language)
		{
			_callback = handler;

			speechRecognizer = SpeechRecognizer.CreateSpeechRecognizer(MainActivity.Instance);
			speechRecognizer.SetRecognitionListener(this);
			speechIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
			speechIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);


			speechIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 3);
			speechIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 1000);
			speechIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 1000);
			speechIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 1000);
			if (Language == "en-US")
				speechIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.Default);
			else if (Language == "de-DE")
				speechIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.German);

			speechRecognizer.StartListening(speechIntent);

		}

		public void Stop()
		{
			speechRecognizer.StopListening();
		}
	}
}
