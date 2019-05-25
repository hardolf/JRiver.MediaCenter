﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MediaCenter.LyricsFinder.Model.LyricServices.ChartLyricsReference {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://api.chartlyrics.com/", ConfigurationName="ChartLyricsReference.apiv1Soap")]
    public interface apiv1Soap {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://api.chartlyrics.com/SearchLyric", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        MediaCenter.LyricsFinder.Model.LyricServices.ChartLyricsReference.SearchLyricResult[] SearchLyric(string artist, string song);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://api.chartlyrics.com/SearchLyric", ReplyAction="*")]
        System.Threading.Tasks.Task<MediaCenter.LyricsFinder.Model.LyricServices.ChartLyricsReference.SearchLyricResult[]> SearchLyricAsync(string artist, string song);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://api.chartlyrics.com/SearchLyricText", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        MediaCenter.LyricsFinder.Model.LyricServices.ChartLyricsReference.SearchLyricResult[] SearchLyricText(string lyricText);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://api.chartlyrics.com/SearchLyricText", ReplyAction="*")]
        System.Threading.Tasks.Task<MediaCenter.LyricsFinder.Model.LyricServices.ChartLyricsReference.SearchLyricResult[]> SearchLyricTextAsync(string lyricText);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://api.chartlyrics.com/GetLyric", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        MediaCenter.LyricsFinder.Model.LyricServices.ChartLyricsReference.GetLyricResult GetLyric(int lyricId, string lyricCheckSum);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://api.chartlyrics.com/GetLyric", ReplyAction="*")]
        System.Threading.Tasks.Task<MediaCenter.LyricsFinder.Model.LyricServices.ChartLyricsReference.GetLyricResult> GetLyricAsync(int lyricId, string lyricCheckSum);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://api.chartlyrics.com/AddLyric", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string AddLyric(int trackId, string trackCheckSum, string lyric, string email);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://api.chartlyrics.com/AddLyric", ReplyAction="*")]
        System.Threading.Tasks.Task<string> AddLyricAsync(int trackId, string trackCheckSum, string lyric, string email);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://api.chartlyrics.com/SearchLyricDirect", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        MediaCenter.LyricsFinder.Model.LyricServices.ChartLyricsReference.GetLyricResult SearchLyricDirect(string artist, string song);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://api.chartlyrics.com/SearchLyricDirect", ReplyAction="*")]
        System.Threading.Tasks.Task<MediaCenter.LyricsFinder.Model.LyricServices.ChartLyricsReference.GetLyricResult> SearchLyricDirectAsync(string artist, string song);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://api.chartlyrics.com/")]
    public partial class SearchLyricResult : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string trackChecksumField;
        
        private int trackIdField;
        
        private string lyricChecksumField;
        
        private int lyricIdField;
        
        private string songUrlField;
        
        private string artistUrlField;
        
        private string artistField;
        
        private string songField;
        
        private int songRankField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string TrackChecksum {
            get {
                return this.trackChecksumField;
            }
            set {
                this.trackChecksumField = value;
                this.RaisePropertyChanged("TrackChecksum");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public int TrackId {
            get {
                return this.trackIdField;
            }
            set {
                this.trackIdField = value;
                this.RaisePropertyChanged("TrackId");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public string LyricChecksum {
            get {
                return this.lyricChecksumField;
            }
            set {
                this.lyricChecksumField = value;
                this.RaisePropertyChanged("LyricChecksum");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public int LyricId {
            get {
                return this.lyricIdField;
            }
            set {
                this.lyricIdField = value;
                this.RaisePropertyChanged("LyricId");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=4)]
        public string SongUrl {
            get {
                return this.songUrlField;
            }
            set {
                this.songUrlField = value;
                this.RaisePropertyChanged("SongUrl");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=5)]
        public string ArtistUrl {
            get {
                return this.artistUrlField;
            }
            set {
                this.artistUrlField = value;
                this.RaisePropertyChanged("ArtistUrl");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=6)]
        public string Artist {
            get {
                return this.artistField;
            }
            set {
                this.artistField = value;
                this.RaisePropertyChanged("Artist");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=7)]
        public string Song {
            get {
                return this.songField;
            }
            set {
                this.songField = value;
                this.RaisePropertyChanged("Song");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=8)]
        public int SongRank {
            get {
                return this.songRankField;
            }
            set {
                this.songRankField = value;
                this.RaisePropertyChanged("SongRank");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.7.2612.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://api.chartlyrics.com/")]
    public partial class GetLyricResult : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string trackChecksumField;
        
        private int trackIdField;
        
        private string lyricChecksumField;
        
        private int lyricIdField;
        
        private string lyricSongField;
        
        private string lyricArtistField;
        
        private string lyricUrlField;
        
        private string lyricCovertArtUrlField;
        
        private int lyricRankField;
        
        private string lyricCorrectUrlField;
        
        private string lyricField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public string TrackChecksum {
            get {
                return this.trackChecksumField;
            }
            set {
                this.trackChecksumField = value;
                this.RaisePropertyChanged("TrackChecksum");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public int TrackId {
            get {
                return this.trackIdField;
            }
            set {
                this.trackIdField = value;
                this.RaisePropertyChanged("TrackId");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public string LyricChecksum {
            get {
                return this.lyricChecksumField;
            }
            set {
                this.lyricChecksumField = value;
                this.RaisePropertyChanged("LyricChecksum");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=3)]
        public int LyricId {
            get {
                return this.lyricIdField;
            }
            set {
                this.lyricIdField = value;
                this.RaisePropertyChanged("LyricId");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=4)]
        public string LyricSong {
            get {
                return this.lyricSongField;
            }
            set {
                this.lyricSongField = value;
                this.RaisePropertyChanged("LyricSong");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=5)]
        public string LyricArtist {
            get {
                return this.lyricArtistField;
            }
            set {
                this.lyricArtistField = value;
                this.RaisePropertyChanged("LyricArtist");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=6)]
        public string LyricUrl {
            get {
                return this.lyricUrlField;
            }
            set {
                this.lyricUrlField = value;
                this.RaisePropertyChanged("LyricUrl");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=7)]
        public string LyricCovertArtUrl {
            get {
                return this.lyricCovertArtUrlField;
            }
            set {
                this.lyricCovertArtUrlField = value;
                this.RaisePropertyChanged("LyricCovertArtUrl");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=8)]
        public int LyricRank {
            get {
                return this.lyricRankField;
            }
            set {
                this.lyricRankField = value;
                this.RaisePropertyChanged("LyricRank");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=9)]
        public string LyricCorrectUrl {
            get {
                return this.lyricCorrectUrlField;
            }
            set {
                this.lyricCorrectUrlField = value;
                this.RaisePropertyChanged("LyricCorrectUrl");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=10)]
        public string Lyric {
            get {
                return this.lyricField;
            }
            set {
                this.lyricField = value;
                this.RaisePropertyChanged("Lyric");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface apiv1SoapChannel : MediaCenter.LyricsFinder.Model.LyricServices.ChartLyricsReference.apiv1Soap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class apiv1SoapClient : System.ServiceModel.ClientBase<MediaCenter.LyricsFinder.Model.LyricServices.ChartLyricsReference.apiv1Soap>, MediaCenter.LyricsFinder.Model.LyricServices.ChartLyricsReference.apiv1Soap {
        
        public apiv1SoapClient() {
        }
        
        public apiv1SoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public apiv1SoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public apiv1SoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public apiv1SoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public MediaCenter.LyricsFinder.Model.LyricServices.ChartLyricsReference.SearchLyricResult[] SearchLyric(string artist, string song) {
            return base.Channel.SearchLyric(artist, song);
        }
        
        public System.Threading.Tasks.Task<MediaCenter.LyricsFinder.Model.LyricServices.ChartLyricsReference.SearchLyricResult[]> SearchLyricAsync(string artist, string song) {
            return base.Channel.SearchLyricAsync(artist, song);
        }
        
        public MediaCenter.LyricsFinder.Model.LyricServices.ChartLyricsReference.SearchLyricResult[] SearchLyricText(string lyricText) {
            return base.Channel.SearchLyricText(lyricText);
        }
        
        public System.Threading.Tasks.Task<MediaCenter.LyricsFinder.Model.LyricServices.ChartLyricsReference.SearchLyricResult[]> SearchLyricTextAsync(string lyricText) {
            return base.Channel.SearchLyricTextAsync(lyricText);
        }
        
        public MediaCenter.LyricsFinder.Model.LyricServices.ChartLyricsReference.GetLyricResult GetLyric(int lyricId, string lyricCheckSum) {
            return base.Channel.GetLyric(lyricId, lyricCheckSum);
        }
        
        public System.Threading.Tasks.Task<MediaCenter.LyricsFinder.Model.LyricServices.ChartLyricsReference.GetLyricResult> GetLyricAsync(int lyricId, string lyricCheckSum) {
            return base.Channel.GetLyricAsync(lyricId, lyricCheckSum);
        }
        
        public string AddLyric(int trackId, string trackCheckSum, string lyric, string email) {
            return base.Channel.AddLyric(trackId, trackCheckSum, lyric, email);
        }
        
        public System.Threading.Tasks.Task<string> AddLyricAsync(int trackId, string trackCheckSum, string lyric, string email) {
            return base.Channel.AddLyricAsync(trackId, trackCheckSum, lyric, email);
        }
        
        public MediaCenter.LyricsFinder.Model.LyricServices.ChartLyricsReference.GetLyricResult SearchLyricDirect(string artist, string song) {
            return base.Channel.SearchLyricDirect(artist, song);
        }
        
        public System.Threading.Tasks.Task<MediaCenter.LyricsFinder.Model.LyricServices.ChartLyricsReference.GetLyricResult> SearchLyricDirectAsync(string artist, string song) {
            return base.Channel.SearchLyricDirectAsync(artist, song);
        }
    }
}
