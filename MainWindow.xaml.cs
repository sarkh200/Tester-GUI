using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Tester_dotnet
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow: Window
	{
		Dictionary<string, string> terms = new();
		Uri WindowPng = new Uri("/Resources/Images/Window.png", UriKind.Relative);
		Uri MaximizePng = new Uri("/Resources/Images/Maximize.png", UriKind.Relative);
		CornerRadius maximizedRad = new CornerRadius(0);
		CornerRadius windowedRad = new CornerRadius(8);
		string pathToCSV = "No user input";
		Dictionary<string, string> shuffleTerms = new Dictionary<string, string>();
		int questionNumber = 0;
		Random r = new Random();
		private Dictionary<string, string> Shuffle_Dictionary(Dictionary<string, string> dictionary)
		{
			Dictionary<string, string> shuffledDictionary = dictionary.OrderBy(x => r.Next()).ToDictionary(k => k.Key, k => k.Value);
			return shuffledDictionary;
		}

		private int RandIntArrayExclude(int [] exclude, int maxRange, int minRange = 0)
		{
			int randomInt = new();
			do
			{
				randomInt = r.Next(minRange, maxRange);
			} while (exclude.Contains(randomInt));

			return randomInt;
		}

		public MainWindow()
		{
			InitializeComponent();
			Border.CornerRadius = windowedRad;
		}

		private void TimerStart()
		{
			System.Timers.Timer timer = new System.Timers.Timer();
			timer.Elapsed += new ElapsedEventHandler(Timer_Tick);
			timer.Interval = 1000;
			timer.Start();
		}

		private void Timer_Tick(object? sender, EventArgs e)
		{

		}

		private void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			this.DragMove();
		}

		private void MinimizeButton_Click(object sender, RoutedEventArgs e)
		{
			WindowState = WindowState.Minimized;
		}

		private void ExitButton_Click(object sender, RoutedEventArgs e)
		{
			System.Windows.Application.Current.Shutdown();
		}
		
		private void Go_Button_Click(object sender, RoutedEventArgs e)
		{
			if(pathToCSV != "No user input")
			{
				if (Test_Selection.IsChecked == true)
				{
					Main_Menu_Grid.Visibility = Visibility.Hidden;
					Test_View.Visibility = Visibility.Visible;
					shuffleTerms = Shuffle_Dictionary(terms);
					Question.Text = shuffleTerms.Keys.ToList() [questionNumber];
					int answerNumeber = r.Next(1, 4);
					int [] questions = new int [] {RandIntArrayExclude(new int [] {1}, 4)};

					switch (answerNumeber)
					{
						case 1:
							Answer_1.Content = shuffleTerms.Values.ToList() [questionNumber];
							Answer_2.Content = shuffleTerms.Values.ToList() [questions[1]];
							break;
						case 2:
							Answer_2.Content = shuffleTerms.Values.ToList() [questionNumber];
							break;
						case 3:
							Answer_3.Content = shuffleTerms.Values.ToList() [questionNumber];
							break;
						case 4:
							Answer_4.Content = shuffleTerms.Values.ToList() [questionNumber];
							break;
						default:
							break;
					}

					Answer_1.Content = shuffleTerms.Values.ToList() [RandIntArrayExclude(new int [] { questionNumber }, shuffleTerms.Count())];
				}
				else if (Practice_Selection.IsChecked == true)
				{
					Main_Menu_Grid.Visibility = Visibility.Hidden;
					Practice_View.Visibility = Visibility.Visible;
				}
				else if (View_Terms_Selection.IsChecked == true)
				{
					Main_Menu_Grid.Visibility = Visibility.Hidden;
					Term_View.Visibility = Visibility.Visible;
				}
			}
		}

		private void Test_Selection_Checked(object sender, RoutedEventArgs e)
		{
			Practice_Selection.IsChecked = false;
			View_Terms_Selection.IsChecked = false;
		}

		private void Practice_Selection_Checked(object sender, RoutedEventArgs e)
		{
			Test_Selection.IsChecked = false;
			View_Terms_Selection.IsChecked = false;
		}

		private void View_Terms_Selection_Checked(object sender, RoutedEventArgs e)
		{
			Practice_Selection.IsChecked = false;
			Test_Selection.IsChecked = false;
		}
		private void Browse_Button_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Multiselect = false;
			openFileDialog.Filter = "CSV file (*.csv)|*.csv";
			if (openFileDialog.ShowDialog() == true)
			{
				Go_Button.Content = "Let's go";
				Go_Button.Width = 112;
				pathToCSV = System.IO.Path.GetFullPath(openFileDialog.FileName);
				Input_Label.Text = $"Path to csv: {pathToCSV}";
				terms = File.ReadAllLines(pathToCSV).Select(line => line.Split(",")).ToDictionary(line => line [0], line => line [1]);
				Terms_Box.Text = string.Join(Environment.NewLine, terms).Replace("[","").Replace("]","").Replace(","," :");
			}
		}

		private void Done_Button_Click(object sender, RoutedEventArgs e)
		{
			Main_Menu_Grid.Visibility = Visibility.Visible;
			Term_View.Visibility = Visibility.Hidden;
		}

		private void Answer_1_Selection_Checked(object sender, RoutedEventArgs e)
		{
			Answer_2.IsChecked = false;
			Answer_3.IsChecked = false;
			Answer_4.IsChecked = false;
		}

		private void Answer_2_Selection_Checked(object sender, RoutedEventArgs e)
		{
			Answer_1.IsChecked = false;
			Answer_3.IsChecked = false;
			Answer_4.IsChecked = false;
		}

		private void Answer_3_Selection_Checked(object sender, RoutedEventArgs e)
		{
			Answer_1.IsChecked = false;
			Answer_2.IsChecked = false;
			Answer_4.IsChecked = false;
		}

		private void Answer_4_Selection_Checked(object sender, RoutedEventArgs e)
		{
			Answer_1.IsChecked = false;
			Answer_2.IsChecked = false;
			Answer_3.IsChecked = false;
		}

		private void Confirm_Button_Click(object sender, RoutedEventArgs e)
		{

		}
	}
}
