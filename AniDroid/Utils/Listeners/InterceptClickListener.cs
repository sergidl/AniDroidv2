using System;
using Android.Views;

namespace AniDroidv2.Utils.Listeners
{
    public class InterceptClickListener : Java.Lang.Object, View.IOnClickListener
    {
        public Action ActionToInvoke { get; }

        public InterceptClickListener(Action action)
        {
            ActionToInvoke = action;
        }

        public void OnClick(View v)
        {
            ActionToInvoke.Invoke();
        }
    }
}