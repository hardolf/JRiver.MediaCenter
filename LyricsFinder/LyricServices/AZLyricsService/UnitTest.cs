using System;
using System.Globalization;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using MediaCenter.LyricsFinder.Model.LyricServices;
using MediaCenter.LyricsFinder.Model.McRestService;

namespace MediaCenter.LyricsFinder.Model.LyricServices.Test
{

    [TestClass]
    public class UnitTest
    {

        private McMplItem _item1, _item2;
        private AZLyricsService _service;


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
                Artist = "dire Straits",
                Name = "brothers in arms"
            };

            _service = new AZLyricsService();
            _service.DataDirectory = Environment.ExpandEnvironmentVariables(@"%USERPROFILE%\Documents\LyricsFinder");
            _service.RefreshServiceSettings();
        }


        [TestMethod]
        public async Task AZLyricsTestMethod01()
        {
            var resultService = await _service.Process(_item1).ConfigureAwait(false);

            Assert.IsNotNull(resultService);
            Assert.AreNotEqual(0, resultService.FoundLyricList.Count);
            Assert.IsNotNull(resultService.FoundLyricList[0]);
            Assert.IsNotNull(resultService.FoundLyricList[0].LyricText);
            Assert.AreNotEqual(0, resultService.FoundLyricList[0].LyricText.Trim().Length);
            Assert.IsTrue(resultService.FoundLyricList[0].LyricCreditText.ToUpperInvariant().Contains("AZLYRICS"));
        }


        [TestMethod]
        public async Task AZLyricsTestMethod02()
        {
            var resultService = await _service.Process(_item2).ConfigureAwait(false);

            Assert.IsNotNull(resultService);
            Assert.AreNotEqual(0, resultService.FoundLyricList.Count);
            Assert.IsNotNull(resultService.FoundLyricList[0]);
            Assert.IsNotNull(resultService.FoundLyricList[0].LyricText);
            Assert.AreNotEqual(0, resultService.FoundLyricList[0].LyricText.Trim().Length);
            Assert.IsTrue(resultService.FoundLyricList[0].LyricCreditText.ToUpperInvariant().Contains("AZLYRICS"));
        }

    }

}
