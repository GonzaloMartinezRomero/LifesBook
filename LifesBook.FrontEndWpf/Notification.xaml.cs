using System.Windows;

namespace LifesBook.FrontEndWpf
{
    /// <summary>
    /// Interaction logic for Notification.xaml
    /// </summary>
    public partial class Notification : Window
    {
        public Notification()
        {
            InitializeComponent();         
        }

        public Notification(string message):this()
        {
            this.NotificationLabel.Content = message;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
