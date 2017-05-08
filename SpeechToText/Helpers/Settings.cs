// Helpers/Settings.cs
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace SpeechToText.Helpers
{
	/// <summary>
	/// This is the Settings static class that can be used in your Core solution or in any
	/// of your client applications. All settings are laid out the same exact way with getters
	/// and setters. 
	/// </summary>
	public static class Settings
	{
		private static ISettings AppSettings
		{
			get
			{
				return CrossSettings.Current;
			}
		}

		#region Setting Constants

		private const string UserNameKey = "username_key";
		private static readonly string UserNameDefault = string.Empty;

		private const string LanguageKey = "language_key";
		private static readonly string LanguageDefault = "en-US";

		private const string IsAuthenticatedKey = "isauthenticated_key";
		private static readonly bool IsAuthenticatedDefault = false;
		#endregion


		public static string UserName
		{
			get
			{
				return AppSettings.GetValueOrDefault<string>(UserNameKey, UserNameDefault);
			}
			set
			{
				AppSettings.AddOrUpdateValue<string>(UserNameKey, value);
			}
		}

		public static string Language
		{
			get
			{
				return AppSettings.GetValueOrDefault<string>(LanguageKey, LanguageDefault);
			}
			set
			{
				AppSettings.AddOrUpdateValue<string>(LanguageKey, value);
			}
		}

		public static bool IsAuthenticated
		{
			get
			{
				return AppSettings.GetValueOrDefault<bool>(IsAuthenticatedKey, IsAuthenticatedDefault);
			}
			set
			{
				AppSettings.AddOrUpdateValue<bool>(IsAuthenticatedKey, value);
			}
		}

	}
}