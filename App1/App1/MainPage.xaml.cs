using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            webView1.NavigationStarting += WebView_NavigationStarting;
        }

        private void WebView_NavigationStarting(object sender, object args)
        {
            var e = (WebViewNavigationStartingEventArgs)args;
            //NavigateWithHeader(e.Uri);
        }

        private void NavigateWithHeader(Uri uri)
        {
            webView1.NavigationStarting -= WebView_NavigationStarting;
            var requestMsg = new HttpRequestMessage(HttpMethod.Get, uri);
            requestMsg.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.125 Safari/537.36 Edg/84.0.522.61");
            webView1.NavigateWithHttpRequestMessage(requestMsg);
            webView1.NavigationStarting += WebView_NavigationStarting;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            webView1.GoBack();
        }

        private void Reload_Click(object sender, RoutedEventArgs e)
        {
            webView1.Refresh();
        }

        private void NPage_Click(object sender, RoutedEventArgs e)
        {
            webView1.Navigate(new Uri("https://teams.microsoft.com/go#"));
        }

        private void Clearcache_Click(object sender, RoutedEventArgs e)
        {
            WebView.ClearTemporaryWebDataAsync();
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            webView1.Navigate(new Uri("https://chat.google.com"));
        }
        private void RqPermission_Click(object sender, RoutedEventArgs e)
        {
            var a = webView1.DeferredPermissionRequests as IList<WebViewDeferredPermissionRequest>;
            webView1.PermissionRequested += (webView1, args) => { };
        }
    }
}
