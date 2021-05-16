using Farm;
using Farm.Models;
using Farm.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Farm.Controls
{
    public class OrganicMaterialsSearchHandler: SearchHandler
    {
        //https://docs.microsoft.com/ko-kr/xamarin/xamarin-forms/app-fundamentals/shell/search
        public Type SelectedItemNavigationTarget { get; set; }
        protected override void OnQueryChanged(string oldValue, string newValue)
        {
            base.OnQueryChanged(oldValue, newValue);

            //if(App.DisInfo != null && App.DisInfo.Count > 0)
            //{
            //    if (string.IsNullOrWhiteSpace(newValue))
            //    {
            //        ItemsSource = null;
            //    }
            //    else
            //    {
            //        ItemsSource = App.DisInfo
            //            .Where(x => x.공시ID.ToLower().Contains(newValue.ToLower()))
            //            .ToList<DisclosureInfomation>();
            //    }
            //}
        }

        protected override async void OnItemSelected(object item)
        {
            base.OnItemSelected(item);

            await Task.Delay(1000);

            ShellNavigationState state = (App.Current.MainPage as Shell).CurrentState;
            // The following route works because route names are unique in this application.
            await Shell.Current.GoToAsync($"{GetNavigationTarget()}?name={((DisclosureInfomation)item).공시ID}");
        }

        string GetNavigationTarget()
        {
            return null; //(Shell.Current as AppShell).Routes.FirstOrDefault(route => route.Value.Equals(SelectedItemNavigationTarget)).Key;
        }

        /// <summary>
        /// 확인키가 눌렸을때 동작
        /// </summary>
        protected override void OnQueryConfirmed()
        {
            
            if(string.IsNullOrEmpty(this.Query) == false)
            {
                SearchAllText(this.Query);
            }

            base.OnQueryConfirmed();
           
            
        }

        private List<DisclosureInfomation> SearchAllText(string text)
        {
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

            return matches;

        }
    }
}