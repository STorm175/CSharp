﻿using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Capture;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WebViewRuntimeComponent
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlankPage1 : Page
    {
        public BlankPage1()
        {
            this.InitializeComponent();
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
            WebView.Reload();
        }
    }
}