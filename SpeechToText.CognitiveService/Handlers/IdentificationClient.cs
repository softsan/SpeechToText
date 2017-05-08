using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.SpeakerRecognition;
using Microsoft.ProjectOxford.SpeakerRecognition.Contract;
using Microsoft.ProjectOxford.SpeakerRecognition.Contract.Verification;

namespace SpeechToText.CognitiveService
{
	public class IdentificationClient
	{
		private string _subscriptionKey = "fe2c6ab154f34c82b1f405424894f5f5";
		private Guid _speakerId = Guid.Empty;
		private SpeakerVerificationServiceClient _serviceClient;


		public IdentificationClient()
		{
			 _serviceClient = new SpeakerVerificationServiceClient(_subscriptionKey);
		}

		private async Task<bool> createProfile()
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
				//setStatus("Cannot create profile: " + exception.Message);
				return false;
			}
			catch (Exception gexp)
			{
				//setStatus("Error: " + gexp.Message);
				return false;
			}
		}


		private async void enrollSpeaker(Stream audioStream)
		{
			try
			{
				
				Enrollment response = await _serviceClient.EnrollAsync(audioStream, _speakerId);
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
				//setStatus("Cannot enroll speaker: " + exception.Message);
			}
			catch (Exception gexp)
			{
				//setStatus("Error: " + gexp.Message);
			}
		}

		/// <summary>
		/// Verifies the speaker by using the audio
		/// </summary>
		/// <param name="audioStream">The audio stream</param>
		private async void verifySpeaker(Stream audioStream)
		{
			try
			{
 				Verification response = await _serviceClient.VerifyAsync(audioStream, _speakerId);
				 
				var result   = response.Result.ToString();
				var confidence  = response.Confidence.ToString();
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
