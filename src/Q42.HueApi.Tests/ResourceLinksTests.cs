using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using Q42.HueApi.Interfaces;
using Q42.HueApi.Models;
using System.Globalization;
using System.Net.Http;
using System.Dynamic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Q42.HueApi.Tests
{
	[TestClass]
	public class ResourceLinkTests
	{
		private IHueClient _client;

		[TestInitialize]
		public void Initialize()
		{
			string ip = ConfigurationManager.AppSettings["ip"].ToString();
			string key = ConfigurationManager.AppSettings["key"].ToString();

			_client = new LocalHueClient(ip, key);
		}

		[TestMethod]
		public async Task GetAll()
		{
			var result = await _client.GetResourceLinksAsync();

			Assert.AreNotEqual(0, result.Count);
		}

		[TestMethod]
		public async Task GetSingle()
		{
			var all = await _client.GetResourceLinksAsync();

			Assert.IsNotNull(all);
			Assert.IsTrue(all.Any());

			var single = await _client.GetResourceLinkAsync(all.First().Id);

			Assert.IsNotNull(single);
		}



		[TestMethod]
		public async Task CreateResourceLinkSingle()
		{
			ResourceLink resourceLink = new ResourceLink();
			resourceLink.Name = "r1";
			resourceLink.Description = "test";
			//resourceLink.ClassId = 1;

			var result = await _client.CreateResourceLinkAsync(resourceLink);

			Assert.IsNotNull(result);
		}



		[TestMethod]
		public async Task UpdateResourceLink()
		{
			ResourceLink resourceLink = new ResourceLink();
			resourceLink.Name = "t1";

			var ResourceLinkId = await _client.CreateResourceLinkAsync(resourceLink);

			//Update name
			resourceLink.Name = "t2";
			await _client.UpdateResourceLinkAsync(ResourceLinkId, resourceLink);

			//Get saved ResourceLink
			var savedResourceLink = await _client.GetResourceLinkAsync(ResourceLinkId);

			//Check 
			Assert.AreEqual(resourceLink.Name, savedResourceLink.Name);

		}

		[TestMethod]
		public async Task DeleteResourceLink()
		{
			ResourceLink resourceLink = new ResourceLink();
			resourceLink.Name = "t1";
			resourceLink.Description = "test";

			var resourceLinkId = await _client.CreateResourceLinkAsync(resourceLink);

			//Delete
			await _client.DeleteResourceLinkAsync(resourceLinkId);

			var exits = await _client.GetResourceLinkAsync(resourceLinkId);
			Assert.IsNull(exits);

		}


	}
}
