#pragma once

/***********************************************************************************
Media Core Commands (for Media Center 9.x and later)
Copyright (c) 2003-2007 J. River, Inc. -- All Rights Reserved.

Each command has a "what command" (i.e. MCC_PLAY_PAUSE) and also an optional parameter,
which is explained by the comment following the command.  If you don't use the parameter,
set it to '0'.

Both parts are numbers. To determine what number a command is, count up from the command above
it with a number.

Example (1): MCC_PLAY_PAUSE = 10000; MCC_PLAY = 10001; MCC_STOP = 10002; etc...
Example (2): MCC_OPEN_FILE = 20000; MCC_OPEN_URL = 20001; etc...

Note: Some commands may only work with the latest version of Media Center.
***********************************************************************************/

enum MC_COMMANDS
{
    MCC_FIRST = 10000,

    ///////////////////////////////////////////////////////////////////////////////
    // Playback (range 10,000 to 20,000)
    // 
    // To issue playback commands to a specific zone, mask these values with the parameter:
    // Current Zone: 0
    // Zone 0: 16777216 (or 0x1000000 hex)
    // Zone 1: 33554432 (or 0x2000000 hex)
    // Zone 2: 50331648 (or 0x3000000 hex)
    // Zone 3: 67108864 (or 0x4000000 hex)
    // Zone 4: 83886080 (or 0x5000000 hex)
    // Zone 5: 100663296 (or 0x6000000 hex)
    // etc... (keep adding 16777216 (or 2^24)) (up to Zone 31)
    //
    // for the geeks, this is the top 6 bits of the 32-bit parameter
    // the lower 24 bits are used for the rest of the parameter (see the C++ macros below if you like)
    // if bit 32 is set, we assume someone passed in a simple negative number, so discard the zone portion
    // 
    // for parameters >= 0: zone number + parameter
    // for parameters < 0: zone number + (16777216 + parameter)
    // example: parameter -1 to zone 3: 67108864 + (16777216 + -1) = 83886079
    ///////////////////////////////////////////////////////////////////////////////
    MCC_PLAYBACK_SECTION = 10000,
    MCC_PLAY_PAUSE = 10000,                        // [ignore]
    MCC_PLAY,                                      // [ignore]
    MCC_STOP,                                      // [BOOL bDisplayWarning]
    MCC_NEXT,                                      // [BOOL bNotActualNext]
    MCC_PREVIOUS,                                  // [ignore]
    MCC_SHUFFLE,                                   // [0: toggle shuffle; 1: shuffle, jump to PN; 2: shuffle, no jump; 3..5: set mode]
    MCC_CONTINUOUS,                                // [0: toggle continuous; 1: off; 2: playlist; 3: song]
    MCC_OBSOLETE_10007,                            // [ignore]
    MCC_FAST_FORWARD,                              // [int nRate]
    MCC_REWIND,                                    // [int nRate]
    MCC_STOP_CONDITIONAL,                          // [ignore]
    MCC_SET_ZONE,                                  // [int nZoneIndex (-1 toggles forward, -2 toggles backwards)]
    MCC_TOGGLE_DISPLAY,                            // [BOOL bExcludeTheaterView]
    MCC_SHOW_WINDOW,                               // [BOOL bJumpToPlayingNow]
    MCC_MINIMIZE_WINDOW,                           // [ignore]
    MCC_PLAY_CPLDB_INDEX,                          // [int nIndex]
    MCC_SHOW_DSP_STUDIO,                           // [ignore]
    MCC_VOLUME_MUTE,                               // [0: toggle; 1: mute; 2: unmute]
    MCC_VOLUME_UP,                                 // [int nDeltaPercent]
    MCC_VOLUME_DOWN,                               // [int nDeltaPercent]
    MCC_VOLUME_SET,                                // [int nPercent]
    MCC_SHOW_PLAYBACK_OPTIONS,                     // [ignore]
    MCC_SET_PAUSE,                                 // [BOOL bPause (-1 toggles)]
    MCC_SET_CURRENTLY_PLAYING_RATING,              // [int nRating (0 means ?)]
    MCC_SHOW_PLAYBACK_ENGINE_MENU,                 // [screen point (loword: x, hiword: y) -- must send directly]
    MCC_PLAY_NEXT_PLAYLIST,                        // [ignore]
    MCC_PLAY_PREVIOUS_PLAYLIST,                    // [ignore]
    MCC_MAXIMIZE_WINDOW,                           // [ignore]
    MCC_RESTORE_WINDOW,                            // [ignore]
    MCC_SET_PLAYERSTATUS,                          // [PLAYER_STATUS_CODES Code]
    MCC_SET_ALTERNATE_PLAYBACK_SETTINGS,           // [BOOL bAlternateSettings (-1 toggles)]
    MCC_SET_PREVIEW_MODE_SETTINGS,                 // [low 12 bits: int nDurationSeconds, high 12 bits: int nStartSeconds]
    MCC_SHOW_PLAYBACK_ENGINE_DISPLAY_PLUGIN_MENU,  // [screen point (loword: x, hiword: y) -- must send directly]
    MCC_DVD_MENU,                                  // [ignore]
    MCC_SEEK_FORWARD,                              // [int nMilliseconds (0 means default -- varies depending on playback type)]
    MCC_SEEK_BACK,                                 // [int nMilliseconds (0 means default -- varies depending on playback type)]
    MCC_STOP_AFTER_CURRENT_FILE,                   // [BOOL bStopAfterCurrentFile (-1 toggles)]
    MCC_DETACH_DISPLAY,                            // [BOOL bDetach (-1 toggles)]
    MCC_SET_MODE_ZONE_SPECIFIC,                    // [UI_MODES Mode]
    MCC_STOP_INTERNAL,                             // [ignore]
    MCC_PLAYING_NOW_REMOVE_DUPLICATES,             // [ignore]
    MCC_SHUFFLE_REMAINING,                         // [ignore]

    ///////////////////////////////////////////////////////////////////////////////
    // File (range 20,000 to 21,000)
    ///////////////////////////////////////////////////////////////////////////////
    MCC_FILE_SECTION = 20000,
    MCC_OPEN_FILE = 20000,                         // [ignore]
    MCC_OPEN_URL,                                  // [ignore]
    MCC_PRINT_LIST,                                // [ignore]
    MCC_EXPORT_PLAYLIST,                           // [int nPlaylistID (-1 for active view)]
    MCC_EXPORT_ALL_PLAYLISTS,                      // [ignore]
    MCC_UPLOAD_FILES,                              // [ignore]
    MCC_EMAIL_FILES,                               // [ignore]
    MCC_EXIT,                                      // [BOOL bForce]
    MCC_UPDATE_LIBRARY,                            // [ignore]
    MCC_CLEAR_LIBRARY,                             // [ignore]
    MCC_EXPORT_LIBRARY,                            // [ignore]
    MCC_BACKUP_LIBRARY,                            // [ignore]
    MCC_RESTORE_LIBRARY,                           // [ignore]
    MCC_LIBRARY_MANAGER,                           // [ignore]
    MCC_IMAGE_ACQUIRE,                             // [ignore]
    MCC_PRINT_IMAGES,                              // [MFKEY nKey (-1 for selected files)]
    MCC_PRINT,                                     // [ignore]
    MCC_OBSOLETE_20017,                            // [ignore]
    MCC_BROADCAST_PLAYLIST,                        // [int nPlaylistID]
    MCC_STOP_BROADCAST_PLAYLIST,                   // [int nPlaylistID]
    MCC_PLAYLIST_BROADCAST_OPTIONS,                // [int nPlaylistID]
    MCC_ADCAST_PLAYLIST,                           // [int nPlaylistID]
    MCC_STOP_ADCAST_PLAYLIST,                      // [int nPlaylistID]
    MCC_PLAYLIST_ADCAST_OPTIONS,                   // [int nPlaylistID]
    MCC_IMPORT_PLAYLIST,                           // [ignore]
    MCC_LOAD_LIBRARY,                              // [int nLibraryIndex]
    MCC_SYNC_LIBRARY,                              // [ignore]
    MCC_EMAIL_PODCAST_FEED,                        // [ignore]

    ///////////////////////////////////////////////////////////////////////////////
    // Edit (range 21,000 to 22,000)
    ///////////////////////////////////////////////////////////////////////////////
    MCC_EDIT_SECTION = 21000,
    MCC_COPY = 21000,                              // [ignore]
    MCC_PASTE,                                     // [ignore]
    MCC_SELECT_ALL,                                // [ignore]
    MCC_SELECT_INVERT,                             // [ignore]
    MCC_DELETE,                                    // [BOOL bAggressive]
    MCC_RENAME,                                    // [ignore]
    MCC_UNDO,                                      // [ignore]
    MCC_REDO,                                      // [ignore]
    MCC_QUICK_SEARCH,                              // [BOOL bRepeatLastSearch]
    MCC_ADD_PLAYLIST,                              // [MEDIAFILE_INFO_ARRAY * paryFiles = NULL]
    MCC_ADD_SMARTLIST,                             // [ignore]
    MCC_ADD_PLAYLIST_GROUP,                        // [ignore]
    MCC_PROPERTIES,                                // [MEDIAFILE_INFO_ARRAY * paryFiles = NULL (-1 toggles) (note: never PostMessage(...) a pointer)]
    MCC_TOGGLE_TAGGING_MODE,                       // [ignore]
    MCC_CUT,                                       // [ignore]
    MCC_DESELECT_ALL,                              // [ignore]
    MCC_DELETE_ALL,                                // [BOOL bAggressive]
    MCC_ADD_PODCAST_FEED,                          // [ignore]
    MCC_EDIT_PODCAST_FEED,                         // [ignore]
    MCC_ADD_PODCAST_DEFAULTS,                      // [ignore]
    MCC_CREATE_STOCK_SMARTLISTS,                   // [ignore]
    MCC_ENABLE_PODCAST_DOWNLOAD,                   // [ignore]
    MCC_DISABLE_PODCAST_DOWNLOAD,                  // [ignore]
    MCC_EDIT_PLAYLIST,                             // [ignore]
    MCC_EDIT_PLAYING_NOW,                          // [ignore]
    MCC_EDIT_DISC_INFORMATION,                     // [ignore]
    MCC_EDIT_SMARTLIST,                            // [int nPlaylistID]

    ///////////////////////////////////////////////////////////////////////////////
    // View (range 22,000 to 23,000)
    ///////////////////////////////////////////////////////////////////////////////
    MCC_VIEW_SECTION = 22000,
    MCC_TOGGLE_MODE = 22000,                       // [-1: Next Mode (else UI_MODES Mode)]
    MCC_THEATER_VIEW,                              // [SHOW_THEATER_VIEW_MODES Mode]
    MCC_PARTY_MODE,                                // [ignore]
    MCC_SHOW_TREE_ROOT,                            // [int nTreeRootIndex]
    MCC_FIND_MEDIA,                                // [wchar * pstrSearch (note: memory will be deleted by receiver)]
    MCC_BACK,                                      // [int nLevels (0 does 1 level)]
    MCC_FORWARD,                                   // [int nLevels (0 does 1 level)]
    MCC_REFRESH,                                   // [int nFlags (1: no webpage refresh)]
    MCC_SET_LIST_STYLE,                            // [int nListStyle (-1 toggles)]
    MCC_SET_MODE,                                  // [UI_MODES Mode]
    MCC_SHOW_ARTISTINFO,                           // [ignore]
    MCC_SHOW_FINDCD,                               // [ignore]
    MCC_SHOW_RECENTLYIMPORTED,                     // [ignore]
    MCC_SHOW_TOPHITS,                              // [ignore]
    MCC_SHOW_RECENTLYPLAYED,                       // [ignore]
    MCC_SET_MEDIA_MODE,                            // [int nMediaMode]
    MCC_CONFIGURE_ACCESS_CONTROL,                  // [ignore]
    MCC_SET_SERVER_MODE,                           // [BOOL bServerMode]
    MCC_SET_MODE_FOR_EXTERNAL_PROGRAM_LAUNCH,      // [int nType (0: starting external app, 1: ending external app)]
    MCC_SET_MODE_FOR_SECOND_INSTANCE_LAUNCH,       // [UI_MODES Mode]
    MCC_HOME,                                      // [ignore]
    MCC_ROLLUP_VIEW_HEADER,                        // [BOOL bRollup (-1: toggle)]
    MCC_FOCUS_SEARCH_CONTROL,                      // [ignore]
    MCC_SET_VIEW_INDEX,                            // [int nViewIndex (-1: toggle, -2: toggle backwards, -3: new view)]

    ///////////////////////////////////////////////////////////////////////////////
    // Tools (range 23,000 to 24,000)
    ///////////////////////////////////////////////////////////////////////////////
    MCC_TOOLS_SECTION = 23000,
    MCC_IMPORT = 23000,                            // [int nFlags (1: bDisableAlreadyRunningWarning)]
    MCC_RIP_CD,                                    // [ignore]
    MCC_BURN,                                      // [ignore]
    MCC_RECORD,                                    // [ignore]
    MCC_CONVERT,                                   // [ignore]
    MCC_ANALYZE_AUDIO,                             // [ignore]
    MCC_MEDIA_EDITOR,                              // [ignore]
    MCC_CD_LABELER,                                // [ignore]
    MCC_MUSICEX_MANAGER,                           // [ignore]
    MCC_PLUGIN_MANAGER,                            // [ignore]
    MCC_SKIN_MANAGER,                              // [ignore]
    MCC_OPTIONS,                                   // [int nPageID]
    MCC_RENAME_CD_FILES,                           // [ignore]
    MCC_WMLICENSE_MANAGER,                         // [ignore]
    MCC_SERVICES_MANAGER,                          // [ignore]
    MCC_HANDHELD_UPLOAD,                           // [loword: nDeviceSessionID (0 gets default), hiword: flags (1: sync only; 2: show warnings)]
    MCC_HANDHELD_UPDATE_UPLOAD_WORKER_FINISHED,    // [int nDeviceSessionID]
    MCC_HANDHELD_CLOSE_DEVICE,                     // [int nDeviceSessionID]
    MCC_HANDHELD_SHOW_OPTIONS,                     // [int nDeviceSessionID]
    MCC_HANDHELD_INFO_DUMP,                        // [BOOL bShowInfo]
    MCC_IMPORT_AUTO_RUN_NOW,                       // [ignore]
    MCC_IMPORT_AUTO_CONFIGURE,                     // [ignore]
    MCC_HANDHELD_EJECT,                            // [int nDeviceSessionID]

    ///////////////////////////////////////////////////////////////////////////////
    // Help (range 24,000 to 25,000)
    ///////////////////////////////////////////////////////////////////////////////
    MCC_HELP_SECTION = 24000,
    MCC_HELP_CONTENTS = 24000,                     // [ignore]
    MCC_HELP_HOWTO_IMPORT_FILES,                   // [ignore]
    MCC_HELP_HOWTO_PLAY_FILES,                     // [ignore]
    MCC_HELP_HOWTO_RIP,                            // [ignore]
    MCC_HELP_HOWTO_BURN,                           // [ignore]
    MCC_HELP_HOWTO_ORGANIZE_FILES,                 // [ignore]
    MCC_HELP_HOWTO_VIEW_SCHEMES,                   // [ignore]
    MCC_HELP_HOWTO_MANAGE_PLAYLISTS,               // [ignore]
    MCC_HELP_HOWTO_EDIT_PROPERTIES,                // [ignore]
    MCC_HELP_HOWTO_FIND,                           // [ignore]
    MCC_HELP_HOWTO_CONFIGURE,                      // [ignore]
    MCC_CHECK_FOR_UPDATES,                         // [ignore]
    MCC_BUY,                                       // [ignore]
    MCC_INSTALL_LICENSE,                           // [ignore]
    MCC_REGISTRATION_INFO,                         // [ignore]
    MCC_PLUS_FEATURES,                             // [ignore]
    MCC_INTERACT,                                  // [ignore]
    MCC_SYSTEM_INFO,                               // [ignore]
    MCC_ABOUT,                                     // [ignore]
    MCC_CONFIGURE_DEBUG_LOGGING,                   // [ignore]
    MCC_WIKI,                                      // [ignore]
    MCC_TEST,                                      // [ignore]
    MCC_SHOW_EULA,                                 // [ignore]

    ///////////////////////////////////////////////////////////////////////////////
    // Tree (range 25,000 to 26,000)
    ///////////////////////////////////////////////////////////////////////////////
    MCC_TREE_SECTION = 25000,
    MCC_ADD_VIEW_SCHEME = 25000,                   // [ignore]
    MCC_EDIT_VIEW_SCHEME,                          // [ignore]
    MCC_OBSOLETE_25002,                            // [ignore]
    MCC_OBSOLETE_25003,                            // [ignore]
    MCC_OBSOLETE_25004,                            // [ignore]
    MCC_OBSOLETE_25005,                            // [ignore]
    MCC_OBSOLETE_25006,                            // [ignore]
    MCC_OBSOLETE_25007,                            // [ignore]
    MCC_TREE_ADD_DIRECTORY,                        // [ignore]
    MCC_TREE_IMPORT,                               // [ignore]
    MCC_TREE_ADD_CD_FOLDER,                        // [ignore]
    MCC_UPDATE_FROM_CD_DATABASE,                   // [ignore]
    MCC_SUBMIT_TO_CD_DATABASE,                     // [ignore]
    MCC_TREE_RIP,                                  // [ignore]
    MCC_CLEAR_PLAYING_NOW,                         // [0: all files; 1: leave playing file]
    MCC_COPY_LISTENING_TO,                         // [BOOL bPaste]
    MCC_TREE_SET_EXPANDED,                         // [0: collapsed; 1: expanded]
    MCC_RESET_VIEW_SCHEMES,                        // [ignore]
    MCC_TREE_ERASE_CD_DVD,                         // [ignore]
    MCC_UPDATE_FROM_CDPLAYER_INI,                  // [ignore]
    MCC_TREE_EJECT,                                // [ignore]
    MCC_TREE_ADD_VIRTUAL_DEVICE,                   // [ignore]
    MCC_TREE_RENAME_PLAYLIST,                      // [int nPlaylistID]

    ///////////////////////////////////////////////////////////////////////////////
    // List (range 26,000 to 27,000)
    ///////////////////////////////////////////////////////////////////////////////
    MCC_LIST_SECTION = 26000,
    MCC_LIST_UPDATE_ORDER = 26000,                 // [ignore]
    MCC_LIST_SHUFFLE_ORDER,                        // [ignore]
    MCC_LIST_IMPORT,                               // [ignore]
    MCC_LIST_REMOVE_ORDER,                         // [ignore]
    MCC_LOCATE_FILE,                               // [int nLocation (-1: on disk (internal); -2: on disk (external); 0-n: library field index)
    MCC_LIST_MOVE_DISK_FILES,                      // [ignore]
    MCC_LIST_INCREMENT_SELECTION,                  // [int nDelta]
    MCC_LIST_REMOVE_DUPLICATES,                    // [ignore]
    MCC_LIST_AUTO_SIZE_COLUMN,                     // [int nColumn, zero-based column index (-1: all)]
    MCC_LIST_CUSTOMIZE_VIEW,                       // [ignore]
    MCC_LIST_COPY_DISK_FILES,                      // [ignore]
    MCC_LIST_SET_RIP_CHECK,                        // [0: uncheck, 1: check, -1: toggle]
    MCC_LIST_DOWNLOAD,                             // [ignore]
    MCC_LIST_GET_LIST_POINTER,                     // [ignore]

    ///////////////////////////////////////////////////////////////////////////////
    // System (range 27,000 to 28,000)
    ///////////////////////////////////////////////////////////////////////////////
    MCC_SYSTEM_SECTION = 27000,
    MCC_KEYSTROKE = 27000,                         // [int nKeyCode]
    MCC_SHUTDOWN,                                  // [ignore]

    ///////////////////////////////////////////////////////////////////////////////
    // Playback engine (range 28,000 to 29,000)
    ///////////////////////////////////////////////////////////////////////////////
    MCC_PLAYBACK_ENGINE_SECTION = 28000,
    MCC_PLAYBACK_ENGINE_ZOOM_IN = 28000,           // [ignore]
    MCC_PLAYBACK_ENGINE_ZOOM_OUT,                  // [ignore]
    MCC_PLAYBACK_ENGINE_UP,                        // [ignore]
    MCC_PLAYBACK_ENGINE_DOWN,                      // [ignore]
    MCC_PLAYBACK_ENGINE_LEFT,                      // [ignore]
    MCC_PLAYBACK_ENGINE_RIGHT,                     // [ignore]
    MCC_PLAYBACK_ENGINE_ENTER,                     // [ignore]
    MCC_IMAGE_FIRST,                               // [ignore]
    MCC_IMAGE_LAST,                                // [ignore]
    MCC_IMAGE_NEXT,                                // [ignore]
    MCC_IMAGE_PREVIOUS,                            // [ignore]
    MCC_IMAGE_PAUSE_SLIDESHOW,                     // [BOOL bPause (-1 toggles)]
    MCC_IMAGE_AUTO_PAN,                            // [BOOL bAutopan (-1 toggles)]
    MCC_IMAGE_TOGGLE_EFFECT,                       // [int nDelta]
    MCC_IMAGE_RAPID_ZOOM,                          // [int nRapidZoom]
    MCC_DVD_SET_AUDIO_STREAM,                      // [int nAudioStream (-1 toggles)]
    MCC_DVD_SHOW_MENU,                             // [ignore]
    MCC_TV_RECORD,                                 // [ignore]
    MCC_TV_SNAPSHOT,                               // [ignore]
    MCC_TV_CHANGE_STANDARD,                        // [ignore]
    MCC_PLAYBACK_ENGINE_OSD_VIDEO_PROC_AMP,        // [int nIndex (0 for brightness, 1 for contrast, etc. -1 cycles)]
    MCC_PLAYBACK_ENGINE_SET_CUR_VIDEO_PROC_AMP,    // [int nStep (... -2, -1, 1, 2, etc. 0 is invalid and will default to 1)]
    MCC_PLAYBACK_ENGINE_SET_ASPECT_RATIO,          // [int nIndex of available ratios (-1 toggles)]
    MCC_PLAYBACK_ENGINE_SCROLL_UP,                 // [ignore]
    MCC_PLAYBACK_ENGINE_SCROLL_DOWN,               // [ignore]
    MCC_PLAYBACK_ENGINE_SCROLL_LEFT,               // [ignore]
    MCC_PLAYBACK_ENGINE_SCROLL_RIGHT,              // [ignore]
    MCC_TV_SET_SAVE_TIME_SHIFTING,                 // [int nSaveMode (0 - 6, -1 cycles by incrementing, -2 cycles by decrementing)]

    ///////////////////////////////////////////////////////////////////////////////
    // Flavor specific (range 29,000 to 30,000) defined as offsets within the flavor
    ///////////////////////////////////////////////////////////////////////////////
    MCC_FLAVOR_SPECIFIC_SECTION = 29000,

    ///////////////////////////////////////////////////////////////////////////////
    // Other (range 30,000 to 31,000)
    ///////////////////////////////////////////////////////////////////////////////
    MCC_OTHER_SECTION = 30000,
    MCC_RELOAD_MC_VIEW = 30000,                    // [ignore]
    MCC_CUSTOMIZE_TOOLBAR,                         // [ignore]
    MCC_PLAY_TV,                                   // [ignore]
    MCC_UPDATE_WEBPAGES,                           // [ignore]
    MCC_SHOW_RUNNING_MC,                           // [BOOL bToggleVisibility]
    MCC_SHOW_MENU,                                 // [int nMenuID]
    MCC_TUNE_TV,                                   // [ignore]
    MCC_PLAY_PLAYLIST,                             // [int nPlaylistID]
    MCC_SENDTO_TOOL,                               // [0: labeler; 1: media editor; 2: default editor; 3: ftp upload; 4: email; 5 Menalto Gallery; 6 Web Gallery]
    MCC_SHOW_VIEW_INFO,                            // [new CMCViewInfo * (for internal use only)]
    MCC_SERVICES_HOME,                             // [ignore]
    MCC_DEVICE_CHANGED,                            // [new DEVICE_CHANGE_INFO * (for internal use only)]
    MCC_CONFIGURE_THEATER_VIEW,                    // [ignore]
    MCC_SET_STATUSTEXT,                            // [wchar * pstrText (note: memory will be deleted by receiver)]
    MCC_UPDATE_UI_AFTER_ACTIVE_WINDOW_CHANGE,      // [ignore]
    MCC_REENUM_PORTABLE_DEVICES,                   // [BOOL bDeviceConnected]
    MCC_PLAY_ADVANCED,                             // [PLAY_COMMAND * pCommand (deleted by receiver)]
    MCC_UPDATE_STATUS_BAR,                         // [ignore]
    MCC_REQUEST_PODCAST_UPDATE,                    // [ignore]
    MCC_REQUEST_PODCAST_PURGE,                     // [ignore]
    MCC_AUDIBLE_ACTIVATE_PC,                       // [BOOL bActivate (TRUE: activate, FALSE: deactivate)]

    ///////////////////////////////////////////////////////////////////////////////
    // Image tools (range 31,000 to 32,000)
    ///////////////////////////////////////////////////////////////////////////////
    MCC_IMAGE_SECTION = 31000,
    MCC_IMAGE_SET_DESKTOP_BACK = 31000,            // [ignore]
    MCC_IMAGE_ROTATE_LEFT,                         // [ignore]
    MCC_IMAGE_ROTATE_RIGHT,                        // [ignore]
    MCC_IMAGE_ROTATE_UPSIDEDOWN,                   // [ignore]
    MCC_IMAGE_RESIZE,                              // [ignore]
    MCC_IMAGE_EDIT,                                // [ignore]

    ///////////////////////////////////////////////////////////////////////////////
    // Query (range 32,000 to 33,000)
    ///////////////////////////////////////////////////////////////////////////////
    MCC_QUERY_SECTION = 32000,

    ///////////////////////////////////////////////////////////////////////////////
    // Commands (used internally -- get routed standard way)
    ///////////////////////////////////////////////////////////////////////////////
    MCC_COMMANDS_SECTION = 33000,
    MCC_GET_SELECTED_FILES = 33000,                // [loword: GET_SELECTION_MODES Mode, hiword: short nFlags (1: for playback)]
    MCC_PRINTVIEW,                                 // [ignore]
    MCC_OUTPUT,                                    // [int nPlaylistID (-1 for active view)]
    MCC_SETFOCUS,                                  // [ignore]
    MCC_SELECT_FILES,                              // [CMediaArray *]
    MCC_DOUBLE_CLICK,                              // [ignore]
    MCC_PLAY_OR_SHOW,                              // [ignore]
    MCC_SHOW_CURRENT_FILE,                         // [int nFlags (1: force, 2: select)]
    MCC_BUY_SELECTED_TRACKS,                       // [int nPurchaseFlags]
    MCC_BUY_ALL_TRACKS,                            // [int nPurchaseFlags]
    MCC_BUY_ALBUM,                                 // [int nPurchaseFlags]
    MCC_UPDATE_AFTER_PLUGIN_INSTALLED,             // [ignore]

    ///////////////////////////////////////////////////////////////////////////////
    // Notifications (used internally -- go to all view windows)
    ///////////////////////////////////////////////////////////////////////////////
    MCC_NOTIFICATIONS_SECTION = 34000,
    MCC_NOTIFY_FONT_CHANGED = 34000,               // [ignore]
    MCC_NOTIFY_VIEW_CHANGED,                       // [ignore]
    MCC_NOTIFY_BEFORE_VIEW_INDEX_CHANGED,          // [ignore]
    MCC_NOTIFY_VIEW_INDEX_CHANGED,                 // [ignore]
    MCC_NOTIFY_PLAYER_INFO_CHANGED,                // [PLAYER_INFO_CHANGES nChange]
    MCC_NOTIFY_TOOLTIPS_CHANGED,                   // [BOOL bEnabled]
    MCC_NOTIFY_OPTIONS_CHANGED,                    // [ignore]
    MCC_UPDATE,                                    // [int nFlags]
    MCC_NOTIFY_FOCUS_CHANGED,                      // [ignore]
    MCC_SAVE_PROPERTIES,                           // [ignore]
    MCC_NOTIFY_UI_MODE_CHANGED,                    // [UI_MODES NewMode]
    MCC_NOTIFY_SELECTION_CHANGED,                  // [HWND hwndSource]
    MCC_NOTIFY_FILE_CHANGED,                       // [int nMFKey (-1: invalidates all files)]
    MCC_NOTIFY_FILE_STATUS_CHANGED,                // [int nMFKey (-1: invalidates all files)]
    MCC_NOTIFY_FILE_ENSURE_VISIBLE,                // [int nMFKey]
    MCC_NOTIFY_GET_TAB_HWNDS,                      // [ignore]
    MCC_NOTIFY_BURNER_QUEUE_CHANGED,               // [int nFlags (1: folder change)]
    MCC_NOTIFY_BURNER_PROGRESS_CHANGED,            // [int nPercentage]
    MCC_NOTIFY_BURNER_STATUS_CHANGED,              // [LPCTSTR pStatus]
    MCC_NOTIFY_BURNER_STARTED,                     // [ignore]
    MCC_NOTIFY_BURNER_FINISHED,                    // [ignore]
    MCC_NOTIFY_BURNER_FAILED,                      // [LPCTSTR pError]
    MCC_NOTIFY_BURNER_CLOSE_UI,                    // [ignore]
    MCC_NOTIFY_BURNER_PREPARE_FOR_NEXT_COPY,       // [LPCTSTR pStatus]
    MCC_NOTIFY_RIP_STARTED,                        // [ignore]
    MCC_NOTIFY_RIP_FINISHED,                       // [ignore]
    MCC_NOTIFY_RIP_FAILED,                         // [LPCTSTR pError]
    MCC_NOTIFY_RIP_PROGRESS_CHANGED,               // [ignore]
    MCC_NOTIFY_RIP_QUEUE_CHANGED,                  // [ignore]
    MCC_NOTIFY_DVD_RIP_STARTED,                    // [ignore]
    MCC_NOTIFY_DVD_RIP_FINISHED,                   // [ignore]
    MCC_NOTIFY_DVD_RIP_FAILED,                     // [int nErrorCode]
    MCC_NOTIFY_DVD_RIP_PROGRESS_CHANGED,           // [int nPercent]
    MCC_NOTIFY_DOWNLOAD_FINISHED,                  // [int nMFKey (-1: unknown)]
    MCC_NOTIFY_DOWNLOAD_FAILED,                    // [int nMFKey (-1: unknown)]
    MCC_NOTIFY_DOWNLOAD_STATUS_CHANGED,            // [LPCTSTR pStatus]
    MCC_NOTIFY_STATUS_CHECKER_COMPLETE,            // [ignore]
    MCC_NOTIFY_ZONE_CHANGED,                       // [ignore]
    MCC_NOTIFY_DISPLAY_OWNER_CHANGED,              // [HWND hwndOwner]
    MCC_NOTIFY_AFTER_FIRST_UPDATE_LAYOUT_WINDOW,   // [ignore]
    MCC_NOTIFY_AFTER_FIRST_UPDATE_APPLY_VIEW_STATE,// [ignore]
    MCC_NOTIFY_PROCESS_TIME_REMAINING,             // [int nSecondsRemaining]
    MCC_NOTIFY_UI_UPDATE_ENABLE_DISABLE_STATES,    // [ignore]
    MCC_NOTIFY_UI_SKIN_CHANGED,                    // [ignore]
    MCC_UPDATE_WINDOW_LAYOUT,                      // [ignore]
    MCC_NOTIFY_SAVE_UI_BEFORE_SHUTDOWN,            // [ignore]
    MCC_NOTIFY_UPDATE_CONTROL_BARS,                // [ignore]
    MCC_NOTIFY_PLAYLIST_FILES_CHANGED,             // [int nPlaylistID]
    MCC_NOTIFY_PLAYLIST_INFO_CHANGED,              // [int nPlaylistID]
    MCC_NOTIFY_PLAYLIST_ADDED_INTERNAL,            // [int nPlaylistID]
    MCC_NOTIFY_PLAYLIST_ADDED_BY_USER,             // [int nPlaylistID]
    MCC_NOTIFY_PLAYLIST_REMOVED,                   // [int nPlaylistID]
    MCC_NOTIFY_PLAYLIST_COLLECTION_CHANGED,        // [ignore]
    MCC_NOTIFY_PLAYLIST_PROPERTIES_CHANGED,        // [int nPlaylistID]
    MCC_NOTIFY_HANDHELD_UPLOAD_STARTED,            // [int nDeviceSessionID (0 gets default)]
    MCC_NOTIFY_HANDHELD_NEW_DEVICE_ARRIVED,        // [ing nDeviceSessionID]
    MCC_NOTIFY_HANDHELD_AFTER_DEVICE_CHANGED,      // [ignore]
    MCC_NOTIFY_HANDHELD_QUEUE_CHANGED,             // [ignore]
    MCC_NOTIFY_HANDHELD_INFO_COMPLETE,             // [ignore]
    MCC_NOTIFY_HANDHELD_AFTER_UPLOAD_FINISHED,     // [ignore]
    MCC_NOTIFY_COMPACT_MEMORY,                     // [ignore]
    MCC_NOTIFY_SEARCH_CHANGED,                     // [ignore]
    MCC_NOTIFY_SEARCH_CONTEXT_CHANGED,             // [ignore]
    MCC_NOTIFY_UPDATE_SHOPPING_CART,               // [CServicePlugin * pServicePlugin]
    MCC_NOTIFY_UPDATE_NAVIGATION_TRAIL,            // [ignore]
    MCC_NOTIFY_IMPORT_STARTED,                     // [BOOL bSilent]
    MCC_NOTIFY_IMPORT_FINISHED,                    // [BOOL bSilent]
    MCC_NOTIFY_ROTATED_IMAGES,                     // [MFKEY nKey]
    MCC_NOTIFY_SERVICE_LOGIN_STATE_CHANGE,         // [BOOL bLoggedIn]
    MCC_NOTIFY_MYGAL_PROGRESS,                     // [ignore]
    MCC_NOTIFY_MYGAL_DONE,                         // [ignore]
    MCC_NOTIFY_PODCAST_CHANGED,                    // [ignore]
    MCC_NOTIFY_CONVERT_PROGRESS,                   // [ignore]
    MCC_NOTIFY_CONVERT_DONE,                       // [ignore]
    MCC_NOTIFY_BREADCRUMBS_CHANGED,                // [ignore]
	MCC_NOTIFY_UI_LANGUAGE_CHANGED,				   // [ignore]	

    ///////////////////////////////////////////////////////////////////////////////
    // Last
    ///////////////////////////////////////////////////////////////////////////////
    MCC_LAST = 40000
};

///////////////////////////////////////////////////////////////////////////////
// Customization specific (used internally)
///////////////////////////////////////////////////////////////////////////////
#define MCC_CUSTOMIZATION_OFFSET 100000

/***********************************************************************************
How to issue Media Core commands

a) Post a WM_MC_COMMAND based message to the MC frame

Example (C++ source code):
HWND hwndMC = FindWindow(_T("MJFrame"), NULL);
PostMessage(hwndMC, WM_MC_COMMAND, MCC_PLAY_PAUSE, 0);

b) Fire the same command through the launcher (i.e. 'MC11.exe') in the system directory

Example (command-line program):
'MC11.exe' /MCC 10000, 0
***********************************************************************************/

// the WM_APP based message (WM_APP = 32768, so WM_MC_COMMAND = 33768)
#define WM_MC_COMMAND (WM_APP + 1000)

// extended MC_COMMAND message that takes a structure with extra information (internal use only)
#define WM_MC_COMMAND_EX (WM_APP + 1001)

// return value for unhandled MCC commands
#define MCC_UNHANDLED 0

// flags for command enable, disable, and check
enum MCC_UPDATEUI_FLAGS
{
    MCC_UPDATEUI_ENABLE = 1,
    MCC_UPDATEUI_DISABLE = 2,
    MCC_UPDATEUI_PRESSED = 4,
};

/***********************************************************************************
Helper macros
***********************************************************************************/

#define IS_MCC_COMMAND_IN_RANGE(INDEX, FIRST, LAST) (((abs((int)INDEX)) >= FIRST) && ((abs((int)INDEX)) < LAST) || ((abs((int)INDEX)) >= FIRST + MCC_CUSTOMIZATION_OFFSET) && ((abs((int)INDEX)) < LAST + MCC_CUSTOMIZATION_OFFSET))
#define IS_MCC_COMMAND_IN_SECTION(INDEX, FIRST) IS_MCC_COMMAND_IN_RANGE(INDEX, FIRST, FIRST + 1000)
#define IS_VALID_MCC_COMMAND(INDEX) IS_MCC_COMMAND_IN_RANGE(INDEX, MCC_FIRST, MCC_LAST)
#define IS_PLAYBACK_MCC_COMMAND(INDEX) IS_MCC_COMMAND_IN_SECTION(INDEX, MCC_PLAYBACK_SECTION)
#define IS_FILE_MCC_COMMAND(INDEX) IS_MCC_COMMAND_IN_SECTION(INDEX, MCC_FILE_SECTION)
#define IS_EDIT_MCC_COMMAND(INDEX) IS_MCC_COMMAND_IN_SECTION(INDEX, MCC_EDIT_SECTION)
#define IS_VIEW_MCC_COMMAND(INDEX) IS_MCC_COMMAND_IN_SECTION(INDEX, MCC_VIEW_SECTION)
#define IS_TOOL_MCC_COMMAND(INDEX) IS_MCC_COMMAND_IN_SECTION(INDEX, MCC_TOOLS_SECTION)
#define IS_HELP_MCC_COMMAND(INDEX) IS_MCC_COMMAND_IN_SECTION(INDEX, MCC_HELP_SECTION)
#define IS_TREE_MCC_COMMAND(INDEX) IS_MCC_COMMAND_IN_SECTION(INDEX, MCC_TREE_SECTION)
#define IS_LIST_MCC_COMMAND(INDEX) IS_MCC_COMMAND_IN_SECTION(INDEX, MCC_LIST_SECTION)
#define IS_SYSTEM_MCC_COMMAND(INDEX) IS_MCC_COMMAND_IN_SECTION(INDEX, MCC_SYSTEM_SECTION)
#define IS_PLAYBACK_ENGINE_MCC_COMMAND(INDEX) IS_MCC_COMMAND_IN_SECTION(INDEX, MCC_PLAYBACK_ENGINE_SECTION)
#define IS_IMAGE_MCC_COMMAND(INDEX) IS_MCC_COMMAND_IN_SECTION(INDEX, MCC_IMAGE_SECTION)
#define IS_INTERNAL_COMMAND_MCC_COMMAND(INDEX) IS_MCC_COMMAND_IN_SECTION(INDEX, MCC_COMMANDS_SECTION)
#define IS_NOTIFY_MCC_COMMAND(INDEX) IS_MCC_COMMAND_IN_SECTION(INDEX, MCC_NOTIFICATIONS_SECTION)
#define IS_CUSTOMIZATION_MCC_COMMAND(INDEX) IS_MCC_COMMAND_IN_RANGE(INDEX, MCC_FIRST + MCC_CUSTOMIZATION_OFFSET, MCC_LAST + MCC_CUSTOMIZATION_OFFSET)

#define MAKE_MCC_PLAYBACK_PARAM(PARAM, ZONE) (((ZONE) == -1) ? ((PARAM) & 0x00FFFFFF) : ((((ZONE) + 1) << 24) & 0xFF000000) | ((PARAM) & 0x00FFFFFF))
#define GET_MCC_PLAYBACK_PARAM(PARAM) (((PARAM) & 0x400000) ? ((PARAM) & 0xFFFFFF) - 0x1000000 : ((PARAM) & 0xFFFFFF))
#define GET_MCC_PLAYBACK_ZONE(PARAM) (((PARAM) & 0x80000000) ? -1 : (((PARAM) >> 24) - 1))

/***********************************************************************************
Defines for internal use
***********************************************************************************/

// update flags
#define MCC_UPDATE_FLAG_THUMBNAILS                           (1 << 0)
#define MCC_UPDATE_FLAG_FILE_PROPERTIES                      (1 << 1)
#define MCC_UPDATE_FLAG_FILE_ADDED_OR_REMOVED                (1 << 2)
#define MCC_UPDATE_FLAG_TREE_STRUCTURE                       (1 << 3)
#define MCC_UPDATE_FLAG_REFILL_LIST                          (1 << 4)
#define MCC_UPDATE_FLAG_ITEM_DELETED                         (1 << 5)
#define MCC_UPDATE_FLAG_NO_PRESERVE_VIEW_STATE               (1 << 6)
#define MCC_UPDATE_FLAG_WEB_VIEW                             (1 << 7)

// update all
#define MCC_UPDATE_FLAG_ALL                                  (0x7FFFFFFF & ~(MCC_UPDATE_FLAG_NO_PRESERVE_VIEW_STATE))

// UI modes
enum UI_MODES
{
    // unknown
    UI_MODE_UNKNOWN = -2000,

    // internal modes
    UI_MODE_INTERNAL_NO_UI = -1000,
    UI_MODE_INTERNAL_STANDARD,
    UI_MODE_INTERNAL_MINI_FREEFORM,
    UI_MODE_INTERNAL_MINI_SLIM,
    UI_MODE_INTERNAL_DISPLAY_WINDOWED,
    UI_MODE_INTERNAL_DISPLAY_FULLSCREEN,
    UI_MODE_INTERNAL_THEATER,

    // toggles, shortcuts, etc.
    UI_MODE_SHORTCUT_TOGGLE_DISPLAY_AND_LAST_USER_INPUT_MODE = -7,
    UI_MODE_SHORTCUT_TOGGLE_DISPLAY_EXCLUDE_THEATER_VIEW = -6,
    UI_MODE_SHORTCUT_TOGGLE_DISPLAY = -5,
    UI_MODE_SHORTCUT_LAST_SHUTDOWN = -4,
    UI_MODE_SHORTCUT_CURRENT = -3,
    UI_MODE_SHORTCUT_CLOSE_DISPLAY = -2,
    UI_MODE_SHORTCUT_NEXT = -1,

    // modes presented to the user
    UI_MODE_STANDARD = 0,
    UI_MODE_MINI,
    UI_MODE_DISPLAY,
    UI_MODE_THEATER,
    UI_MODE_COUNT,
};

// player changes
#define PLAYER_INFO_CHANGE_ALL                               0xFFFF
#define PLAYER_INFO_CHANGE_PLAYERSTATE                       (1 << 0)
#define PLAYER_INFO_CHANGE_VOLUME                            (1 << 1)
#define PLAYER_INFO_CHANGE_FILEINFO                          (1 << 2)
#define PLAYER_INFO_CHANGE_PLAYLISTSTATE                     (1 << 3)
#define PLAYER_INFO_CHANGE_EQCHANGE                          (1 << 4)
#define PLAYER_INFO_CHANGE_IMAGE                             (1 << 5)
#define PLAYER_INFO_CHANGE_PLAYING_FILE                      (1 << 6)

// player status codes
enum PLAYER_STATUS_CODES
{
    PLAYER_STATUS_CODE_BUFFERING,
    PLAYER_STATUS_CODE_LOCATING,
    PLAYER_STATUS_CODE_CONNECTING,
    PLAYER_STATUS_CODE_DOWNLOADING_CODEC,
    PLAYER_STATUS_CODE_ACQUIRING_LICENSE,
    PLAYER_STATUS_CODE_INDIVIDUALIZE_STARTING,
    PLAYER_STATUS_CODE_INDIVIDUALIZE_CONNECTING,
    PLAYER_STATUS_CODE_INDIVIDUALIZE_REQUESTING,
    PLAYER_STATUS_CODE_INDIVIDUALIZE_RECEIVING,
    PLAYER_STATUS_CODE_INDIVIDUALIZE_COMPLETED,
};

// theater view modes
enum SHOW_THEATER_VIEW_MODES
{
    SHOW_THEATER_VIEW_MODE_TOGGLE_THEATER_VIEW,
    SHOW_THEATER_VIEW_MODE_HOME,
    SHOW_THEATER_VIEW_MODE_PLAYING_NOW,
    SHOW_THEATER_VIEW_MODE_AUDIO,
    SHOW_THEATER_VIEW_MODE_IMAGES,
    SHOW_THEATER_VIEW_MODE_VIDEOS,
    SHOW_THEATER_VIEW_MODE_PLAYLISTS,
    SHOW_THEATER_VIEW_MODE_CD_DVD,
    SHOW_THEATER_VIEW_MODE_TV,
};

// get selection modes
enum GET_SELECTION_MODES
{
    GET_SELECTION_EXACT,
    GET_SELECTION_ALL_ON_NONE,
    GET_SELECTION_ALL_ON_ONE_OR_NONE,
    GET_SELECTION_ALL,
};
