using System;
using Xamarin.Forms;

namespace SpeechToText
{
	/// <summary>
	/// Utility for setting the root page
	/// </summary>
	public static class NavService
	{
		private static System.Action<Page> _factory;
		/// <summary>
		/// Init with a function that sets the root page
		/// </summary>
		/// <param name="factoryMethod"></param>
		public static void Init(System.Action<Page> factoryMethod)
		{
			_factory = factoryMethod;
		}

		/// <summary>
		/// Sets the root view
		/// </summary>
		/// <param name="target"></param>
		/// <param name="addNavigation"></param>
		public static void SetRoot(Page target, bool addNavigation)
		{
			try
			{
				if (addNavigation)
					_factory(new NavigationPage(target));
				else
					_factory(target);
			}
			catch (Exception ex)
			{
				
			}
		}
	}
}
