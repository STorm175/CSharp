using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Services.Store;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Web.Http;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App_test
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private static readonly string JS_CODE_ATAG_REMOVE_TARGET = @"
                var aElems = document.getElementsByTagName('a');
                if (aElems && aElems.length > 0) {
                    var len = aElems.length;
                    for (var i = 0; i < len; i++) {
                        var targetVal = aElems[i].getAttribute('target');
                        if (targetVal == '_blank' || targetVal == '_new') {
                            aElems[i].removeAttribute('target');
                            //aElems[i].innerHTML = '***' + aElems[i].innerHTML + '***'; // 置換個所確認用コード
                        }
                    }
                }";
        public MainPage()
        {
            this.InitializeComponent();
            webView1.NavigationStarting += WebView_NavigationStarting;
            webView1.WebResourceRequested += WebView1_WebResourceRequested;
            webView1.NavigationCompleted += WebView1_NavigationCompleted;
            webView1.PermissionRequested += WebView1_PermissionRequested;
            webView1.NewWindowRequested += WebView1_NewWindowRequested;

            webView1.LongRunningScriptDetected += WebView1_LongRunningScriptDetected;
            webView1.ContentLoading += webView1_ContentLoading;
            webView1.DOMContentLoaded += WebView1_DOMContentLoaded;
            webView1.ScriptNotify += WebView1_ScriptNotify;
            webView1.UnviewableContentIdentified += WebView1_UnviewableContentIdentified;
            //var a = webView1.Settings;
            //var b = webView1.AllowedScriptNotifyUris;
            //WebView.ClearTemporaryWebDataAsync();
            //checkRequest();
        }

        private void WebView1_NewWindowRequested(WebView sender, WebViewNewWindowRequestedEventArgs args)
        {
            throw new NotImplementedException();
        }

        private void WebView1_UnviewableContentIdentified(WebView sender, WebViewUnviewableContentIdentifiedEventArgs args)
        {
            //throw new NotImplementedException();
        }

        private async void WebView1_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            //await webView1.InvokeScriptAsync("eval", new string[] { "window.confirm = function(confirmMessage) { window.external.notify('typeConfirm:' + confirmMessage) }" });
            //await webView1.InvokeScriptAsync("eval", new string[] { "window.alert = function(AlertMessage) { window.external.notify('typeAlert:' + AlertMessage) }" });
            //string result = await webView1.InvokeScriptAsync("eval", new string[] { "window.confirm = function (ConfirmMessage) {window.external.notify(ConfirmMessage)}" });
        }

        private void WebView1_ScriptNotify(object sender, NotifyEventArgs e)
        {
            string msg = e.Value;
        }

        private async void WebView1_DOMContentLoaded(WebView sender, WebViewDOMContentLoadedEventArgs args)
        {
        }

        private void webView1_ContentLoading(WebView sender, WebViewContentLoadingEventArgs args)
        {
            if (args.Uri != null)
            {
                //statusTextBlock.Text = "Loading content for " + args.Uri.ToString();
            }
        }

        private async void WebView1_LongRunningScriptDetected(WebView sender, WebViewLongRunningScriptDetectedEventArgs args)
        {
            string functionString = "window.onerror = function(error, url, line) {window.external.notify( 'ERR:'+error+' url'+url+' Line: '+line);};";
            var ret = await webView1.InvokeScriptAsync("eval", new string[] { functionString });
        }

        private void WebView1_WebResourceRequested(WebView sender, WebViewWebResourceRequestedEventArgs args)
        {
        }

        private void WebView1_PermissionRequested(WebView sender, WebViewPermissionRequestedEventArgs args)
        {
            args.PermissionRequest.Allow();
            if (args.PermissionRequest.PermissionType == WebViewPermissionType.Geolocation ||
                args.PermissionRequest.PermissionType == WebViewPermissionType.Media)
            {
                args.PermissionRequest.Allow();
            }
        }

        private async void checkRequest()
        {
            bool result = await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings:privacy-microphone"));

            bool result1 = await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings:privacy-webcam"));
        }

        private void WebView_NavigationStarting(object sender, object args)
        {
            var e = (WebViewNavigationStartingEventArgs)args;
            NavigateWithHeader(e.Uri);
            //NavigateWithHeaderPost(e.Uri);
            // WebView native object must be inserted in the OnNavigationStarting event handler
            //KeyHandler winRTObject = new KeyHandler();
            // Expose the native WinRT object on the page's global object
            //webView1.AddWebAllowedObject("NotifyApp", winRTObject);
        }

        private void NavigateWithHeader(Uri uri)
        {
            webView1.NavigationStarting -= WebView_NavigationStarting;
            var requestMsg = new HttpRequestMessage(HttpMethod.Get, uri);
            requestMsg.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.125 Safari/537.36 Edg/84.0.522.61");
            webView1.NavigateWithHttpRequestMessage(requestMsg);
            webView1.NavigationStarting += WebView_NavigationStarting;
        }

        private void NavigateWithHeaderPost(Uri uri)
        {
            webView1.NavigationStarting -= WebView_NavigationStarting;
            var requestMsg = new HttpRequestMessage(HttpMethod.Post, uri);
            requestMsg.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.125 Safari/537.36 Edg/84.0.522.61");
            webView1.NavigateWithHttpRequestMessage(requestMsg);
            webView1.NavigationStarting += WebView_NavigationStarting;
        }

        private StoreContext context = null;

        public async void GetAppInfo()
        {
            if (context == null)
            {
                context = StoreContext.GetDefault();
                // If your app is a desktop app that uses the Desktop Bridge, you
                // may need additional code to configure the StoreContext object.
                // For more info, see https://aka.ms/storecontext-for-desktop.
            }

            // Get app store product details. Because this might take several moments,   
            // display a ProgressRing during the operation.
            //workingProgressRing.IsActive = true;
            StoreProductResult queryResult = await context.GetStoreProductForCurrentAppAsync();
            var queryResult1 = await context.GetAppAndOptionalStorePackageUpdatesAsync();
            //workingProgressRing.IsActive = false;

            if (queryResult.Product == null)
            {
                // The Store catalog returned an unexpected result.
                //textBlock.Text = "Something went wrong, and the product was not returned.";
                var str = "";
                //// Show additional error info if it is available.
                if (queryResult.ExtendedError != null)
                {
                    str += $"\nExtendedError: {queryResult.ExtendedError.Message}";
                }

                //return;
            }

            // Display the price of the app.
            var test = $"The price of this app is: {queryResult.Product.Price.FormattedBasePrice}";
        }

        public async void GetProductInfo()
        {
            if (context == null)
            {
                context = StoreContext.GetDefault();
                // If your app is a desktop app that uses the Desktop Bridge, you
                // may need additional code to configure the StoreContext object.
                // For more info, see https://aka.ms/storecontext-for-desktop.
            }

            // Specify the kinds of add-ons to retrieve.
            string[] productKinds = { "Durable" };
            List<String> filterList = new List<string>(productKinds);

            // Specify the Store IDs of the products to retrieve.
            string[] storeIds = new string[] { "9NBLGGH4TNMP", "9NBLGGH4TNMN" };

            StoreProductQueryResult queryResult =
                await context.GetStoreProductsAsync(filterList, storeIds);

            if (queryResult.ExtendedError != null)
            {
                // The user may be offline or there might be some other server failure.
                var str = $"ExtendedError: {queryResult.ExtendedError.Message}";
                return;
            }

            foreach (KeyValuePair<string, StoreProduct> item in queryResult.Products)
            {
                // Access the Store info for the product.
                StoreProduct product = item.Value;

                // Use members of the product object to access info for the product...
            }
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
            //webView1.Navigate(new Uri("https://www.youtube.com/"));
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

        private async void WebViewWithJSInjection_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            string result = await webView1.InvokeScriptAsync("eval", new string[] { "window.alert = function (AlertMessage) {window.external.notify(AlertMessage)}" });
        }

        private async void WebViewWithJSInjection_ScriptNotify(object sender, NotifyEventArgs e)
        {
            Windows.UI.Popups.MessageDialog dialog = new Windows.UI.Popups.MessageDialog(e.Value);
            await dialog.ShowAsync();
        }
    }

    public sealed class KeyHandler
    {
        public void setKeyCombination(int keyPress)
        {
            Debug.WriteLine("Called from WebView! {0}", keyPress);
        }
    }
}
