using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel;
using System.Reflection;
using System.Diagnostics;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AppMain
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private AppServiceConnection inventoryService;

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            // Add the connection.
            if (this.inventoryService == null)
            {
                this.inventoryService = new AppServiceConnection();

                // Here, we use the app service name defined in the app service 
                // provider's Package.appxmanifest file in the <Extension> section.
                this.inventoryService.AppServiceName = "com.microsoft.inventory1";

                // Use Windows.ApplicationModel.Package.Current.Id.FamilyName 
                // within the app service provider to get this value.
                //this.inventoryService.PackageFamilyName = "c4bc62bb-a471-4a91-aa67-658910528f24_bjabc325vzr3p";
                this.inventoryService.PackageFamilyName = "2b4aab33-1619-4565-8bcb-b61fdc1f27ce_bjabc325vzr3p";
                //this.inventoryService.PackageFamilyName = "e9cfc5b8-3c2e-4c50-b078-0655e3c6ce23_bjabc325vzr3p";

                var status = await this.inventoryService.OpenAsync();

                if (status != AppServiceConnectionStatus.Success)
                {
                    textBox.Text = "Failed to connect";
                    this.inventoryService = null;
                    return;
                }
            }

            // Call the service.
            int idx = int.Parse(textBox.Text);
            var message = new ValueSet();
            message.Add("Command", "Item");
            message.Add("ID", idx);
            AppServiceResponse response = await this.inventoryService.SendMessageAsync(message);
            string result = "";

            if (response.Status == AppServiceResponseStatus.Success)
            {
                // Get the data  that the service sent to us.
                if (response.Message["Status"] as string == "OK")
                {
                    result = response.Message["Result"] as string;
                }
            }

            message.Clear();
            message.Add("Command", "Price");
            message.Add("ID", idx);
            response = await this.inventoryService.SendMessageAsync(message);

            if (response.Status == AppServiceResponseStatus.Success)
            {
                // Get the data that the service sent to us.
                if (response.Message["Status"] as string == "OK")
                {
                    result += " : Price = " + response.Message["Result"] as string;
                }
            }

            textBox.Text = result;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await LaunchAppForResults();
        }

        async Task<string> LaunchAppForResults()
        {
            var testAppUri = new Uri("appwebview2:?2000:2000"); // The protocol handled by the launched app
            var options = new LauncherOptions();
            options.TargetApplicationPackageFamilyName = "c4bc62bb-a471-4a91-aa67-658910528f24_bjabc325vzr3p";
            options.DesiredRemainingView = Windows.UI.ViewManagement.ViewSizePreference.UseMore;

            var inputData = new ValueSet();
            inputData["TestData"] = "https://meet.google.com/";

            string theResult = "";
            LaunchUriResult result = await Launcher.LaunchUriForResultsAsync(testAppUri, options, inputData);
            if (result.Status == LaunchUriStatus.Success &&
                result.Result != null &&
                result.Result.ContainsKey("ReturnedData"))
            {
                ValueSet theValues = result.Result;
                theResult = theValues["ReturnedData"] as string;
            }
            return theResult;
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (ApiInformation.IsApiContractPresent("Windows.ApplicationModel.FullTrustAppContract", 1, 0))
            {
                await FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppAsync();
            }
        }

        public Func<Task<bool>> GetLauchFullProcessTask()
        {
            return async () =>
            {
                try
                {
                    //アプリ起動用ブロック
                    // Make sure the BackgroundProcess is in your AppX folder, if not rebuild the solution
                    //await Task.Run(() => {
                    if (ApiInformation.IsApiContractPresent("Windows.ApplicationModel.FullTrustAppContract", 1, 0))
                    {
                        await FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppAsync();
                    }
                    FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppAsync();

                    return true;
                }
                catch (Exception ee)
                {
                    System.Diagnostics.Debug.WriteLine("LaunchFullTrustProcessForCurrentAppAsync Failed");
                    System.Diagnostics.Debug.WriteLine(ee.Message);
                    System.Diagnostics.Debug.WriteLine(ee.StackTrace);
                    return false;
                }
            };
        }
    }
}
