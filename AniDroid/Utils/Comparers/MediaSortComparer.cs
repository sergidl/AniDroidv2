﻿using System;
using AniDroidv2.AniList.Models.MediaModels;

namespace AniDroidv2.Utils.Comparers
{
    public class MediaSortComparer : BaseAniDroidv2Comparer<Media>
    {
        private readonly MediaSortType _sort;

        public MediaSortComparer(MediaSortType sort, SortDirection direction) : base(direction)
        {
            _sort = sort;
        }

        public override int CompareInternal(Media x, Media y)
        {
            switch (_sort)
            {
                case MediaSortType.NoSort:
                    return 0;
                case MediaSortType.Title:
                    return SortString(x, y, m => m.Title.UserPreferred);
                case MediaSortType.Popularity:
                    return SortNumber(x, y, m => m.Popularity);
                case MediaSortType.AverageScore:
                    return SortNumber(x, y, m => m.AverageScore);
                case MediaSortType.DateReleased:
                    return SortDate(x, y, m => m.StartDate?.GetFuzzyDate() ?? DateTime.MinValue);
                case MediaSortType.Duration:
                    return SortNumber(x, y, m => m.Duration);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public enum MediaSortType
        {
            NoSort = 0,
            Title = 1,
            AverageScore = 4,
            Popularity = 5,
            DateReleased = 6,
            Duration = 9
        }
    }
}