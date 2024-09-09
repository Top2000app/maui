using Top2000MauiApp.Globalisation;
using Top2000MauiApp.XamarinForms;
using Top2000.Features.Searching;


namespace Top2000MauiApp.Searching
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class View : ContentPage, ISortGroupNameProvider
    {
        public View()
        {
            BindingContext = App.GetService<ViewModel>();
            InitializeComponent();
        }

        public ViewModel ViewModel => (ViewModel)BindingContext;

        public string GetNameForGroup(IGroup group)
        {
            return GetNameForGroupOrSortBy(group)
                ?? throw new NotImplementedException($"Group '{group.GetType()}' was not defined yet.");
        }

        public string GetNameForSort(ISort sort)
        {
            return GetNameForGroupOrSortBy(sort)
                ?? throw new NotImplementedException($"Display text for sort option '{sort.GetType()}' was not defined.");
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.OnActivate(this);

            if (string.IsNullOrWhiteSpace(ViewModel.QueryText))
            {
                ViewModel.IsFlat = ViewModel.GroupBy.Value is GroupByNothing;
            }

            GroupSortLayout.IsVisible = false;
            SetTitlesForButton();
        }

        protected override bool OnBackButtonPressed()
        {
            if (this.trackInformation.IsVisible)
            {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                CloseTrackInformationAsync();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                return true;
            }

            return base.OnBackButtonPressed();
        }

        private string? GetNameForGroupOrSortBy(object item)
        {
            return item switch
            {
                GroupByNothing _ => AppResources.None,
                SortByArtist _ => AppResources.Artist,
                GroupByArtist _ => AppResources.Artist,
                SortByRecordedYear _ => AppResources.Year,
                GroupByRecordedYear _ => AppResources.Year,
                SortByTitle _ => AppResources.Title,
                _ => null,
            };
        }

        async private void OnListingSelected(object sender, SelectionChangedEventArgs e)
        {
            if (ViewModel.SelectedTrack is null) return;

            var infoTask = trackInformation.LoadTrackDetailsAsync(ViewModel.SelectedTrack.Id, CloseTrackInformationAsync);

            Shell.SetNavBarIsVisible(this, false);
            Shell.SetTabBarIsVisible(this, false);

            await trackInformation.TranslateTo(this.Width * -1, 0, 0);

            trackInformation.IsVisible = true;
            await trackInformation.TranslateTo(0, 0);

            await infoTask;
        }

        private async Task CloseTrackInformationAsync()
        {
            ViewModel.SelectedTrack = null;

            Shell.SetTabBarIsVisible(this, true);
            Shell.SetNavBarIsVisible(this, true);
            await this.trackInformation.TranslateTo(this.Width * -1, 0);
            this.trackInformation.IsVisible = false;
        }

        async private void OnGroupByButtonClick(object sender, System.EventArgs e)
        {
            var groups = ViewModel.GroupByOptions.Select(x => x.Name).ToArray();

            string toGroup = await DisplayActionSheet(AppResources.GroupByHeader, AppResources.Cancel, null, groups);

            if (!string.IsNullOrEmpty(toGroup) && toGroup != AppResources.Cancel)
            {
                GroupByButton.Text = $"{Translator.Instance["GroupByHeader"]} {toGroup}";

                var groupBy = ViewModel.GroupByOptions.SingleOrDefault(x => x.Name == toGroup);
                if (groupBy != null)
                {
                    ViewModel.GroupBy = groupBy;
                    ViewModel.ReSortGroup();
                }
            }
        }

        async private void OnSortByButtonClick(object sender, System.EventArgs e)
        {
            var sortings = ViewModel.SortByOptions.Select(x => x.Name).ToArray();

            string toSort = await DisplayActionSheet(AppResources.SortByHeader, AppResources.Cancel, null, sortings);

            if (!string.IsNullOrEmpty(toSort) && toSort != AppResources.Cancel)
            {
                SortByButton.Text = $"{Translator.Instance["SortByHeader"]} {toSort}";

                var sortBy = ViewModel.SortByOptions.SingleOrDefault(x => x.Name == toSort);
                if (sortBy != null)
                {
                    ViewModel.SortBy = sortBy;
                    ViewModel.ReSortGroup();
                }
            }
        }

        private void SetTitlesForButton()
        {
            GroupByButton.Text = $"{Translator.Instance["GroupByHeader"]} {ViewModel.GroupBy.Name}";
            SortByButton.Text = $"{Translator.Instance["SortByHeader"]} {ViewModel.SortBy.Name}";
        }

        private void ShowSortGroupLayout(object sender, EventArgs e)
        {
            GroupSortLayout.IsVisible = !GroupSortLayout.IsVisible;
        }
    }
}
