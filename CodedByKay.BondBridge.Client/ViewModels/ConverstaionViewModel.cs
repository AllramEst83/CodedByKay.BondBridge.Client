using CodedByKay.BondBridge.Client.DataTemplates;
using CodedByKay.BondBridge.Client.MessageEvents;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;

namespace CodedByKay.BondBridge.Client.ViewModels
{
    public partial class ConverstaionViewModel : BaseViewModel
    {
        [ObservableProperty]
        string converstaionName;
        [ObservableProperty]
        ObservableCollection<Message> messages = new ObservableCollection<Message>();
        [ObservableProperty]
        string textInput;
        public ConverstaionViewModel()
        {
            converstaionName = "Morris Wiberg";
            Messages.Add(new Message() { Text = "Hej pappa!", IsCurrentUser = false });
            Messages.Add(new Message() { Text = "Hej Morris!", IsCurrentUser = true });
            Messages.Add(new Message() { Text = "Hej pappa!", IsCurrentUser = false });
            Messages.Add(new Message() { Text = "Hej Morris!", IsCurrentUser = true });
            Messages.Add(new Message() { Text = "Hej pappa!", IsCurrentUser = false });
            Messages.Add(new Message() { Text = "Hej Morris!", IsCurrentUser = true });
            Messages.Add(new Message() { Text = "Hej pappa!", IsCurrentUser = false });
            Messages.Add(new Message() { Text = "Hej Morris!", IsCurrentUser = true });
            Messages.Add(new Message() { Text = "Hej pappa!", IsCurrentUser = false });
            Messages.Add(new Message() { Text = "Hej Morris!", IsCurrentUser = true });
            Messages.Add(new Message() { Text = "Hej pappa!", IsCurrentUser = false });
            Messages.Add(new Message() { Text = "Hej pappa!", IsCurrentUser = false });
            Messages.Add(new Message() { Text = "Hej pappa!", IsCurrentUser = false });

            Messages.Add(new Message() { Text = "Hej Morris!", IsCurrentUser = true });
            Messages.Add(new Message() { Text = "Hej Morris!", IsCurrentUser = true });
            Messages.Add(new Message() { Text = "Hej pappa!", IsCurrentUser = false });
            Messages.Add(new Message() { Text = "Hej pappa!", IsCurrentUser = false });
            Messages.Add(new Message() { Text = "Hej pappa!", IsCurrentUser = false });
            Messages.Add(new Message() { Text = "Hej Morris!", IsCurrentUser = true });
            Messages.Add(new Message() { Text = "Hej Morris!", IsCurrentUser = true });

            Messages.Add(new Message() { Text = "Hej Morris!", IsCurrentUser = true });
            Messages.Add(new Message() { Text = "Hej Morris!", IsCurrentUser = true });
            Messages.Add(new Message() { Text = "Hej Morris!", IsCurrentUser = true });
            Messages.Add(new Message() { Text = "Hej Morris!", IsCurrentUser = true });
        }

        private async Task ScrollToLastMessage()
        {
            await Task.Delay(100);
            MainThread.BeginInvokeOnMainThread(() =>
            {
                WeakReferenceMessenger.Default.Send(new ScrollMessageListoLastMessage());
            });
        }

        [RelayCommand]
        public async Task SendMessage()
        {
            Messages.Add(new Message() { Text = TextInput, IsCurrentUser = true });

            await ScrollToLastMessage();

            TextInput = string.Empty;
            await Toast.Make($"Message sent: {TextInput}", CommunityToolkit.Maui.Core.ToastDuration.Long).Show(CancellationToken.None);
        }
    }
}
