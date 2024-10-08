﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
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
	public class DeliveryBlock
	{
		[JsonProperty("responsible")]
		public string Responsible { get; set; }

		[JsonProperty("russian_project_name")]
		public string RussianProjectName { get; set; }

		[JsonProperty("status")]
		public string Status { get; set; }

		[JsonProperty("color")]
		public string Color { get; set; }

		[JsonProperty("date_Of_start")]
		public string DateOfStart { get; set; }

		[JsonProperty("date_Of_end")]
		public string DateOfEnd { get; set; }

		[JsonProperty("specification_number")]
		public int SpecificationNumber { get; set; }

		[JsonProperty("percentage_Of_completion")]
		public int PercentageOfCompletion { get; set; }
	}
	public partial class DeliverySchedulePage : Page
	{
		const string token = "475b0031b70c2eb3bc7824d2bccafc7b0ecca9cbfffe258bb1a2c83d7b9d7703";
		Dictionary<string, string> colorToStyleMap = new Dictionary<string, string>
		{
			{ "green", "TextBox.Rectangle.Graph.Complete" }
		  , { "white", "TextBox.Rectangle.Graph.Planned" }
		  , { "yellow", "TextBox.Rectangle.Graph.StartingSoon" }
		  , { "blue", "TextBox.Rectangle.Graph.InProgress" }
		  , { "red ", "TextBox.Rectangle.Graph.Late" }
		};
		public DeliverySchedulePage()
		{
			InitializeComponent();
			Loaded += DeliverySchedulePage_Loaded;
		}
		private async void DeliverySchedulePage_Loaded(object sender, RoutedEventArgs e)
		{
			await LoadDeliveryScheduleAsync();
		}
		private async Task LoadDeliveryScheduleAsync()
		{
			using (HttpClient client = new HttpClient())
			{
				string ApiUrl = $"{Values.Route.ENDPOINT}deliveryschedule/";
				try
				{
					var request = new HttpRequestMessage(HttpMethod.Get, ApiUrl);
					request.Headers.Add("Authorization", $"Token {token}");
					var response = await client.SendAsync(request);

					if (response.IsSuccessStatusCode)
					{
						var json = await response.Content.ReadAsStringAsync();
						var blocks = JsonConvert.DeserializeObject<List<DeliveryBlock>>(json);
						//SuccesDeliveryLoad(blocks);
					}
					else
					{
						ErrorDeliveryLoad();
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show($"An error occurred: {ex.Message}");
				}
			}
		}

		private Thickness StartPositionCalculate(DateTime date) 
		{
			int currentYear = DateTime.Now.Year;
			int currentMonth = DateTime.Now.Month;
			int year = date.Year;
			int month = date.Month;
			int day = date.Day;
			int startPosition = 20 * (day - 1);
			for (int i = currentMonth; i < month; i++)
			{
				startPosition += DateTime.DaysInMonth(year, i) * 20;
			}
			for (int i = currentYear; i < year; i++)
			{
				startPosition += DateTime.IsLeapYear(i) ? 366 * 20 : 365 * 20;
			}
			return new Thickness(startPosition, 0, 0, 0);
		}
		private int WidthPositionCalculate(DateTime start, DateTime end)
		{
			int syear = start.Year;
			int smonth = start.Month;
			int sday = start.Day;
			int eyear = end.Year;
			int emonth = end.Month;
			int eday = end.Day;
			int width = 20 * (eday - sday + 1);
			//TODO: подумать над адекватной реализацией этой темы
			for (int i = smonth; i < emonth; i++)
			{
				width += DateTime.DaysInMonth(syear, i) * 20;
			}
			for (int i = syear; i < eyear; i++)
			{
				width += DateTime.IsLeapYear(i) ? 366 * 20 : 365 * 20;
			}
			return width;
		}
		private void SuccesDeliveryLoad(List<DeliveryBlock> blocks)
		{
			if (blocks.Count == 0)
				return;

			DateTime minDate = DateTime.MaxValue;
			DateTime maxDate = DateTime.MinValue;


			foreach (DeliveryBlock block in blocks)
			{
				DateTime startDate = DateTime.Parse(block.DateOfStart);
				DateTime endDate = DateTime.Parse(block.DateOfEnd);

				if (startDate < minDate)
					minDate = startDate;

				if (endDate > maxDate)
					maxDate = endDate;

				TextBox textBox = new TextBox();
				textBox.Style = (Style)FindResource(colorToStyleMap[block.Color]);
				textBox.Margin = StartPositionCalculate(DateTime.Parse(block.DateOfStart));
				textBox.Width = WidthPositionCalculate(DateTime.Parse(block.DateOfStart), DateTime.Parse(block.DateOfEnd));
				textBox.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;

				RowDefinition newRow = new RowDefinition();
				newRow.Height = new GridLength(1, GridUnitType.Auto);
				Deliveries.RowDefinitions.Add(newRow);

				Deliveries.Children.Add(textBox);
				Grid.SetRow(textBox, Deliveries.RowDefinitions.Count - 1);
			}

			//CreateTimeLine(minDate, maxDate);
		}
		private void CreateTimeLine(DateTime minDate, DateTime maxDate)
		{
			TimeSpan totalDays = maxDate - minDate;
			int totalMonths = (maxDate.Year - minDate.Year) * 12 + maxDate.Month - minDate.Month + 1;

			for (int i = 0; i < totalMonths; i++)
			{
				DateTime monthDate = minDate.AddMonths(i);
				int daysInMonth = DateTime.DaysInMonth(monthDate.Year, monthDate.Month);

				Grid monthGrid = new Grid();
				monthGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(40) });
				monthGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(40) });

				TimeLine.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(daysInMonth * 20) });
				Grid.SetColumn(monthGrid, i);
				TimeLine.Children.Add(monthGrid);

				TextBlock monthLabel = new TextBlock();
				monthLabel.Style = (Style)FindResource("LabelStyle");
				monthLabel.VerticalAlignment = VerticalAlignment.Top;
				monthLabel.HorizontalAlignment = HorizontalAlignment.Left;
				monthLabel.Margin = new Thickness(0, 24, 0, 0);
				monthLabel.Text = monthDate.ToString("MMMM");

				Grid.SetRow(monthLabel, 0);
				monthGrid.Children.Add(monthLabel);

				StackPanel datesPanel = new StackPanel();
				datesPanel.Orientation = Orientation.Horizontal;
				datesPanel.HorizontalAlignment = HorizontalAlignment.Left;
				datesPanel.VerticalAlignment = VerticalAlignment.Center;

				for (int j = 1; j <= daysInMonth; j++)
				{
					TextBox dateTextBox = new TextBox();
					dateTextBox.Style = (Style)FindResource("TextBox.Circle.Date.Default");

					string dayString = j < 10 ? j.ToString().PadLeft(2, '0') : j.ToString();
					dateTextBox.Text = dayString;

					DateTime currentDate = new DateTime(monthDate.Year, monthDate.Month, j);
					if (currentDate.DayOfWeek == DayOfWeek.Saturday || currentDate.DayOfWeek == DayOfWeek.Sunday)
					{
						dateTextBox.Style = (Style)FindResource("TextBox.Circle.Date.Weekend");
					}
					if (currentDate.Date == DateTime.Today)
					{
						dateTextBox.Style = (Style)FindResource("TextBox.Circle.Date.CurrentDay");
					}

					datesPanel.Children.Add(dateTextBox);
				}

				Grid.SetRow(datesPanel, 1);
				monthGrid.Children.Add(datesPanel);
			}
		}
		private void ErrorDeliveryLoad()
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

	}
}
