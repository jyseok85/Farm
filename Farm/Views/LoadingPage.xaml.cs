using Farm.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Farm.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadingPage : ContentPage
    {
        public LoadingPage()
        {
            InitializeComponent();
        }

        private const string templatefilename= "test.txt";
        /// <summary>
        /// 공시 유기농업자재 조회 OPENAPI
        /// </summary>
        private const string url = "http://211.237.50.150:7080/openapi/9434477610cfc03aa0a8b7b86f2e01e9e7fb98e27a22a7fc25813511fc376a8d/json/Grid_20200929000000000606_1/{0}/{1}";
        public async Task UpdateData()
        {

            var notes = new List<DisclosureInfomation>();
            //설치버전기준 최소 카운트도 비교한다.


            string data = string.Empty;

            //저장된 데이터가 없을경우 배포된 기본 데이터 로드
            if (File.Exists(App.AppDataPath) == false)
            {
                data = await LoadInstalledData();
            }
            else
            {
                //마지막 저장된 데이터 로드
                data = LoadSavedData();

                if (data.Length == 0)
                {
                    data = await LoadInstalledData();
                }
            }

            int from = 1;
            int to = 1;
            string searchUrl = string.Format(url, from, to);


            var obj = await GetRepositoriesAsync(searchUrl);
            Dictionary<string, object> dic = JsonConvert.DeserializeObject<Dictionary<string,object>>(obj.ToString());

            if(dic.Count > 0)
            {
                //Dictionary<string, object> dic2 = JsonConvert.DeserializeObject<Dictionary<string,object>>(dic["Grid_20200929000000000606_1"].ToString());
                //if (dic2.ContainsKey("totalCnt"))
                //    string totalCnt = dic2["totalCnt"].ToString();

                //if(dic2.ContainsKey("row"))
                //{
                //    List<Disclo>
                //}

            }
            //서버에서 데이터를 조회한다.(카운트)

            //마지막 저장된 숫자보다 클 경우에 추가된 데이터를 조회한다. 

            //Json 파일로 저장을 한다.

            //파일을 읽어서 CollectionView 에 데이터를 넣는다.
            var files = Directory.EnumerateFiles(App.FolderPath, "*.notes.txt");
            foreach (var filename in files)
            {
                notes.Add(new DisclosureInfomation
                {
                    //Filename = filename,
                    //Text = File.ReadAllText(filename),
                    //Date = File.GetCreationTime(filename)
                });
            }


            await Task.Delay(2000);

        }
        public async Task<object> GetRepositoriesAsync(string uri)
        {
            HttpClient client = new HttpClient();
            string content = string.Empty;
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    content = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\tERROR {0}", ex.Message);
            }
            return content;
        }
        private void Save(string contents)
        {
            File.WriteAllText(App.AppDataPath, contents);
        }
        private string LoadSavedData()
        {
            return File.ReadAllText(App.AppDataPath);
        }

        private async Task<string> LoadInstalledData()
        {
            using (var stream = await FileSystem.OpenAppPackageFileAsync(templatefilename))
            {
                using (var reader = new StreamReader(stream))
                {
                    return await reader.ReadToEndAsync();
                }
            }
        }
    }
}