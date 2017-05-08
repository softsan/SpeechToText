using System;
using System.Collections.Generic;
using SpeechToText.Helpers;
using Xamarin.Forms;

namespace SpeechToText
{
	public partial class HomePage : ContentPage
	{
		public HomePage()
		{
			InitializeComponent();
		}

		void OnLogout(object sender, EventArgs e)
		{
			Settings.IsAuthenticated = false;
			Settings.UserName = string.Empty;

			NavService.SetRoot(new SignInPage(), true);
		}
	}
}
