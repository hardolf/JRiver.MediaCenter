using System;
using System.Linq;

using MediaCenter.SharedComponents;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace MediaCenter.LyricsFinder.Tests.SharedComponents
{

    /// <summary>
    /// Unit tests of the exception extensions in <see cref="Utility"/>.
    /// </summary>
    /// <remarks>
    /// <para>These tests are hermetic: no network, no data file, no user configuration.</para>
    /// <para><c>InnerExceptionChain</c> is what lets the error dialog show every failing lyric service and
    /// not just the first one, see section 5.6 of the architecture report.</para>
    /// </remarks>
    [TestClass]
    public class UtilityTests
    {

        [TestMethod]
        public void InnerExceptionChain_WithAPlainChain_ReturnsTheInnerExceptionsInOrder()
        {
            var innermost = new InvalidOperationException("Innermost");
            var middle = new NotSupportedException("Middle", innermost);
            var outer = new Exception("Outer", middle);

            var chain = outer.InnerExceptionChain().ToList();

            Assert.AreEqual(2, chain.Count);
            Assert.AreSame(middle, chain[0]);
            Assert.AreSame(innermost, chain[1]);
        }


        [TestMethod]
        public void InnerExceptionChain_WithoutAnyInnerException_ReturnsNothing()
        {
            var chain = new Exception("Alone").InnerExceptionChain().ToList();

            Assert.AreEqual(0, chain.Count);
        }


        [TestMethod]
        public void InnerExceptionChain_WithAnAggregateException_ReturnsAllOfItsInnerExceptions()
        {
            var boom1 = new InvalidOperationException("Boom 1");
            var boom2 = new NotSupportedException("Boom 2");
            var boom3 = new TimeoutException("Boom 3");
            var aggregate = new AggregateException(boom1, boom2, boom3);

            var chain = aggregate.InnerExceptionChain().ToList();

            // The plain InnerException walk would have stopped after the first one.
            Assert.AreEqual(3, chain.Count);
            Assert.AreSame(boom1, chain[0]);
            Assert.AreSame(boom2, chain[1]);
            Assert.AreSame(boom3, chain[2]);
        }


        [TestMethod]
        public void InnerExceptionChain_WithANestedAggregateException_ReturnsEveryLevel()
        {
            // This is the shape the error dialog gets when several lyric services fail on the same song.
            var innermost = new TimeoutException("Innermost");
            var boom1 = new InvalidOperationException("Boom 1", innermost);
            var boom2 = new NotSupportedException("Boom 2");
            var aggregate = new AggregateException(boom1, boom2);
            var outer = new Exception("A lyric service failed.", aggregate);

            var chain = outer.InnerExceptionChain().ToList();

            Assert.AreEqual(4, chain.Count);
            Assert.AreSame(aggregate, chain[0]);
            Assert.AreSame(boom1, chain[1]);
            Assert.AreSame(boom2, chain[2]);
            Assert.AreSame(innermost, chain[3]);
        }


        [TestMethod]
        public void InnerExceptionChain_WithNull_ThrowsBeforeItIsEnumerated()
        {
            Assert.ThrowsExactly<ArgumentNullException>(() => { _ = ((Exception)null).InnerExceptionChain(); });
        }

    }

}
