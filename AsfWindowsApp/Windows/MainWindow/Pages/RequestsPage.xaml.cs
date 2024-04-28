using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AsfWindowsApp.Windows.MainWindow.Pages
{
	public partial class RequestsPage : Page
	{
		public RequestsPage()
		{
			InitializeComponent(); 
		}
		private void Projects_Click(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new ProjectsPage());
		}
		private void Schedule_Click(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new DeliverySchedulePage());
		}
		private void Requests_Click(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new RequestsPage());
		}
		private void Settings_Click(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new SettingsPage());
		}

	}
}

