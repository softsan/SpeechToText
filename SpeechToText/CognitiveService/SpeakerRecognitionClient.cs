using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.SpeakerRecognition;
using Microsoft.ProjectOxford.SpeakerRecognition.Contract;
using Microsoft.ProjectOxford.SpeakerRecognition.Contract.Verification;

namespace SpeechToText
{
	public class SpeakerRecognitionClient
	{
		private string _subscriptionKey = "fe2c6ab154f34c82b1f405424894f5f5";
		private Guid _speakerId = Guid.Empty;
		private SpeakerVerificationServiceClient _serviceClient;


		public SpeakerRecognitionClient()
		{
			_serviceClient = new SpeakerVerificationServiceClient(_subscriptionKey);
			initializeSpeaker();
		}

		private async void initializeSpeaker()
		{

			if (_speakerId == Guid.Empty)
			{
				bool created = await createProfile();
				if (created)
				{
					//refreshPhrases();
				}
				//resetBtn.IsEnabled = false;
			}
			else
			{

				//refreshPhrases();

				//string remEnrollments = _storageHelper.readValue(MainWindow.SPEAKER_ENROLLMENTS);
				int _remainingEnrollments;
				//if (!Int32.TryParse(remEnrollments, out _remainingEnrollments))
				//{
				Profile profile = await _serviceClient.GetProfileAsync(_speakerId);
				_remainingEnrollments = profile.RemainingEnrollmentsCount;
				//_storageHelper.writeValue(MainWindow.SPEAKER_ENROLLMENTS, _remainingEnrollments.ToString());
				//}
				//string enrollmentStatus = _storageHelper.readValue(MainWindow.SPEAKER_ENROLLMENT_STATUS);
				//if (enrollmentStatus != null && !enrollmentStatus.Equals("Empty"))
				//{
				//resetBtn.IsEnabled = true;
				//}

				//setDisplayText(_remainingEnrollments.ToString(), _storageHelper.readValue(MainWindow.SPEAKER_PHRASE_FILENAME));
			}
		}

		public async Task<bool> createProfile()
		{
			//setStatus("Creating Profile...");
			try
			{

				CreateProfileResponse response = await _serviceClient.CreateProfileAsync("en-us");
				_speakerId = response.ProfileId;

				Profile profile = await _serviceClient.GetProfileAsync(_speakerId);

				//setDisplayText(profile.RemainingEnrollmentsCount.ToString(), "");

				//_storageHelper.writeValue(MainWindow.SPEAKER_ENROLLMENTS, profile.RemainingEnrollmentsCount.ToString());
				//_storageHelper.writeValue(MainWindow.SPEAKER_PHRASE_FILENAME, "");
				//_storageHelper.writeValue(MainWindow.SPEAKER_ENROLLMENT_STATUS, "Empty");

				return true;
			}
			catch (CreateProfileException exception)
			{
				Debug.WriteLine("Cannot create profile: " + exception.Message);
				return false;
			}
			catch (Exception gexp)
			{
				Debug.WriteLine("Error: " + gexp.Message);
				return false;
			}
		}


		public async void enrollSpeaker()
		{
			try
			{

				var file = await PCLStorage.FileSystem.Current.LocalStorage.GetFileAsync("Todo.wav");
				var fileStream = await file.OpenAsync(PCLStorage.FileAccess.Read);

				Enrollment response = await _serviceClient.EnrollAsync(fileStream, _speakerId);
				if (response.RemainingEnrollments == 0)
				{
					//MessageBox.Show("You have now completed the minimum number of enrollments. You may perform verification or add more enrollments", "Speaker enrolled");
					//_storageHelper.writeValue(MainWindow.SPEAKER_ENROLLMENT_STATUS, "Enrolled");
				}
				else
				{
					//_storageHelper.writeValue(MainWindow.SPEAKER_ENROLLMENT_STATUS, "Enrolling");
				}

				//resetBtn.IsEnabled = true;
			}
			catch (EnrollmentException exception)
			{
				Debug.WriteLine("Cannot enroll speaker: " + exception.Message);
			}
			catch (Exception gexp)
			{
				Debug.WriteLine("Error: " + gexp.Message);
			}
		}

		/// <summary>
		/// Verifies the speaker by using the audio
		/// </summary>
		/// <param name="audioStream">The audio stream</param>
		public async void verifySpeaker()
		{
			try
			{

				var file = await PCLStorage.FileSystem.Current.LocalStorage.GetFileAsync("Todo.wav");
				var fileStream = await file.OpenAsync(PCLStorage.FileAccess.Read);
				Verification response = await _serviceClient.VerifyAsync(fileStream, _speakerId);

				var result = response.Result.ToString();
				var confidence = response.Confidence.ToString();
				if (response.Result == Result.Accept)
				{

				}
				else
				{

				}
			}
			catch (VerificationException exception)
			{

			}
			catch (Exception e)
			{

			}
		}
	}
}
