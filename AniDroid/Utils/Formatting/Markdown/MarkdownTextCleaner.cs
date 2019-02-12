﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using AniDroid.Base;
using Markdig;

namespace AniDroid.Utils.Formatting.Markdown
{
    public static class MarkdownTextCleaner
    {
        private static readonly Regex MalformedHeaderRegex = new Regex(@"^\ {0,3}(\#+)([^\s\#]{1})", RegexOptions.Compiled | RegexOptions.Multiline);

        public static string FormatMarkdownText(string text)
        {
            text = text.Replace("~~~", "");

            text = MalformedHeaderRegex.Replace(text, AddSpaceToMarkup);

            return text;
        }

        public static ISpanned ConvertToSpanned(string text)
        {
            var pipeline = new MarkdownPipelineBuilder().UseEmphasisExtras().UseEmojiAndSmiley().UseListExtras().Build();

            var cleanedText = FormatMarkdownText(text);
            var parsedText = Markdig.Markdown.ToHtml(cleanedText, pipeline);
            return BaseAniDroidActivity.FromHtml(parsedText);
        }

        private static string AddSpaceToMarkup(Match match)
        {
            return $"{match.Groups.ElementAtOrDefault(1)?.Value} {match.Groups.ElementAtOrDefault(2)?.Value}";
        }
    }
}