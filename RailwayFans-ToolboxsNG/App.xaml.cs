using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.VoiceCommands;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace RailwayFans_ToolboxsNG
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        public Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();
                await Fileupdate();
                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(routePage), e.Arguments);
                }
                // Ensure the current window is active
                Window.Current.Activate();
            }
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        private async Task Fileupdate()
        {
            var topUserLanguage = Windows.System.UserProfile.GlobalizationPreferences.Languages[0];
            var language = new Windows.Globalization.Language(topUserLanguage);
            var displayName = language.Script;
            String currentLang = "";
            if (localSettings.Values["language"] == null) 
            {
                if (localSettings.Values["first"] == null) //第一次启动，语言值为空
                {
                    switch (displayName) //选择App语言
                    {
                        case "Hans":
                            currentLang = "Hans";
                            break;
                        case "Hant":
                            currentLang = "Hant";
                            break;
                        case "Jpan":
                            currentLang = "Jpan";
                            break;
                        case "Kore":
                            currentLang = "Kore";
                            break;
                        default:
                            currentLang = "EN";
                            break;
                    }
                    localSettings.Values["language"] = currentLang; //设置语言值为当前系统语言
                    StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(@"ms-appx:///EMU" + currentLang + ".db")); //选择当前语言数据库
                    StorageFolder folder = Windows.Storage.ApplicationData.Current.LocalFolder;
                    await file.CopyAsync(folder);
                    StorageFile current = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync("EMU" + currentLang + ".db"); //复制数据库到LocalState目录
                    await current.RenameAsync("EMU.db");
                    localSettings.Values["first"] = "2016-07-31"; //写入当前数据库版本值
                }
            }
            else if (localSettings.Values["language"].ToString() != displayName) //用户切换系统语言
            {
                switch (displayName) //选择App语言
                {
                    case "Hans":
                        currentLang = "Hans";
                        break;
                    case "Hant":
                        currentLang = "Hant";
                        break;
                    case "Jpan":
                        currentLang = "Jpan";
                        break;
                    case "Kore":
                        currentLang = "Kore";
                        break;
                    default:
                        currentLang = "EN";
                        break;
                }
                StorageFile current = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync("EMU.db");
                await current.DeleteAsync(); //删除旧语言版本数据库 
                StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(@"ms-appx:///EMU" + currentLang + ".db"));//选择当前语言数据库
                StorageFolder folder = Windows.Storage.ApplicationData.Current.LocalFolder;
                await file.CopyAsync(folder);
                current = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync("EMU" + currentLang + ".db"); //复制数据库到LocalState目录
                await current.RenameAsync("EMU.db");
                localSettings.Values["first"] = "2016-07-31";//写入当前数据库版本值
                localSettings.Values["language"] = currentLang; //设置语言值为当前系统语言
            }
        }
    }
}
