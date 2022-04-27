using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Media.Capture;
using Windows.ApplicationModel.Activation;
using System.Diagnostics;
using Windows.UI.ViewManagement;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AppWebView2
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Windows.System.ProtocolForResultsOperation _operation = null;
        private ProtocolForResultsActivatedEventArgs _data = null;
        public Uri Uri { get; set; }
        public MainPage()
        {
            this.InitializeComponent();
            
            DataContext = this;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var protocolForResultsArgs = e.Parameter as ProtocolForResultsActivatedEventArgs;
            _data = protocolForResultsArgs;
            // Set the ProtocolForResultsOperation field.
            if (protocolForResultsArgs == null)
            {
                Debug.WriteLine("protocolForResultsArgs == null");
                return;
            }
            else
            {
                _operation = protocolForResultsArgs.ProtocolForResultsOperation;
                Debug.WriteLine("protocolForResultsArgs= " + protocolForResultsArgs.Data);
                if (protocolForResultsArgs.Data.ContainsKey("TestData"))
                {
                    string dataFromCaller = protocolForResultsArgs.Data["TestData"] as string;
                    Debug.WriteLine("protocolForResultsArgs= " + dataFromCaller);
                    ApplicationView.PreferredLaunchViewSize = new Size(800, 800);
                    ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
                    WebView.Source = new Uri(dataFromCaller);
                    ValueSet result = new ValueSet();
                    result["ReturnedData"] = "The returned result";
                    _operation.ReportCompleted(result);
                }
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private async void CameraTest_Click(object sender, RoutedEventArgs e)
        {

            MediaCapture mediaCapture = new MediaCapture();
            var settings = new MediaCaptureInitializationSettings();
            settings.StreamingCaptureMode = StreamingCaptureMode.Video;
            await mediaCapture.InitializeAsync(settings);

            await WebView.ExecuteScriptAsync("navigator.mediaDevices.getUserMedia({video: true})");
        }

        private async void MicrophoneTest_Click(object sender, RoutedEventArgs e)
        {
            MediaCapture mediaCapture = new MediaCapture();
            var settings = new MediaCaptureInitializationSettings();
            settings.StreamingCaptureMode = StreamingCaptureMode.Audio;
            await mediaCapture.InitializeAsync(settings);

            await WebView.ExecuteScriptAsync("navigator.mediaDevices.getUserMedia({audio: true})");
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            WebView.GoBack();
        }

        private void Forward_Click(object sender, RoutedEventArgs e)
        {
            WebView.GoForward();
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            //WebView.Reload();
            //WebView.NavigateToString("https://www.google.com.vn/");
            WebView.Source = new Uri("https://www.google.com.vn/");
        }
    }
}
