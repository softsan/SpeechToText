using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ModernHttpClient;
using Newtonsoft.Json;

namespace SpeechToText
{
	public class UserController
	{
		HttpClient client;

		private const string BaseUrl = "http://sajservice.azurewebsites.net/api/user";

		public UserController()
		{
			client = new HttpClient();
		}

		public async Task<IEnumerable<User>> GetAll()
		{
			try
			{
				using (client = new HttpClient(new NativeMessageHandler()))
				{
					var result = await client.GetStringAsync(BaseUrl);
					var response = JsonConvert.DeserializeObject<IEnumerable<User>>(result);
					return response;
				}
			}
			catch (Exception ex)
			{
				return null;
			}
		}

		public async Task<User> GetByName(string username,string password)
		{
			
			try
			{
				using (client = new HttpClient(new NativeMessageHandler()))
				{
					var result = await client.GetStringAsync(BaseUrl + "/byName?username=" + username + "&password=" + password);
					var response = JsonConvert.DeserializeObject<User>(result);
					return response;
				}
			}
			catch (Exception ex)
			{
				return null;
			}
		}

		public async Task<User> GetBySecret(string username, string secret)
		{
			try
			{
				using (client = new HttpClient(new NativeMessageHandler()))
				{
					var result = await client.GetStringAsync(BaseUrl + "/byName?username=" + username + "&secret=" + secret);
					var response = JsonConvert.DeserializeObject<User>(result);
					return response;
				}
			}
			catch (Exception ex)
			{
				return null;
			}
		}

		public async Task<User> PostUser(User user)
		{
			try
			{
				using (client = new HttpClient(new NativeMessageHandler()))
				{
					var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
					var response = await client.PostAsync(BaseUrl, content);
					return JsonConvert.DeserializeObject<User>(await response.Content.ReadAsStringAsync());
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message + "\n" + ex.StackTrace);
				return null;
			}
		}

	}

	public class User
	{
		public long UserId { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
		public string FullName { get; set; }
		public string City { get; set; }
		public string SecretSentence { get; set; }
	}
}
