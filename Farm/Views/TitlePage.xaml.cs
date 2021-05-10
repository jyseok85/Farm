using Farm.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Farm.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TitlePage : ContentPage
    {
        public TitlePage()
        {
            InitializeComponent();
        }

        private const string templatefilename= "test.txt";

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

                if(data.Length == 0)
                {
                    data = await LoadInstalledData();
                }
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