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
		string token = "2b34d859415eea9e3e7453f7306e3e71e9140487c371818c4ba0d753a8ab9210";
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

			HttpResponseMessage response = await client.SendAsync(request);

			//string responseBody = await response.Content.ReadAsStringAsync();

			if (response.IsSuccessStatusCode)
			{
				NavigationService.Navigate(new SettingsPage());
			} else
			{
				Save_Button.IsEnabled = false;
				Input.Style = (Style)FindResource("TextBox.Rectangle.small-error");
			}
		}
		private void BackToMain_Click(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new SettingsPage());
		}

		private void Input_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (Input.Style == (Style)FindResource("TextBox.Rectangle.small-error"))
			{

				Input.Style = (Style)FindResource("TextBox.Rectangle.small");
			}
			Save_Button.IsEnabled = Input.Text != null;
		}
	}
}
