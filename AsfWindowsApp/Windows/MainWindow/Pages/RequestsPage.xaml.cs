using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
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
	public class RequestBlock
	{
		[JsonProperty("document")]
		public string Document { get; set; }

		[JsonProperty("request")]
		public string Request { get; set; }

		[JsonProperty("resolution")]
		public string Resolution { get; set; }

		[JsonProperty("creator")]
		public int Creator { get; set; }

		[JsonProperty("specification_number")]
		public int SpecificationNumber { get; set; }

		[JsonProperty("addressee")]
		public int Addressee { get; set; }
	}
	public partial class RequestsPage : Page
	{
		const string token = "aad17cda65915d890d27ba38c9129ca4926944cbda6644ad6d883f2088c72e47";
		public RequestsPage()
		{
			InitializeComponent();
			Loaded += RequestsPage_Loaded;
		}
		private async void RequestsPage_Loaded(object sender, RoutedEventArgs e)
		{
			await LoadRequestsAsync(); // убрать тест
		}
		private async Task LoadRequestsAsync()
		{
			using (HttpClient client = new HttpClient())
			{
				string ApiUrl = $"{Values.Route.ENDPOINT}request/allrequests/";
				try
				{
					var request = new HttpRequestMessage(HttpMethod.Get, ApiUrl);
					request.Headers.Add("Authorization", $"Token {token}");
					var response = await client.SendAsync(request);

					if (response.IsSuccessStatusCode)
					{
						var json = await response.Content.ReadAsStringAsync();
						var blocks = JsonConvert.DeserializeObject<List<RequestBlock>>(json);
						SuccesRequestsLoad(blocks);
					}
					else
					{
						ErrorRequestsLoad();
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show($"An error occurred: {ex.Message}");
				}
			}
		}
		private async Task LoadRequestsAsyncTest()
		{
			var json = File.ReadAllText("test.txt");
			var blocks = JsonConvert.DeserializeObject<List<RequestBlock>>(json);
			SuccesRequestsLoad(blocks);
		}
		private void SuccesRequestsLoad(List<RequestBlock> blocks)
		{
			int i = 1;
			foreach (RequestBlock block in blocks)
			{
				Border border = new Border();
				mainStackPanel.Children.Add(border);
				border.Height = 257;
				border.CornerRadius = new CornerRadius(16);
				border.BorderThickness = new Thickness(1);
				border.BorderBrush = (SolidColorBrush)FindResource("Color.Black-200");
				border.Margin = new Thickness(0, 8, 0, 0);
				border.Background = (SolidColorBrush)FindResource("Color.White");

				StackPanel stackPanel = new StackPanel();
				border.Child = stackPanel;
				stackPanel.Margin = new Thickness(24, 16, 24, 16);

				TextBlock textBlock = new TextBlock();
				stackPanel.Children.Add(textBlock);
				textBlock.Text = $"Запрос №{i}";
				textBlock.Style = (Style)FindResource("H3Style");

				textBlock = new TextBlock();
				stackPanel.Children.Add(textBlock);
				textBlock.Text = $"Создатель: {block.Creator}";
				textBlock.Style = (Style)FindResource("InpLabelStyle");

				textBlock = new TextBlock();
				stackPanel.Children.Add(textBlock);
				textBlock.Text = $"Адресат: {block.Addressee}";
				textBlock.Style = (Style)FindResource("InpLabelStyle");

				textBlock = new TextBlock();
				stackPanel.Children.Add(textBlock);
				textBlock.Text = $"Документ: {block.Document}";
				textBlock.Style = (Style)FindResource("InpLabelStyle");

				textBlock = new TextBlock();
				stackPanel.Children.Add(textBlock);
				textBlock.Text = "Текст запроса:";
				textBlock.Style = (Style)FindResource("InpLabelStyle");
				textBlock.FontWeight = FontWeights.Bold;
				textBlock.Margin = new Thickness(0, 16, 0, 0);

				textBlock = new TextBlock();
				stackPanel.Children.Add(textBlock);
				textBlock.Text = block.Request;
				textBlock.Style = (Style)FindResource("InpLabelStyle");

				textBlock = new TextBlock();
				stackPanel.Children.Add(textBlock);
				textBlock.Text = "Резолюция:";
				textBlock.Style = (Style)FindResource("InpLabelStyle");
				textBlock.FontWeight = FontWeights.Bold;
				textBlock.Margin = new Thickness(0, 16, 0, 0);

				textBlock = new TextBlock();
				stackPanel.Children.Add(textBlock);
				textBlock.Text = block.Resolution;
				textBlock.Style = (Style)FindResource("InpLabelStyle");

				i++;
			}
		}
		private void ErrorRequestsLoad()
		{
			MessageBox.Show("Dupa:");
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

		private void NewRequest_Click(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new NewRequestPage());
		}
	}
}

