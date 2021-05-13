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

    namespace Farm.Models
    {
        public partial class Loading 
        {
            public Loading()
            {

            }

            //private const string installedDataName = "initdata.json";
            private const string installedDataName = "inittest.json";

            private const string API_KEY = "Grid_20200929000000000606_1";
            /// <summary>
            /// 공시 유기농업자재 조회 OPENAPI
            /// </summary>
            private const string url = "http://211.237.50.150:7080/openapi/9434477610cfc03aa0a8b7b86f2e01e9e7fb98e27a22a7fc25813511fc376a8d/json/Grid_20200929000000000606_1/{0}/{1}";


            /// <summary>
            /// 1. AppDataDirectory 경로에서 파일을 불러옵니다. 
            /// 2. 파일이 없을경우(처음진행) 패키지경로(OpenAppPackageFileAsync)에서 읽어옵니다. 
            /// 3. 서버에서 총 데이터 수를 가져옵니다. 
            /// 4. 현재 로컬에 저장된 갯수보다 더 많을 경우에 추가로 데이터를 받아옵니다.
            /// 5. 데이터를 저장해서 다음부터는 업데이트가 없는 이상 로컬 파일만 받도록 합니다. 
            /// </summary>
            /// <returns></returns>
            public async Task InitDataSetting(Label StatusMessage)
            {
                string data = string.Empty;


                //저장된 데이터가 없을경우 배포된 기본 데이터 로드
                if (File.Exists(App.AppDataPath) == false)
                {
                    StatusMessage.Text = "초기 데이터 설정중..";
                    await Task.Delay(5000);

                    data = await LoadInstalledData();
                    Save(data);
                }
                else
                {
                    StatusMessage.Text = "저장된 데이터 불러오는 중..";
                    await Task.Delay(5000);

                    //마지막 저장된 데이터 로드
                    data = LoadSavedData();

                    if (data.Length == 0) //저장된 데이터의 길이가 0 이면 기본 데이터 로드
                    {
                        data = await LoadInstalledData();
                    }
                }
                return;
                List<DisclosureInfomation> savedData = JsonConvert.DeserializeObject<List<DisclosureInfomation>>(data);

                //서버에서 데이터를 조회한다.(카운트)
                int serverDataCount = 0;
                try
                {
                    StatusMessage.Text = "서버에서 최신 데이터를 가져옵니다.";
                    serverDataCount = await GetServerDataTotalCount();
                    await Task.Delay(1000);

                    //마지막 Row Num 을 조회해서 일치하는지 확인할수도 있고, 
                    //저장된거의 마지막 RowNum 과 비교해서 받을수도 있는데, 저 Row Num 이 db에서 자동생성값이 아니라. 실제 row index 일경우에는 역시 틀어지고,, 그냥 안할랜다.
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("\tERROR {0}", ex.Message);
                    StatusMessage.Text = "서버접속 실패.. 로컬 데이터를 사용합니다.";
                    App.DisInfo = savedData;
                }

                //현재 저장된 갯수보다 서버에 데이터가 많을 경우에 추가된 데이터를 조회한다. 
                if (serverDataCount > savedData.Count)
                {
                    StatusMessage.Text = "데이터를 업데이트중 입니다.";
                    await Task.Delay(1000);
                    int lastSaveRow;
                    int.TryParse(savedData[savedData.Count - 1].ROW_NUM, out lastSaveRow);

                    List<DisclosureInfomation> updated = await GetUpdatedData(lastSaveRow + 1, serverDataCount);

                    //기존 데이터에 추가한다. 로컬 스토리지에 저장한다. 
                    if (updated.Count > 0)
                    {
                        savedData.AddRange(updated);

                        string jsonStr = JsonConvert.SerializeObject(savedData);
                        Save(jsonStr);
                    }
                }
                StatusMessage.Text = "작업 완료";

                await Task.Delay(3000);
                App.DisInfo = savedData;
            }

            private async Task<int> GetServerDataTotalCount()
            {
                int from = 1;
                int to = 1;
                string searchUrl = string.Format(url, from, to);

                #region 총 Row Count 조회
                int tCount = 0;
                var obj = await GetHttpRequest(searchUrl);
                Dictionary<string, object> dic = JsonConvert.DeserializeObject<Dictionary<string,object>>(obj.ToString());

                if (dic.Count > 0)
                {
                    if (dic.ContainsKey(API_KEY) == true)
                    {
                        Dictionary<string, object> dic2 = JsonConvert.DeserializeObject<Dictionary<string,object>>(dic[API_KEY].ToString());
                        if (dic2.ContainsKey("totalCnt"))
                        {
                            string totalCnt = dic2["totalCnt"].ToString();
                            tCount = Convert.ToInt32(totalCnt);
                        }
                    }
                }
                #endregion

                return tCount;
            }

            private async Task<List<DisclosureInfomation>> GetUpdatedData(int from, int to)
            {
                string searchUrl = string.Format(url, from, to);
                List<DisclosureInfomation> data = new List<DisclosureInfomation>();
                if (to - from > 1000)
                {
                    int roopCount = (to - from) / 1000 + 1;
                    to = from + 1000;

                    for (int i = 0; i < roopCount; i++)
                    {
                        searchUrl = string.Format(url, from, to);
                        data.AddRange(await GetUpdatedRowData(searchUrl));
                        from = 1 + to;
                        to += 1000;
                    }
                }
                else
                {
                    data.AddRange(await GetUpdatedRowData(searchUrl));
                }

                return data;
            }

            private async Task<List<DisclosureInfomation>> GetUpdatedRowData(string url)
            {
                var obj = await GetHttpRequest(url);
                Dictionary<string, object> dic = JsonConvert.DeserializeObject<Dictionary<string,object>>(obj.ToString());

                List <DisclosureInfomation> item = new List<DisclosureInfomation>();
                if (dic.Count > 0)
                {
                    Dictionary<string, object> dic2 = JsonConvert.DeserializeObject<Dictionary<string,object>>(dic[API_KEY].ToString());
                    if (dic2.ContainsKey("row"))
                    {
                        item = JsonConvert.DeserializeObject<List<DisclosureInfomation>>(dic2["row"].ToString());
                    }
                }

                return item;
            }

            private async Task<object> GetHttpRequest(string uri)
            {
                HttpClient client = new HttpClient();
                string content = string.Empty;
                try
                {
                    client.Timeout = TimeSpan.FromSeconds(5);
                    HttpResponseMessage response = await client.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        content = await response.Content.ReadAsStringAsync();
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("\tERROR {0}", ex.Message);
                    throw ex;
                }
                return content;
            }



            /// <summary>
            /// 로컬 스토리지에 저장합니다. 
            /// </summary>
            /// <param name="contents"></param>
            private void Save(string contents)
            {
                File.WriteAllText(App.AppDataPath, contents);
            }
            /// <summary>
            /// 로컬 스토리지에서 파일을 불러옵니다. 
            /// </summary>
            /// <returns></returns>
            private string LoadSavedData()
            {
                return File.ReadAllText(App.AppDataPath);
            }

            /// <summary>
            /// 배포 데이터 경로에서 파일을 불러옵니다. 
            /// </summary>
            /// <returns></returns>
            private async Task<string> LoadInstalledData()
            {
                using (var stream = await FileSystem.OpenAppPackageFileAsync(installedDataName))
                {
                    using (var reader = new StreamReader(stream))
                    {
                        return await reader.ReadToEndAsync();
                    }
                }
            }



        }
    }
