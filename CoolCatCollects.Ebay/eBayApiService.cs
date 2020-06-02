using CoolCatCollects.Ebay.Models.Responses;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;

namespace CoolCatCollects.Ebay
{
	public class eBayApiService
	{
		private const string ContentType = "application/x-www-form-urlencoded";
		private const string RefreshToken = "v^1.1#i^1#p^3#r^1#I^3#f^0#t^Ul4xMF84OjgxNTA5MDlBRUM0Njk5Rjk4RUQ4NjBGNTNDMkQ1MTZGXzBfMSNFXjI2MA==";
		private const string Scope = "https://api.ebay.com/oauth/api_scope https://api.ebay.com/oauth/api_scope/sell.marketing.readonly https://api.ebay.com/oauth/api_scope/sell.marketing https://api.ebay.com/oauth/api_scope/sell.inventory.readonly https://api.ebay.com/oauth/api_scope/sell.inventory https://api.ebay.com/oauth/api_scope/sell.account.readonly https://api.ebay.com/oauth/api_scope/sell.account https://api.ebay.com/oauth/api_scope/sell.fulfillment.readonly https://api.ebay.com/oauth/api_scope/sell.fulfillment https://api.ebay.com/oauth/api_scope/sell.analytics.readonly https://api.ebay.com/oauth/api_scope/sell.finances https://api.ebay.com/oauth/api_scope/sell.payment.dispute https://api.ebay.com/oauth/api_scope/commerce.identity.readonly";
		private const string ApiUrl = "https://api.ebay.com/";

		private static string AccessToken = "";

		public void Authorise()
		{
			var client = new RestClient(ApiUrl)
			{
				Authenticator = new HttpBasicAuthenticator("PeterSto-PackingS-PRD-2bcdaddba-d636f456", "PRD-c545f399ba93-1a8c-49fd-a302-2de4")
			};

			var request = new RestRequest("identity/v1/oauth2/token", Method.POST);

			request.AddHeader("Content-Type", ContentType);
			request.AddParameter("grant_type", "refresh_token");
			request.AddParameter("refresh_token", RefreshToken);
			request.AddParameter("scope", Scope);

			var response = client.Execute(request);

			if (response.StatusCode != System.Net.HttpStatusCode.OK)
			{
				throw new Exception("Authentication failed!");
			}

			var obj = JsonConvert.DeserializeObject<AuthResponseModel>(response.Content);

			AccessToken = obj.access_token;
		}

		public string GetRequest(string url)
		{
			if (String.IsNullOrEmpty(AccessToken))
			{
				Authorise();
			}

			var client = new RestClient(ApiUrl);

			client.AddDefaultHeader("Authorization", $"Bearer {AccessToken}");

			var request = new RestRequest(url);

			var response = client.Execute(request);

			if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
			{
				Authorise();

				client = new RestClient(ApiUrl)
				{
					Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(AccessToken)
				};

				request = new RestRequest(url);

				response = client.Execute(request);
			}

			return response.Content;
		}
	}
}
