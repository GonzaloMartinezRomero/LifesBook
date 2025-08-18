using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LifesBook.Backend.Application;
using LifesBook.Backend.Application.Service.HistoryManager.Abstract;
using LifesBook.Backend.Model;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace LifesBook.FrontEndWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _password;
        private History _currentHistory;
        private readonly IHistoryManager _historyManager = LoadHistoryManagerService();

        private static IHistoryManager LoadHistoryManagerService()
        {
            var servicesCollection = new ServiceCollection();

            servicesCollection.AddHistoryManagerApplication();

            var configBuilder = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfiguration configuration = configBuilder.Build();

            servicesCollection.AddSingleton<IConfiguration>(configuration);

            return servicesCollection.BuildServiceProvider().GetRequiredService<IHistoryManager>();
        }

        public MainWindow()
        {
            var loginWindow = new Login();
            loginWindow.ShowDialog();
            
            _password = loginWindow.Password;

            InitializeComponent();

            this.InputTextBox.Text = String.Empty;
            this.TitleLabel.Content = String.Empty; 

            LoadHistories();
        }

        private void LoadHistories()
        {
            try
            {
                this.HistoyListBox.Items.Clear();

                List<History> histories = _historyManager.LoadAllHistories(_password);

                foreach(History history in histories) 
                {
                    this.HistoyListBox.Items.Add(history);
                }
            }
            catch (Exception e)
            {
                new Notification(e.Message).ShowDialog();                
            }
        }

        private void HistoyListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = (sender as ListBox)?.SelectedItem;
            if (item is History selectedHistory)
            {
                History loadedHistory = _historyManager.LoadHistory(_password, selectedHistory.Id);
                RenderHistory(loadedHistory);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_currentHistory != null)
                {
                    _historyManager.UpdateHistory(_currentHistory.Id, _password, this.InputTextBox.Text);
                    new Notification("Saved successfully").ShowDialog();
                }   
                else
                    new Notification("Select history before").ShowDialog();
            }
            catch(Exception ex)
            {
                new Notification(ex.Message).ShowDialog();
            }
            
        }

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                History newHistory = _historyManager.SaveHistory(DateTime.Now, _password, String.Empty);
                LoadHistories();
                RenderHistory(newHistory);
            }
            catch(Exception ex)
            {
                new Notification(ex.Message).ShowDialog();
            }
        }

        private void RenderHistory(History history)
        {
            _currentHistory = history;
            this.InputTextBox.Text = history.Content;
            this.TitleLabel.Content = history.Date.ToString("dddd dd MMMM 'of' yyyy");
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();   
        }
    }
}