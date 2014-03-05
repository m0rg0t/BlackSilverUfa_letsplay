using BlackSilverUfa_letsplay.Data;
using Callisto.Controls;
using MyToolkit.Multimedia;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Шаблон элемента страницы сведений об элементе задокументирован по адресу http://go.microsoft.com/fwlink/?LinkId=234232

namespace BlackSilverUfa_letsplay
{
    /// <summary>
    /// Страница, на которой отображаются сведения об отдельном элементе внутри группы; при этом можно с помощью жестов
    /// перемещаться между другими элементами из этой группы.
    /// </summary>
    public sealed partial class ItemDetailPage : BlackSilverUfa_letsplay.Common.LayoutAwarePage
    {
        public ItemDetailPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Заполняет страницу содержимым, передаваемым в процессе навигации. Также предоставляется любое сохраненное состояние
        /// при повторном создании страницы из предыдущего сеанса.
        /// </summary>
        /// <param name="navigationParameter">Значение параметра, передаваемое
        /// <see cref="Frame.Navigate(Type, Object)"/> при первоначальном запросе этой страницы.
        /// </param>
        /// <param name="pageState">Словарь состояния, сохраненного данной страницей в ходе предыдущего
        /// сеанса. Это значение будет равно NULL при первом посещении страницы.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            try
            {
                // Разрешение сохраненному состоянию страницы переопределять первоначально отображаемый элемент
                if (pageState != null && pageState.ContainsKey("SelectedItem"))
                {
                    navigationParameter = pageState["SelectedItem"];
                }

                // TODO: Создание соответствующей модели данных для области проблемы, чтобы заменить пример данных
                item = SampleDataSource.GetItem((String)navigationParameter);
                //this.DefaultViewModel["Group"] = item.Group;
                //this.DefaultViewModel["Items"] = item.Group.Items;
                this.DefaultViewModel["Item"] = item;
                try
                {
                    this.Player.Tag = item.Content.ToString();
                }
                catch { };
                this.Description.Text = item.Description;
                this.Title.Text = item.Title;
                this.pageTitle.Text = item.Group.Title;
                //this.Image.Source = item.Image;
                //this.flipView.SelectedItem = item;
            }
            catch { };
        }


        void Settings_CommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            try
            {
                var viewAboutPage = new SettingsCommand("", "About", cmd =>
                {
                    //(Window.Current.Content as Frame).Navigate(typeof(AboutPage));
                    var settingsFlyout = new SettingsFlyout();
                    settingsFlyout.Content = new About();
                    settingsFlyout.HeaderText = "About";

                    settingsFlyout.IsOpen = true;
                });
                args.Request.ApplicationCommands.Add(viewAboutPage);

                /*var viewAboutMalukahPage = new SettingsCommand("", "About Malukah", cmd =>
                {
                    var settingsFlyout = new SettingsFlyout();
                    settingsFlyout.Content = new AboutMalukah();
                    settingsFlyout.HeaderText = "About Malukah";

                    settingsFlyout.IsOpen = true;
                });
                args.Request.ApplicationCommands.Add(viewAboutMalukahPage);*/
            }
            catch { };
        }

        private SampleDataItem item;

        /// <summary>
        /// Сохраняет состояние, связанное с данной страницей, в случае приостановки приложения или
        /// удаления страницы из кэша навигации. Значения должны соответствовать требованиям сериализации
        /// <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">Пустой словарь, заполняемый сериализуемым состоянием.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
            //var selectedItem = (SampleDataItem)this.flipView.SelectedItem;
            pageState["SelectedItem"] = item.UniqueId;
        }

        private async void image_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                await Launcher.LaunchUriAsync(new Uri(((Image)sender).Tag.ToString()));
            }
            catch { };
        }

        private string GetYouTubeID(string youTubeUrl)
        {
            //Setup the RegEx Match and give it 
            Match regexMatch = Regex.Match(youTubeUrl, "^[^v]+v=(.{11}).*",
                               RegexOptions.IgnoreCase);
            if (regexMatch.Success)
            {
                return regexMatch.Groups[1].Value;
            }
            return youTubeUrl;
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var url = await YouTube.GetVideoUriAsync(GetYouTubeID(((Button)sender).Tag.ToString()), YouTubeQuality.Quality480P);
            //var player = new MediaElement();
            //..Source = url.Uri;
            //Player.Play();
        }

        private async void Player_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //var url = await YouTube.GetVideoUriAsync(GetYouTubeID(((MediaElement)sender).Tag.ToString()), YouTubeQuality.Quality480P);
                var url = await YouTube.GetVideoUriAsync(GetYouTubeID((item as SampleDataItem).Content.ToString()), YouTubeQuality.Quality480P);
                var url2 = await YouTube.GetVideoUriAsync(GetYouTubeID((item as SampleDataItem).Content.ToString()), YouTubeQuality.Quality720P);
                //var player = new MediaElement();

                this.PlayerFull.Source = url2.Uri;
                this.PlayerFull.Stop();

                ((MediaElement)sender).Source = url.Uri;
                ((MediaElement)sender).Play();
            }
            catch { };
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            if (FullscreenOff)
            {
                this.Player.Play();
            }
            else
            {
                this.PlayerFull.Play();
            };
        }

        bool FullscreenOff = true;

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            if (FullscreenOff)
            {
                this.Player.Pause();
            }
            else
            {
                this.PlayerFull.Pause();
            };
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            if (FullscreenOff)
            {
                this.Player.Stop();
            }
            else {
                this.PlayerFull.Stop();
            };
        }

        private void Fullscreen_Click(object sender, RoutedEventArgs e)
        {
            if (FullscreenOff)
            {
                this.Player.Pause();
                this.PlayerFull.Position = this.Player.Position;

                this.Player.Visibility = Visibility.Collapsed;
                this.PlayerFull.Visibility = Visibility.Visible;
                this.scrollViewer.Visibility = Visibility.Collapsed;

                this.PlayerFull.Play();

                this.pageTitle.Visibility = Visibility.Collapsed;
                FullscreenOff = false;
            }
            else
            {
                this.PlayerFull.Pause();
                this.Player.Position = this.PlayerFull.Position;

                this.Player.Visibility = Visibility.Visible;
                this.PlayerFull.Visibility = Visibility.Collapsed;
                this.scrollViewer.Visibility = Visibility.Visible;

                this.Player.Play();

                this.pageTitle.Visibility = Visibility.Visible;
                FullscreenOff = true;
            };
        }
    }
}
