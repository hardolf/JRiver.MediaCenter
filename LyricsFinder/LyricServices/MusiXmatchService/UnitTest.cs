﻿using System;
using System.Threading.Tasks;

using MediaCenter.LyricsFinder.Model.LyricServices;
using MediaCenter.LyricsFinder.Model.McRestService;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MediaCenter.LyricsFinder.Model.LyricServices.Test
{

    [TestClass]
    public class UnitTest
    {

        private McMplItem _item;
        private MusiXmatchService _service;


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

            _service = new MusiXmatchService();
            _service.DataDirectory = Environment.ExpandEnvironmentVariables(@"%USERPROFILE%\Documents\LyricsFinder");
            _service.RefreshServiceSettings();
        }


        [TestMethod]
        public async Task MusiXmatchTestMethod01()
        {
            var resultService = await _service.ProcessAsync(_item).ConfigureAwait(false);

            Assert.IsNotNull(resultService);
            Assert.AreNotEqual(0, resultService.FoundLyricList.Count);
            Assert.IsNotNull(resultService.FoundLyricList[0]);
            Assert.IsNotNull(resultService.FoundLyricList[0].LyricText);
            Assert.AreNotEqual(0, resultService.FoundLyricList[0].LyricText.Trim().Length);
            Assert.IsTrue(resultService.FoundLyricList[0].LyricCreditText.ToUpperInvariant().Contains("MUSIXMATCH"));
        }

    }

}
