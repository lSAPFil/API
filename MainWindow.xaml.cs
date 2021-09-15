using System;
using System.IO;
using System.Windows;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.Http;
using System.Windows.Controls;
using System.Text.RegularExpressions;

namespace TestProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Instance { get; private set; }
        public MainWindow()
        {
            InitializeComponent();
            Instance = this;
        }

        public static async void GetApi(string GetId)
        {
            string baseURL = $"http://tmgwebtest.azurewebsites.net/api/textstrings/{GetId}";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("TMG-Api-Key", "0J/RgNC40LLQtdGC0LjQutC4IQ==");
                    
                    using (HttpResponseMessage res = await client.GetAsync(baseURL))
                    {
                        using (HttpContent content = res.Content)
                        {
                            string data = await content.ReadAsStringAsync();

                            ParseAndViw(data.ToString());
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            String s = IdentityLine.Text;
            GetApi(s);
        }

        public static void ParseAndViw(string some)
        {  
            string json = some;

            var options = new JsonDocumentOptions
            {
                AllowTrailingCommas = true
            };

            JsonDocument document = JsonDocument.Parse(json, options);
            JsonElement element = document.RootElement.GetProperty("text");

            string[] cleanText =  Regex.Replace(element.ToString(), "[^A-Za-z0-9 ]", "").Split(new char[] { ' ' });
            int count = Regex.Matches(element.ToString(), @"[ауоыиэяюёеaeiouüùöûïÃãÈèÑñ₣ÊêÕõÔôÅåÙùÁáÛûÍíÏïÌìÓóÒòÚúÝýÿyîèôòùạọèêẹìéçÀäàEYUIOAЕУЫАОЯИЮЁÄÂÇÉÈÊÎÔÙÛÏâ]", RegexOptions.IgnoreCase).Count;
            
            MainWindow.Instance.TextLine.Text= element.ToString();
            MainWindow.Instance.TextValue.Text = (cleanText.Length).ToString();
            MainWindow.Instance.TextValue2.Text = count.ToString();
        }
       

        public class Item
        {
            public string stamp;
        }
    }
}
