using CodedByKay.BondBridge.Client.Interfaces;
using CodedByKay.BondBridge.Client.Pages;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace CodedByKay.BondBridge.Client.ViewModels
{

    public partial class LogViewModel : BaseViewModel
    {
        private readonly IBondBridgeDataService _bondBridgeDataService;

        [ObservableProperty]
        public ObservableCollection<Models.Response.LogResponse> logs = [];
        [ObservableProperty]
        public bool isExpanded;
        [ObservableProperty]
        public bool isRefreshing;
        public LogViewModel(IBondBridgeDataService bondBridgeDataService)
        {
            _bondBridgeDataService = bondBridgeDataService;

            GetLatestLogs();
        }
        private void GetLatestLogs()
        {
            Task.Run(async () =>
            {
                // Asynchronously fetch data without blocking the UI thread.
                var logs = await _bondBridgeDataService.GetData<List<Models.Response.LogResponse>>("/api/log/latest");

                // Once data is fetched, update the ObservableCollection on the UI thread.
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    foreach (var log in logs)
                    {
                        Logs.Add(log);
                    }
                });
            });
        }

        //Används inte just nu
        [RelayCommand]
        public async Task Refresh()
        {
            IsRefreshing = true;

            Logs.Clear();

            var logs = await _bondBridgeDataService.GetData<List<Models.Response.LogResponse>>("/api/Log/latest");
            foreach (var log in logs)
            {
                Logs.Add(log);
            }
            IsRefreshing = false;
        }

        [RelayCommand]
        public async Task ShowItem(Models.Response.LogResponse logResponse)
        {
            var logResponseJson = JsonSerializer.Serialize(logResponse);
            await Shell.Current.GoToAsync($"{nameof(LogDetailsPage)}?logResponseDetails={Uri.EscapeDataString(logResponseJson)}");
        }
    }
}
