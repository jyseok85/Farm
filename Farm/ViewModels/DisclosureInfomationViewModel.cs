using Farm.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;
using Xamarin.Essentials;

namespace Farm.ViewModels
{
    public class DisclosureInfomationViewModel : INotifyPropertyChanged
    {
        int itemCount = 0;
        const int PageSize = 10;
        const int RefreshDuration = 2;
        bool isRefreshing;

        public ObservableCollection<DisclosureInfomation> CollectionViewItems { get; private set; } = new ObservableCollection<DisclosureInfomation>();
        public ObservableCollection<DisclosureInfomation> SearchResult { get; private set; } = new ObservableCollection<DisclosureInfomation>();

        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set
            {
                isRefreshing = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoadMoreDataCommand => new Command(GetNextPageOfData);
        /// <summary>
        /// 굳이 사용할 필요가..
        /// </summary>
        public ICommand RefreshCommand => new Command(async () => await RefreshDataAsync());


        public DisclosureInfomationViewModel()
        {

        }

        public void InitDataSetting()
        {
            GetNextPageOfData();
        }
        /// <summary>
        /// 10개씩 CollectionViewItems 에 추가한다. 
        /// </summary>
        void GetNextPageOfData()
        {
            if(SearchResult.Count == 0)
            {
                DisclosureInfomation d = new DisclosureInfomation();
                d.상표명 = "검색된 데이터가 없습니다.";
                d.공시번호 = "";
                CollectionViewItems.Add(d);
                return;
            }

            if (itemCount < SearchResult.Count)
            {
                int start = itemCount;
                int end = itemCount + PageSize;
                for (int i = start; i < end; i++)
                {
                    if (SearchResult.Count > i)
                    {
                        공시종료일에따른_컨트롤배경_색상계산(SearchResult[i]);

                        공시기간만들기(SearchResult[i]);

                        CollectionViewItems.Add(SearchResult[i]);

                        itemCount++;
                    }
                }
            }
        }

        private void 공시종료일에따른_컨트롤배경_색상계산(DisclosureInfomation info)
        {
            string enddate = info.공시종료;
            try
            {
                enddate = enddate.Insert(6, " ");
                enddate = enddate.Insert(4, " ");
            }
            catch(Exception ex)
            {
                if(info.공시번호.Contains("부적합"))
                {
                    info.공시만료일_색상 = "#ff0000";
                    info.공시만료일_텍스트 = "부적합";
                }
                return;
            }
            DateTime endDateTime = Convert.ToDateTime(enddate);
            int result = DateTime.Compare(DateTime.Now, endDateTime);
            if (result > 0)
            {
                info.공시만료일_색상 = "#ff2626";
                info.공시만료일_텍스트 = "만료됨";
            }
            else
            {
                int furtureResult = DateTime.Compare( DateTime.Now.AddMonths(1), endDateTime);
                if (furtureResult > 0)
                {
                    info.공시만료일_색상 = "#ffb226";
                    info.공시만료일_텍스트 = "만료예정";
                }
                else
                {
                    info.공시만료일_색상 = "#62b765";
                    info.공시만료일_텍스트 = "공시중";

                }
            }
        }

        private void 공시기간만들기(DisclosureInfomation info)
        {

            string startDate = info.공시시작;
            try
            {
                startDate = startDate.Insert(6, ".");
                startDate = startDate.Insert(4, ".");
            }
            catch
            {

            }


            string enddate = info.공시종료;
            try
            {
                enddate = enddate.Insert(6, ".");
                enddate = enddate.Insert(4, ".");
            }
            catch
            {
            }
            info.공시기간 = startDate + " ~ " + enddate;
        }
        /// <summary>
        /// 굳이 사용할 필요가...
        /// </summary>
        /// <returns></returns>
        async Task RefreshDataAsync()
        {
            IsRefreshing = true;
            await Task.Delay(TimeSpan.FromSeconds(RefreshDuration));
            GetNextPageOfData();
            IsRefreshing = false;
        }

        public List<DisclosureInfomation> SearchAllText(string text)
        {
            if (App.DisInfo == null)
                return new List<DisclosureInfomation>();

            SearchResult.Clear();

            Stopwatch sw = new Stopwatch();
            sw.Start();

            var matches = App.DisInfo.Where(x => x.공시번호.Contains(text)
                                    || x.자재구분.Contains(text)
                                    || x.자재명칭.Contains(text)
                                    || x.상표명.Contains(text)
                                    || x.제조업체명.Contains(text)
                                    || x.대표자명.Contains(text)
                                    || x.사업장주소.Contains(text)
                                    || x.관리기관명.Contains(text)).OrderBy(x => x.공시번호).ToList();

            sw.Stop();
            Console.WriteLine("SearchAll : " + sw.ElapsedMilliseconds);

            foreach(var item in matches)
                SearchResult.Add(item);

            return matches;
        }
        public List<DisclosureInfomation> SearchText(SearchProperty property)
        {
            SearchResult.Clear();
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
            else if (property.지정상태 == 지정상태.기간만료)
            {
                기간만료기준일 = int.Parse(DateTime.Now.Date.ToString("yyyyMMdd"));
            }

            sw.Start();
            var matches = App.DisInfo.Where(x => x.공시번호.Contains(property.공시번호)
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

            foreach (var item in matches)
                SearchResult.Add(item);

            return matches;
        }


        private void SaveSelectedItem(DisclosureInfomation info)
        {
            string majorKey = "유기농업자재";
            Preferences.Set(majorKey + "상표명", info.상표명);
            Preferences.Set(majorKey + "제조업체명", info.제조업체명);
            Preferences.Set(majorKey + "공시번호", info.공시번호);
            Preferences.Set(majorKey + "공시기간", info.공시기간);
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
