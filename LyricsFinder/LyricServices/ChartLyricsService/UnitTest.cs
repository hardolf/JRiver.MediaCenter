using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using MediaCenter.LyricsFinder.Model.LyricServices;
using MediaCenter.McWs;

namespace MediaCenter.LyricsFinder.Model.LyricServices.Test
{

    [TestClass]
    public class UnitTest
    {

        private McMplItem _item;
        private ChartLyricsService _service;


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

            _service = (ChartLyricsService)LyricsFinderDataType.GetLyricService<ChartLyricsService>();
        }


        [TestMethod]
        public async Task ChartLyricsTestMethod01()
        {
            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                var resultService = await _service.ProcessAsync(_item, cancellationTokenSource.Token).ConfigureAwait(false);

                Assert.IsNotNull(resultService);
                Assert.AreNotEqual(0, resultService.FoundLyricList.Count);
                Assert.IsNotNull(resultService.FoundLyricList[0]);
                Assert.IsNotNull(resultService.FoundLyricList[0].LyricText);
                Assert.AreNotEqual(0, resultService.FoundLyricList[0].LyricText.Trim().Length);
                Assert.IsTrue(resultService.FoundLyricList[0].LyricCreditText.ToUpperInvariant().Contains("CHARTLYRICS")); 
            }
        }

    }

}
