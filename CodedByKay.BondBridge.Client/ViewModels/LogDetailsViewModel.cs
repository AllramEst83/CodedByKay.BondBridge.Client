using CommunityToolkit.Mvvm.ComponentModel;

namespace CodedByKay.BondBridge.Client.ViewModels
{
    [QueryProperty(nameof(LogResponseDetails), "logResponseDetails")]
    public partial class LogDetailsViewModel : BaseViewModel
    {
        [ObservableProperty]
        private Models.Response.LogResponse recivedLogResponse = new();
        public string LogResponseDetails
        {
            set
            {
                var deserializedObject = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.Response.LogResponse>(Uri.UnescapeDataString(value));
                RecivedLogResponse = deserializedObject ?? new Models.Response.LogResponse();
            }
        }

        public LogDetailsViewModel() { }
    }
}
