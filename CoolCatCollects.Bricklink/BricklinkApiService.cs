using CoolCatCollects.Bricklink.Models;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;

namespace CoolCatCollects.Bricklink
{
	public class BricklinkApiService
	{
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

		public T GetRequest<T>(string url) where T : class
		{
			var str = GetRequest(url);

			return JsonConvert.DeserializeObject<T>(str);
		}
	}
}
