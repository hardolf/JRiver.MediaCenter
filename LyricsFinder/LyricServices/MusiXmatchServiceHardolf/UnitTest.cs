using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using MediaCenter.LyricsFinder.Model.LyricServices;
using MediaCenter.LyricsFinder.Model.McRestService;


namespace MediaCenter.LyricsFinder.Model.LyricServices.Test
{

    [TestClass]
    public class UnitTest
    {

        private McMplItem _item;
        private MusiXmatchServiceHardolf _service;


        [TestCleanup]
        public void Cleanup()
        {
        }


        [TestInitialize]
        public void Init()
        {
            _item = new McMplItem
            {
                Artist = "Dire Straits",
                Name = "Sultans of Swing"
            };

            _service = new MusiXmatchServiceHardolf();
        }


        [TestMethod]
        public void TestMethod01()
        {
            var resultService = _service.Process(_item);

            Assert.IsNotNull(resultService);
            Assert.AreNotEqual(0, resultService.FoundLyricsList.Count);
            Assert.IsNotNull(resultService.FoundLyricsList[0]);
            Assert.IsNotNull(resultService.FoundLyricsList[0].LyricText);
            Assert.AreNotEqual(0, resultService.FoundLyricsList[0].LyricText.Trim().Length);
            Assert.IsTrue(resultService.FoundLyricsList[0].LyricCreditText.ToUpperInvariant().Contains("MUSIXMATCH"));
            Assert.IsTrue(resultService.FoundLyricsList[0].LyricCreditText.ToUpperInvariant().Contains("HARDOLF"));
        }

    }

}
