using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Xml;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using MediaCenter.LyricsFinder.Model.LyricServices;
using MediaCenter.LyricsFinder.Model.McRestService;
using MediaCenter.SharedComponents;

namespace MediaCenter.LyricsFinder.Model.LyricServices.Test
{

    [TestClass]
    public class UnitTest
    {

        private McMplItem _item1, _item2, _item3;
        private ApiseedsService _service;


        [TestCleanup]
        public void Cleanup()
        {
        }


        [TestInitialize]
        public void Init()
        {
            _item1 = new McMplItem
            {
                Artist = "Dire Straits",
                Name = "Sultans of Swing"
            };

            _item2 = new McMplItem
            {
                Artist = "Eliza Gilkyson",
                Name = "xxx" // Does not exist
            };

            _item3 = new McMplItem
            {
                Artist = "xxx", // Does not exist
                Name = "yyy" // Does not exist
            };

            _service = (ApiseedsService)LyricsFinderDataType.GetLyricService<ApiseedsService>();
        }


        [TestMethod]
        public async Task ApiseedsTestMethod01()
        {
            var resultService = await _service.ProcessAsync(_item1).ConfigureAwait(false);

            Assert.IsNotNull(resultService);
            Assert.AreNotEqual(0, resultService.FoundLyricList.Count);
            Assert.IsNotNull(resultService.FoundLyricList[0]);
            Assert.IsNotNull(resultService.FoundLyricList[0].LyricText);
            Assert.AreNotEqual(0, resultService.FoundLyricList[0].LyricText.Trim().Length);
            Assert.IsTrue(resultService.FoundLyricList[0].LyricCreditText.ToUpperInvariant().Contains("APISEEDS"));
        }


        [TestMethod]
        public async Task ApiseedsTestMethod02()
        {
            var resultService = await _service.ProcessAsync(_item2).ConfigureAwait(false);

            Assert.IsNotNull(resultService);
            Assert.AreEqual(0, resultService.FoundLyricList.Count);
            Assert.AreEqual(resultService.LyricResult, LyricResultEnum.NotFound);
        }


        [TestMethod]
        public async Task ApiseedsTestMethod03()
        {
            var resultService = await _service.ProcessAsync(_item3).ConfigureAwait(false);

            Assert.IsNotNull(resultService);
            Assert.AreEqual(0, resultService.FoundLyricList.Count);
            Assert.AreEqual(resultService.LyricResult, LyricResultEnum.NotFound);
        }

    }

}
