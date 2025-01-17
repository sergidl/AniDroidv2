﻿using System;
using System.Linq;
using Android.Content;
using Android.Widget;
using AndroidX.AppCompat.App;
using AniDroidv2.Base;
using AniDroidv2.SearchResults;

namespace AniDroidv2.Dialogs
{
    public class SearchDialog
    {
        public static void Create(BaseAniDroidv2Activity context, Action<string, string> searchAction, string searchType = "", string searchTerm = null)
        {
            var dialogue = context.LayoutInflater.Inflate(Resource.Layout.Dialog_Search, null);
            var searchTypeView = dialogue.FindViewById<Spinner>(Resource.Id.Search_Type);
            var searchTermView = dialogue.FindViewById<EditText>(Resource.Id.Search_Text);
            searchTypeView.Adapter = new ArrayAdapter<string>(context, Resource.Layout.View_SpinnerDropDownItem, SearchResultsActivity.AniListSearchTypes.AllTypes);

            searchTermView.Text = searchTerm;

            if (SearchResultsActivity.AniListSearchTypes.AllTypes.Contains(searchType))
                searchTypeView.SetSelection(Array.FindIndex(SearchResultsActivity.AniListSearchTypes.AllTypes, x => x == searchType));

            var a = new AlertDialog.Builder(context, context.GetThemedResourceId(Resource.Attribute.Dialog_Theme)).Create();
            a.SetView(dialogue);
            a.SetTitle("Search AniList");
            a.SetButton((int)DialogButtonType.Neutral, "Cancel", (aS, eV) => a.Dismiss());
            a.SetButton((int)DialogButtonType.Positive, "Search", (aS, ev) => searchAction((string)searchTypeView.Adapter.GetItem(searchTypeView.SelectedItemPosition), searchTermView.Text));

            searchTermView.EditorAction += (tS, tE) =>
            {
                if (tE.ActionId == Android.Views.InputMethods.ImeAction.Search)
                {
                    searchAction((string) searchTypeView.Adapter.GetItem(searchTypeView.SelectedItemPosition),
                        searchTermView.Text);
                }
            };

            a.Show();
        }
    }
}