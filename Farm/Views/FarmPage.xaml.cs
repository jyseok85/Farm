using OganicInput.Models;
using OganicInput.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OganicInput.Views
{
    public partial class OganicInputPage : ContentPage
    {
        //readonly string _fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "notes.txt");

        private string SearchText { get; set; } 
        public OganicInputPage()
        {
            InitializeComponent();
            LoadHistory();
            HistoryCollectionView.ItemsSource = SelectedHistory;
            LastSelectedItem.ItemsSource = LastSelected;

        }

        /// <summary>
        /// 페이지가 표시되면   
        /// </summary>
        protected async override void OnAppearing()
        {
            base.OnAppearing();

            if (App.IsFirstLoad)
            {
                App.IsFirstLoad = false;
                await Navigation.PushModalAsync(new LoadingView());
            }
        }

        //ObservableCollection 을 사용해야 View에서 가져갈수 있다. 
        ObservableCollection<DisclosureInfomation> SelectedHistory = new ObservableCollection<DisclosureInfomation>();
        ObservableCollection<DisclosureInfomation> LastSelected = new ObservableCollection<DisclosureInfomation>();

        private void LoadHistory()
        {
            if (File.Exists(App.HistoryPath))
            {
                string jsonData = File.ReadAllText(App.HistoryPath);
                SelectedHistory = JsonConvert.DeserializeObject<ObservableCollection<DisclosureInfomation>>(jsonData);
                if (SelectedHistory.Count > 0)
                {
                    LastSelected.Clear();
                    LastSelected.Add(SelectedHistory[0]);
                }
            }
        }
        private void SaveHistory(DisclosureInfomation note)
        {
            if (SelectedHistory.Contains(note))
            {
                SelectedHistory.Remove(note);
            }
            else if (SelectedHistory.Count > 10)
            {
                SelectedHistory.RemoveAt(SelectedHistory.Count - 1);
            }

            SelectedHistory.Insert(0, note);
            if (SelectedHistory.Count > 0)
            {
                LastSelected.Clear();
                LastSelected.Add(SelectedHistory[0]);
            }

            string jsonStr = JsonConvert.SerializeObject(SelectedHistory.Distinct());
            File.WriteAllText(App.HistoryPath, jsonStr);
        }
        async void OnHistorySelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var view = sender as CollectionView;
            if (view.SelectedItem != null && e.CurrentSelection != null)
            {
                DisclosureInfomation note = (DisclosureInfomation)e.CurrentSelection.FirstOrDefault();
                string jsonData = JsonConvert.SerializeObject(note);
                double diff = ((double)CustomTitle.Height - 10) / 2;
                await Shell.Current.GoToAsync($"OganicInputDetailPage?DisInfo={jsonData}&Key={diff}");
            }
            view.SelectedItem = null;
        }
        async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var view = sender as CollectionView;
            if (view.SelectedItem != null && e.CurrentSelection != null)
            { 
                DisclosureInfomation note = (DisclosureInfomation)e.CurrentSelection.FirstOrDefault();
               
                // Navigate to the NoteEntryPage, passing the filename as a query parameter.
                //DisclosureInfomation note = (DisclosureInfomation)e.CurrentSelection.FirstOrDefault();

                string jsonData = JsonConvert.SerializeObject(note);
                double diff = ((double)CustomTitle.Height - 1) / 2;
                await Shell.Current.GoToAsync($"OganicInputDetailPage?DisInfo={jsonData}&Key={diff}");

                SaveHistory(note);
                view.SelectedItem = null;
            }

        }

        void SearchBarNoUnderline_Focused(object sender, EventArgs e)
        {
            //SearchBar searchBar = (SearchBar)sender;
            //searchResults.ItemsSource = DataService.GetSearchResults(searchBar.Text);
            //((OganicInputPage)Shell.Current.CurrentPage).aaa();
            isShowLabel = false;
#pragma warning disable CS4014 // 이 호출을 대기하지 않으므로 호출이 완료되기 전에 현재 메서드가 계속 실행됩니다.
            CloseLabel();
#pragma warning restore CS4014 // 이 호출을 대기하지 않으므로 호출이 완료되기 전에 현재 메서드가 계속 실행됩니다.
        }

        private async Task CloseLabel()
        {
            //double yValue = ((double)CustomTitle.Height) - SearchControl.Height;
            double yValue = ((double)CustomTitle.Height - 10) / 2;
            //double yValue = SearchTitleLabel.Height;
            _ = CustomTitle.TranslateTo(0, -yValue);
            _ = Body.TranslateTo(0, -yValue);
            Body.Margin = new Thickness(0, 0, 0, -yValue);
           // CancelButton.Text = "취";
            while (GridCancelButtonArea.Width.Value < 1 && this.isShowLabel == false)
            {
                GridLength grid = new GridLength(GridCancelButtonArea.Width.Value + 0.2, GridUnitType.Star);
                
                GridCancelButtonArea.Width = grid;
                //if (GridCancelButtonArea.Width.Value >= 0.6)
                //{
                //    CancelButton.Text = "취소";
                //}
                await Task.Delay(1);

            }
        }

        private async Task ShowLabel()
        {
            _ = CustomTitle.TranslateTo(0, 0);
            _ = Body.TranslateTo(0, 0);

            Body.Margin = 0;

            while (this.isShowLabel == true)
            {
                double targetValue = GridCancelButtonArea.Width.Value - 0.2;
                if (targetValue <= 0)
                {
                    isShowLabel = false;
                    targetValue = 0;
                }

                GridLength grid = new GridLength(targetValue, GridUnitType.Star);
                GridCancelButtonArea.Width = grid;
                await Task.Delay(1);
            }
        }

        bool isShowLabel = false;
        private void SearchBarNoUnderline_CancelButtonPressed(object sender, EventArgs e)
        {
            isShowLabel = true;
            DefaultBody.IsVisible = true;
            SearchItemBody.IsVisible = false;
            SearchBarNoUnderline.Unfocus();

#pragma warning disable CS4014 // 이 호출을 대기하지 않으므로 호출이 완료되기 전에 현재 메서드가 계속 실행됩니다.
            ShowLabel();
#pragma warning restore CS4014 // 이 호출을 대기하지 않으므로 호출이 완료되기 전에 현재 메서드가 계속 실행됩니다.
        }

        private void SearchBarNoUnderline_SearchButtonPressed(object sender, EventArgs e)
        {
            DefaultBody.IsVisible = false;
            SearchItemBody.IsVisible = true;

            //CollectionView 표시
            //기존꺼 삭제
            DisclosureInfomationViewModel disclosure = new DisclosureInfomationViewModel();
            int searchCount = disclosure.SearchAllText(SearchBarNoUnderline.Text).Count;
            SearchResultMessage.Text = string.Format("(총 조회건수 : {0} 건)", searchCount);
            disclosure.InitDataSetting();
            BindingContext = disclosure;
        }
        void OnCollectionViewRemainingItemsThresholdReached(object sender, EventArgs e)
        {
            // Retrieve more data here, or via the RemainingItemsThresholdReachedCommand.
            // This sample retrieves more data using the RemainingItemsThresholdReachedCommand.
        }
    }
}
