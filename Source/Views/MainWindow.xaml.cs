using Microsoft.Maps.MapControl.WPF;
using System;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Threading;

namespace Source.Views;

public partial class MainWindow : Window, INotifyPropertyChanged
{
	private BakuBus? _bakuBus;
	public BakuBus? BakuBus
	{
		get { return _bakuBus; }
		set
		{
			_bakuBus = value;
			NotifyPropertyChanged(nameof(BakuBus));
		}
	}

	public MainWindow()
	{
		InitializeComponent();
		DataContext = this;
		string bingMapKey = "VjUUjuRrgz7UAqN9d4W6~apN7MMFQlYzNqoepweIh1g~AtILO5mMbs-QjG16NrrQ4WDsuI7U8aeUtWoDyQwQ2AJfB1mFr6NDBM9mlN9U6mlr";
		m.CredentialsProvider = new ApplicationIdCredentialsProvider(bingMapKey);
		DispatcherTimer timer = new();
		timer.Interval = new TimeSpan(5000);
		timer.Tick += Timer_Tick;
		timer.Start();
	}

	private void Timer_Tick(object? sender, EventArgs e)
	{
		// txt.Text = DateTime.Now.ToLongTimeString();
	}

	private async void Window_Loaded(object sender, RoutedEventArgs e)
	{
		var fileName = "BakuBus.json";
		var dir = new DirectoryInfo("../../../");
		var path = Path.Combine(dir.FullName, fileName);
		var jsonStr = await File.ReadAllTextAsync(path);

		BakuBus = JsonSerializer.Deserialize<BakuBus>(jsonStr);
		cmbox.Items.Add("None");

		for (int i = 0; i < BakuBus?.Buses.ToArray().Length; i++)
		{
			cmbox.Items.Add(BakuBus?.Buses[i].Attributes.DISPLAY_ROUTE_CODE);
			Pushpin pin = new Pushpin();
			double.TryParse(BakuBus?.Buses[i].Attributes.LONGITUDE.Substring(0, 5).Replace(',', '.'), out double longitude);
			double.TryParse(BakuBus?.Buses[i].Attributes.LATITUDE.Substring(0, 5).Replace(',', '.'), out double latitude);
			pin.Location = new Location(longitude, latitude);
			m.Children.Add(pin);
		}
	}

	public event PropertyChangedEventHandler? PropertyChanged;
	void NotifyPropertyChanged(string propertyName)
		=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}