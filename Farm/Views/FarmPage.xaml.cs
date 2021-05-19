using Farm.Models;
using Farm.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Farm.Views
{
    public partial class FarmPage : ContentPage
    {
        //readonly string _fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "notes.txt");


        private string SearchText { get; set; } 
        public FarmPage()
        {
            InitializeComponent();
            SearchItemBody.TranslationY= -50;
        }

        /// <summary>
        /// 페이지가 표시되면 
        /// </summary>
        protected async override void OnAppearing()
        {
            base.OnAppearing();

            await App.GetInitialData(StatusMessage);
        }

        async void OnAddClicked(object sender, EventArgs e)
        {
            // Navigate to the NoteEntryPage, without passing any data.
            await Shell.Current.GoToAsync(nameof(DisclosureInfoEntryPage));
        }

        async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection != null)
            {
                // Navigate to the NoteEntryPage, passing the filename as a query parameter.
                DisclosureInfomation note = (DisclosureInfomation)e.CurrentSelection.FirstOrDefault();
                await Shell.Current.GoToAsync($"{nameof(DisclosureInfoEntryPage)}?{nameof(DisclosureInfoEntryPage.ItemId)}={note.공시ID}");
            }
        }

        public void aaa()
        {
            SearchLabel.IsVisible = false;
        }

        void SearchBarNoUnderline_Focused(object sender, EventArgs e)
        {
            //SearchBar searchBar = (SearchBar)sender;
            //searchResults.ItemsSource = DataService.GetSearchResults(searchBar.Text);
            //((FarmPage)Shell.Current.CurrentPage).aaa();
            Default = SearchLabel.Bounds;
            isShowLabel = false;
#pragma warning disable CS4014 // 이 호출을 대기하지 않으므로 호출이 완료되기 전에 현재 메서드가 계속 실행됩니다.
            CloseLabel();
#pragma warning restore CS4014 // 이 호출을 대기하지 않으므로 호출이 완료되기 전에 현재 메서드가 계속 실행됩니다.
        }

        private async Task CloseLabel()
        {
            _ = CustomTitle.TranslateTo(0, -50);
            _ = Body.TranslateTo(0, -50);

            while (CancelButton.Width.Value < 1 && this.isShowLabel == false)
            {
                GridLength grid = new GridLength(CancelButton.Width.Value + 0.2, GridUnitType.Star);
                CancelButton.Width = grid;
                await Task.Delay(1);

            }
        }

        private async Task ShowLabel()
        {
            _ = CustomTitle.TranslateTo(0, 0);
            _ = Body.TranslateTo(0, 0);

            while (this.isShowLabel == true)
            {
                double targetValue = CancelButton.Width.Value - 0.2;
                if (targetValue <= 0)
                {
                    isShowLabel = false;
                    targetValue = 0;
                }

                GridLength grid = new GridLength(targetValue, GridUnitType.Star);
                CancelButton.Width = grid;
                await Task.Delay(1);
            }
        }

                                                                        
        Rectangle Default = Rectangle.Zero;
        bool isShowLabel = false;
        private void SearchBarNoUnderline_CancelButtonPressed(object sender, EventArgs e)
        {
             isShowLabel = true;
            Body.IsVisible = true;
            SearchBarNoUnderline.Unfocus();

#pragma warning disable CS4014 // 이 호출을 대기하지 않으므로 호출이 완료되기 전에 현재 메서드가 계속 실행됩니다.
            ShowLabel();
#pragma warning restore CS4014 // 이 호출을 대기하지 않으므로 호출이 완료되기 전에 현재 메서드가 계속 실행됩니다.
        }

        private void SearchBarNoUnderline_SearchButtonPressed(object sender, EventArgs e)
        {
            Body.IsVisible = false;

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
