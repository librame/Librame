using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Librame.Extensions.Data.Tests
{
    using Collections;

    public class PagingTests
    {
        [Fact]
        public void AllTest()
        {
            var list = new List<TestPaging>();

            for (var i = 0; i < 99; i++)
            {
                list.Add(new TestPaging
                {
                    Id = i + 1,
                    Name = nameof(PagingTests) + (i + 1)
                });
            }

            var paging = list.AsPagingByIndex(2, 20);
            Assert.Equal(20, paging.Size);
            Assert.Equal(21, paging.First().Id);
        }

    }
}
