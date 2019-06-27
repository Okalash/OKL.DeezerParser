using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DeezerParser
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        class Song
        {
            public string SongName { get; set; }
            public string Artist { get; set; }
            public string Duration { get; set; }
        }
        private async Task GetInfoAsync(string linkToParse)
        {
            var client = new HttpClient();
            var arrayLink = linkToParse.Split('/');   
            string html = await client.GetStringAsync("https://api.deezer.com/playlist/"+ arrayLink[arrayLink.Length-1]);
            var playlist = JObject.Parse(html);
            var songs = new List<Song>();
            foreach (var track in playlist["tracks"]["data"])
            {
                var kek = track.ToString();
                int dur = int.Parse(track["duration"].ToString());
                int minutes = dur / 60;
                string seconds = (dur % 60).ToString();
                if (int.Parse(seconds) < 10)
                {
                    seconds = "0" + seconds;
                }
                songs.Add(new Song()
                {
                    Artist = track["artist"]["name"].ToString(),
                    SongName = track["title"].ToString(),
                    Duration = minutes + ":" + seconds
                });

            }
            
            SongsGrid.ItemsSource = songs;
            SongsGrid.CanUserDeleteRows = false;
            SongsGrid.CanUserAddRows = false;
            SongsGrid.CanUserReorderColumns = false;
            SongsGrid.IsReadOnly = true;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GetInfoAsync(link.Text);
        }
    }
}

