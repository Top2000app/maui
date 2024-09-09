﻿

namespace Top2000MauiApp.Searching
{
    public class TracksSearchHandler : SearchHandler
    {
        private ViewModel ViewModel => (ViewModel)BindingContext;

        async protected override void OnQueryChanged(string oldValue, string newValue)
        {
            if (ViewModel != null && newValue != null && newValue != oldValue)
            {
                ViewModel.QueryText = newValue;
                await ViewModel.ExceuteSearchAsync();
            }
        }
    }
}