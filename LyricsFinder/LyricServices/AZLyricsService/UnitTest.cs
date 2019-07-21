using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using MediaCenter.McWs;

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

            _service = (AZLyricsService)LyricsFinderDataType.GetLyricService<AZLyricsService>();
        }


        [TestMethod]
        public async Task AZLyricsTestMethod01()
        {
            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                var resultService = await _service.ProcessAsync(_item1, cancellationTokenSource.Token).ConfigureAwait(false);

                Assert.IsNotNull(resultService);
                Assert.AreNotEqual(0, resultService.FoundLyricList.Count);
                Assert.IsNotNull(resultService.FoundLyricList[0]);
                Assert.IsNotNull(resultService.FoundLyricList[0].LyricText);
                Assert.AreNotEqual(0, resultService.FoundLyricList[0].LyricText.Trim().Length);
                Assert.IsTrue(resultService.FoundLyricList[0].LyricCreditText.ToUpperInvariant().Contains("AZLYRICS"));
            }
        }


        [TestMethod]
        public async Task AZLyricsTestMethod02()
        {
            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                var resultService = await _service.ProcessAsync(_item2, cancellationTokenSource.Token).ConfigureAwait(false);

                Assert.IsNotNull(resultService);
                Assert.AreNotEqual(0, resultService.FoundLyricList.Count);
                Assert.IsNotNull(resultService.FoundLyricList[0]);
                Assert.IsNotNull(resultService.FoundLyricList[0].LyricText);
                Assert.AreNotEqual(0, resultService.FoundLyricList[0].LyricText.Trim().Length);
                Assert.IsTrue(resultService.FoundLyricList[0].LyricCreditText.ToUpperInvariant().Contains("AZLYRICS")); 
            }
        }

    }

}
