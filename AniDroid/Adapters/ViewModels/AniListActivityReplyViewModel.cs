﻿using System.Linq;
using Android.Content.Res;
using Android.Graphics;
using Android.Text;
using AniDroidv2.AniList.Models.ActivityModels;
using AniDroidv2.Base;

namespace AniDroidv2.Adapters.ViewModels
{
    public class AniListActivityReplyViewModel : AniDroidv2AdapterViewModel<ActivityReply>
    {
        public ISpanned DetailFormatted { get; protected set; }
        public string TimestampText { get; protected set; }
        public string LikeCount { get; protected set; }
        public ColorStateList LikeIconColor { get; protected set; }

        private readonly int? _userId;
        private readonly Color _defaultIconColor;

        public AniListActivityReplyViewModel(ActivityReply model, Color defaultIconColor, int? userId) : base(model)
        {
            _userId = userId;
            _defaultIconColor = defaultIconColor;

            SetupViewModel();
        }

        public static AniListActivityReplyViewModel CreateViewModel(ActivityReply model, Color defaultIconColor, int? userId)
        {
            return new AniListActivityReplyViewModel(model, defaultIconColor, userId);
        }

        private void SetupViewModel()
        {
            TitleText = Model.User?.Name;
            DetailFormatted = BaseAniDroidv2Activity.FromHtml(Model.Text);
            TimestampText = Model.GetAgeString(Model.CreatedAt);
            LikeCount = (Model.Likes?.Count ?? 0).ToString();
            ImageUri = Model.User.Avatar.Large ?? Model.User.Avatar.Medium;

            if (_userId.HasValue)
            {
                LikeIconColor = ColorStateList.ValueOf(
                    Model.Likes?.Any(x => x.Id == _userId) == true
                        ? Color.Crimson
                        : _defaultIconColor);
            }
        }

        public override void RecreateViewModel()
        {
            SetupViewModel();
        }
    }
}