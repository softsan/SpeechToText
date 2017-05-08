using System;
using System.Collections.Generic;
using SpeechToText.Helpers;
using Xamarin.Forms;

namespace SpeechToText
{
	public partial class InitializationPage : ContentPage
	{
		public InitializationPage()
		{
			InitializeComponent();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			if (!Settings.IsAuthenticated && !string.IsNullOrWhiteSpace(Settings.UserName))
			{
				NavService.SetRoot(new SecretSentencePage(), true);
			}
			else if (string.IsNullOrWhiteSpace(Settings.UserName))
			{
				NavService.SetRoot(new SignInPage(), true);
			}
			else
			{
				NavService.SetRoot(new HomePage(), true);
			}
		}
	}
}
