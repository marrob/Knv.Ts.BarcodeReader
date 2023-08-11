
namespace Knv.Ts.BarcodeReader
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using NUnit.Framework;

    [TestFixture]
    internal class Reader_UnitTest
    {

        [Test]
        public void FakeReader()
        {
            var resp = BarcodReader.Read("", "", 1, true);
            Assert.AreEqual("1234567890", resp);
        }


        [Test]
        public void ECUTSReader()
        {
            var resp = BarcodReader.Read(resource:"169.254.160.42", type: "LAN", timeoutMs: 2000, simulation: false);
            Assert.AreEqual("TT21460200002", resp);
        }
    }
}
