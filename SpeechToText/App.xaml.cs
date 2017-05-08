using SpeechToText.Helpers;
using Xamarin.Forms;

namespace SpeechToText
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();


		}

		protected override void OnStart()
		{
			Settings.IsAuthenticated = false;
			 //config View Service
            NavService.Init(page =>
            {
                MainPage = page;
            });

            //set init page
            NavService.SetRoot(new InitializationPage(), false);
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
			Settings.IsAuthenticated = false;
		}
	}
}
