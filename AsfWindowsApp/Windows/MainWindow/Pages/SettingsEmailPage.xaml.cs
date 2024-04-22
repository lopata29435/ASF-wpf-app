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
	/// Логика взаимодействия для SettingsEmail.xaml
	/// </summary>
	public partial class SettingsEmailPage : Page
	{
		string token = "394f657232f931de37e3c26626b9ef57da0262f38a046f501bb2ddf6e7dbb192";
		public SettingsEmailPage()
		{
			InitializeComponent();
		}
		private async void SaveEmail_Click(object sender, RoutedEventArgs e)
		{
			string email = Input.Text;


			HttpClient client = new HttpClient();

			client.DefaultRequestHeaders.Add("Authorization", $"Token {token}");
			var data = new StringContent($"{{\"email\": \"{email}\"}}", Encoding.UTF8, "application/json");
			var request = new HttpRequestMessage(new HttpMethod("PATCH"), $"{Values.Route.ENDPOINT}settings/changeemail/")
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
			}
		}
	}
}
