﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Aurora.Shared.Extensions;
using Aurora.Shared.MVVM;
using Windows.UI.Xaml.Media.Imaging;
using Aurora.Music.Core.Storage;

namespace Aurora.Music.ViewModels
{
    class AlbumViewModel : ViewModelBase
    {
        public AlbumViewModel(Core.Models.Album item)
        {
            SongsID = item.Songs;
            Name = item.Name;
            if (!item.PicturePath.IsNullorEmpty())
            {
                artworkUri = new Uri(item.PicturePath);
            }
            Genres = item.Genres;
            Year = item.Year;
            AlbumSort = item.AlbumSort;
            TrackCount = item.TrackCount;
            DiscCount = item.DiscCount;
            AlbumArtists = item.AlbumArtists;
            AlbumArtistsSort = item.AlbumArtistsSort;
            ReplayGainAlbumGain = item.ReplayGainAlbumGain;
            ReplayGainAlbumPeak = item.ReplayGainAlbumPeak;
        }

        public AlbumViewModel() { }

        public int[] SongsID { get; private set; }

        public List<SONG> Songs { get; set; } = new List<SONG>();

        private Uri artworkUri;
        public Uri Artwork
        {
            get
            {
                return artworkUri;
            }
            set
            {
                SetProperty(ref artworkUri, value);
            }
        }

        private string title;
        public string Name
        {
            get { return title; }
            set
            {
                SetProperty(ref title, value);
            }
        }

        public virtual string[] Genres { get; set; }
        public virtual uint Year { get; set; }
        public virtual string AlbumSort { get; set; }
        public virtual uint TrackCount { get; set; }
        public virtual uint DiscCount { get; set; }
        public virtual string[] AlbumArtists { get; set; }
        public virtual string[] AlbumArtistsSort { get; set; }
        public virtual double ReplayGainAlbumGain { get; set; }
        public virtual double ReplayGainAlbumPeak { get; set; }

        internal async Task<IEnumerable<SONG>> GetSongsAsync()
        {
            if (Songs.Count == SongsID.Length)
            {
                return Songs;
            }
            Songs.Clear();
            var opr = SQLOperator.Current();
            var s = await opr.GetSongsAsync(SongsID);
            var s1 = s.OrderBy(x => x.Track);
            s1 = s1.OrderBy(x => x.Disc);
            Songs.AddRange(s1);
            return s1;
        }
    }
}
