using System;
using System.Collections.Generic;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using SpeechToText.Helpers;
using Xamarin.Forms;

namespace SpeechToText
{
	public partial class SecretSentencePage : ContentPage
	{
		private string _secretSentence;
		public string SecretSentence
		{
			get
			{
				return _secretSentence;
			}
			set
			{
				_secretSentence = value;
				OnPropertyChanged();
			}
		}


		private string _authenticateMessage;
		public string AuthenticateMessage
		{
			get
			{
				return _authenticateMessage;
			}
			set
			{
				_authenticateMessage = value;
				OnPropertyChanged();
			}
		}

		private string _secretImage;
		public string SecretImage
		{
			get
			{
				return _secretImage;
			}
			set
			{
				_secretImage = value;
				OnPropertyChanged();
			}
		}

		private string _secretSmiley;
		public string SecretSmiley
		{
			get
			{
				return _secretSmiley;
			}
			set
			{
				_secretSmiley = value;
				OnPropertyChanged();
			}
		}

		public SecretSentencePage()
		{
			InitializeComponent();
			BindingContext = this;
			//audioRecordService = DependencyService.Get<IAudioRecorderService>();
		}

		//IAudioRecorderService audioRecordService;
		SpeakerRecognitionClient client = new SpeakerRecognitionClient();

		protected override async void OnAppearing()
		{
			base.OnAppearing();
			try
			{
				if ((await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Microphone)) != PermissionStatus.Granted)
					await CrossPermissions.Current.RequestPermissionsAsync(Permission.Microphone);
				if ((await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage)) != PermissionStatus.Granted)
					await CrossPermissions.Current.RequestPermissionsAsync(Permission.Storage);
				if ((await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Photos)) != PermissionStatus.Granted)
					await CrossPermissions.Current.RequestPermissionsAsync(Permission.Photos);
			}
			catch (Exception ex)
			{
				 
				return;
			}
		}

		async void OnRecognizeSpeechButtonClicked(object sender, EventArgs e)
		{
			SecretSmiley = string.Empty;
			AuthenticateMessage = string.Empty;
			SecretSentence = string.Empty;
			SecretImage = string.Empty;

			var speechService = DependencyService.Get<ISpeechToTextService>();
			if (speechService != null)
			{
				speechService.Start(HandleAction, Settings.Language);
				 
			}
		}

		void HandleAction(EventArgsVoiceRecognition obj)
		{
			SecretSentence = obj.Text;
			Authenticate();
		}


		async void OnSubmit(object sender, System.EventArgs e)
		{
			if (string.IsNullOrWhiteSpace(SecretSentence))
			{
				await DisplayAlert("Error", "Secret sentence is required!", "OK");
				return;
			}

			if (!Settings.IsAuthenticated)
			{
				await DisplayAlert("Error", "You could not be authenticated.", "OK");
				return;
			}

			NavService.SetRoot(new HomePage(), true);

		}

		async void Authenticate()
		{
			if (string.IsNullOrWhiteSpace(SecretSentence))
			{
				await DisplayAlert("Error", "Secret sentence is required!", "OK");
				return;
			}


			var service = new UserController();
			IsBusy = true;
			var result = await service.GetBySecret(Settings.UserName, SecretSentence);
			if (result != null)
			{
				IsBusy = false;

				Settings.IsAuthenticated = true;
				AuthenticateMessage = " You are authenticated.";
				SecretSmiley = "happysmiley.png";
				SecretImage = "greencheck.png";

			}
			else
			{
				Settings.IsAuthenticated = false;
				AuthenticateMessage = " You could not be authenticated.";
				SecretSmiley = "sadsmiley.png";
				SecretImage = "wrongred.png";
			}
			IsBusy = false;
		}
	}
}