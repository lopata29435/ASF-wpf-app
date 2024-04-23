using System;

using System.Net.Http;

using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Navigation;




namespace AsfWindowsApp.Windows.MainWindow.Pages
{
	/// <summary>
	/// Логика взаимодействия для TestPage.xaml
	/// </summary>
	public partial class SettingsPage : Page
	{
		string token = "2b34d859415eea9e3e7453f7306e3e71e9140487c371818c4ba0d753a8ab9210";
		public SettingsPage()
		{
			Loaded += YourPage_Loaded;
			InitializeComponent();
		}

		private async void YourPage_Loaded(object sender, RoutedEventArgs e)
		{
			string[] about = await GetAbout();
			EmailText.Text = about[0];
			FioText.Text = about[1];
			RoleText.Text = about[2];
		}
		private async Task<string[]> GetAbout()
		{
			string[] output = new string[3];
			HttpClient client = new HttpClient();
			client.DefaultRequestHeaders.Add("Authorization", $"Token {token}");
			var response = await client.GetAsync($"{Values.Route.ENDPOINT}settings/about/");
			try
			{
				if (response.IsSuccessStatusCode)
				{
					var responseBody = await response.Content.ReadAsStringAsync();
					string pattern = @"""email"":\s*""(.*?)"".*?""name"":\s*""(.*?)"".*?""role"":\s*(.*?)";
					Match match = Regex.Match(responseBody, pattern, RegexOptions.Singleline);

					if (match.Success)
					{
						output[0] = match.Groups[1].Value;
						output[1] = match.Groups[2].Value;
						output[2] = match.Groups[3].Value;
					}
					else
					{
						MessageBox.Show($"A server error occurred 1");
					}
				}
				else
				{
					MessageBox.Show($"A server error occurred 2");
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"An error occurred: {ex.Message}");
			}
			return output;
			
		}
		private void NameEdit_Click(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new SettingsNamePage());
		}

		private void EmailEdit_Click(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new SettingsEmailPage());
		}

		private void PasswordEdit_Click(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new SettingsPasswordPage());
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
