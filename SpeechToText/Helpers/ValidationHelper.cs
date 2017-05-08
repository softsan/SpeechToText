using System;
using System.Text.RegularExpressions;

namespace SpeechToText
{
	public static class ValidationHelper
	{
		/// <summary>
		/// Returns true if the input string is a valid System.Net.Mail email address
		/// </summary>
		/// <param name="emailaddress">the string to validate</param>
		/// <returns>true if a valid email</returns>
		public static bool IsEmail(this string emailaddress)
		{
			if (string.IsNullOrEmpty(emailaddress))
				return false;

			//http://stackoverflow.com/questions/16167983/best-regular-expression-for-email-validation-in-c-sharp
			return Regex.IsMatch(emailaddress, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
		}

	}

}
