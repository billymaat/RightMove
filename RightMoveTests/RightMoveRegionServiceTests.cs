using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace RightMove.Tests
{
    public class RightMoveRegionServiceTests
    {

        [Test]
        public void TestMethod()
        {
            var rightMoveRegioNService = new RightMoveRegionService();
            var val = rightMoveRegioNService.Search("London").GetAwaiter().GetResult();
        }
    }
}
