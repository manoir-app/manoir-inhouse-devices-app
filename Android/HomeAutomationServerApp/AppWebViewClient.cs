using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HomeAutomationServerApp
{
    internal class AppWebViewClient : WebViewClient
    {
        public AppWebViewClient()
        {

        }
        protected AppWebViewClient(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public override bool ShouldOverrideUrlLoading(WebView view, IWebResourceRequest request)
        {
            //view.LoadUrl(request.Url.ToString());
            return false;
        }
    }
}