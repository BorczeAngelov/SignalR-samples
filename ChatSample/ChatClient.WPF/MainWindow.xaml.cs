using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace ChatClient.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private readonly HubConnection _connection;
        private bool _isConnected;
        private string _userMessage;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            _connection = new HubConnectionBuilder()
               .WithUrl("https://localhost:44340/chat")
               .Build();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<string> Messages { get; } = new ObservableCollection<string>();
        public string UserName { get; set; } = "WPF user";

        public string UserMessage
        {
            get => _userMessage;
            set
            {
                if (_userMessage != value)
                {
                    _userMessage = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UserMessage)));
                }
            }
        }

        public bool IsConnected
        {
            get => _isConnected;
            private set
            {
                if (_isConnected != value)
                {
                    _isConnected = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsConnected)));
                }
            }
        }


        private async void ConnectWithServer(object sender, RoutedEventArgs e)
        {
            _connection.On<string, string>("broadcastMessage", (user, message) =>
            {
                this.Dispatcher.Invoke(() =>
                {
                    var newMessage = $"{user}: {message}";
                    Messages.Add(newMessage);
                });
            });

            try
            {
                await _connection.StartAsync();
                Messages.Add("Connection started");
                IsConnected = true;
            }
            catch (Exception ex)
            {
                Messages.Add(ex.Message);
            }
        }

        private async void SendMessage(object sender, RoutedEventArgs e)
        {
            try
            {
                await _connection.InvokeAsync("Send",
                    UserName, UserMessage);

                UserMessage = string.Empty;
            }
            catch (Exception ex)
            {
                Messages.Add(ex.Message);
            }
        }
    }
}
