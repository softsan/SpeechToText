using System;
namespace SpeechToText
{
	public interface ISpeechToTextService
	{
		void Start(System.Action<EventArgsVoiceRecognition> handler,string Language);
		void Stop();
	}

	public interface IAudioRecorderService
	{
		void StartRecording();
		void StopRecording();
	}
}
