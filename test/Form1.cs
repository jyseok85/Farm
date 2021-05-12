using Farm.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            string data = LoadSavedData();

            cc = JsonConvert.DeserializeObject<List<DisclosureInfomation>> (data);
            dd = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(data);

            SearchProperty sp = new SearchProperty();
            sp.제조업체명 = "팜스";
            sp.공시종료 = "20210101";
            cc = SearchText(sp);

        
        }

        List<DisclosureInfomation> cc = new List<DisclosureInfomation>();
        List<Dictionary<string,string>> dd = new List<Dictionary<string, string>>();

        private string localPath = Path.Combine(Application.StartupPath,"test.json");

        private const string url = "http://211.237.50.150:7080/openapi/9434477610cfc03aa0a8b7b86f2e01e9e7fb98e27a22a7fc25813511fc376a8d/json/Grid_20200929000000000606_1/{0}/{1}";
        public async Task UpdateData()
        {

            //설치버전기준 최소 카운트도 비교한다.


            string data = string.Empty;

            ////저장된 데이터가 없을경우 배포된 기본 데이터 로드
            //if (File.Exists(App.AppDataPath) == false)
            //{
            //    data = await LoadInstalledData();
            //}
            //else
            //{
            //    //마지막 저장된 데이터 로드
            //    data = LoadSavedData();

            //    if (data.Length == 0)
            //    {
            //        data = await LoadInstalledData();
            //    }
            //}

            int from = 1;
            int to = 1;
            string searchUrl = string.Format(url, from, to);


            var obj = await GetRepositoriesAsync(searchUrl);
            Dictionary<string, object> dic = JsonConvert.DeserializeObject<Dictionary<string,object>>(obj.ToString());

            if (dic.Count > 0)
            {
                Dictionary<string, object> dic2 = JsonConvert.DeserializeObject<Dictionary<string,object>>(dic["Grid_20200929000000000606_1"].ToString());
                if (dic2.ContainsKey("totalCnt"))
                {
                    string totalCnt = dic2["totalCnt"].ToString();

                    if (dic2.ContainsKey("row"))
                    {
                        List<Dictionary<string,string>> item = JsonConvert.DeserializeObject<List<Dictionary<string,string>>>(dic2["row"].ToString());
                    }
                }
            }


            //서버에서 데이터를 조회한다.(카운트)

            //마지막 저장된 숫자보다 클 경우에 추가된 데이터를 조회한다. 

            //Json 파일로 저장을 한다.

            //파일을 읽어서 CollectionView 에 데이터를 넣는다.
            //var files = Directory.EnumerateFiles(App.FolderPath, "*.notes.txt");
            //foreach (var filename in files)
            //{
            //    notes.Add(new DisclosureInfomation
            //    {
            //        //Filename = filename,
            //        //Text = File.ReadAllText(filename),
            //        //Date = File.GetCreationTime(filename)
            //    });
            //}


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
                //Debug.WriteLine("\tERROR {0}", ex.Message);
            }
            return content;
        }
        private void Save(string contents)
        {
            File.WriteAllText(localPath, contents);
        }
        private string LoadSavedData()
        {
            return File.ReadAllText(localPath);
        }

        //private List<Dictionary<string,string>> SearchAllText(string text)
        //{
        //    Stopwatch sw = new Stopwatch();
        //    sw.Start();
            
        //    //속도가 10배이상 느림
        //    var matches = dd.AsParallel().Where(s => s.Values.Any(x => x.Contains(text))).ToList();

        //    sw.Stop();
        //    Console.WriteLine("2 - " + sw.ElapsedMilliseconds);

        //    return matches;
        //}

        private List<DisclosureInfomation> SearchAllText(string text)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var matches = cc.Where(x=> x.공시번호.Contains(text) 
                                    || x.자재구분.Contains(text) 
                                    || x.자재명칭.Contains(text) 
                                    || x.상표명.Contains(text) 
                                    || x.제조업체명.Contains(text) 
                                    || x.대표자명.Contains(text) 
                                    || x.사업장주소.Contains(text) 
                                    || x.관리기관명.Contains(text)).OrderBy(x => x.공시번호).ToList();

            sw.Stop();
            Console.WriteLine("SearchAll : " + sw.ElapsedMilliseconds);
            return matches;
        }
        private List<DisclosureInfomation> SearchText(SearchProperty property)
        {
            Stopwatch sw = new Stopwatch();

            int start_date;
            int end_date;
            if (property.공시시작 != string.Empty)
                start_date = int.Parse(property.공시시작);
            else
                start_date = int.Parse("19000101");

            if (property.공시종료 != string.Empty)
                end_date = int.Parse(property.공시종료);
            else
                end_date = int.Parse("21000101");

            int 공시목록기준일 = start_date;
            int 기간만료기준일 = end_date;
            if (property.지정상태 == 지정상태.공시목록)
            {
                공시목록기준일 = int.Parse(DateTime.Now.Date.ToString("yyyyMMdd"));
            }
            else if(property.지정상태 == 지정상태.기간만료)
            {
                기간만료기준일 = int.Parse(DateTime.Now.Date.ToString("yyyyMMdd"));
            }

            sw.Start();
            var matches = cc.Where(x => x.공시번호.Contains(property.공시번호) 
                                && x.자재구분.Contains(property.자재구분) 
                                && x.자재명칭.Contains(property.자재명칭) 
                                && x.상표명.Contains(property.상표명) 
                                && x.제조업체명.Contains(property.제조업체명) 
                                && x.대표자명.Contains(property.대표자명) 
                                && x.사업장주소.Contains(property.사업장주소) 
                                && x.관리기관명.Contains(property.관리기관명)
                                //&& x.시험작물또는병충해.Contains(property.시험작물또는병충해) <-- 추가된다면..
                                && int.Parse(x.공시시작) >= start_date
                                && int.Parse(x.공시종료) <= end_date
                                && int.Parse(x.공시종료) >= 공시목록기준일 //공시종료가 오늘날짜보다 이후목록만 조회 값이 없을경우 초기값으로 전체조회됨
                                && int.Parse(x.공시종료) <= 기간만료기준일 //공시종료가 오늘날짜보다 이전목록만 조회 값이 없을경우 초기값으로 전체조회됨0
                                )
                                .OrderBy(x => x.공시번호).ToList();

            sw.Stop();
            Console.WriteLine("Search : " + sw.ElapsedMilliseconds);

            return matches;

        }
        private async void 기초데이터만들기()
        {
            //기본조회
            int from = 1;
            int to = 1;
            string searchUrl = string.Format(url, from, to);

            #region 총 Row Count 조회
            int tCount = 0;
            var obj = await GetRepositoriesAsync(searchUrl);
            Dictionary<string, object> dic = JsonConvert.DeserializeObject<Dictionary<string,object>>(obj.ToString());


            if (dic.Count > 0)
            {

                if (dic.ContainsKey("Grid_20200929000000000606_1") == false)
                {

                }

                Dictionary<string, object> dic2 = JsonConvert.DeserializeObject<Dictionary<string,object>>(dic["Grid_20200929000000000606_1"].ToString());
                if (dic2.ContainsKey("totalCnt"))
                {
                    string totalCnt = dic2["totalCnt"].ToString();
                    tCount = Convert.ToInt32(totalCnt);
                }
            }
            #endregion

            List<Dictionary<string, string>> data = new List<Dictionary<string, string>>();

            #region 1000건씩 조회해서 리스트에 추가한다.
            if (tCount > 0)
            {
                int roopCount = (tCount / 1000) + 1;

                from = 1;
                to = 1000;

                for (int i = 0; i < roopCount; i++)
                {
                    searchUrl = string.Format(url, from, to);
                    data.AddRange(await GetRowData(searchUrl));
                    from = 1 + to;
                    to += 1000;
                }
            }
            #endregion

            //저장
            Save(JsonConvert.SerializeObject(data));

        }

        private async Task<List<Dictionary<string, string>>> GetRowData(string url)
        {
            var obj = await GetRepositoriesAsync(url);
            Dictionary<string, object> dic = JsonConvert.DeserializeObject<Dictionary<string,object>>(obj.ToString());

            List <Dictionary<string, string>> item = new List<Dictionary<string, string>>();
            if (dic.Count > 0)
            {
                Dictionary<string, object> dic2 = JsonConvert.DeserializeObject<Dictionary<string,object>>(dic["Grid_20200929000000000606_1"].ToString());
                if (dic2.ContainsKey("row"))
                {
                    item = JsonConvert.DeserializeObject<List<Dictionary<string,string>>>(dic2["row"].ToString());
                }
            }

            return item;
        }
        //private async Task<string> LoadInstalledData()
        //{
        //    using (var stream = await File.ReadAllTexttemplatefilename))
        //    {
        //        using (var reader = new StreamReader(stream))
        //        {
        //            return await reader.ReadToEndAsync();
        //        }
        //    }
        //}
    }
}
