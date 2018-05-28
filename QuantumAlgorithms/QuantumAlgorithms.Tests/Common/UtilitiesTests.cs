using System.Linq;
using NUnit.Framework;
using QuantumAlgorithms.Common;

namespace QuantumAlgorithms.Tests.Common
{
    [TestFixture]
    public class UtilitiesTests
    {
        [Test]
        public void IsEvenTest_1()
        {
            Assert.IsTrue(Utilities.IsEven(4));
        }

        [Test]
        public void IsEvenTest_2()
        {
            Assert.IsFalse(Utilities.IsEven(5));
        }

        [Test]
        public void GCDTest()
        {
            Assert.AreEqual(9, Utilities.GCD(63, 18));
        }

        [Test]
        public void IsCoprimeTest_1()
        {
            Assert.IsTrue(Utilities.IsCoprime(63, 17));
        }
        [Test]
        public void IsCoprimeTest_2()
        {
            Assert.IsFalse(Utilities.IsCoprime(63, 9));
        }

        [Test]
        public void BitSizeTest()
        {
            Assert.AreEqual(10, Utilities.BitSize(512));
        }

        [Test]
        public void BinaryIntegerRepresentationTest()
        {
            Assert.AreEqual(true, Utilities.BinaryIntegerRepresentation(7, 8).ToArray()[0]);
            Assert.AreEqual(true, Utilities.BinaryIntegerRepresentation(7, 8).ToArray()[1]);
            Assert.AreEqual(true, Utilities.BinaryIntegerRepresentation(7, 8).ToArray()[2]);
            Assert.AreEqual(false, Utilities.BinaryIntegerRepresentation(7, 8).ToArray()[3]);
            Assert.AreEqual(false, Utilities.BinaryIntegerRepresentation(7, 8).ToArray()[4]);
            Assert.AreEqual(false, Utilities.BinaryIntegerRepresentation(7, 8).ToArray()[5]);
            Assert.AreEqual(false, Utilities.BinaryIntegerRepresentation(7, 8).ToArray()[6]);
            Assert.AreEqual(false, Utilities.BinaryIntegerRepresentation(7, 8).ToArray()[7]);
        }

        [Test]
        public void CalculateExpModTest()
        {
            Assert.AreEqual(7, Utilities.CalculateExpMod(7, 13, 57));
        }
    }
}