using System;
using System.Collections.Generic;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using SpeechToText.Helpers;
using Xamarin.Forms;

namespace SpeechToText
{
	public partial class SignInPage : ContentPage
	{
		
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

		private UserController userController;

		public SignInPage()
		{
			InitializeComponent();
			userController = new UserController();
			BindingContext = this;
			NavigationPage.SetHasNavigationBar(this, false);
		}



		async void OnSignIn(object sender, EventArgs e)
		{
			if (string.IsNullOrWhiteSpace(UserName))
			{
				await DisplayAlert("Error", "Username is required", "OK");
				return;
			}

			if (string.IsNullOrWhiteSpace(Password))
			{
				await DisplayAlert("Error", "Password is required", "OK");
				return;
			}

			//if (UserName.IsEmail())
			//{
			//	await DisplayAlert("Error", "Invalid Email", "OK");
			//	return;
			//}
			IsBusy = true;
			var result = await userController.GetByName(UserName, Password);
			if (result != null)
			{
				IsBusy = false;
				Settings.UserName = UserName;
				NavService.SetRoot(new HomePage(), true);

			}
			else
			{
				IsBusy = false;
				Settings.UserName = string.Empty;
				await DisplayAlert("Error", "Invalid credentials.", "OK");
			}

		}

		void OnSignUp(object sender, EventArgs e)
		{
			NavService.SetRoot(new SignUpPage(), true);
		}
	}
}
