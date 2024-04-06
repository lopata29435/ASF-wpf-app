using System;

using System.Net.Http;

using System.Text;
using System.Windows;
using System.Windows.Controls;




namespace AsfWindowsApp.Windows.MainWindow.Pages
{
	/// <summary>
	/// Логика взаимодействия для TestPage.xaml
	/// </summary>
	public partial class TestPage : Page
	{
		string token = "2b34d859415eea9e3e7453f7306e3e71e9140487c371818c4ba0d753a8ab9210";
		public TestPage()
		{
			InitializeComponent();
		}

		private void nameEdit(object sender, RoutedEventArgs e)
		{
			gridMain.Visibility = Visibility.Collapsed;
			gridName.Visibility = Visibility.Visible;
        }

		private void BackFromNameToMane(object sender, RoutedEventArgs e)
		{
			gridName.Visibility = Visibility.Collapsed;
			Input.Text = string.Empty;
			gridMain.Visibility = Visibility.Visible;
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

			string responseBody = await response.Content.ReadAsStringAsync();

			test1.Text = response.StatusCode.ToString();
			test2.Text = responseBody;
			
		}
    }
}
