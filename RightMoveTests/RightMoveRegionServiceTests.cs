using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace RightMove.Tests
{
    public class RightMoveRegionServiceTests
    {

        [Test]
        public void TestMethod()
        {
            var moveRegionService = new RightMoveRegionService();
            var val = moveRegionService.SearchAsync("London", new CancellationToken()).GetAwaiter().GetResult();
        }
    }
}
