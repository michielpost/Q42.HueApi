using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using Q42.HueApi.Interfaces;

namespace Q42.HueApi.Tests
{
    [TestClass]
    public class CapabilitiesTest
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
        public async Task GetCapabilitiesTest()
        {
            var result = await _client.GetCapabilitiesAsync();

            Assert.IsNotNull(result);
        }

    }
}
