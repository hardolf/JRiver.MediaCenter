﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

using MediaCenter.LyricsFinder.Model.Helpers;
using MediaCenter.McWs;
using MediaCenter.SharedComponents;


namespace MediaCenter.LyricsFinder.Model.LyricServices
{

    /// <summary>
    /// STANDS4 Networks lyrics service.
    /// </summary>
    /// <seealso cref="AbstractLyricService" />
    [Serializable]
    public class Stands4Service : AbstractLyricService, ILyricService
    {

        /// <summary>
        /// Gets or sets the daily quota.
        /// </summary>
        /// <value>
        /// The daily quota.
        /// </value>
        [XmlElement]
        public virtual int DailyQuota { get; set; }

        /// <summary>
        /// Gets or sets the quota reset time, with time zone of the lyric server.
        /// </summary>
        /// <value>
        /// The quota reset time.
        /// </value>
        [XmlElement]
        public virtual ServiceDateTimeWithZone QuotaResetTime { get; set; }

        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        /// <value>
        /// The token.
        /// </value>
        [XmlElement]
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        [XmlElement]
        public string UserId { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="LyricsStandsService"/> class.
        /// </summary>
        public Stands4Service()
            : base()
        {
            IsImplemented = true;
            QuotaResetTime = new ServiceDateTimeWithZone(DateTime.Now.Date, TimeZoneInfo.Local); // Default is midnight in the client time zone
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Stands4Service" /> class as a copy of the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        public Stands4Service(Stands4Service source)
            : base(source)
        {
            if (source is null) throw new ArgumentNullException(nameof(source));

            DailyQuota = source.DailyQuota;
            QuotaResetTime = source.QuotaResetTime;
            Token = source.Token;
            UserId = source.UserId;
        }


        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>
        ///   <see cref="Stands4Service" /> object.
        /// </returns>
        public override ILyricService Clone()
        {
            var ret = new Stands4Service(this);

            // The hit and request counters are added back to the source service after a search.
            // This is done in the LyricSearch.SearchAsync method.

            ret.CreateDisplayProperties();

            return ret;
        }


        /// <summary>
        /// Creates the display properties.
        /// </summary>
        public override void CreateDisplayProperties()
        {
            base.CreateDisplayProperties();

            const string defTz = "UTC";
            var errMsg = string.Empty;

            DisplayProperties.Add(nameof(Token), Token, null, isEditAllowed: true);
            DisplayProperties.Add(nameof(UserId), UserId, "User ID", isEditAllowed: true);
            DisplayProperties.Add(nameof(DailyQuota), DailyQuota.ToString(Constants.IntegerFormat, CultureInfo.CurrentCulture), 
                "Max. daily number of requests", "The quota for maximum daily number of requests to the lyric service", true, 0);

            try
            {
                DisplayProperties.Add("QuotaResetTimeZone", "QuotaResetTime", "TimeZoneId", QuotaResetTime.ServiceTimeZone.StandardName, 
                    "Service time zone", "The display name for the lyric server time zone's standard time. " + Constants.NewLine +
                    "See the Name column on https://en.wikipedia.org/wiki/List_of_time_zone_abbreviations", true, defTz);
            }
            catch (TimeZoneNotFoundException ex)
            {
                errMsg = $"{ex.Message} Please set the proper time zone name, e.g. \"{defTz}\".";
                QuotaResetTime = new ServiceDateTimeWithZone(DateTime.UtcNow, TimeZoneInfo.Utc);
                DisplayProperties.Add("QuotaResetTimeZone", "QuotaResetTime", "TimeZoneId", QuotaResetTime.ServiceTimeZone.StandardName, 
                    "Service time zone", "The display name for the lyric server time zone's standard time. " + Constants.NewLine +
                    "See the Name column on https://en.wikipedia.org/wiki/List_of_time_zone_abbreviations", true, defTz);
            }

            try
            {
                DisplayProperties.Add("QuotaResetTimeService", "QuotaResetTime", "ServiceLocalTime", QuotaResetTime.ServiceLocalTime.AddDays(1).ToString(Constants.DateTimeFormat, CultureInfo.CurrentCulture),
                    "Next quota reset local time, service", "The next quota reset, local time for the lyric service", true,
                    DateTime.Now.ToLocalTime().AddDays(1).ToString(Constants.DateTimeFormat, CultureInfo.CurrentCulture));
            }
            catch (TimeZoneNotFoundException ex)
            {
                errMsg = $"{ex.Message}.";
                QuotaResetTime = new ServiceDateTimeWithZone(DateTime.UtcNow, TimeZoneInfo.Utc);
                DisplayProperties.Add("QuotaResetTimeZone", "QuotaResetTime", "TimeZoneId", QuotaResetTime.ServiceTimeZone.StandardName, 
                    "Service time zone", "The display name for the lyric server time zone's standard time. " + Constants.NewLine +
                    "See the Name column on https://en.wikipedia.org/wiki/List_of_time_zone_abbreviations", true, defTz);
            }

            DisplayProperties.Add("QuotaResetTimeClient", QuotaResetTime.ClientLocalTime.AddDays(1).ToString(Constants.DateTimeFormat, CultureInfo.CurrentCulture), 
                "Next quota reset local time, this client", "The next quota reset, local time for the client machine", false);

            if (!errMsg.IsNullOrEmptyTrimmed())
                throw new Exception(errMsg);
        }


        /// <summary>
        /// Extracts all the lyrics from all the results.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="resultList">The result list.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="isGetAll">if set to <c>true</c> extracts all lyrics from all the Uris; else exits after the first hit.</param>
        /// <param name="isRigorous">if set to <c>true</c> the match criteria are rigorous; else they are lax.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">uris</exception>
        protected virtual async Task ExtractAllLyricTextsAsync(McMplItem item, StandsResultListType resultList, CancellationToken cancellationToken, bool isGetAll = false, bool isRigorous = false)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (resultList == null) throw new ArgumentNullException(nameof(resultList));
            if (!IsActive) return;

            if (isGetAll)
            {
                // Parallel search
                var tasks = new List<Task>();

                // If any of the results match our playlist item (song), process it
                foreach (var result in resultList)
                {
                    if (!result.Artist.Equals(item.Artist, StringComparison.InvariantCultureIgnoreCase)
                        || !result.Song.Equals(item.Name, StringComparison.InvariantCultureIgnoreCase))
                        continue;

                    if (isRigorous
                        && !result.Album.Equals(item.Album, StringComparison.InvariantCultureIgnoreCase))
                        continue;

                    tasks.Add(ExtractOneLyricTextAsync(new UriBuilder(result.SongLink).Uri, cancellationToken));
                }

                await Task.WhenAll(tasks).ConfigureAwait(false);
            }
            else
            {
                // Serial search, probably hits on the first try
                // If any of the results match our playlist item (song), process it
                foreach (var result in resultList)
                {
                    if (!result.Artist.Equals(item.Artist, StringComparison.InvariantCultureIgnoreCase)
                        || !result.Song.Equals(item.Name, StringComparison.InvariantCultureIgnoreCase))
                        continue;

                    if (isRigorous
                        && !result.Album.Equals(item.Album, StringComparison.InvariantCultureIgnoreCase))
                        continue;

                    var lyricText = await ExtractOneLyricTextAsync(new UriBuilder(result.SongLink).Uri, cancellationToken).ConfigureAwait(false);

                    if (!lyricText.IsNullOrEmptyTrimmed())
                        break;
                }
            }
        }


        /// <summary>
        /// Extracts the result text and sets the FoundLyricsText.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// If found, the found lyric text string; else null.
        /// </returns>
        /// <exception cref="ArgumentNullException">uri</exception>
        /// <exception cref="Exception">Missing closing \">\" in \"pre\" start tag in result HTML.</exception>
        protected override async Task<string> ExtractOneLyricTextAsync(Uri uri, CancellationToken cancellationToken)
        {
            if (uri == null) throw new ArgumentNullException(nameof(uri));
            if (!IsActive) return string.Empty;

            var ret = await base.ExtractOneLyricTextAsync(uri, cancellationToken).ConfigureAwait(false);
            var html = await HttpGetStringAsync(uri).ConfigureAwait(false);

            // ¤<!doctype html>¤<!--[if lt IE 7]> <html class="no-js lt-ie9 lt-ie8 lt-ie7" lang="en"> <![endif]-->¤<!--[if IE 7]>    <html class="no-js lt-ie9 lt-ie8" lang="en"> <![endif]-->¤<!--[if IE 8]>    <html class="no-js lt-ie9" lang="en"> <![endif]-->¤<!--[if gt IE 8]><!--> <html class="no-js" lang="en"> <!--<![endif]--><head>¤¤<!-- DFP head code - START -->¤¤<script>¤    var adsStart = (new Date()).getTime();¤    function detectWidth() {¤        return window.screen.width || window.innerWidth || window.document.documentElement.clientWidth || Math.min(window.innerWidth, window.document.documentElement.clientWidth) || window.innerWidth || window.document.documentElement.clientWidth || window.document.getElementsByTagName('body')[0].clientWidth;¤    }¤¤    var TIMEOUT = 1000;¤    var EXCHANGE_RATE = 3.42;¤    var screenSizeMobile = 768;¤¤    var googletag = googletag || {};¤    googletag.cmd = googletag.cmd || [];¤    var pbjs = pbjs || {};¤    pbjs.que = pbjs.que || [];¤    var adUnits = adUnits || [];¤¤    function initAdServer() {¤        if (pbjs.initAdserverSet) return;¤        (function() {¤            var gads = document.createElement('script');¤            gads.async = true;¤            gads.type = 'text/javascript';¤            var useSSL = 'https:' == document.location.protocol;¤            gads.src = (useSSL ? 'https:' : 'http:') +¤                '//www.googletagservices.com/tag/js/gpt.js';¤            var node = document.getElementsByTagName('script')[0];¤            node.parentNode.insertBefore(gads, node);¤        })();¤        pbjs.initAdserverSet = true;¤    };¤    pbjs.timeout = setTimeout(initAdServer, TIMEOUT);¤    pbjs.timeStart = adsStart;¤¤    // SizeMapping¤    var mapSizeMRU = [[300, 250], [300, 600]];¤    if(detectWidth() < screenSizeMobile) mapSizeMRU = [[300, 250]];¤¤    var dfpNetwork = '52304935';¤¤    // START: Defining Adunits¤    adUnits.push({¤            network: dfpNetwork,¤            adunit: "Lyrics_IA_300x250_A",¤            size: [[300, 250]],¤            code: 'div-gpt-ad-Lyrics_IA_300x250_A',¤            assignToVariableName: false // false if not in use¤         }); ¤    adUnits.push({¤            network: dfpNetwork,¤            adunit: "Lyrics_IA_300x250_B",¤            size: [[300, 250]],¤            code: 'div-gpt-ad-Lyrics_IA_300x250_B',¤            assignToVariableName: false // false if not in use¤         }); ¤    adUnits.push({¤            network: dfpNetwork,¤            adunit: "Lyrics_LR_300x250_A",¤            size: mapSizeMRU,¤            code: 'div-gpt-ad-Lyrics_LR_300x250_A',¤            assignToVariableName: false // false if not in use¤         }); ¤    adUnits.push({¤            network: dfpNetwork,¤            adunit: "Lyrics_LR_300x250_B",¤            size: mapSizeMRU,¤            code: 'div-gpt-ad-Lyrics_LR_300x250_B',¤            assignToVariableName: false // false if not in use¤         }); ¤    adUnits.push({¤            network: dfpNetwork,¤            adunit: "Lyrics_LR_300x250_C",¤            size: mapSizeMRU,¤            code: 'div-gpt-ad-Lyrics_LR_300x250_C',¤            assignToVariableName: false // false if not in use¤         }); ¤    adUnits.push({¤            network: dfpNetwork,¤            adunit: "Lyrics_LR_300x250_D",¤            size: mapSizeMRU,¤            code: 'div-gpt-ad-Lyrics_LR_300x250_D',¤            assignToVariableName: false // false if not in use¤         }); ¤    adUnits.push({¤            network: dfpNetwork,¤            adunit: "Lyrics_LR_125x125",¤            size: [[125, 125]],¤            code: 'div-gpt-ad-Lyrics_LR_125x125',¤            assignToVariableName: false // false if not in use¤         }); ¤¤    // END: Defining Adunits¤¤    googletag.cmd.push(function() {¤      if(adUnits){¤        var dfpSlots = [];¤        for (var i = 0, len = adUnits.length; i < len; i++) {¤          dfpSlots[i] = googletag.defineSlot('/'+adUnits[i].network+'/'+adUnits[i].adunit, adUnits[i].size, adUnits[i].code).addService(googletag.pubads());¤          if(adUnits[i].assignToVariableName && (adUnits[i].assignToVariableName !== null)) window[adUnits[i].assignToVariableName] = dfpSlots[i];¤        }¤      }¤    });¤    googletag.cmd.push(function() {¤        // Header Bidding Targeting¤        pbjs.que.push(function() {pbjs.setTargetingForGPTAsync();});¤¤        // Set targeting¤        googletag.pubads().setTargeting("device", (detectWidth() < screenSizeMobile) ? "mobile" : "desktop");¤¤        // Init DFP¤        googletag.pubads().enableSingleRequest();¤        googletag.pubads().collapseEmptyDivs();¤        googletag.enableServices();¤    });¤</script>¤<script type="text/javascript" async src="https://www.lyrics.com/adunits/prebid.js"></script>    ¤<!-- DFP head code - END --><meta charset="utf-8">¤<meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">¤<title>Kingdom of Gold Lyrics</title>¤<meta name="description" content="Lyrics to Kingdom of Gold by Mark Knopfler from the Privateering [Bonus CD] [Bonus DVD] [Deluxe] album - including song video, artist biography, translations and more!">¤<meta name="keywords" content="Kingdom of Gold lyrics, lyrics for Kingdom of Gold, Kingdom of Gold song, Kingdom of Gold words, lyrics from Privateering [Bonus CD] [Bonus DVD] [Deluxe]">¤\t<meta name="viewport" content="width=device-width">¤<base href="https://www.lyrics.com/">¤¤<script>¤version = '1.0.5';¤</script>¤¤<!-- Bootstrap compiled and minified CSS -->¤<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.2/css/bootstrap.min.css">¤<!--<link rel="stylesheet" href="--><!--/app_common/css/normalize.css">-->¤<link rel="stylesheet" href="root/app_common/css/lyrc.css?v=1.0.6">¤<!--[if lt IE 9]> <link rel="stylesheet" href="root/app_common/css/lyrc-ie8.css"> <![endif]-->¤<!--[if lt IE 8]> <link rel="stylesheet" href="root/app_common/css/lyrc-ie7.css"> <![endif]-->¤<link rel="shortcut icon" type="image/x-icon" href="root/app_common/img/favicon_lyrc.ico">¤<link rel="search" type="application/opensearchdescription+xml" title="Lyrics.com" href="https://www.lyrics.com/open-search.xml">¤¤<!--[if lt IE 9]>¤<script src="root/app_common/js/libs/modernizr-2.8.3.custom.min.js"></script>¤<script src="root/app_common/js/libs/html5shiv.min.js"></script>¤<script src="root/app_common/js/libs/respond.min.js"></script>¤<![endif]--><script type="text/javascript">¤  var _gaq = _gaq || [];¤  _gaq.push(['_setAccount', 'UA-172613-15']);¤  _gaq.push(['_trackPageview']);¤  (function() {¤    var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;¤    ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';¤    var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);¤  })();¤</script><meta property="fb:app_id" content="118234861672613"/>¤<meta name="google-signin-client_id" content="567628204450-49mrbnlqde6k322k6j1nmpstf86djv24.apps.googleusercontent.com">¤¤\t<meta property="og:url" content="https://www.lyrics.com/lyric/27741520" />¤\t<link rel="canonical" href="https://www.lyrics.com/lyric/27741520" />¤¤</head>¤<body id="s4-page-lyric" data-fb="118234861672613" data-atp="ra-4f75bf3d5305fac2">¤<div id="fb-root"></div>¤<script>(function(d, s, id) {¤  var js, fjs = d.getElementsByTagName(s)[0];¤  if (d.getElementById(id)) return;¤  js = d.createElement(s); js.id = id;¤  js.src = "//connect.facebook.net/en_US/sdk.js#xfbml=1&appId=118234861672613&version=v2.3";¤  fjs.parentNode.insertBefore(js, fjs);¤}(document, 'script', 'facebook-jssdk'));</script>¤¤<div id="page-container">¤<header id="header">¤\t<div id="header-int" class="clearfix">\t\t¤\t\t\t\t\t<div id="user-login">¤\t\t\t\t<a href="login.php">Login</a>&nbsp;<i class="fa fa-sign-in" aria-hidden="true"></i>¤\t\t\t</div>¤\t\t\t\t<div id="network-header"><span id="network-header-trigger">The&nbsp;<span class="hidden-xs">STANDS4&nbsp;</span>Network<span class="arw">&#9776;</span></span>¤\t<ul id="network-header-links" style="display:none;">¤\t\t<li class="nw-abbreviations"><a href="http://www.abbreviations.com/">ABBREVIATIONS</a></li>¤\t\t<li class="nw-anagrams"><a href="http://www.anagrams.net/">ANAGRAMS</a></li>¤\t\t<li class="nw-biographies"><a href="http://www.biographies.net/">BIOGRAPHIES</a></li>¤\t\t<li class="nw-convert"><a href="http://www.convert.net/">CONVERSIONS</a></li>¤\t\t<li class="nw-definitions"><a href="https://www.definitions.net/">DEFINITIONS</a></li>¤\t\t<li class="nw-grammar"><a href="https://www.grammar.com/">GRAMMAR</a></li>¤\t\t<li class="nw-lyrics"><a href="https://www.lyrics.com/">LYRICS</a></li>¤\t\t<li class="nw-math"><a href="http://www.math.net/">MATH</a></li>¤\t\t<li class="nw-phrases"><a href="http://www.phrases.net/">PHRASES</a></li>¤\t\t<li class="nw-poetry"><a href="http://www.poetry.net/">POETRY</a></li>¤\t\t<li class="nw-quotes"><a href="https://www.quotes.net/">QUOTES</a></li>¤\t\t<li class="nw-references"><a href="http://www.references.net/">REFERENCES</a></li>¤\t\t<li class="nw-rhymes"><a href="https://www.rhymes.net/">RHYMES</a></li>¤\t\t<li class="nw-scripts"><a href="https://www.scripts.com/">SCRIPTS</a></li>¤\t\t<li class="nw-symbols"><a href="http://www.symbols.com/">SYMBOLS</a></li>¤\t\t<li class="nw-synonyms"><a href="http://www.synonyms.com/">SYNONYMS</a></li>¤\t\t<li class="nw-uszip"><a href="http://www.uszip.com/">USZIP</a></li>¤\t</ul>¤</div>\t</div>¤</header><div id="main" role="main" class="container">¤<div id="content-top" class="content-top">¤\t<div class="view-desktop hidden-xs">¤\t\t<form id="search-frm" method="get" action="subserp.php">¤<div id="page-top-search" class="rc5">¤\t<div id="page-word-search">¤\t\t<input type="text" id="page-word-search-query" class="rc5" name="st" value="" placeholder="Search for lyrics or artists..." autocomplete="off">¤\t\t<div id="page-word-search-icon"><i class="fa fa-search" aria-hidden="true"></i></div>¤\t\t<button type="submit" class="btn primary" id="page-word-search-button">Search</button>¤\t</div>¤\t<div id="page-word-search-ops">¤\t\t<div><input type="radio" class="custom-rb" name="qtype" id="page-word-search-op1" value="1" checked="checked"><label for="page-word-search-op1"><span>in Lyrics</span></label></div>¤\t\t<div><input type="radio" class="custom-rb" name="qtype" id="page-word-search-op2" value="2"><label for="page-word-search-op2"><span>in Artists</span></label></div>¤\t\t<div><input type="radio" class="custom-rb" name="qtype" id="page-word-search-op3" value="3"><label for="page-word-search-op3"><span>in Albums</span></label></div>¤\t</div>¤</div>¤<div id="page-letter-search" class="rc5"><a href="/artists/0">#</a><a href="/artists/A">A</a><a href="/artists/B">B</a><a href="/artists/C">C</a><a href="/artists/D">D</a><a href="/artists/E">E</a><a href="/artists/F">F</a><a href="/artists/G">G</a><a href="/artists/H">H</a><a href="/artists/I">I</a><a href="/artists/J">J</a><a href="/artists/K">K</a><a href="/artists/L">L</a><a href="/artists/M">M</a><a href="/artists/N">N</a><a href="/artists/O">O</a><a href="/artists/P">P</a><a href="/artists/Q">Q</a><a href="/artists/R">R</a><a href="/artists/S">S</a><a href="/artists/T">T</a><a href="/artists/U">U</a><a href="/artists/V">V</a><a href="/artists/W">W</a><a href="/artists/X">X</a><a href="/artists/Y">Y</a><a href="/artists/Z">Z</a><span class="vbar">&nbsp;</span><a href="justadded.php" class="z">NEW</a><a href="random.php" class="z">RANDOM</a></div>¤</form>\t\t<div id="page-top-logo" onclick="location.href='https://www.lyrics.com/';"><img src="root/app_common/img/top_logo_lyr.png" alt="Lyrics.com" title="Lyrics.com"></div>¤\t</div>¤\t<div class="view-mobile visible-xs">¤\t\t<form id="search-frm" method="get" action="subserp.php">¤\t<div id="page-top-search" class="rc5">¤\t\t<div id="page-word-search">¤\t\t\t<input type="text" id="page-word-search-query" class="rc5" name="st" value="" autocomplete="off">¤\t\t\t<div id="page-word-search-icon"><i class="fa fa-search" aria-hidden="true"></i></div>¤\t\t\t<button type="submit" class="btn primary" id="page-word-search-button">Search</button>¤\t\t</div>¤\t\t<div id="page-word-search-ops">¤\t\t\t<div><input type="radio" class="custom-rb" name="qtype" id="page-word-search-m-op1" value="1" checked="checked"><label for="page-word-search-m-op1"><span>in Lyrics</span></label></div>¤\t\t\t<div><input type="radio" class="custom-rb" name="qtype" id="page-word-search-m-op2" value="2"><label for="page-word-search-m-op2"><span>in Artists</span></label></div>¤\t\t\t<div><input type="radio" class="custom-rb" name="qtype" id="page-word-search-m-op3" value="3"><label for="page-word-search-m-op3"><span>in Albums</span></label></div>¤\t\t</div>¤\t</div>¤\t<div id="page-letter-search" class="rc5"><a href="/artists/0">#</a><a href="/artists/A">A</a><a href="/artists/B">B</a><a href="/artists/C">C</a><a href="/artists/D">D</a><a href="/artists/E">E</a><a href="/artists/F">F</a><a href="/artists/G">G</a><a href="/artists/H">H</a><a href="/artists/I">I</a><a href="/artists/J">J</a><a href="/artists/K">K</a><a href="/artists/L">L</a><a href="/artists/M">M</a><a href="/artists/N">N</a><a href="/artists/O">O</a><a href="/artists/P">P</a><a href="/artists/Q">Q</a><a href="/artists/R">R</a><a href="/artists/S">S</a><a href="/artists/T">T</a><a href="/artists/U">U</a><a href="/artists/V">V</a><a href="/artists/W">W</a><a href="/artists/X">X</a><a href="/artists/Y">Y</a><a href="/artists/Z">Z</a><span class="vbar">&nbsp;</span><a href="justadded.php" class="z">NEW</a><a href="random.php" class="z">RANDOM</a></div>¤</form>\t\t<div id="page-top-logo" onclick="location.href='https://www.lyrics.com/';"><img src="root/app_common/img/top_logo_lyr.png" alt="Lyrics.com" title="Lyrics.com"></div>¤\t</div>¤</div>¤<div class="row">¤\t¤\t<div id="content-main" class="col-sm-8 col-sm-push-4">¤¤\t\t<div id="content-body">¤\t\t\t¤\t\t\t\t\t\t¤\t\t\t<div class="clearfix">¤\t\t\t\t¤\t\t\t\t<div class="lyric clearfix">¤\t\t\t\t\t¤\t\t\t\t\t<hgroup dir="ltr">¤\t\t\t\t\t\t<div style="position:relative;float:right;top:-8px;right:1px"><div class="fb-like" data-href="" data-width="" data-height="" data-colorscheme="light" data-layout="button" data-action="like" data-show-faces="true" data-send="false"></div></div>\t\t\t\t\t\t¤\t\t\t\t\t\t<div style="position:relative;float:right;top:-3px;right:13px">¤\t\t\t\t\t\t\t<p id="print-style-icon">¤\t\t\t\t\t\t\t\t<a target="_blank" href="email.php?id=27741520&type=lyric" class="rc3 s" title="click to email Kingdom of Gold to a friend"><span style="margin-left: -1px;" class="glyphicon glyphicon-envelope"></span></a>&nbsp;¤\t\t\t\t\t\t\t\t<a target="_blank" href="print.php?id=27741520" class="rc3 s"  title="click to send Kingdom of Gold to your printer"><span class="glyphicon glyphicon-print"></span></a>&nbsp;¤\t\t\t\t\t\t\t\t<a href="sheetmusic.php?id=27741520" class="rc3 s" title="click for Kingdom of Gold sheet music"><span style="margin-left: -1px;" class="glyphicon glyphicon-music"></span> Notes</a>&nbsp;¤\t\t\t\t\t\t\t\t<a href="playlist.php?add=27741520&sub=0" class="rc3 s"  title="click to add Kingdom of Gold to your playlist"><span class="glyphicon glyphicon-list"></span> Playlist</a>¤\t\t\t\t\t\t\t</p>¤\t\t\t\t\t\t</div>¤\t\t\t\t\t\t<h2 id="lyric-title-text" class="lyric-title">Kingdom of Gold</h2>¤\t\t\t\t\t\t<h3 class="lyric-artist"><a href="artist/Mark%20Knopfler/94636">Mark Knopfler</a>\t\t\t\t\t\t\t<span style="font-weight: 400; font-size: 15px; float:right;"><a href="https://www.amazon.com/gp/search?index=digital-music&keywords=Mark Knopfler Kingdom of Gold&tag=stands4com-20"  target="_blank"><i style="color: #666;" class="fa fa-shopping-cart" aria-hidden="true"></i>&nbsp;Buy<span class="hidden-xs"> This Song</span></a></span>¤\t\t\t\t\t\t\t<span style="font-weight: 400; font-size: 15px; float:right;"><a href="sync.php?id=27741520"  target="_blank"><i style="color: #666;" class="fa fa-handshake-o" aria-hidden="true"></i>&nbsp;<span class="hidden-xs">Request A </span>License&nbsp;&nbsp;&nbsp;</a></span>¤\t\t\t\t\t\t</h3>¤\t\t\t\t\t</hgroup>¤\t\t\t\t\t\t\t\t\t\t¤\t\t\t\t\t\t\t\t\t\t¤\t\t\t\t\t<div class="lyric-infobox clearfix">\t¤\t\t\t\t\t\t\t\t\t\t\t\t\t<div class="artist-thumb" id="featured-artist-avatar" data-author="Mark Knopfler" data-img="1"><a href="artist/Mark%20Knopfler/94636"><img src="https://encrypted-tbn3.gstatic.com/images?q=tbn:ANd9GcQPipTyIuNZaav2UXQqCQHnREhwjDKoyDKesZ_HRyOr47PUpdC9vjR8WUM" style="width:120px;height:125px;margin-left:0px;"></a></div>¤\t\t\t\t\t\t\t\t\t\t\t\t¤\t\t\t\t\t\t<div class="artist-meta">¤\t\t\t\t\t\t\t<div class="flr">¤                \t\t\t\t\t<p id="favorite-style-select" data-key="94636" data-page="lyric">¤                \t\t\t\t\t\t<a href="javascript:void(0);" class="rc3 s " id="artist-rate-btn" data-votes="1" data-aid="94636" data-sub="0" data-uid="" data-target="#vote-modal" alt="click to add your vote" title="click to add your vote"><i class="fa fa-star" aria-hidden="true"></i> <span>FAVORITE</span></a>¤                \t\t\t\t\t\t<span id="author-rate-votes" onclick="location.href='https://www.lyrics.com/artist-fans/94636';">(1 fan)</span>¤                \t\t\t\t\t</p>¤                \t\t\t\t</div>¤\t\t\t\t\t\t\t<h4><a href="artist/Mark%20Knopfler/94636">Mark Knopfler</a></h4>¤\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t<p class="bio">Mark Freuder Knopfler, OBE (born 12 August 1949) is a British guitarist, singer, songwriter, record producer and film score composer. He is best known as the lead guitarist, vocalist, and songwriter for the British rock band Dire Straits, which he co-founded in 1977. After Dire Straits disbanded in 1995, Knopfler went on to record and produce seven solo albums, including Golden Heart (1996), Sailing to Philadelphia (2000), The Ragpicker's Dream (2002), Shangri-La (2004), Kill To Get Crimson (2007), Get Lucky (2009) and Privateering (2012). He has composed and produced film scores for eight films, including Local Hero (1983), Cal (1984), The Princess Bride (1987), and Wag the Dog (1997). In addition to his work with Dire Straits and as a solo artist and composâ€¦ <a href="artist/Mark%20Knopfler/94636">more &raquo;</a></p>¤\t\t\t\t\t\t\t\t\t\t\t\t\t¤        \t\t\t\t\t\t<br>¤        \t\t\t\t\t\t¤        \t\t\t\t\t\t<div class="lyric-details">¤            \t\t\t\t\t\t<dl>¤            \t\t\t\t\t\t\t            \t\t\t\t\t\t\t\t<dt>Year:</dt>¤        \t\t\t\t\t\t\t\t\t<dd class="dd-margin"><a href="/year/2012" title="See all songs from this year">2012</a></dd>¤            \t\t\t\t\t\t\t            \t\t\t\t\t\t\t<dt><i class="fa fa-eye" aria-hidden="true"></i></dt>¤            \t\t\t\t\t\t\t<dd>126&nbsp;Views</dd>¤            \t\t\t\t\t\t</dl>¤        \t\t\t\t\t\t</div>¤\t\t\t\t\t\t</div>¤\t\t\t\t\t</div>¤\t\t\t\t\t¤\t\t\t\t\t\t\t\t\t\t¤\t\t\t\t\t<script>¤/* Lyrics.net - TFP - Above */¤(function() {¤                var opts = {¤\t\t                \t\tartist: "Mark Knopfler",¤\t\t                        song: "Kingdom of Gold",¤                                adunit_id: 39384263,¤                                div_id: "cf_async_" + Math.floor((Math.random() * 999999999))¤                };¤                document.write('<div id="'+opts.div_id+'"></div>');var c=function(){cf.showAsyncAd(opts)};if(window.cf)c();else{cf_async=!0;var r=document.createElement("script"),s=document.getElementsByTagName("script")[0];r.async=!0;r.src="https://srv.clickfuse.com/showads/showad.js";r.readyState?r.onreadystatechange=function(){if("loaded"==r.readyState||"complete"==r.readyState)r.onreadystatechange=null,c()}:r.onload=c;s.parentNode.insertBefore(r,s)};¤})();¤</script>\t\t\t\t\t¤\t\t\t\t\t<pre id="lyric-body-text" class="lyric-body" dir="ltr" data-lang="en">The high <a style="color:#333;" href="https://www.definitions.net/definition/priest">priest</a> of <a style="color:#333;" href="https://www.definitions.net/definition/money">money</a> looks down on the river\r¤The dawn <a style="color:#333;" href="https://www.definitions.net/definition/coming">coming</a> up on his <a style="color:#333;" href="https://www.definitions.net/definition/kingdom">kingdom</a> of gold\r¤When the rim of the sun <a style="color:#333;" href="https://www.definitions.net/definition/sends">sends</a> an <a style="color:#333;" href="https://www.definitions.net/definition/arrow">arrow</a> of silver\r¤He <a style="color:#333;" href="https://www.definitions.net/definition/prays">prays</a> to the gods of the <a style="color:#333;" href="https://www.definitions.net/definition/bought">bought</a> and the sold\r¤\r¤He <a style="color:#333;" href="https://www.definitions.net/definition/turns">turns</a> to his symbols, his <a style="color:#333;" href="https://www.definitions.net/definition/ribbons">ribbons</a> of numbers\r¤They <a style="color:#333;" href="https://www.definitions.net/definition/circle">circle</a> and spin on <a style="color:#333;" href="https://www.definitions.net/definition/their">their</a> mystical scroll\r¤He <a style="color:#333;" href="https://www.definitions.net/definition/looks">looks</a> for a sign <a style="color:#333;" href="https://www.definitions.net/definition/while">while</a> the city <a style="color:#333;" href="https://www.definitions.net/definition/still">still</a> slumbers\r¤And the <a style="color:#333;" href="https://www.definitions.net/definition/ribbons">ribbons</a> and the <a style="color:#333;" href="https://www.definitions.net/definition/river">river</a> forever unroll\r¤In his <a style="color:#333;" href="https://www.definitions.net/definition/kingdom">kingdom</a> of gold, his <a style="color:#333;" href="https://www.definitions.net/definition/kingdom">kingdom</a> of gold\r¤Kingdom of gold, his <a style="color:#333;" href="https://www.definitions.net/definition/kingdom">kingdom</a> of gold\r¤Kingdom of gold\r¤\r¤On the <a style="color:#333;" href="https://www.definitions.net/definition/horizon">horizon</a> an <a style="color:#333;" href="https://www.definitions.net/definition/enemy">enemy</a> haven\r¤Sends <a style="color:#333;" href="https://www.definitions.net/definition/traces">traces</a> of <a style="color:#333;" href="https://www.definitions.net/definition/smoke">smoke</a> high up into the sky\r¤A pack of dog <a style="color:#333;" href="https://www.definitions.net/definition/jackals">jackals</a> and a <a style="color:#333;" href="https://www.definitions.net/definition/rabble">rabble</a> of ravens\r¤Who'll come for his fortress, his <a style="color:#333;" href="https://www.definitions.net/definition/castle">castle</a> on high\r¤In his <a style="color:#333;" href="https://www.definitions.net/definition/kingdom">kingdom</a> of gold, <a style="color:#333;" href="https://www.definitions.net/definition/kingdom">kingdom</a> of gold\r¤Kingdom of gold, his <a style="color:#333;" href="https://www.definitions.net/definition/kingdom">kingdom</a> of gold\r¤Kingdom of gold\r¤\r¤His axes and <a style="color:#333;" href="https://www.definitions.net/definition/armour">armour</a> will <a style="color:#333;" href="https://www.definitions.net/definition/conquer">conquer</a> these devils\r¤The <a style="color:#333;" href="https://www.definitions.net/definition/turbulent">turbulent</a> raiders will <a style="color:#333;" href="https://www.definitions.net/definition/falter">falter</a> and fall\r¤Their <a style="color:#333;" href="https://www.definitions.net/definition/leaders">leaders</a> be taken, <a style="color:#333;" href="https://www.definitions.net/definition/their">their</a> camps <a style="color:#333;" href="https://www.definitions.net/definition/burned">burned</a> and levelled\r¤They'll hang in the wind from his <a style="color:#333;" href="https://www.definitions.net/definition/citadel">citadel</a> walls\r¤In his <a style="color:#333;" href="https://www.definitions.net/definition/kingdom">kingdom</a> of gold, <a style="color:#333;" href="https://www.definitions.net/definition/kingdom">kingdom</a> of gold\r¤Kingdom of gold, <a style="color:#333;" href="https://www.definitions.net/definition/kingdom">kingdom</a> of gold\r¤Kingdom of gold</pre>¤\t\t\t\t\t¤\t\t\t\t\t<script>¤/* Lyrics.net - TFP - Below */¤(function() {¤                var opts = {¤                                artist: "Mark Knopfler",¤                                song: "Kingdom of Gold",¤                                adunit_id: 39384264,¤                                div_id: "cf_async_" + Math.floor((Math.random() * 999999999))¤                };¤                document.write('<div id="'+opts.div_id+'"></div>');var c=function(){cf.showAsyncAd(opts)};if(window.cf)c();else{cf_async=!0;var r=document.createElement("script"),s=document.getElementsByTagName("script")[0];r.async=!0;r.src="https://srv.clickfuse.com/showads/showad.js";r.readyState?r.onreadystatechange=function(){if("loaded"==r.readyState||"complete"==r.readyState)r.onreadystatechange=null,c()}:r.onload=c;s.parentNode.insertBefore(r,s)};¤})();¤</script>\t\t\t\t\t¤\t\t\t\t\t\t\t\t\t\t¤\t\t\t\t\t<div class="lyric-credits clearfix">¤\t\t\t\t\t\t<div style="position:relative;float:right;top:-3px;right:-3px">¤\t\t\t\t\t\t\t<p id="print-style-icon-2">¤\t\t\t\t\t\t\t\t<a target="_blank" href="sheetmusic.php?id=27741520" class="rc3 s" title="click for Kingdom of Gold sheet music"><span style="margin-left: -1px;" class="glyphicon glyphicon-music"></span> Sheet Music</a>&nbsp;¤\t\t\t\t\t\t\t\t<a target="_blank" href="email.php?id=27741520&type=lyric" class="rc3 s" title="click to email Kingdom of Gold to a friend"><span style="margin-left: -1px;" class="glyphicon glyphicon-envelope"></span></a>&nbsp;¤\t\t\t\t\t\t\t\t<a target="_blank" href="print.php?id=27741520" class="rc3 s"  title="click to send  to your printer"><span class="glyphicon glyphicon-print"></span></a>&nbsp;¤\t\t\t\t\t\t\t\t<a href="playlist.php?add=27741520&sub=0" class="rc3 s"  title="click to add  to your playlist"><span class="glyphicon glyphicon-list"></span> Playlist</a>¤\t\t\t\t\t\t\t</p>¤\t\t\t\t\t\t</div>¤\t\t\t\t\t\t<!-- <div style="float:right;margin:0 0 0 20px;"></div> -->¤\t\t\t\t\t\t<p>Written by: MARK KNOPFLER</p>¤\t\t\t\t\t\t<p>Lyrics Â© Universal Music Publishing Group</p>¤\t\t\t\t\t\t<p>Lyrics Licensed &amp; Provided by <a rel="nofollow" href="http://www.lyricfind.com">LyricFind</a></p>¤\t\t\t\t\t</div>¤\t\t\t\t\t¤\t\t\t\t</div>¤\t\t\t\t¤\t\t\t\t\t\t\t\t¤\t\t\t\t<!--¤\t\t\t\t¤\t\t\t\t¤\t\t\t\t<section class="translate">¤\t\t\t\t\t<hgroup>¤\t\t\t\t\t\t<h1>Translation</h1>¤\t\t\t\t\t\t<h4>Translate these <a href="https://www.lyrics.com/lyric/27741520/Mark+Knopfler/Kingdom+of+Gold">Kingdom of Gold</a> lyrics to another language:</h4>¤\t\t\t\t\t</hgroup>¤\t\t\t\t\t<div id="term-translations" class="well rc5">¤\t\t\t\t\t\t<p class="translate-select">Select another language: <select data-trackid="27741520" name="tlang" id="trans-lang-select"><option value="">- Select -</option><option value="zh-CN">ç®€ä½“ä¸­æ–‡ (Chinese - Simplified)</option><option value="zh-TW">ç¹\u0081é«”ä¸­æ–‡ (Chinese - Traditional)</option><option value="es">EspaÃ±ol (Spanish)</option><option value="ja">æ—¥æœ¬èªž (Japanese)</option><option value="pt">PortuguÃªs (Portuguese)</option><option value="de">Deutsch (German)</option><option value="ar">Ø§Ù„Ø¹Ø±Ø¨ÙŠØ© (Arabic)</option><option value="fr">FranÃ§ais (French)</option><option value="ru">Ð ÑƒÑ\u0081Ñ\u0081ÐºÐ¸Ð¹ (Russian)</option><option value="kn">à²•à²¨à³\u008dà²¨à²¡ (Kannada)</option><option value="ko">í•œêµ­ì–´ (Korean)</option><option value="iw">×¢×‘×¨×™×ª (Hebrew)</option><option value="uk">Ð£ÐºÑ€Ð°Ñ—Ð½Ñ\u0081ÑŒÐºÐ¸Ð¹ (Ukrainian)</option><option value="ur">Ø§Ø±Ø¯Ùˆ (Urdu)</option><option value="hu">Magyar (Hungarian)</option><option value="hi">à¤®à¤¾à¤¨à¤• à¤¹à¤¿à¤¨à¥\u008dà¤¦à¥€ (Hindi)</option><option value="id">Indonesia (Indonesian)</option><option value="it">Italiano (Italian)</option><option value="ta">à®¤à®®à®¿à®´à¯\u008d (Tamil)</option><option value="tr">TÃ¼rkÃ§e (Turkish)</option><option value="te">à°¤à±†à°²à±\u0081à°—à±\u0081 (Telugu)</option><option value="th">à¸ à¸²à¸©à¸²à¹„à¸—à¸¢ (Thai)</option><option value="vi">Tiáº¿ng Viá»‡t (Vietnamese)</option><option value="cs">ÄŒeÅ¡tina (Czech)</option><option value="pl">Polski (Polish)</option><option value="id">Bahasa Indonesia (Indonesian)</option><option value="ro">RomÃ¢neÈ™te (Romanian)</option><option value="nl">Nederlands (Dutch)</option><option value="el">Î•Î»Î»Î·Î½Î¹ÎºÎ¬ (Greek)</option><option value="la">Latinum (Latin)</option><option value="sv">Svenska (Swedish)</option><option value="da">Dansk (Danish)</option><option value="fi">Suomi (Finnish)</option><option value="fa">Ù\u0081Ø§Ø±Ø³ÛŒ (Persian)</option><option value="yi">×™×™Ö´×“×™×© (Yiddish)</option><option value="no">Norsk (Norwegian)</option></select></p>¤\t\t\t\t\t</div>¤\t\t\t\t</section>¤\t\t\t\t\t¤\t\t\t\t-->¤\t\t\t\t \t\t¤\t\t\t\t<section class="translate">¤\t\t\t\t\t\t\t¤\t\t\t\t\t<h4>Discuss the <a href="https://www.lyrics.com/lyric/27741520/Mark+Knopfler/Kingdom+of+Gold">Kingdom of Gold Lyrics</a> with the community:</h4>¤\t¤\t\t\t\t\t<div class="fb-comments" data-width="100%" data-href="http://www.lyrics.com/lyric/27741520/Mark+Knopfler/Kingdom+of+Gold" data-numposts="5" data-colorscheme="light" data-order-by="reverse_time"></div>¤\t\t\t\t\t\t\t\t\t\t¤\t\t\t\t</section>¤\t\t\t\t¤\t\t\t\t<section class="biblio">¤\t\t\t\t\t<hgroup>¤\t\t\t\t\t\t<h1>Citation</h1>¤\t\t\t\t\t\t<h4>Use the citation below to add these lyrics to your bibliography:</h4>¤\t\t\t\t\t</hgroup>¤\t\t\t\t\t<div class="well rc5">¤\t\t\t\t\t\t<p id="cite-style-select" data-term="Kingdom of Gold" data-key="27741520" data-page="lyric"><strong>Style:</strong><a href="javascript:void(0);" class="rc3 s" id="cite-style-select-MLA" data-type="MLA">MLA</a><a href="javascript:void(0);" class="rc3" id="cite-style-select-Chicago" data-type="Chicago">Chicago</a><a href="javascript:void(0);" class="rc3" id="cite-style-select-APA" data-type="APA">APA</a></p>¤\t\t\t\t\t\t<p id="cite-content"><cite>"Kingdom of Gold Lyrics."</cite> <em>Lyrics.com.</em> STANDS4 LLC, 2018. Web. 20 Apr. 2018. &lt;<a href="https://www.lyrics.com/lyric/27741520" class="force-wrap">https://www.lyrics.com/lyric/27741520</a>&gt;.</p>¤\t\t\t\t\t</div>¤\t\t\t\t</section>¤\t\t\t\t¤\t\t\t</div>¤\t\t\t¤\t\t\t\t\t\t¤\t\t\t\t\t\t¤\t\t\t\t<div class="callout clearfix row">¤\t\t\t\t\t<div>¤\t\t\t\t\t\t<div class="callout-int">¤\t\t\t\t\t\t\t<div class="row">¤\t\t\t\t\t\t\t\t<div class="col-xs-12 col-sm-8">¤\t\t\t\t\t\t\t\t\t<hgroup>¤\t\t\t\t\t\t\t\t\t\t<h1>Missing lyrics by Mark Knopfler?</h1>¤\t\t\t\t\t\t\t\t\t\t<h3>Know any other songs by Mark Knopfler? Don't keep it to yourself!</h3>¤\t\t\t\t\t\t\t\t\t</hgroup>¤\t\t\t\t\t\t\t\t</div>¤\t\t\t\t\t\t\t\t<div class="col-xs-12 col-sm-4">¤\t\t\t\t\t\t\t\t\t<div class="actions"><button type="button" class="btn primary lrg" onclick="location.href='https://www.lyrics.com/addlyric.php?aid=94636';">Add it Here</button></div>¤\t\t\t\t\t\t\t\t</div>¤\t\t\t\t\t\t\t</div>¤\t\t\t\t\t\t</div>¤\t\t\t\t\t</div>¤\t\t\t\t</div>¤\t\t\t¤\t\t\t\t\t\t¤\t\t\t<div id="vote-modal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">¤    <div class="modal-dialog">¤        <div class="modal-content">¤            <div class="modal-header">¤                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>¤                <h4 class="modal-title" id="myModalLabel">You need to be logged in to favorite.</h4>¤            </div>¤            <div class="modal-body edit-content">¤    \t\t\t\t<div class="row social-signin">¤    \t\t\t\t\t<div class="col-xs-12 col-sm-6">¤    \t\t\t\t\t\t<div class='g-signin2' data-onsuccess='onSuccess' data-onfailure='onFailure' data-scope='profile email' data-width='240' data-height='40' data-longtitle='true' data-theme='dark'></div>    \t\t\t\t\t</div>¤    \t\t\t\t\t<div class="col-xs-12 col-sm-6">¤    \t\t\t\t\t\t<div class='fb-login-button' onlogin='checkLoginState();' data-scope='email' data-width='240px' data-max-rows='3' data-size='large' data-show-faces='false' data-auto-logout-link='false' data-use-continue-as='true'></div>    \t\t\t\t\t</div>¤    \t\t\t\t</div>¤        \t\t\t¤        \t\t\t<ul class="sn-message"></ul>¤        \t\t\t¤    \t\t\t\t<div class="or-container">¤                    <hr class="or-hr">¤                    <div id="or">or</div>¤                </div>¤                ¤                <div class="row">¤                \t\t<div class="col-sm-6 short_signin">¤                \t\t\t<h4>Create a new account</h4>¤                \t\t\t<div class="text-center waiting"><i class="fa fa-spinner fa-spin fa-3x fa-fw"></i></div>¤                \t\t\t<form id="modal-signup-frm" method="post" action="">¤                \t\t\t\t<ul class="errors-list"></ul>\t¤                \t\t\t\t<input type="hidden" name="action" id="frm-action" value="short_signin">¤                \t\t\t\t<input type="hidden" name="source" id="frm-source" value="0">¤                \t\t\t\t<input type="hidden" name="website" id="website" value="3">¤                \t\t\t\t<input type="hidden" name="sra" id="fld-sra" value="178.157.250.129, 10.30.158.44">¤                \t\t\t\t¤                \t\t\t\t<div class="bcols clearfix row">¤                \t\t\t\t\t¤                \t\t\t\t\t<section class="col-xs-12">¤                \t\t\t\t\t\t<div class="col-left">¤                \t\t\t\t\t\t\t<p>¤                \t\t\t\t\t\t\t\t<label for="fld-rname">Your name:<span class="mandatory">*<span>Required</span></span></label>¤                \t\t\t\t\t\t\t\t<input type="text" name="rname" id="fld-rname" value="" maxlength="100" class="fw">¤                \t\t\t\t\t\t\t</p>¤                \t\t\t\t\t\t</div>¤                \t\t\t\t\t</section>¤                \t\t\t\t\t¤                \t\t\t\t\t<section class="col-xs-12">¤                \t\t\t\t\t\t<div class="col-left">¤                \t\t\t\t\t\t\t<p>¤                \t\t\t\t\t\t\t\t<label for="fld-uemail">Your email address:<span class="mandatory">*<span>Required</span></span></label>¤                \t\t\t\t\t\t\t\t<input type="text" name="uemail" id="fld-uemail" value="" maxlength="100" class="fw">¤                \t\t\t\t\t\t\t</p>¤                \t\t\t\t\t\t</div>¤                \t\t\t\t\t</section>¤                \t\t\t\t\t¤                \t\t\t\t\t<section class="col-xs-12">¤                \t\t\t\t\t\t<div class="col-left">¤                \t\t\t\t\t\t\t<p>¤                \t\t\t\t\t\t\t\t<label for="fld-uname">Pick a user name:<span class="mandatory">*<span>Required</span></span></label>¤                \t\t\t\t\t\t\t\t<input type="text" name="uname" id="fld-uname" value="" maxlength="50" class="fw">¤                \t\t\t\t\t\t\t</p>¤                \t\t\t\t\t\t</div>¤                \t\t\t\t\t</section>¤                \t\t\t\t\t¤                \t\t\t\t\t<section class="col-xs-12">¤                \t\t\t\t\t\t<div class="col-left text-right">¤                \t\t\t\t\t\t\t<p><button type="submit" class="btn primary">Join</button></p>¤                \t\t\t\t\t\t</div>¤                \t\t\t\t\t</section>¤                \t\t\t\t\t¤                \t\t\t\t</div>¤                \t\t\t¤                \t\t\t</form>¤                \t\t</div>¤                \t\t<div class="col-sm-6 short_login">¤                \t\t\t<h4>Log In</h4>¤                \t\t\t<div class="text-center waiting"><i class="fa fa-spinner fa-spin fa-3x fa-fw"></i></div>¤                \t\t\t<form id="modal-login-frm" method="post" action="">¤                \t\t\t\t<ul class="errors-list"></ul>¤                \t\t\t\t<input type="hidden" name="action" id="frm-action" value="short_login">¤                \t\t\t\t<div class="scols clearfix">¤                \t\t\t\t\t<section>¤            \t\t\t\t\t\t\t<p>¤            \t\t\t\t\t\t\t\t<label for="fld-uname">Username:<span class="mandatory">*<span>Required</span></span></label>¤            \t\t\t\t\t\t\t\t<input type="text" name="uname" id="fld-uname" value="" maxlength="50" class="fw">¤            \t\t\t\t\t\t\t</p>¤                \t\t\t\t\t</section>¤                \t\t\t\t\t¤                \t\t\t\t\t<section>¤            \t\t\t\t\t\t\t<p>¤            \t\t\t\t\t\t\t\t<label for="fld-upass">Password:<span class="mandatory">*<span>Required</span></span></label>¤            \t\t\t\t\t\t\t\t<input type="password" name="upass" id="fld-upass" value="" maxlength="30" class="fw">¤            \t\t\t\t\t\t\t</p>¤                \t\t\t\t\t</section>¤                \t\t\t\t\t¤                \t\t\t\t\t<section>¤            \t\t\t\t\t\t\t<div class="inner-section text-right">¤            \t\t\t\t\t\t\t\t<p><button type="submit" class="btn primary">Log In</button></p>¤            \t\t\t\t\t\t\t\t<br>¤            \t\t\t\t\t\t\t\t<p class="ptext">Forgot your password?&nbsp;&nbsp;&nbsp;<button type="button" class="btn secondary xxsml" onclick="location.href='https://www.lyrics.com/forgotpass.php';">Retrieve it</button></p>¤            \t\t\t\t\t\t\t</div>¤                \t\t\t\t\t</section>¤                \t\t\t\t\t¤                \t\t\t\t</div>¤                \t\t\t</form>¤                \t\t</div>¤                </div>¤                ¤            </div>¤        </div>¤    </div>¤</div>\t\t</div>¤\t</div>¤\t¤\t<div id="content-aside" class="col-sm-4 col-sm-pull-8">¤\t\t¤\t\t<div class="hidden-xs">¤    <div class="sep-area"><hr class="sep"></div>¤<div class="tagline">¤\t<h2>The Web's Largest Resource for</h2>¤\t<h1>Music, Songs <span>&amp;</span> Lyrics</h1>¤</div>    <div class="nsep"><hr><h3>A Member Of The <span>STANDS4 Network</span></h3></div>¤    <div id="sb-social">¤\t<div class="clearfix">¤\t<div class="social" title="Share this page on Facebook"><a id="share-facebook" href="javascript:void(0);" target="_blank"><span class="fb"><i class="fa fa-facebook" aria-hidden="true"></i></span></a></div>¤\t<div class="social" title="Share this page on Twitter"><a id="share-twitter" href="javascript:void(0);" target="_blank"><span class="tw"><i class="fa fa-twitter" aria-hidden="true"></i></span></a></div>¤\t<div class="social" title="Share this page on Google+"><a id="share-googleplus" href="javascript:void(0);" target="_blank"><span class="gp"><i class="fa fa-google-plus" aria-hidden="true"></i></span></a></div>¤\t<div class="social" title="Share this page with AddThis"><a id="share-addthis" href="javascript:void(0);" target="_blank"><span class="at"><i class="fa fa-plus" aria-hidden="true"></i></span></a></div>¤\t</div>¤</div>    <div class="sep-area"><hr class="sep"></div>¤</div>\t\t¤\t\t\t\t<div class="cblocks">¤\t\t\t¤\t\t\t<div class="cblock falbum">¤\t\t\t\t<hgroup class="clearfix">¤\t\t\t\t\t<h4>Watch the song video</h4>¤\t\t\t\t\t<h3>Kingdom of Gold</h3>¤\t\t\t\t</hgroup>¤\t\t\t\t<div class="cblock-int">¤\t\t\t\t<br/>¤\t\t\t\t<center>¤\t\t\t\t\t<div class="youtube-player" data-id="w5s2NDV1Nhg"></div>¤\t\t\t\t</center>¤\t\t\t\t<br/>¤\t\t\t\t</div>¤\t\t\t</div>¤\t\t</div>¤\t\t¤\t\t\t\t<div class="cblocks">¤\t\t\t¤\t\t\t<div class="cblock falbum">¤\t\t\t\t<hgroup class="clearfix">¤\t\t\t\t\t<h4>more tracks from the album</h4>¤\t\t\t\t\t<h3><a href="/album/2563331">Privateering [Bonus CD] [Bonus DVD] [Deluxe]</a></h3>¤\t\t\t\t</hgroup>¤\t\t\t\t\t\t\t\t\t<div class="album-thumb"><a href="/album/2563331"><img src="https://encrypted-tbn3.gstatic.com/images?q=tbn:ANd9GcTDWPevXPHnW4W7WhRKu1Y92bmjYCsb_093cBpsfMDiP8i4ZKNFk0vPwA" alt=""></a></div>¤\t\t\t\t\t\t\t\t<div class="cblock-int">¤\t\t\t\t\t<ul><li><a href="/lyric/27741550/Privateering+%5BBonus+CD%5D+%5BBonus+DVD%5D+%5BDeluxe%5D/Redbud+Tree">Redbud Tree</a></li><li><a href="/lyric/27741549/Privateering+%5BBonus+CD%5D+%5BBonus+DVD%5D+%5BDeluxe%5D/Haul+Away">Haul Away</a></li><li><a href="/lyric/27741548/Privateering+%5BBonus+CD%5D+%5BBonus+DVD%5D+%5BDeluxe%5D/Don%27t+Forget+Your+Hat">Don't Forget Your Hat</a></li><li><a href="/lyric/27741547/Privateering+%5BBonus+CD%5D+%5BBonus+DVD%5D+%5BDeluxe%5D/Privateering">Privateering</a></li><li><a href="/lyric/27741546/Privateering+%5BBonus+CD%5D+%5BBonus+DVD%5D+%5BDeluxe%5D/Miss+You+Blues">Miss You Blues</a></li><li><a href="/lyric/27741545/Privateering+%5BBonus+CD%5D+%5BBonus+DVD%5D+%5BDeluxe%5D/Corned+Beef+City">Corned Beef City</a></li><li><a href="/lyric/27741544/Privateering+%5BBonus+CD%5D+%5BBonus+DVD%5D+%5BDeluxe%5D/Go%2C+Love">Go, Love</a></li><li><a href="/lyric/27741543/Privateering+%5BBonus+CD%5D+%5BBonus+DVD%5D+%5BDeluxe%5D/Hot+or+What">Hot or What</a></li><li><a href="/lyric/27741542/Privateering+%5BBonus+CD%5D+%5BBonus+DVD%5D+%5BDeluxe%5D/Yon+Two+Crows">Yon Two Crows</a></li><li><a href="/lyric/27741541/Privateering+%5BBonus+CD%5D+%5BBonus+DVD%5D+%5BDeluxe%5D/Seattle">Seattle</a></li><li><a href="/lyric/27741540/Privateering+%5BBonus+CD%5D+%5BBonus+DVD%5D+%5BDeluxe%5D/Kingdom+of+Gold">Kingdom of Gold</a></li><li><a href="/lyric/27741539/Privateering+%5BBonus+CD%5D+%5BBonus+DVD%5D+%5BDeluxe%5D/Got+to+Have+Something">Got to Have Something</a></li><li><a href="/lyric/27741538/Privateering+%5BBonus+CD%5D+%5BBonus+DVD%5D+%5BDeluxe%5D/Radio+City+Serenade">Radio City Serenade</a></li><li><a href="/lyric/27741537/Privateering+%5BBonus+CD%5D+%5BBonus+DVD%5D+%5BDeluxe%5D/I+Used+to+Could">I Used to Could</a></li><li><a href="/lyric/27741536/Privateering+%5BBonus+CD%5D+%5BBonus+DVD%5D+%5BDeluxe%5D/Gator+Blood">Gator Blood</a></li><li><a href="/lyric/27741535/Privateering+%5BBonus+CD%5D+%5BBonus+DVD%5D+%5BDeluxe%5D/Bluebird">Bluebird</a></li><li><a href="/lyric/27741534/Privateering+%5BBonus+CD%5D+%5BBonus+DVD%5D+%5BDeluxe%5D/Dream+of+the+Drowned+Submariner">Dream of the Drowned Submariner</a></li><li><a href="/lyric/27741533/Privateering+%5BBonus+CD%5D+%5BBonus+DVD%5D+%5BDeluxe%5D/Blood+and+Water">Blood and Water</a></li><li><a href="/lyric/27741532/Privateering+%5BBonus+CD%5D+%5BBonus+DVD%5D+%5BDeluxe%5D/Today+Is+Okay">Today Is Okay</a></li><li><a href="/lyric/27741530/Privateering+%5BBonus+CD%5D+%5BBonus+DVD%5D+%5BDeluxe%5D/Redbud+Tree">Redbud Tree</a></li><li><a href="/lyric/27741529/Privateering+%5BBonus+CD%5D+%5BBonus+DVD%5D+%5BDeluxe%5D/Haul+Away">Haul Away</a></li><li><a href="/lyric/27741528/Privateering+%5BBonus+CD%5D+%5BBonus+DVD%5D+%5BDeluxe%5D/Don%27t+Forget+Your+Hat">Don't Forget Your Hat</a></li><li><a href="/lyric/27741527/Privateering+%5BBonus+CD%5D+%5BBonus+DVD%5D+%5BDeluxe%5D/Privateering">Privateering</a></li><li><a href="/lyric/27741526/Privateering+%5BBonus+CD%5D+%5BBonus+DVD%5D+%5BDeluxe%5D/Miss+You+Blues">Miss You Blues</a></li><li><a href="/lyric/27741525/Privateering+%5BBonus+CD%5D+%5BBonus+DVD%5D+%5BDeluxe%5D/Corned+Beef+City">Corned Beef City</a></li><li><a href="/lyric/27741524/Privateering+%5BBonus+CD%5D+%5BBonus+DVD%5D+%5BDeluxe%5D/Go%2C+Love">Go, Love</a></li><li><a href="/lyric/27741523/Privateering+%5BBonus+CD%5D+%5BBonus+DVD%5D+%5BDeluxe%5D/Hot+or+What">Hot or What</a></li><li><a href="/lyric/27741522/Privateering+%5BBonus+CD%5D+%5BBonus+DVD%5D+%5BDeluxe%5D/Yon+Two+Crows">Yon Two Crows</a></li><li><a href="/lyric/27741521/Privateering+%5BBonus+CD%5D+%5BBonus+DVD%5D+%5BDeluxe%5D/Seattle">Seattle</a></li><li><a href="/lyric/27741519/Privateering+%5BBonus+CD%5D+%5BBonus+DVD%5D+%5BDeluxe%5D/Got+to+Have+Something">Got to Have Something</a></li><li><a href="/lyric/27741518/Privateering+%5BBonus+CD%5D+%5BBonus+DVD%5D+%5BDeluxe%5D/Radio+City+Serenade">Radio City Serenade</a></li><li><a href="/lyric/27741517/Privateering+%5BBonus+CD%5D+%5BBonus+DVD%5D+%5BDeluxe%5D/I+Used+to+Could">I Used to Could</a></li><li><a href="/lyric/27741516/Privateering+%5BBonus+CD%5D+%5BBonus+DVD%5D+%5BDeluxe%5D/Gator+Blood">Gator Blood</a></li><li><a href="/lyric/27741515/Privateering+%5BBonus+CD%5D+%5BBonus+DVD%5D+%5BDeluxe%5D/Bluebird">Bluebird</a></li><li><a href="/lyric/27741513/Privateering+%5BBonus+CD%5D+%5BBonus+DVD%5D+%5BDeluxe%5D/Blood+and+Water">Blood and Water</a></li><li><a href="/lyric/27741512/Privateering+%5BBonus+CD%5D+%5BBonus+DVD%5D+%5BDeluxe%5D/Today+Is+Okay">Today Is Okay</a></li></ul>¤\t\t\t\t</div>¤\t\t\t</div>¤\t\t\t¤\t\t</div>¤\t\t\t\t¤\t\t<script>¤/* Lyrics.net - MedRec */¤cf_page_artist = "Mark Knopfler";¤cf_page_song = "Kingdom of Gold";¤cf_adunit_id = "39381629";¤</script>¤<script src="//srv.clickfuse.com/showads/showad.js"></script>¤\t\t¤<section><!-- /52304935/Lyrics_LR_300x250_A -->¤<div id='div-gpt-ad-Lyrics_LR_300x250_A'>¤<script>¤googletag.cmd.push(function() { googletag.display('div-gpt-ad-Lyrics_LR_300x250_A'); });¤</script>¤</div></section>¤<section><!-- /52304935/Lyrics_LR_300x250_B -->¤<div id='div-gpt-ad-Lyrics_LR_300x250_B'>¤<script>¤googletag.cmd.push(function() { googletag.display('div-gpt-ad-Lyrics_LR_300x250_B'); });¤</script>¤</div></section>¤\t<div class="cblocks">¤\t\t<div class="cblock promotions-list">¤\t\t\t<hgroup class="clearfix">¤\t\t\t\t<h4>Our awesome collection of</h4>¤\t\t\t\t<h3>Promoted Songs</h3>¤\t\t\t</hgroup>¤\t\t\t<span class="more"><a href="promoted-songs.php">&raquo;</a></span>¤\t\t\t<div class="cblock-int">¤\t\t\t\t<ul><li><div class="promo-img"><a href="/sublyric/49675"><img src="data:image/png;base64,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" alt="" title="Lou"></a></div><div class="promo-details"><div class="promo-title"><strong><a href="/sublyric/49675">Beaten</a></strong></div><div class="promo-artist"><a href="sub_artist.php?name=Lou">Lou</a></div></div></li></ul>¤\t\t\t</div>¤\t\t\t<p class="text-right"><button type="button" class="btn primary xxsml" onclick="location.href='promotion.php'">Get promoted&nbsp;<i class="fa fa-bullhorn"></i></button></p>¤\t\t</div>¤\t</div>¤<section><!-- /52304935/Lyrics_LR_300x250_C -->¤<div id='div-gpt-ad-Lyrics_LR_300x250_C'>¤<script>¤googletag.cmd.push(function() { googletag.display('div-gpt-ad-Lyrics_LR_300x250_C'); });¤</script>¤</div></section>¤<section><!-- /52304935/Lyrics_LR_300x250_D -->¤<div id='div-gpt-ad-Lyrics_LR_300x250_D'>¤<script>¤googletag.cmd.push(function() { googletag.display('div-gpt-ad-Lyrics_LR_300x250_D'); });¤</script>¤</div></section>¤\t\t\t\t¤\t</div>¤\t¤</div>¤¤¤<div id="page-bottom-banner" class="clearfix hidden-xs">¤\t<div><script>¤/* Lyrics.net - Leaderboard */¤cf_page_artist = "Mark Knopfler";¤cf_page_song = "Kingdom of Gold";¤cf_adunit_id = "39381630";¤</script>¤<script src="//srv.clickfuse.com/showads/showad.js"></script></div>¤</div>¤¤¤</div>¤<footer id="footer">¤\t<div id="footer-int" class="clearfix container-fluid">¤¤\t\t<div class="row">¤\t\t\t<div class="col-xs-12 col-sm-3">¤\t\t\t\t<div class="row">¤\t\t\t\t\t<div class="col-xs-6 col-sm-5">¤\t\t\t\t\t\t<ul>¤\t\t\t\t\t\t\t<h5>Company</h5>¤\t\t\t\t\t\t\t<li><a href="https://www.lyrics.com/">Home</a></li>¤\t\t\t\t\t\t\t<li><a href="about.php?slc=Lyrics">About</a></li>¤\t\t\t\t\t\t\t<li><a href="news.php">News</a></li>¤\t\t\t\t\t\t\t<li><a href="press.php">Press</a></li>¤\t\t\t\t\t\t\t<li><a href="awards.php">Awards</a></li>¤\t\t\t\t\t\t\t<li><a href="testimonials.php">Testimonials</a></li>¤\t\t\t\t\t\t</ul>¤\t\t\t\t\t</div>¤¤\t\t\t\t\t<div class="col-xs-6 col-sm-7">¤\t\t\t\t\t\t<ul>¤\t\t\t\t\t\t\t<h5>Editorial</h5>¤\t\t\t\t\t\t\t<li><a href="login.php">Login</a></li>¤\t\t\t\t\t\t\t<li><a href="addlyric.php">Add new Lyrics</a></li>¤\t\t\t\t\t\t\t<li><a href="addalbum.php">Add a new Album</a></li>¤\t\t\t\t\t\t\t<li class="ex"><a href="signup.php">Become an Editor</a></li>¤\t\t\t\t\t\t\t<li><a href="editors.php">Meet the Editors</a></li>¤\t\t\t\t\t\t\t<li><a href="justadded.php">Recently Added</a></li>¤\t\t\t\t\t\t\t<li class="ex"><a href="activity.php">Activity Log</a></li>¤\t\t\t\t\t\t\t<li><a href="https://www.lyrics.com/toplyrics.php">Most Popular</a></li>¤\t\t\t\t\t\t</ul>¤\t\t\t\t\t</div>¤\t\t\t\t</div>¤\t\t\t</div>¤¤\t\t\t<div class="col-xs-12 col-sm-4">¤\t\t\t\t<div class="row">¤\t\t\t\t\t<div class="col-xs-6 col-sm-6">¤\t\t\t\t\t\t<ul>¤\t\t\t\t\t\t\t<h5>Services</h5>¤\t\t\t\t\t\t\t<li><a href="http://www.abbreviations.com/tools.php">Tools</a></li>¤\t\t\t\t\t\t\t<li class="ex"><a href="playlist.php">Your Playlist</a></li>¤\t\t\t\t\t\t\t<li><a href="invite.php">Tell a Friend</a></li>¤\t\t\t\t\t\t\t<li><a id="page-bookmark" href="">Bookmark Us</a></li>¤\t\t\t\t\t\t\t<li><a href="lyrics_api.php">Lyrics API</a></li>¤\t\t\t\t\t\t\t<li class="ex"><a href="promotion.php">Promote&nbsp;<i class="fa fa-bullhorn fa-1x"></i></a></li>¤\t\t\t\t\t\t\t<li><a href="song-lyrics-generator.php">Lyrics Generator</a></li>¤\t\t\t\t\t\t</ul>¤\t\t\t\t\t</div>¤¤\t\t\t\t\t<div class="col-xs-6 col-sm-6">¤\t\t\t\t\t\t<ul class="last">¤\t\t\t\t\t\t\t<h5>Legal &amp; Contact</h5>¤\t\t\t\t\t\t\t<li><a href="terms.php">Terms of Use</a></li>¤\t\t\t\t\t\t\t<li><a href="privacy.php">Privacy Policy</a></li>¤\t\t\t\t\t\t\t<li><a href="contact.php">Contact Us</a></li>¤\t\t\t\t\t\t\t<li class="ex"><a href="advertise.php">Advertise</a></li>¤\t\t\t\t\t\t\t<li><a href="affiliate-program.php">Affiliate Program\t</a></li>¤\t\t\t\t\t\t</ul>¤\t\t\t\t\t</div>¤\t\t\t\t</div>¤\t\t\t</div>¤¤\t\t\t¤<div id="s4-network" class="col-xs-12 col-sm-5">¤\t<div  class="row">¤\t\t<h5 class="col-xs-12 col-sm-12">The STANDS4 Network</h5>¤\t</div>¤¤\t<!-- Desktop version -->¤\t<div class="clearfix row hidden-xs">¤\t\t<div class="col-xs-12 col-sm-8">¤\t\t\t<div class="row">¤\t\t\t\t<div class="col-xs-6 col-sm-6">¤\t\t\t\t\t<ul>¤\t\t\t\t\t\t<li class="nw-abbreviations"><a rel="nofollow" href="http://www.abbreviations.com/">Abbreviations</a></li>¤\t\t\t\t\t\t<li class="nw-convert"><a rel="nofollow" href="http://www.convert.net/">Conversions</a></li>¤\t\t\t\t\t\t<li class="nw-lyrics"><a rel="nofollow" href="https://www.lyrics.com/">Lyrics</a></li>¤\t\t\t\t\t\t<li class="nw-phrases"><a rel="nofollow" href="http://www.phrases.net/">Phrases</a></li>¤\t\t\t\t\t\t<li class="nw-references"><a rel="nofollow" href="http://www.references.net/">References</a></li>¤\t\t\t\t\t\t<li class="nw-symbols"><a rel="nofollow" href="http://www.symbols.com/">Symbols</a></li>¤\t\t\t\t\t</ul>¤\t\t\t\t</div>¤¤\t\t\t\t<div class="col-xs-6 col-sm-6">¤\t\t\t\t\t<ul>¤\t\t\t\t\t\t<li class="nw-anagrams"><a rel="nofollow" href="http://www.anagrams.net/">Anagrams</a></li>¤\t\t\t\t\t\t<li class="nw-definitions"><a rel="nofollow" href="https://www.definitions.net/">Definitions</a></li>¤\t\t\t\t\t\t<li class="nw-ua"><a rel="nofollow" href="http://www.literature.com/">Literature</a></li>¤\t\t\t\t\t\t<li class="nw-poetry"><a rel="nofollow" href="http://www.poetry.net/">Poetry</a></li>¤\t\t\t\t\t\t<li class="nw-rhymes"><a rel="nofollow" href="https://www.rhymes.net/">Rhymes</a></li>¤\t\t\t\t\t\t<li class="nw-synonyms"><a rel="nofollow" href="https://www.synonyms.com/">Synonyms</a></li>¤\t\t\t\t\t</ul>¤\t\t\t\t</div>¤\t\t\t</div>¤\t\t</div>¤¤\t\t<div class="col-xs-12 col-sm-4">¤\t\t\t<ul class="last">¤\t\t\t\t<li class="nw-biographies"><a rel="nofollow" href="http://www.biographies.net/">Biographies</a></li>¤\t\t\t\t<li class="nw-grammar"><a rel="nofollow" href="https://www.grammar.com/">Grammar</a></li>¤\t\t\t\t<li class="nw-math"><a rel="nofollow" href="http://www.math.net/">Math</a></li>¤\t\t\t\t<li class="nw-quotes"><a rel="nofollow" href="https://www.quotes.net/">Quotes</a></li>¤\t\t\t\t<li class="nw-scripts"><a rel="nofollow" href="https://www.scripts.com/">Scripts</a></li>¤\t\t\t\t<li class="nw-uszip"><a rel="nofollow" href="http://www.uszip.com/">Zip Codes</a></li>¤\t\t\t</ul>¤\t\t</div>¤¤\t</div>¤¤\t<!-- Mobile version -->¤\t<div class="clearfix row hidden-sm hidden-md hidden-lg">¤\t\t<div class="col-xs-12 col-sm-8">¤\t\t\t<div class="row">¤\t\t\t\t<div class="col-xs-6 col-sm-6">¤\t\t\t\t\t<ul>¤\t\t\t\t\t\t<li class="nw-abbreviations"><a rel="nofollow" href="http://www.abbreviations.com/">Abbreviations</a></li>¤\t\t\t\t\t\t<li class="nw-anagrams"><a rel="nofollow" href="http://www.anagrams.net/">Anagrams</a></li>¤\t\t\t\t\t\t<li class="nw-biographies"><a rel="nofollow" href="http://www.biographies.net/">Biographies</a></li>¤\t\t\t\t\t\t<li class="nw-convert"><a rel="nofollow" href="http://www.convert.net/">Conversions</a></li>¤\t\t\t\t\t\t<li class="nw-definitions"><a rel="nofollow" href="https://www.definitions.net/">Definitions</a></li>¤\t\t\t\t\t\t<li class="nw-grammar"><a rel="nofollow" href="https://www.grammar.com/">Grammar</a></li>¤\t\t\t\t\t\t<li class="nw-ua"><a rel="nofollow" href="http://www.literature.com/">Literature</a></li>¤\t\t\t\t\t\t<li class="nw-lyrics"><a rel="nofollow" href="https://www.lyrics.com/">Lyrics</a></li>¤\t\t\t\t\t\t<li class="nw-math"><a rel="nofollow" href="http://www.math.net/">Math</a></li>¤\t\t\t\t\t</ul>¤\t\t\t\t</div>¤¤\t\t\t\t<div class="col-xs-6 col-sm-6">¤\t\t\t\t\t<ul>¤\t\t\t\t\t\t<li class="nw-phrases"><a rel="nofollow" href="http://www.phrases.net/">Phrases</a></li>¤\t\t\t\t\t\t<li class="nw-poetry"><a rel="nofollow" href="http://www.poetry.net/">Poetry</a></li>¤\t\t\t\t\t\t<li class="nw-quotes"><a rel="nofollow" href="https://www.quotes.net/">Quotes</a></li>¤\t\t\t\t\t\t<li class="nw-references"><a rel="nofollow" href="http://www.references.net/">References</a></li>¤\t\t\t\t\t\t<li class="nw-rhymes"><a rel="nofollow" href="https://www.rhymes.net/">Rhymes</a></li>¤\t\t\t\t\t\t<li class="nw-scripts"><a rel="nofollow" href="https://www.scripts.com/">Scripts</a></li>¤\t\t\t\t\t\t<li class="nw-symbols"><a rel="nofollow" href="http://www.symbols.com/">Symbols</a></li>¤\t\t\t\t\t\t<li class="nw-synonyms"><a rel="nofollow" href="https://www.synonyms.com/">Synonyms</a></li>¤\t\t\t\t\t\t<li class="nw-uszip"><a rel="nofollow" href="http://www.uszip.com/">Zip Codes</a></li>¤\t\t\t\t\t</ul>¤\t\t\t\t</div>¤\t\t\t</div>¤\t\t</div>¤\t</div>¤¤\t<div class="clearfix">¤\t\t<div class="copyright"><strong>&copy; 2001-2018 STANDS4 LLC.</strong><br>All rights reserved.</div>¤\t\t<div id="social-icons">¤\t\t\t<a rel="nofollow" href="http://www.facebook.com/STANDS4" target="_blank"><div class="social fb"><i class="fa fa-facebook" aria-hidden="true"></i></div></a>¤\t\t\t<a rel="nofollow" href="http://twitter.com/justadded" target="_blank"><div class="social tw"><i class="fa fa-twitter" aria-hidden="true"></i></div></a>¤\t\t\t<a rel="nofollow" href="https://plus.google.com/+abbreviations/" rel="publisher" target="_blank"><div class="social gp"><i class="fa fa-google-plus" aria-hidden="true"></i></div></a>¤\t\t</div>¤\t</div>¤</div>¤\t\t</div>¤¤\t</div>¤</footer>¤¤</div>¤<link rel="stylesheet" href="//fonts.googleapis.com/css?family=Droid+Sans:400,700|Droid+Serif:400,700,400italic,700italic|Droid+Sans+Mono|Yanone+Kaffeesatz:200,300,400,700|Goudy+Bookletter+1911|Lobster+Two:400,700,400italic,700italic|Original+Surfer" media="all">¤<link href="https://maxcdn.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css" rel="stylesheet">¤<link rel="stylesheet" href="root/app_common/css/smoothness/jquery-ui-1.11.3.custom.min.css">¤¤<!--[if gt IE 8]>-->¤<script src="root/app_common/js/libs/modernizr-2.8.3.custom.min.js"></script>¤<!--<![endif]-->¤¤<script src="//code.jquery.com/jquery-1.11.2.min.js"></script>¤<script>window.jQuery || document.write('<script src="root/app_common/js/libs/jquery-1.11.2.min.js"><\\/script>')</script>¤¤<script src="//ajax.googleapis.com/ajax/libs/jqueryui/1.11.3/jquery-ui.min.js"></script>¤<script>window.jQuery || document.write('<script src="root/app_common/js/libs/jquery-ui-1.11.3.custom.min.js"><\\/script>')</script>¤¤¤<script src="root/app_common/js/libs/jquery.placeholder.min.js" async></script>¤<script src="root/app_common/js/libs/wselect.min.js" async></script>¤<!-- Bootstrap compiled and minified JavaScript -->¤<script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.2/js/bootstrap.min.js"></script>¤<!-- <script src="https://use.fontawesome.com/1d5fda5f00.js" async></script> -->¤<script src="js/lyrc.min.js?v=1.0.5" async></script>¤¤</body>¤</html>¤<!-- Timer: 0.4927 secs | Server: ip-10-229-66-164 -->
            // <pre id="lyric-body-text" class="lyric-body" dir="ltr" data-lang="en">The high <a style="color:#333;" href="https://www.definitions.net/definition/priest">priest</a> of <a style="color:#333;" href="https://www.definitions.net/definition/money">money</a> looks down on the river¤The dawn <a style="color:#333;" href="https://www.definitions.net/definition/coming">coming</a> up on his <a style="color:#333;" href="https://www.definitions.net/definition/kingdom">kingdom</a> of gold¤When the rim of the sun <a style="color:#333;" href="https://www.definitions.net/definition/sends">sends</a> an <a style="color:#333;" href="https://www.definitions.net/definition/arrow">arrow</a> of silver¤He <a style="color:#333;" href="https://www.definitions.net/definition/prays">prays</a> to the gods of the <a style="color:#333;" href="https://www.definitions.net/definition/bought">bought</a> and the sold¤¤He <a style="color:#333;" href="https://www.definitions.net/definition/turns">turns</a> to his symbols, his <a style="color:#333;" href="https://www.definitions.net/definition/ribbons">ribbons</a> of numbers¤They <a style="color:#333;" href="https://www.definitions.net/definition/circle">circle</a> and spin on <a style="color:#333;" href="https://www.definitions.net/definition/their">their</a> mystical scroll¤He <a style="color:#333;" href="https://www.definitions.net/definition/looks">looks</a> for a sign <a style="color:#333;" href="https://www.definitions.net/definition/while">while</a> the city <a style="color:#333;" href="https://www.definitions.net/definition/still">still</a> slumbers¤And the <a style="color:#333;" href="https://www.definitions.net/definition/ribbons">ribbons</a> and the <a style="color:#333;" href="https://www.definitions.net/definition/river">river</a> forever unroll¤In his <a style="color:#333;" href="https://www.definitions.net/definition/kingdom">kingdom</a> of gold, his <a style="color:#333;" href="https://www.definitions.net/definition/kingdom">kingdom</a> of gold¤Kingdom of gold, his <a style="color:#333;" href="https://www.definitions.net/definition/kingdom">kingdom</a> of gold¤Kingdom of gold¤¤On the <a style="color:#333;" href="https://www.definitions.net/definition/horizon">horizon</a> an <a style="color:#333;" href="https://www.definitions.net/definition/enemy">enemy</a> haven¤Sends <a style="color:#333;" href="https://www.definitions.net/definition/traces">traces</a> of <a style="color:#333;" href="https://www.definitions.net/definition/smoke">smoke</a> high up into the sky¤A pack of dog <a style="color:#333;" href="https://www.definitions.net/definition/jackals">jackals</a> and a <a style="color:#333;" href="https://www.definitions.net/definition/rabble">rabble</a> of ravens¤Who'll come for his fortress, his <a style="color:#333;" href="https://www.definitions.net/definition/castle">castle</a> on high¤In his <a style="color:#333;" href="https://www.definitions.net/definition/kingdom">kingdom</a> of gold, <a style="color:#333;" href="https://www.definitions.net/definition/kingdom">kingdom</a> of gold¤Kingdom of gold, his <a style="color:#333;" href="https://www.definitions.net/definition/kingdom">kingdom</a> of gold¤Kingdom of gold¤¤His axes and <a style="color:#333;" href="https://www.definitions.net/definition/armour">armour</a> will <a style="color:#333;" href="https://www.definitions.net/definition/conquer">conquer</a> these devils¤The <a style="color:#333;" href="https://www.definitions.net/definition/turbulent">turbulent</a> raiders will <a style="color:#333;" href="https://www.definitions.net/definition/falter">falter</a> and fall¤Their <a style="color:#333;" href="https://www.definitions.net/definition/leaders">leaders</a> be taken, <a style="color:#333;" href="https://www.definitions.net/definition/their">their</a> camps <a style="color:#333;" href="https://www.definitions.net/definition/burned">burned</a> and levelled¤They'll hang in the wind from his <a style="color:#333;" href="https://www.definitions.net/definition/citadel">citadel</a> walls¤In his <a style="color:#333;" href="https://www.definitions.net/definition/kingdom">kingdom</a> of gold, <a style="color:#333;" href="https://www.definitions.net/definition/kingdom">kingdom</a> of gold¤Kingdom of gold, <a style="color:#333;" href="https://www.definitions.net/definition/kingdom">kingdom</a> of gold¤Kingdom of gold</pre>
            // (<pre id="lyric-body-text" class="lyric-body" dir="ltr" data-lang="en">)(.*)(</pre>)

            // Convert linefeeds and Reduce the input to the lyrics between the "pre" tags
            var input = html.Replace(Constants.NewLine, "¤").Replace("\r", "¤").Replace("\n", "¤");
            var idx1 = input.IndexOf("<pre", StringComparison.InvariantCultureIgnoreCase);
            var idx2 = input.IndexOf("</pre", StringComparison.InvariantCultureIgnoreCase);

            if ((idx1 > 0) && (idx2 > idx1))
                input = input.Substring(idx1, idx2 - idx1);
            else
                return null;

            // Get the contents inside the "pre" tags
            idx1 = input.IndexOf(">", StringComparison.InvariantCultureIgnoreCase);

            if (idx1 > 0)
                input = input.Substring(idx1 + 1);
            else
                throw new Exception("Missing closing \">\" in \"pre\" start tag in result HTML.");

            // Remove the "a" tags
            string output;
            var pattern = "<a[^>]*>(.*?)</a>";
            var replacement = "${1}";
            var rgx = new Regex(pattern);

            output = rgx.Replace(input, replacement);

            // Convert linefeeds back to the result
            ret = output.Replace("¤", Constants.NewLine);

            // If found, add the found lyric to the list
            if (!ret.IsNullOrEmptyTrimmed())
                await AddFoundLyric(ret, new Uri(uri.AbsoluteUri)).ConfigureAwait(false);

            return ret;
        }


        /// <summary>
        /// Gets value indicating whether this service's quota is exceeded.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this service's quota is exceeded; otherwise, <c>false</c>.
        /// </returns>
        public override async Task<bool> IsQuotaExceededAsync()
        {
            var ret = await base.IsQuotaExceededAsync().ConfigureAwait(false);

            // UTC date / time calculations for the quota
            var nowDate = DateTime.UtcNow.Date;
            var quotaDate = QuotaResetTime.UniversalTime.Date;
            var quotaDiffDays = (int)Math.Ceiling(nowDate.Subtract(quotaDate).TotalDays);

            if (quotaDiffDays > 0)
            {
                QuotaResetTime.AddDays(quotaDiffDays);
                await ResetTodayCountersAsync().ConfigureAwait(false);
                await Logging.LogAsync(0, $"A new quota-day has begun for lyric service \"{Credit.ServiceName}\", request counters are reset.").ConfigureAwait(false);
            }

            ret = (DailyQuota > 0) && (RequestCountToday > DailyQuota);

            // For now, we will not return the calculation result.
            // Instead we let the lyric service tell us in its response, when the quota has been exceeded.
            // Look for "<ERROR>DAILY USAGE EXCEEDED</ERROR>" in the ProcessAsync method.
            ret = false;

            return ret;
        }


        /// <summary>
        /// Processes the specified MediaCenter item.
        /// </summary>
        /// <param name="mcItem">The current Media Center item.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="isGetAll">if set to <c>true</c> get all search hits; else get the first one only.</param>
        /// <returns>
        ///   <see cref="AbstractLyricService" /> descendant object of type <see cref="Stands4Service" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">item</exception>
        /// <exception cref="LyricsQuotaExceededException">Lyric service \"{Credit.ServiceName}\" is exceeding the daily limit of {DailyQuota} requests per day "
        /// + $"and is now disabled in LyricsFinder, no more requests will be sent to this service until corrected.</exception>
        /// <exception cref="LyricServiceCommunicationException"></exception>
        /// <exception cref="LyricServiceBaseException"></exception>
        /// <exception cref="NullReferenceException">Response is null</exception>
        /// <exception cref="Exception">\"{Credit.ServiceName}\" call failed: \"{ex.Message}\". Request: \"{req.RequestUri.ToString()}\".</exception>
        public override async Task<AbstractLyricService> ProcessAsync(McMplItem mcItem, CancellationToken cancellationToken, bool isGetAll = false)
        {
            if (mcItem == null) throw new ArgumentNullException(nameof(mcItem));

            var credit = Credit as CreditType;
            var ub = new UriBuilder($"{Credit.ServiceUrl}?uid={UserId}&tokenid={Token}&term={mcItem.Name}");
            var txt = string.Empty;

            try
            {
                await base.ProcessAsync(mcItem, cancellationToken).ConfigureAwait(false); // Result: not found

                // Do the request
                txt = await HttpGetStringAsync(ub.Uri).ConfigureAwait(false);

                // Quota exceeded?
                if (txt.ToUpperInvariant().Contains("<ERROR>DAILY USAGE EXCEEDED</ERROR>"))
                    QuotaError(mcItem, isGetAll);

                // Deserialize the returned JSON
                var results = txt.XmlDeserializeFromString<StandsResultListType>();

                // Did we get results back from the service?
                if (results != null)
                {
                    // First we try a rigorous test
                    await ExtractAllLyricTextsAsync(mcItem, results, cancellationToken, isGetAll, true).ConfigureAwait(false);

                    // If not found or if we want all possible results, we next try a more lax test without the album
                    if (IsActive && !LyricsFinderData.MainData.StrictSearchOnly
                        && (isGetAll || (LyricResult != LyricsResultEnum.Found)))
                        await ExtractAllLyricTextsAsync(mcItem, results, cancellationToken, isGetAll, false).ConfigureAwait(false);
                }
            }
            catch (HttpRequestException ex)
            {
                throw new LyricServiceCommunicationException($"{Credit.ServiceName} request failed for: " +
                    Constants.NewLine + $"\"{mcItem.Artist}\" - \"{mcItem.Album}\" - \"{mcItem.Name}\".",
                    isGetAll, Credit, mcItem, ub.Uri, ex);
            }
            catch (Exception ex)
            {
                throw new LyricServiceBaseException($"{Credit.ServiceName} process failed for: " +
                    Constants.NewLine + $"\"{mcItem.Artist}\" - \"{mcItem.Album}\" - \"{mcItem.Name}\".",
                    isGetAll, Credit, mcItem, ex);
            }

            return this;
        }


        /// <summary>
        /// Sets the IsActive property to <c>false</c> and throws a quota error.
        /// </summary>
        /// <param name="mcItem">The current Media Center item.</param>
        /// <param name="isGetAll">if set to <c>true</c> get all search hits; else get the first one only.</param>
        /// <exception cref="LyricsQuotaExceededException">Lyric service ... is exceeding its quota...</exception>
        /// <remarks>
        /// Do NOT call the base routine!
        /// It would throw a generic exception instead of the one in this routine!
        /// </remarks>
        protected override void QuotaError(McMplItem mcItem = null, bool isGetAll = false)
        {
            IsActive = false;

            throw new LyricsQuotaExceededException(Constants.NewLine
                + $"Lyric service \"{Credit.ServiceName}\" is exceeding the daily limit of {DailyQuota} requests per day. " + Constants.NewLine
                + "The service is now disabled in LyricsFinder. " + Constants.NewLine
                + "Check the service daily request count in the lyric service form. " + Constants.NewLine
                + "No more requests will be sent to this service until corrected.",
                isGetAll, Credit, mcItem);
        }


        /// <summary>
        /// Refreshes the service settings from the service configuration file.
        /// </summary>
        public override async Task RefreshServiceSettingsAsync()
        {
            await base.RefreshServiceSettingsAsync().ConfigureAwait(false);

            if (IsConfigurationFileUsed)
            {
                var quotaResetTimeZone = TimeZoneInfo.FindSystemTimeZoneById(ServiceSettingsValue(Settings, "QuotaResetTimeZone"));

                QuotaResetTime = new ServiceDateTimeWithZone(DateTime.Parse(ServiceSettingsValue(Settings, "QuotaResetTime"), CultureInfo.CurrentCulture), quotaResetTimeZone);
            }

            CreateDisplayProperties();
        }


        /// <summary>
        /// Validates the display properties.
        /// </summary>
        public override void ValidateDisplayProperties()
        {
            base.ValidateDisplayProperties();

            // Test initialization
            var dps = new Dictionary<string, DisplayProperty>
            {
                { nameof(Token), Token },
                { nameof(UserId), UserId },
                { nameof(DailyQuota), DailyQuota.ToString(Constants.IntegerFormat, CultureInfo.CurrentCulture) },

                { "QuotaResetTimeZone", "QuotaResetTime", "TimeZoneId", QuotaResetTime.ServiceTimeZone.StandardName, null },

                { "QuotaResetTimeService", QuotaResetTime.ServiceLocalTime.AddDays(1).ToString(Constants.DateTimeFormat, CultureInfo.CurrentCulture) },
                { "QuotaResetTimeClient", QuotaResetTime.ClientLocalTime.AddDays(1).ToString(Constants.DateTimeFormat, CultureInfo.CurrentCulture) }
            };
        }

    }



    /// <summary>
    /// Result type from STANDS4 service.
    /// </summary>
    [ClassInterface(ClassInterfaceType.None)]
    [Serializable, XmlType("result")]
    public class StandsResultType
    {
        [XmlElement("song")]
        public string Song { get; set; }

        [XmlElement("song-link")]
        public string SongLink { get; set; }

        [XmlElement("artist")]
        public string Artist { get; set; }

        [XmlElement("artist-link")]
        public string ArtistLink { get; set; }

        [XmlElement("album")]
        public string Album { get; set; }

        [XmlElement("album-link")]
        public string AlbumLink { get; set; }
    }



    /// <summary>
    /// Results root type from STANDS4 service.
    /// </summary>
    [ClassInterface(ClassInterfaceType.None)]
    [Serializable, XmlRoot(ElementName = "results", IsNullable = false)]
    public class StandsResultListType : List<StandsResultType>
    {
    }

}
