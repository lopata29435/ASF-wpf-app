using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
	/// <summary>
	/// Логика взаимодействия для SettingsNamePage.xaml
	/// </summary>
	public partial class SettingsNamePage : Page
	{
		string token = "aad17cda65915d890d27ba38c9129ca4926944cbda6644ad6d883f2088c72e47";
		public SettingsNamePage()
		{
			InitializeComponent();
		}
		private async void SaveFio_Click(object sender, RoutedEventArgs e)
		{
			string fio = Input.Text;


			HttpClient client = new HttpClient();

			client.DefaultRequestHeaders.Add("Authorization", $"Token {token}");
			var data = new StringContent($"{{\"name\": \"{fio}\"}}", Encoding.UTF8, "application/json");
			var request = new HttpRequestMessage(new HttpMethod("PATCH"), $"{Values.Route.ENDPOINT}settings/changefio/")
			{
				Content = data
			};
			try
			{
				HttpResponseMessage response = await client.SendAsync(request);

				if (response.IsSuccessStatusCode)
				{
					NavigationService.Navigate(new SettingsPage());
				}
				else
				{
					Save_Button.IsEnabled = false;
					Input.Style = (Style)FindResource("TextBox.Rectangle.small-error");
					ErrorText.Visibility = Visibility.Visible;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"An error occurred: {ex.Message}");
			}
		}
		private void BackToMain_Click(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new SettingsPage());
		}

		private void Input_TextChanged(object sender, TextChangedEventArgs e)
		{
			Save_Button.IsEnabled = Input.Text != "";
			if (Input.Style == (Style)FindResource("TextBox.Rectangle.small-error"))
			{
				Input.Style = (Style)FindResource("TextBox.Rectangle.small");
				ErrorText.Visibility = Visibility.Hidden;
			}
		}
	}
}
