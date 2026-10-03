// Test-only substitutes for excluded WinRT, Callisto, MyToolkit, data loading and generated XAML.
// All external operations fail closed. Tests call only the production URL parser.
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Windows.Foundation { } namespace Windows.Foundation.Collections { }
namespace Windows.UI.Xaml.Controls.Primitives { } namespace Windows.UI.Xaml.Data { } namespace Windows.UI.Xaml.Media { } namespace Windows.UI.Xaml.Navigation { }
namespace Windows.UI.Xaml { public class RoutedEventArgs : EventArgs {} public enum Visibility { Visible, Collapsed } }
namespace Windows.UI.Xaml.Input { public class TappedRoutedEventArgs : EventArgs {} }
namespace Windows.UI.Xaml.Controls {
 public class Control { public object Tag; public string Text; public Windows.UI.Xaml.Visibility Visibility; }
 public class Image : Control {} public class Button : Control {} public class ScrollViewer : Control {}
 public class MediaElement : Control { public Uri Source; public TimeSpan Position; public void Play() { throw new Exception("Media not allowed"); } public void Pause() { throw new Exception("Media not allowed"); } public void Stop() { throw new Exception("Media not allowed"); } }
}
namespace Windows.System { public static class Launcher { public static Task<bool> LaunchUriAsync(Uri uri) { throw new Exception("Launching not allowed"); } } }
namespace Windows.UI.ApplicationSettings {
 public class SettingsPane {} public class SettingsCommand { public SettingsCommand(string id,string name,Action<object> action) {} }
 public class SettingsPaneCommandsRequestedEventArgs { public Request Request = new Request(); } public class Request { public List<SettingsCommand> ApplicationCommands = new List<SettingsCommand>(); }
}
namespace Callisto.Controls { public class SettingsFlyout { public object Content; public string HeaderText; public bool IsOpen; } }
namespace MyToolkit.Multimedia { public enum YouTubeQuality { Quality480P, Quality720P } public class Video { public Uri Uri; } public static class YouTube { public static Task<Video> GetVideoUriAsync(string id,YouTubeQuality quality) { throw new Exception("Network not allowed"); } } }
namespace BlackSilverUfa_letsplay.Data {
 public class SampleDataGroup { public string Title; }
 public class SampleDataItem { public string UniqueId; public string Content; public string Description; public string Title; public SampleDataGroup Group; }
 public static class SampleDataSource { public static SampleDataItem GetItem(string id) { throw new Exception("Data loading not allowed"); } }
}
namespace BlackSilverUfa_letsplay.Common { public class LayoutAwarePage { public Dictionary<string,object> DefaultViewModel = new Dictionary<string,object>(); protected virtual void LoadState(object param,Dictionary<string,object> state) {} protected virtual void SaveState(Dictionary<string,object> state) {} } }
namespace BlackSilverUfa_letsplay {
 public class About {}
 public sealed partial class ItemDetailPage {
  void InitializeComponent() {} public string ParseForTest(string url) { return GetYouTubeID(url); }
  public Windows.UI.Xaml.Controls.MediaElement Player = new Windows.UI.Xaml.Controls.MediaElement();
  public Windows.UI.Xaml.Controls.MediaElement PlayerFull = new Windows.UI.Xaml.Controls.MediaElement();
  public Windows.UI.Xaml.Controls.Control Description = new Windows.UI.Xaml.Controls.Control();
  public Windows.UI.Xaml.Controls.Control Title = new Windows.UI.Xaml.Controls.Control();
  public Windows.UI.Xaml.Controls.Control pageTitle = new Windows.UI.Xaml.Controls.Control();
  public Windows.UI.Xaml.Controls.ScrollViewer scrollViewer = new Windows.UI.Xaml.Controls.ScrollViewer();
 }
}
