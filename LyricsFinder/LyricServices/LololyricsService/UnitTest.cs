using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using MediaCenter.McWs;
using MediaCenter.SharedComponents;

namespace MediaCenter.LyricsFinder.Model.LyricServices.Test
{

    [TestClass]
    public class UnitTest
    {

        private McMplItem _item1, _item2, _item3, _item4, _item5;
        private LololyricsService _service;


        [TestCleanup]
        public void Cleanup()
        {
        }


        [TestInitialize]
        public void Init()
        {
            _item1 = new McMplItem
            {
                Artist = "Prefix",
                Name = "Sloopvergunning" // Exists
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

            _item4 = new McMplItem
            {
                Artist = "Pattern J",
                Name = "Chromozome" // Exists
            };


            _item5 = new McMplItem
            {
                Name = "Chromozome" // Title only search - fails
            };

            _service = (LololyricsService)LyricsFinderDataType.GetLyricService<LololyricsService>();
        }


        [TestMethod]
        public async Task LololyricsTestMethod01()
        {
            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                var resultService = await _service.ProcessAsync(_item1, cancellationTokenSource.Token).ConfigureAwait(false);

                Assert.IsNotNull(resultService);
                Assert.AreNotEqual(0, resultService.FoundLyricList.Count);
                Assert.IsNotNull(resultService.FoundLyricList[0]);
                Assert.IsNotNull(resultService.FoundLyricList[0].LyricText);
                Assert.AreNotEqual(0, resultService.FoundLyricList[0].LyricText.Trim().Length);
                Assert.IsTrue(resultService.FoundLyricList[0].LyricCreditText.ToUpperInvariant().Contains("LOLOLYRICS"));
            }
        }


        [TestMethod]
        public async Task LololyricsTestMethod02()
        {
            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                var resultService = await _service.ProcessAsync(_item2, cancellationTokenSource.Token).ConfigureAwait(false);

                Assert.IsNotNull(resultService);
                Assert.AreEqual(0, resultService.FoundLyricList.Count);
                Assert.AreEqual(resultService.LyricResult, LyricsResultEnum.NotFound);
            }
        }


        [TestMethod]
        public async Task LololyricsTestMethod03()
        {
            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                var resultService = await _service.ProcessAsync(_item3, cancellationTokenSource.Token).ConfigureAwait(false);

                Assert.IsNotNull(resultService);
                Assert.AreEqual(0, resultService.FoundLyricList.Count);
                Assert.AreEqual(resultService.LyricResult, LyricsResultEnum.NotFound);
            }
        }


        [TestMethod]
        public async Task LololyricsTestMethod04()
        {
            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                var resultService = await _service.ProcessAsync(_item4, cancellationTokenSource.Token).ConfigureAwait(false);

                Assert.IsNotNull(resultService);
                Assert.AreNotEqual(0, resultService.FoundLyricList.Count);
                Assert.IsNotNull(resultService.FoundLyricList[0]);
                Assert.IsNotNull(resultService.FoundLyricList[0].LyricText);
                Assert.AreNotEqual(0, resultService.FoundLyricList[0].LyricText.Trim().Length);
                Assert.IsTrue(resultService.FoundLyricList[0].LyricCreditText.ToUpperInvariant().Contains("LOLOLYRICS"));
            }
        }


        [TestMethod]
        public async Task LololyricsTestMethod05()
        {
            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                var resultService = await _service.ProcessAsync(_item5, cancellationTokenSource.Token).ConfigureAwait(false);

                Assert.IsNotNull(resultService);
                Assert.AreEqual(0, resultService.FoundLyricList.Count);
                Assert.AreEqual(resultService.LyricResult, LyricsResultEnum.NotFound);
            }
        }

    }

}
