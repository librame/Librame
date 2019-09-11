﻿using Xunit;

namespace Librame.Extensions.Core.Tests
{
    public class CharBufferExtensionsTests
    {
        [Fact]
        public void SplitKeyValueStringByIndexOfTest()
        {
            var str = "123456";
            var buffer = (CharMemoryBuffer)str;

            // Change
            buffer.ChangeMemory("123:456=789:987".ToCharArray());
            Assert.NotEqual(str.Length, buffer.Memory.Length);

            // IndexOf: char separator
            var pair = buffer.SplitKeyValueByIndexOf(':');
            Assert.Equal("123", pair.Key);
            Assert.Equal("456=789:987", pair.Value);

            // LastIndexOf: char separator
            pair = buffer.SplitKeyValueByLastIndexOf(':');
            Assert.Equal("123:456=789", pair.Key);
            Assert.Equal("987", pair.Value);

            // IndexOf: string separator
            pair = buffer.SplitKeyValueByIndexOf(":456=789:");
            Assert.Equal("123", pair.Key);
            Assert.Equal("987", pair.Value);

            // LastIndexOf: string separator
            pair = buffer.SplitKeyValueByLastIndexOf("=789:987");
            Assert.Equal("123:456", pair.Key);
            Assert.Empty(pair.Value);
        }

    }
}
