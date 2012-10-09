using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.Resources.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using System.Collections.Specialized;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using System.Xml.Linq;

// Модель данных, определяемая этим файлом, служит типичным примером строго типизированной
// модели, которая поддерживает уведомление при добавлении, удалении или изменении членов. Выбранные
// имена свойств совпадают с привязками данных из стандартных шаблонов элементов.
//
// Приложения могут использовать эту модель в качестве начальной точки и добавлять к ней дополнительные элементы или полностью удалить и
// заменить ее другой моделью, соответствующей их потребностям.

namespace BlackSilverUfa_letsplay.Data
{
    /// <summary>
    /// Базовый класс объектов <see cref="SampleDataItem"/> и <see cref="SampleDataGroup"/>, который
    /// определяет свойства, общие для обоих объектов.
    /// </summary>
    [Windows.Foundation.Metadata.WebHostHidden]
    public abstract class SampleDataCommon : BlackSilverUfa_letsplay.Common.BindableBase
    {
        private static Uri _baseUri = new Uri("ms-appx:///");

        public SampleDataCommon(String uniqueId, String title, String subtitle, String imagePath, String description)
        {
            this._uniqueId = uniqueId;
            this._title = title;
            this._description = description;
            this._imagePath = imagePath;
        }

        private string _uniqueId = string.Empty;
        public string UniqueId
        {
            get { return this._uniqueId; }
            set { this.SetProperty(ref this._uniqueId, value); }
        }

        private string _title = string.Empty;
        public string Title
        {
            get { return this._title; }
            set { this.SetProperty(ref this._title, value); }
        }

        private string _description = string.Empty;
        public string Description
        {
            get { return this._description; }
            set { this.SetProperty(ref this._description, value); }
        }

        private string _link = string.Empty;
        public string Link
        {
            get { return this._link; }
            set { this.SetProperty(ref this._link, value); }
        }

        private ImageSource _image = null;
        private String _imagePath = null;
        public ImageSource Image
        {
            get
            {
                if (this._image == null && this._imagePath != null)
                {
                    this._image = new BitmapImage(new Uri(SampleDataCommon._baseUri, this._imagePath));
                }
                return this._image;
            }

            set
            {
                this._imagePath = null;
                this.SetProperty(ref this._image, value);
            }
        }

        public void SetImage(String path)
        {
            this._image = null;
            this._imagePath = path;
            this.OnPropertyChanged("Image");
        }

        public override string ToString()
        {
            return this.Title;
        }
    }

    /// <summary>
    /// Универсальная модель данных элементов.
    /// </summary>
    public class SampleDataItem : SampleDataCommon
    {
        public SampleDataItem(String uniqueId, String title, String subtitle, String imagePath, String description, String content, SampleDataGroup group)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
            this._content = content;
            this._group = group;
        }

        private string _content = string.Empty;
        public string Content
        {
            get { return this._content; }
            set { this.SetProperty(ref this._content, value); }
        }

        private SampleDataGroup _group;
        public SampleDataGroup Group
        {
            get { return this._group; }
            set { this.SetProperty(ref this._group, value); }
        }
    }

    /// <summary>
    /// Универсальная модель данных групп.
    /// </summary>
    public class SampleDataGroup : SampleDataCommon
    {
        public SampleDataGroup(String uniqueId, String title, String subtitle, String imagePath, String description)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
            Items.CollectionChanged += ItemsCollectionChanged;
        }

        private string _author = string.Empty;
        public string Author
        {
            get { return this._author; }
            set { this.SetProperty(ref this._author, value); }
        }

        private void ItemsCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // Предоставляет подмножество полной коллекции элементов, привязываемой из объекта GroupedItemsPage
            // по двум причинам: GridView не виртуализирует большие коллекции элементов и оно
            // улучшает работу пользователей при просмотре групп с большим количеством
            // элементов.
            //
            // Отображается максимальное число столбцов (12), поскольку это приводит к заполнению столбцов сетки
            // сколько строк отображается: 1, 2, 3, 4 или 6

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewStartingIndex < 12)
                    {
                        TopItems.Insert(e.NewStartingIndex,Items[e.NewStartingIndex]);
                        if (TopItems.Count > 12)
                        {
                            TopItems.RemoveAt(12);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                    if (e.OldStartingIndex < 12 && e.NewStartingIndex < 12)
                    {
                        TopItems.Move(e.OldStartingIndex, e.NewStartingIndex);
                    }
                    else if (e.OldStartingIndex < 12)
                    {
                        TopItems.RemoveAt(e.OldStartingIndex);
                        TopItems.Add(Items[11]);
                    }
                    else if (e.NewStartingIndex < 12)
                    {
                        TopItems.Insert(e.NewStartingIndex, Items[e.NewStartingIndex]);
                        TopItems.RemoveAt(12);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldStartingIndex < 12)
                    {
                        TopItems.RemoveAt(e.OldStartingIndex);
                        if (Items.Count >= 12)
                        {
                            TopItems.Add(Items[11]);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    if (e.OldStartingIndex < 12)
                    {
                        TopItems[e.OldStartingIndex] = Items[e.OldStartingIndex];
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    TopItems.Clear();
                    while (TopItems.Count < Items.Count && TopItems.Count < 12)
                    {
                        TopItems.Add(Items[TopItems.Count]);
                    }
                    break;
            }
        }

        private ObservableCollection<SampleDataItem> _items = new ObservableCollection<SampleDataItem>();
        public ObservableCollection<SampleDataItem> Items
        {
            get { return this._items; }
        }

        private ObservableCollection<SampleDataItem> _topItem = new ObservableCollection<SampleDataItem>();
        public ObservableCollection<SampleDataItem> TopItems
        {
            get {return this._topItem; }
        }
    }

    /// <summary>
    /// Создает коллекцию групп и элементов с жестко заданным содержимым.
    /// 
    /// SampleDataSource инициализируется подстановочными данными, а не реальными рабочими
    /// данными, чтобы пример данных был доступен как во время разработки, так и во время выполнения.
    /// </summary>
    public sealed class SampleDataSource
    {
        private static SampleDataSource _sampleDataSource = new SampleDataSource();

        private ObservableCollection<SampleDataGroup> _allGroups = new ObservableCollection<SampleDataGroup>();
        public ObservableCollection<SampleDataGroup> AllGroups
        {
            get { return this._allGroups; }
        }

        public async Task<string> MakeWebRequestForYouTube(string url="http://gdata.youtube.com/feeds/api/users/BlackSilverUfa/playlists")
        {
            HttpClient http = new System.Net.Http.HttpClient();
            HttpResponseMessage response = await http.GetAsync(url);
            return await response.Content.ReadAsStringAsync();
        }

        public static IEnumerable<SampleDataGroup> GetGroups(string uniqueId)
        {
            if (!uniqueId.Equals("AllGroups")) throw new ArgumentException("Only 'AllGroups' is supported as a collection of groups");
            
            return _sampleDataSource.AllGroups;
        }

        public static SampleDataGroup GetGroup(string uniqueId)
        {
            // Для небольших наборов данных можно использовать простой линейный поиск
            var matches = _sampleDataSource.AllGroups.Where((group) => group.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public static SampleDataItem GetItem(string uniqueId)
        {
            // Для небольших наборов данных можно использовать простой линейный поиск
            var matches = _sampleDataSource.AllGroups.SelectMany(group => group.Items).Where((item) => item.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public SampleDataSource()
        {
            this.LoadData();
        }

        public async void LoadData()
        {
            string playlistsxml = await this.MakeWebRequestForYouTube();

            XDocument playlists = XDocument.Parse(playlistsxml);
            //playlists.Descendants("feed").Descendants("Entry")

            XNamespace ns = "http://www.w3.org/2005/Atom";
            XNamespace media = "http://search.yahoo.com/mrss/";
            XNamespace yt = "http://gdata.youtube.com/schemas/2007";
            XNamespace gd = "http://schemas.google.com/g/2005";
            

            var estates = from e in playlists.Descendants(ns + "entry") select e;
            var items = estates.ToList();
            foreach (var item in items)
            {
                var id = item.Element(ns + "id").Value.ToString();
                var title = item.Element(ns + "title").Value.ToString();
                var link = item.Elements(ns + "link").FirstOrDefault(c => c.Attribute("rel").Value.ToString() == "alternate").Attribute("href").Value.ToString();
                var image = "";
                try
                {
                    image = item.Elements(media + "group").FirstOrDefault(c => c.Attribute(yt + "name").Value.ToString() == "hqdefault").Attribute("url").Value.ToString();
                }
                catch {
                    image = "/Assets/LightGray.png";
                };

                var feedLink = item.Element(gd + "feedLink").Attribute("href").Value.ToString();

                var description = item.Element(yt+ "description").Value.ToString();
                var group6 = new SampleDataGroup(id.ToString(),
                    title,
                    "",
                    image,
                    description);
                group6.Link = link;

                string playlistdataxml = await this.MakeWebRequestForYouTube(feedLink);

                XDocument playlistdata = XDocument.Parse(playlistdataxml);

                var videos1 = from e in playlistdata.Descendants(ns + "entry") select e;
                var videos = videos1.ToList();
                foreach (var video in videos)
                {
                    var vid = video.Element(ns + "id").Value.ToString();
                    var vtitle = video.Element(ns + "title").Value.ToString();
                    var vdescription = video.Element(yt + "description").Value.ToString();

                    var vimage = "";
                    try
                    {
                        vimage = item.Element(media + "group").Descendants(media + "thumbnail").First().Element(media + "thumbnail").Attribute("url").Value.ToString();
                        //.FirstOrDefault(c => c.Attribute(yt + "name").Value.ToString() == "hqdefault").Attribute("url").Value.ToString();
                    }
                    catch
                    {
                        vimage = "/Assets/LightGray.png";
                    };

                    var player = "";
                    try
                    {
                        player = item.Element(media + "player").Attribute("url").Value.ToString();
                    }
                    catch
                    {
                    };

                    group6.Items.Add(new SampleDataItem(vid,
                    vtitle,
                    player,
                    vimage,
                    vdescription,
                    "",
                    group6));
                };

                this.AllGroups.Add(group6);
            };
            /*XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
            //Geting XMl from the file.
            TextReader tr = new StreamReader(Server.MapPath("book1.xml"));
            //Deserialize back to object from XML
            List<T> b = (List<T>)serializer.Deserialize(tr);
            tr.Close();*/
        }
    }
}
