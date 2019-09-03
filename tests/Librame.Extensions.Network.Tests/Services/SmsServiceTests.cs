﻿using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Network.Tests
{
    public class SmsServiceTests
    {
        private ISmsService _service;

        public SmsServiceTests()
        {
            _service = TestServiceProvider.Current.GetRequiredService<ISmsService>();
        }


        [Fact]
        public void SendAsyncTest()
        {
            Assert.ThrowsAsync<System.Net.WebException>(() =>
            {
                return _service.SendAsync("TestData: 123456");
            });
        }

    }
}