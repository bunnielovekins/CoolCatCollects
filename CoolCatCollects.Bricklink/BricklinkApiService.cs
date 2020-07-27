using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;

namespace CoolCatCollects.Bricklink
{
	/// <summary>
	/// Wrapper for sending requests to Bricklink API
	/// </summary>
	public class BricklinkApiService
	{
		/// <summary>
		/// Sends a get request, returns result as a string
		/// </summary>
		/// <param name="url">Url to send to, relative to the BL base url</param>
		/// <returns>The result, as a string</returns>
		public string GetRequest(string url)
		{
			var client = new RestClient(Statics.ApiUrl)
			{
				Authenticator = OAuth1Authenticator.ForProtectedResource(
					Statics.ConsumerKey,
					Statics.ConsumerSecret,
					Statics.TokenValue,
					Statics.TokenSecret
				)
			};

			var request = new RestRequest(url);

			var response = client.Execute(request);

			return response.Content;
		}

		/// <summary>
		/// Sends a get request and deserialises the result
		/// </summary>
		/// <typeparam name="T">Type to deserialise to</typeparam>
		/// <param name="url">Url to send to, relative to the BL base url</param>
		/// <returns>The result as type T</returns>
		public T GetRequest<T>(string url) where T : class
		{
			var str = GetRequest(url);

			return JsonConvert.DeserializeObject<T>(str);
		}
	}
}
