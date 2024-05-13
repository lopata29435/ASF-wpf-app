using Microsoft.Win32;
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
	/// <summary>
	/// Логика взаимодействия для NewRequestPage.xaml
	/// </summary>
	public partial class NewRequestPage : Page
	{
		string token = "aad17cda65915d890d27ba38c9129ca4926944cbda6644ad6d883f2088c72e47";
		OpenFileDialog openFileDialog1 = null;
		public NewRequestPage()
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

		private void Back_Click(object sender, RoutedEventArgs e)
		{
			NavigationService.Navigate(new RequestsPage());
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			openFileDialog1 = new OpenFileDialog();
			openFileDialog1.Title = "Выберите файл";
			openFileDialog1.Filter = "*.pdf;*.doc;*.docx|*.pdf;*.doc;*.docx"; // можно разделить

			bool? result = openFileDialog1.ShowDialog();
			((Button)sender).Content = openFileDialog1.FileName.Length == 0 ? "Выберете файл" : System.IO.Path.GetFileName(openFileDialog1.FileName);
			Save_Button.IsEnabled = IsSaveButtonActive();
		}
		private async void Save_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				using (var client = new HttpClient())
				using (var formData = new MultipartFormDataContent())
				using (var fileStream = File.OpenRead(openFileDialog1.FileName))
				{
					client.DefaultRequestHeaders.Add("Authorization", $"Token {token}");
					// Добавление параметров запроса
					formData.Add(new StreamContent(fileStream), "document", System.IO.Path.GetFileName(openFileDialog1.FileName));
					formData.Add(new StringContent(Request.Text), "request");
					formData.Add(new StringContent(Resolution.Text), "resolution");
					formData.Add(new StringContent(Number.Text), "specification_number");
					formData.Add(new StringContent(Addressee.Text), "addressee");

					// Отправка запроса
					var response = await client.PostAsync($"{Values.Route.ENDPOINT}request/createrequest/", formData);

					if (response.IsSuccessStatusCode)
					{
						MessageBox.Show("Документ успешно отправлен на сервер.");
					}
					else
					{
						MessageBox.Show(await response.Content.ReadAsStringAsync());
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"An error occurred: {ex.Message}");
			}
		}

		private bool IsSaveButtonActive()
		{
			if (Number.Text.Length == 0) return false;
			if (Addressee.Text.Length == 0) return false;
			if (Request.Text.Length == 0) return false;
			if (Resolution.Text.Length == 0) return false;
			if (openFileDialog1 == null || openFileDialog1.FileName.Length == 0) return false;
			return true;
		}

		private void Number_TextChanged(object sender, TextChangedEventArgs e)
		{
			Save_Button.IsEnabled = IsSaveButtonActive();
		}

		private void Addressee_TextChanged(object sender, TextChangedEventArgs e)
		{
			Save_Button.IsEnabled = IsSaveButtonActive();
		}

		private void Request_TextChanged(object sender, TextChangedEventArgs e)
		{
			Save_Button.IsEnabled = IsSaveButtonActive();
		}

		private void Resolution_TextChanged(object sender, TextChangedEventArgs e)
		{
			Save_Button.IsEnabled = IsSaveButtonActive();
		}
	}
}
