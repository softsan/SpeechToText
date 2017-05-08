using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using SpeechToText.Helpers;
using Xamarin.Forms;

namespace SpeechToText
{
	public partial class SignUpPage : ContentPage
	{

		private bool IsSecretSentenceButtonTapped;

		private string _language;
		public string Language
		{
			get { return _language; }
			set
			{
				_language = value;
				OnPropertyChanged("Language");
			}
		}

		private string _userName;
		public string UserName
		{
			get { return _userName; }
			set
			{
				_userName = value;
				OnPropertyChanged("UserName");
			}
		}

		private string _password;
		public string Password
		{
			get { return _password; }
			set
			{
				_password = value;
				OnPropertyChanged("Password");
			}
		}

		private string _fullName;
		public string FullName
		{
			get { return _fullName; }
			set
			{
				_fullName = value;
				OnPropertyChanged("FullName");
			}
		}

		private string _city;
		public string City
		{
			get { return _city; }
			set
			{
				_city = value;
				OnPropertyChanged("City");
			}
		}

		private string _secretSentence;
		public string SecretSentence
		{
			get { return _secretSentence; }
			set
			{
				_secretSentence = value;
				OnPropertyChanged("SecretSentence");
			}
		}

		private string _confirmSecretSentence;
		public string ConfirmSecretSentence
		{
			get { return _confirmSecretSentence; }
			set
			{
				_confirmSecretSentence = value;
				OnPropertyChanged("ConfirmSecretSentence");
			}
		}

		//IAudioRecorderService audioRecordService;
		//SpeakerRecognitionClient client = new SpeakerRecognitionClient();


		public SignUpPage()
		{
			InitializeComponent();
			BindingContext = this;
			NavigationPage.SetHasNavigationBar(this, false);
			//audioRecordService = DependencyService.Get<IAudioRecorderService>();

		}


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
	

		void OnSecretSentence(object sender, EventArgs e)
		{
			IsSecretSentenceButtonTapped = true;
			SpeechToText();
		}

		void OnConfirmSecretSentence(object sender, EventArgs e)
		{
			IsSecretSentenceButtonTapped = false;
            SpeechToText();
		}

		private async void SpeechToText()
		{
			if (string.IsNullOrWhiteSpace(Language))
			{
				await DisplayAlert("Error", "Please select a language first.", "OK");
				return;
			}

			var speechService = DependencyService.Get<ISpeechToTextService>();
			if (speechService != null)
			{
				//audioRecordService.StartRecording();
				speechService.Start(HandleAction, Language);

			}
		}


		void HandleAction(EventArgsVoiceRecognition obj)
		{
			if (IsSecretSentenceButtonTapped)
				SecretSentence = obj.Text;
			else
				ConfirmSecretSentence = obj.Text;

		//	audioRecordService.StopRecording();

		//	client.enrollSpeaker();
		}

		async void OnSignUp(object sender, EventArgs e)
		{
			if (!await ValidateData())
			{
				return;
			}

			var isValid = SecretSentence.Equals(ConfirmSecretSentence);
			if (!isValid)
			{
				await DisplayAlert("Error", "Secret sentence and confirm secret sentence does not match!", "OK");
				return;
			}
			var userservie = new UserController();

			IsBusy = true;
			var r = await userservie.GetByName(UserName, Password);
			if (r != null)
			{
				IsBusy = false;
				await DisplayAlert("Error", "User already exist!", "OK");
				return;
			}

			var result = await userservie.PostUser(new User() { 
				UserName = UserName,
				Password = Password,
				FullName= FullName,
				City=City,
				SecretSentence= SecretSentence
			
			});
			if (result != null)
			{
				IsBusy = false;
				Settings.IsAuthenticated = true;
				Settings.UserName = UserName;
				NavService.SetRoot(new HomePage(), true);
			}
			else
			{
				Settings.UserName = string.Empty;
				Settings.IsAuthenticated = false;

			}
			IsBusy = false;

		}

		void OnLanguage_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			var picker = sender as Picker;
			var selectedValue = picker.Items[picker.SelectedIndex];
			if (selectedValue == "English")
			{
				Language = "en-US";
				Settings.Language = Language;
			}
			else
			{
				Language = "de-DE";
				Settings.Language = Language;
			}
		}

		private async Task<bool> ValidateData()
		{
			bool isSuccess = true;
			if (string.IsNullOrWhiteSpace(UserName))
			{
				await DisplayAlert("Error", "Email is required", "OK");
				isSuccess = false;
			}

			else if (string.IsNullOrWhiteSpace(Password))
			{
				await DisplayAlert("Error", "Password is required", "OK");
				isSuccess = false;
			}
			else if (string.IsNullOrWhiteSpace(FullName))
			{
				await DisplayAlert("Error", "Full name is required", "OK");
				isSuccess = false;
			}
			else if (string.IsNullOrWhiteSpace(City))
			{
				await DisplayAlert("Error", "City is required", "OK");
				isSuccess = false;
			}


			return isSuccess;
		}
	}
}
