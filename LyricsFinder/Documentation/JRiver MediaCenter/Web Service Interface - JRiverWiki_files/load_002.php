var mediaWikiLoadStart=(new Date()).getTime(),mwPerformance=(window.performance&&performance.mark)?performance:{mark:function(){}};mwPerformance.mark('mwLoadStart');function isCompatible(str){var ua=str||navigator.userAgent;return!!('querySelector'in document&&'localStorage'in window&&'addEventListener'in window&&!(ua.match(/webOS\/1\.[0-4]/)||ua.match(/PlayStation/i)||ua.match(/SymbianOS|Series60|NetFront|Opera Mini|S40OviBrowser|MeeGo/)||(ua.match(/Glass/)&&ua.match(/Android/))));}(function(){var NORLQ,script;if(!isCompatible()){document.documentElement.className=document.documentElement.className.replace(/(^|\s)client-js(\s|$)/,'$1client-nojs$2');NORLQ=window.NORLQ||[];while(NORLQ.length){NORLQ.shift()();}window.NORLQ={push:function(fn){fn();}};window.RLQ={push:function(){}};return;}function startUp(){mw.config=new mw.Map(true);mw.loader.addSource({"local":"/load.php"});mw.loader.register([["site","xx8W9Ecy"],["noscript","sWD8kgDX",[],"noscript"],["filepage","Tj0NJfs7"],[
"user.groups","EWHjMPFi",[],"user"],["user","znxaGj8k",[],"user"],["user.cssprefs","GqV9IPpY",[],"private"],["user.defaults","pPg/TJeS"],["user.options","C9rS/VRT",[6],"private"],["user.tokens","bCtU2NFV",[],"private"],["mediawiki.language.data","bHbxm6Jh",[174]],["mediawiki.skinning.elements","5JNETUgo"],["mediawiki.skinning.content","rL78YjKq"],["mediawiki.skinning.interface","gCnU+L4s"],["mediawiki.skinning.content.parsoid","h1gyBVjQ"],["mediawiki.skinning.content.externallinks","I+RtOQs7"],["jquery.accessKeyLabel","ONupERwI",[25,130]],["jquery.appear","7p7+teMe"],["jquery.arrowSteps","shldTX/h"],["jquery.async","NpLA3vWS"],["jquery.autoEllipsis","gffdsw5T",[37]],["jquery.badge","XYyIAIXz",[171]],["jquery.byteLength","xoy0z36R"],["jquery.byteLimit","KW+RcdHS",[21]],["jquery.checkboxShiftClick","nIhkS1en"],["jquery.chosen","qzGDcolQ"],["jquery.client","G2CLFBBn"],["jquery.color","NHE1rOPl",[27]],["jquery.colorUtil","65EqEI/I"],["jquery.confirmable","Jzfs1FDN",[175]],["jquery.cookie",
"uWi06H3h"],["jquery.expandableField","hF/0SUj4"],["jquery.farbtastic","fNYUwnSr",[27]],["jquery.footHovzer","tNL1bPHZ"],["jquery.form","mw8uz+Sq"],["jquery.fullscreen","fjHz/oxB"],["jquery.getAttrs","R47Lm+AY"],["jquery.hidpi","5NgMEQpm"],["jquery.highlightText","Sez+KV/B",[242,130]],["jquery.hoverIntent","I/lj5j27"],["jquery.i18n","OhPEGSZv",[173]],["jquery.localize","P//o6ydZ"],["jquery.makeCollapsible","/uCpa8TQ"],["jquery.mockjax","6OlZ7R45"],["jquery.mw-jump","uLaH4JFY"],["jquery.mwExtension","Qm2VtaKn"],["jquery.placeholder","ja/6gXKp"],["jquery.qunit","S8iO0rLD"],["jquery.qunit.completenessTest","pWUPenAF",[46]],["jquery.spinner","UFakl3WA"],["jquery.jStorage","OFiwI3M3",[92]],["jquery.suggestions","gd0n1cN2",[37]],["jquery.tabIndex","jFL9il1y"],["jquery.tablesorter","RAAotRMI",[242,130,176]],["jquery.textSelection","VreVWWBN",[25]],["jquery.throttle-debounce","NVucLdiE"],["jquery.xmldom","O4hKnmmS"],["jquery.tipsy","6CarcJzR"],["jquery.ui.core","mYSiCD57",[58],"jquery.ui"],[
"jquery.ui.core.styles","Vsa8l9Zd",[],"jquery.ui"],["jquery.ui.accordion","iSZZefbA",[57,77],"jquery.ui"],["jquery.ui.autocomplete","eehbMGG1",[66],"jquery.ui"],["jquery.ui.button","PykpnWFk",[57,77],"jquery.ui"],["jquery.ui.datepicker","9YKVlOQ+",[57],"jquery.ui"],["jquery.ui.dialog","F1Py4L0C",[61,64,68,70],"jquery.ui"],["jquery.ui.draggable","7uIPbdgT",[57,67],"jquery.ui"],["jquery.ui.droppable","ygxxtI3P",[64],"jquery.ui"],["jquery.ui.menu","6UOl06bE",[57,68,77],"jquery.ui"],["jquery.ui.mouse","pe7L2OX7",[77],"jquery.ui"],["jquery.ui.position","81oJBJBi",[],"jquery.ui"],["jquery.ui.progressbar","Gwommv+9",[57,77],"jquery.ui"],["jquery.ui.resizable","WVUasD7J",[57,67],"jquery.ui"],["jquery.ui.selectable","uBScO1KP",[57,67],"jquery.ui"],["jquery.ui.slider","5bhQ1qrY",[57,67],"jquery.ui"],["jquery.ui.sortable","Ih9bjVi3",[57,67],"jquery.ui"],["jquery.ui.spinner","niFDAeut",[61],"jquery.ui"],["jquery.ui.tabs","n0wwfG//",[57,77],"jquery.ui"],["jquery.ui.tooltip","EVEc0t74",[57,68,77],
"jquery.ui"],["jquery.ui.widget","eOXNQfzv",[],"jquery.ui"],["jquery.effects.core","X5QThXoO",[],"jquery.ui"],["jquery.effects.blind","iXXfvSrD",[78],"jquery.ui"],["jquery.effects.bounce","RT22oytr",[78],"jquery.ui"],["jquery.effects.clip","SnRW/bg5",[78],"jquery.ui"],["jquery.effects.drop","nlaeluDG",[78],"jquery.ui"],["jquery.effects.explode","vZlr48Ft",[78],"jquery.ui"],["jquery.effects.fade","h52H0kv1",[78],"jquery.ui"],["jquery.effects.fold","3xKgCVPj",[78],"jquery.ui"],["jquery.effects.highlight","ex/oQn4Q",[78],"jquery.ui"],["jquery.effects.pulsate","7x44jwfk",[78],"jquery.ui"],["jquery.effects.scale","8Q1/G78k",[78],"jquery.ui"],["jquery.effects.shake","U40dOM2W",[78],"jquery.ui"],["jquery.effects.slide","V5CxvU1T",[78],"jquery.ui"],["jquery.effects.transfer","/yMStjYa",[78],"jquery.ui"],["json","erzglJxF",[],null,null,"return!!(window.JSON\u0026\u0026JSON.stringify\u0026\u0026JSON.parse);"],["moment","CcqQ958m"],["mediawiki.apihelp","RnmItywH"],["mediawiki.template","pB3Nq5Mw"
],["mediawiki.template.mustache","AZhQEfgD",[95]],["mediawiki.template.regexp","lnfUKSy3",[95]],["mediawiki.apipretty","3zjHLrd5"],["mediawiki.api","WpNjMz1V",[147,8]],["mediawiki.api.category","SSXLeYGh",[135,99]],["mediawiki.api.edit","5d7IAmVs",[135,99]],["mediawiki.api.login","lPvh14oL",[99]],["mediawiki.api.options","9u4wg46N",[99]],["mediawiki.api.parse","wQ0dxPiX",[99]],["mediawiki.api.upload","QZcJ2woO",[242,92,101]],["mediawiki.api.user","GtM00pMO",[99]],["mediawiki.api.watch","+KzwyNue",[99]],["mediawiki.api.messages","j5skGx0u",[99]],["mediawiki.content.json","mfprqG+/"],["mediawiki.confirmCloseWindow","r21TfS2q"],["mediawiki.debug","PY85ae18",[32,56]],["mediawiki.debug.init","A/0CLsmK",[111]],["mediawiki.feedback","5QLYWYko",[135,126,250]],["mediawiki.feedlink","CJnnQcAJ"],["mediawiki.filewarning","AL8vKZdR",[245]],["mediawiki.ForeignApi","iIZ8HI9q",[117]],["mediawiki.ForeignApi.core","UUrw0XgR",[99,243]],["mediawiki.helplink","BLJmqbZz"],["mediawiki.hidpi","UNwMapr7",[36],
null,null,"return'srcset'in new Image();"],["mediawiki.hlist","tES04H8s"],["mediawiki.htmlform","Y+REo44E",[22,130]],["mediawiki.htmlform.styles","SY3hkDOM"],["mediawiki.htmlform.ooui.styles","ufUQw1qi"],["mediawiki.icon","6Muo/l4v"],["mediawiki.inspect","i6JMYVAN",[21,92,130]],["mediawiki.messagePoster","fVqxYhvl",[116]],["mediawiki.messagePoster.wikitext","dTWNuQD7",[101,126]],["mediawiki.notification","BsyQWykJ",[183]],["mediawiki.notify","Ysk38c6c"],["mediawiki.RegExp","5WCm5bJp"],["mediawiki.pager.tablePager","IMq3sYVc"],["mediawiki.searchSuggest","N5lojoI9",[35,45,50,99]],["mediawiki.sectionAnchor","pDvL9TTN"],["mediawiki.storage","QoCkC7kO"],["mediawiki.Title","Isgd/XWF",[21,147]],["mediawiki.Upload","kgM6p2vB",[105]],["mediawiki.ForeignUpload","ROREx6fS",[116,136]],["mediawiki.ForeignStructuredUpload.config","JtmRymYi"],["mediawiki.ForeignStructuredUpload","OOdpMjkj",[138,137]],["mediawiki.Upload.Dialog","8+PH7AuW",[141]],["mediawiki.Upload.BookletLayout","H3VmHDhN",[136,175,
145,240,93,250,256,257]],["mediawiki.ForeignStructuredUpload.BookletLayout","SDjBRHCW",[139,141,108,179,236,234]],["mediawiki.toc","TJSuxwfW",[151]],["mediawiki.Uri","12lcYjxA",[147,97]],["mediawiki.user","r2uX5rki",[106,151,7]],["mediawiki.userSuggest","VK1CHpzU",[50,99]],["mediawiki.util","UhAInJej",[15,129]],["mediawiki.viewport","1md2zOBX"],["mediawiki.checkboxtoggle","mPFYw/rh"],["mediawiki.checkboxtoggle.styles","R2SvX+VW"],["mediawiki.cookie","uac3Fx++",[29]],["mediawiki.toolbar","knaJjjQf"],["mediawiki.experiments","nwrbcTpX"],["mediawiki.raggett","CVT0fORv"],["mediawiki.action.edit","JWppkai7",[22,53,156]],["mediawiki.action.edit.styles","LmP2/AVf"],["mediawiki.action.edit.collapsibleFooter","uq9kExhh",[41,151,124]],["mediawiki.action.edit.preview","f0dn8HZZ",[33,48,53,161,99,175]],["mediawiki.action.edit.stash","qnOE1CYj",[35,99]],["mediawiki.action.history","Of3lDYgX"],["mediawiki.action.history.diff","pYovBPb0"],["mediawiki.action.view.dblClickEdit","1w2nH9lb",[183,7]],[
"mediawiki.action.view.metadata","vixO1LCR"],["mediawiki.action.view.categoryPage.styles","CJcP8lnC"],["mediawiki.action.view.postEdit","PFqvmw4I",[151,175,95]],["mediawiki.action.view.redirect","lj3tHVRB",[25]],["mediawiki.action.view.redirectPage","rKTUohvc"],["mediawiki.action.view.rightClickEdit","EpZHxWwN"],["mediawiki.action.edit.editWarning","yHl/X8de",[53,110,175]],["mediawiki.action.view.filepage","+SokICKn"],["mediawiki.language","c8yA1vwU",[172,9]],["mediawiki.cldr","WND4V7IO",[173]],["mediawiki.libs.pluralruleparser","JXM0dXrn"],["mediawiki.language.init","RWplUq3j"],["mediawiki.jqueryMsg","3BFJOerX",[242,171,147,7]],["mediawiki.language.months","CAzK4kVG",[171]],["mediawiki.language.names","dVn8oVHH",[174]],["mediawiki.language.specialCharacters","v31HqVfX",[171]],["mediawiki.libs.jpegmeta","s1wuUl53"],["mediawiki.page.gallery","Nbt+UmXw",[54,181]],["mediawiki.page.gallery.styles","L0hLBsyr"],["mediawiki.page.ready","1bPMqCdt",[15,23,41,43,45]],["mediawiki.page.startup",
"6UqLjo0Z",[147]],["mediawiki.page.patrol.ajax","Eb5PwWjN",[48,135,99,183]],["mediawiki.page.watch.ajax","SCyfBP65",[107,183]],["mediawiki.page.image.pagination","Eau2XOM8",[48,147]],["mediawiki.special","Q4fjUCAq"],["mediawiki.special.apisandbox.styles","NgSQYQdo"],["mediawiki.special.apisandbox","85d81c7L",[99,175,187,235,244]],["mediawiki.special.block","+HQNpvHH",[147]],["mediawiki.special.blocklist","Eja6Txw/"],["mediawiki.special.changeslist","sCIjW1/6"],["mediawiki.special.changeslist.legend","92KNUQeh"],["mediawiki.special.changeslist.legend.js","qGM9Jskn",[41,151]],["mediawiki.special.changeslist.enhanced","Ed+VQZ2W"],["mediawiki.special.changeslist.visitedstatus","xRfQHpfe"],["mediawiki.special.comparepages.styles","pR7Trwgk"],["mediawiki.special.edittags","YH8jpVya",[24]],["mediawiki.special.edittags.styles","nm85b9yA"],["mediawiki.special.import","IAwRIcBl"],["mediawiki.special.movePage","hyPzZAhb",[232]],["mediawiki.special.movePage.styles","UoSUYOcu"],[
"mediawiki.special.pageLanguage","9qOtQ9rb",[245]],["mediawiki.special.pagesWithProp","5/csa+uo"],["mediawiki.special.preferences","3m9GnLJ4",[110,171,128]],["mediawiki.special.preferences.styles","HZBj/+lt"],["mediawiki.special.recentchanges","oZ50Y1yY",[187]],["mediawiki.special.search","M6vLE0R7",[238]],["mediawiki.special.undelete","h6DaU821"],["mediawiki.special.upload","ThjThHAK",[48,135,99,110,175,179,95]],["mediawiki.special.userlogin.common.styles","Goco4+kc"],["mediawiki.special.userlogin.signup.styles","N2jMpuFt"],["mediawiki.special.userlogin.login.styles","3q5Ku5zL"],["mediawiki.special.userlogin.signup.js","h71IUKOM",[54,99,175]],["mediawiki.special.unwatchedPages","zBGzH3Hz",[135,107]],["mediawiki.special.watchlist","rN/PT2aN"],["mediawiki.special.version","OkSj7Qqk"],["mediawiki.legacy.config","Gmu9hVha"],["mediawiki.legacy.commonPrint","nfkqKx6i"],["mediawiki.legacy.protect","lwCAr8ue",[22]],["mediawiki.legacy.shared","aqLQgcjh"],["mediawiki.legacy.oldshared",
"+uWRRaCl"],["mediawiki.legacy.wikibits","Xc3773x3",[147]],["mediawiki.ui","ctCxbQdW"],["mediawiki.ui.checkbox","kr5eRg2i"],["mediawiki.ui.radio","L265Xbgn"],["mediawiki.ui.anchor","Y3qsqp0A"],["mediawiki.ui.button","nh5752gr"],["mediawiki.ui.input","okv+sMHS"],["mediawiki.ui.icon","eGoZhuw9"],["mediawiki.ui.text","+NFtzLc4"],["mediawiki.widgets","Q74+qovT",[19,22,135,99,233,248]],["mediawiki.widgets.styles","zYP5uqTI"],["mediawiki.widgets.DateInputWidget","rzcCMz+L",[93,248]],["mediawiki.widgets.datetime","jKUeCbd5",[245]],["mediawiki.widgets.CategorySelector","+xz/UfJ0",[116,135,248]],["mediawiki.widgets.UserInputWidget","j2Z+yEgv",[248]],["mediawiki.widgets.SearchInputWidget","LSTI92Ne",[132,232]],["mediawiki.widgets.SearchInputWidget.styles","r11I+DR0"],["mediawiki.widgets.StashedFileWidget","e4apsW29",[245]],["es5-shim","GDdr+hsM",[],null,null,"return(function(){'use strict';return!this\u0026\u0026!!Function.prototype.bind;}());"],["dom-level2-shim","xw1hRC/c",[],null,null,
"return!!window.Node;"],["oojs","EuuhdeGo",[241,92]],["oojs-ui","W7GEksjf",[249,248,250]],["oojs-ui-core","mtEgeIW1",[171,243,246]],["oojs-ui-core.styles","TCOeDb6A",[251,252,253],null,null,"return!!jQuery('meta[name=\"X-OOUI-PHP\"]').length;"],["oojs-ui.styles","HAKO71EW",[251,252,253],null,null,"return!!jQuery('meta[name=\"X-OOUI-PHP\"]').length;"],["oojs-ui-widgets","Zksitbaf",[245]],["oojs-ui-toolbars","tKR+G5UN",[245]],["oojs-ui-windows","reDfQR7w",[245]],["oojs-ui.styles.icons","3Yeo507B"],["oojs-ui.styles.indicators","PwrVS5mf"],["oojs-ui.styles.textures","4+Sm/aJs"],["oojs-ui.styles.icons-accessibility","T0FG33ry"],["oojs-ui.styles.icons-alerts","kNu35b1I"],["oojs-ui.styles.icons-content","aVAtWPuZ"],["oojs-ui.styles.icons-editing-advanced","ICDSVLZZ"],["oojs-ui.styles.icons-editing-core","57VzQF4l"],["oojs-ui.styles.icons-editing-list","rAM4eRON"],["oojs-ui.styles.icons-editing-styling","4lpas3rF"],["oojs-ui.styles.icons-interactions","mbxgwUW+"],["oojs-ui.styles.icons-layout"
,"Mcuu2Wgx"],["oojs-ui.styles.icons-location","l0u3nAGC"],["oojs-ui.styles.icons-media","mULddXbl"],["oojs-ui.styles.icons-moderation","Voy5OO5M"],["oojs-ui.styles.icons-movement","QcR6qmsj"],["oojs-ui.styles.icons-user","11e0lXjB"],["oojs-ui.styles.icons-wikimedia","Ciz4NQao"],["skins.monobook.styles","97A3S7zI"],["ext.embedVideo","qZffjkVD"]]);;mw.config.set({"wgLoadScript":"/load.php","debug":!1,"skin":"monobook","stylepath":"/skins","wgUrlProtocols":"bitcoin\\:|ftp\\:\\/\\/|ftps\\:\\/\\/|geo\\:|git\\:\\/\\/|gopher\\:\\/\\/|http\\:\\/\\/|https\\:\\/\\/|irc\\:\\/\\/|ircs\\:\\/\\/|magnet\\:|mailto\\:|mms\\:\\/\\/|news\\:|nntp\\:\\/\\/|redis\\:\\/\\/|sftp\\:\\/\\/|sip\\:|sips\\:|sms\\:|ssh\\:\\/\\/|svn\\:\\/\\/|tel\\:|telnet\\:\\/\\/|urn\\:|worldwind\\:\\/\\/|xmpp\\:|\\/\\/","wgArticlePath":"/index.php/$1","wgScriptPath":"","wgScriptExtension":".php","wgScript":"/index.php","wgSearchType":null,"wgVariantArticlePath":!1,"wgActionPaths":{},"wgServer":"//wiki.jriver.com",
"wgServerName":"wiki.jriver.com","wgUserLanguage":"en","wgContentLanguage":"en","wgTranslateNumerals":!0,"wgVersion":"1.27.4","wgEnableAPI":!0,"wgEnableWriteAPI":!0,"wgMainPageTitle":"Main Page","wgFormattedNamespaces":{"-2":"Media","-1":"Special","0":"","1":"Talk","2":"User","3":"User talk","4":"JRiverWiki","5":"JRiverWiki talk","6":"File","7":"File talk","8":"MediaWiki","9":"MediaWiki talk","10":"Template","11":"Template talk","12":"Help","13":"Help talk","14":"Category","15":"Category talk"},"wgNamespaceIds":{"media":-2,"special":-1,"":0,"talk":1,"user":2,"user_talk":3,"jriverwiki":4,"jriverwiki_talk":5,"file":6,"file_talk":7,"mediawiki":8,"mediawiki_talk":9,"template":10,"template_talk":11,"help":12,"help_talk":13,"category":14,"category_talk":15,"image":6,"image_talk":7,"project":4,"project_talk":5},"wgContentNamespaces":[0],"wgSiteName":"JRiverWiki","wgDBname":"wiki","wgExtraSignatureNamespaces":[],"wgAvailableSkins":{"monobook":"MonoBook","fallback":"Fallback","apioutput":
"ApiOutput"},"wgExtensionAssetsPath":"/extensions","wgCookiePrefix":"wiki","wgCookieDomain":"","wgCookiePath":"/","wgCookieExpiration":15552000,"wgResourceLoaderMaxQueryLength":2000,"wgCaseSensitiveNamespaces":[],"wgLegalTitleChars":" %!\"$&'()*,\\-./0-9:;=?@A-Z\\\\\\^_`a-z~+\\u0080-\\uFFFF","wgResourceLoaderStorageVersion":1,"wgResourceLoaderStorageEnabled":!1,"wgResourceLoaderLegacyModules":[],"wgForeignUploadTargets":[],"wgEnableUploads":!0});var RLQ=window.RLQ||[];while(RLQ.length){RLQ.shift()();}window.RLQ={push:function(fn){fn();}};window.NORLQ={push:function(){}};}script=document.createElement('script');script.src="/load.php?debug=false&lang=en&modules=jquery%2Cmediawiki&only=scripts&skin=monobook&version=EkyLrXbD";script.onload=script.onreadystatechange=function(){if(!script.readyState||/loaded|complete/.test(script.readyState)){script.onload=script.onreadystatechange=null;script=null;startUp();}};document.getElementsByTagName('head')[0].appendChild(script);}());