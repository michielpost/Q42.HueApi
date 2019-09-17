using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Q42.HueApi.Interfaces;
using Q42.HueApi.Models;
using Q42.HueApi.Models.Groups;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Q42.HueApi
{
	/// <summary>
	/// Partial HueClient, contains requests to the /resourcelinks/ url
	/// </summary>
	public partial class HueClient : IHueClient_ResourceLinks
  {

		/// <summary>
		/// Deletes a single ResourceLink
		/// </summary>
		/// <param name="resourceLinkId"></param>
		/// <returns></returns>
		public async Task<IReadOnlyCollection<DeleteDefaultHueResult>> DeleteResourceLinkAsync(string resourceLinkId)
		{
			CheckInitialized();

			HttpClient client = await GetHttpClient().ConfigureAwait(false);
			//Delete resource link 1
			var result = await client.DeleteAsync(new Uri(ApiBase + string.Format("resourcelinks/{0}", resourceLinkId))).ConfigureAwait(false);

			string jsonResult = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

			return DeserializeDefaultHueResult<DeleteDefaultHueResult>(jsonResult);

		}


		/// <summary>
		/// Get all ResourceLinks
		/// </summary>
		/// <returns></returns>
		public async Task<IReadOnlyCollection<ResourceLink>> GetResourceLinksAsync()
		{
			CheckInitialized();

			HttpClient client = await GetHttpClient().ConfigureAwait(false);
			string stringResult = await client.GetStringAsync(new Uri(String.Format("{0}resourcelinks", ApiBase))).ConfigureAwait(false);

			List<ResourceLink> results = new List<ResourceLink>();

			JToken token = JToken.Parse(stringResult);
			if (token.Type == JTokenType.Object)
			{
				//Each property is a light
				var jsonResult = (JObject)token;

				foreach (var prop in jsonResult.Properties())
				{
					ResourceLink newResourceLink = JsonConvert.DeserializeObject<ResourceLink>(prop.Value.ToString());
					newResourceLink.Id = prop.Name;

					results.Add(newResourceLink);
				}

			}

			return results;

		}

		/// <summary>
		/// Get the state of a single ResourceLink
		/// </summary>
		/// <returns></returns>
		public async Task<ResourceLink?> GetResourceLinkAsync(string id)
		{
			CheckInitialized();

			HttpClient client = await GetHttpClient().ConfigureAwait(false);
			string stringResult = await client.GetStringAsync(new Uri(String.Format("{0}resourcelinks/{1}", ApiBase, id))).ConfigureAwait(false);

			ResourceLink? resourceLink = DeserializeResult<ResourceLink>(stringResult);

			if (resourceLink != null && string.IsNullOrEmpty(resourceLink.Id))
				resourceLink.Id = id;

			return resourceLink;


		}

		/// <summary>
		/// Create a ResourceLink
		/// </summary>
		/// <param name="ResourceLink"></param>
		/// <returns></returns>
		public async Task<string?> CreateResourceLinkAsync(ResourceLink resourceLink)
		{
			CheckInitialized();

			string command = JsonConvert.SerializeObject(resourceLink, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });

			HttpClient client = await GetHttpClient().ConfigureAwait(false);

			//Create ResourceLink
			var result = await client.PostAsync(new Uri(ApiBase + "resourcelinks"), new JsonContent(command)).ConfigureAwait(false);

			var jsonResult = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

			DefaultHueResult[] resourceLinkResult = JsonConvert.DeserializeObject<DefaultHueResult[]>(jsonResult);

			if (resourceLinkResult.Length > 0 && resourceLinkResult[0].Success != null && !string.IsNullOrEmpty(resourceLinkResult[0].Success.Id))
			{
				return resourceLinkResult[0].Success.Id;
			}

			return null;
		}

		/// <summary>
		/// Update a ResourceLink
		/// </summary>
		/// <param name="id"></param>
		/// <param name="ResourceLink"></param>
		/// <returns></returns>
		public async Task<HueResults> UpdateResourceLinkAsync(string id, ResourceLink resourceLink)
		{
			CheckInitialized();

			string command = JsonConvert.SerializeObject(resourceLink, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });

			HttpClient client = await GetHttpClient().ConfigureAwait(false);

			//Create ResourceLink
			var result = await client.PutAsync(new Uri(string.Format("{0}resourcelinks/{1}", ApiBase, id)), new JsonContent(command)).ConfigureAwait(false);

			var jsonResult = await result.Content.ReadAsStringAsync().ConfigureAwait(false);

			return DeserializeDefaultHueResult(jsonResult);

		}


	}
}
