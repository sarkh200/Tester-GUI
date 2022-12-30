using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Annotations;
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
		Dictionary<string, string> shuffleTerms = new Dictionary<string, string>();
		Uri WindowPng = new Uri("/Resources/Images/Window.png", UriKind.Relative);
		Uri MaximizePng = new Uri("/Resources/Images/Maximize.png", UriKind.Relative);
		Random r = new Random();
		string pathToCSV = "No user input";
		private int answerNumeber;
		int questionNumber = 0;
		int answerSelected = 0;
		int correct = 0;

		private Dictionary<string, string> Shuffle_Dictionary(Dictionary<string, string> dictionary)
		{
			Dictionary<string, string> shuffledDictionary = dictionary.OrderBy(x => r.Next()).ToDictionary(k => k.Key, k => k.Value);
			return shuffledDictionary;
		}

		private void Test()
		{
			Main_Menu_Grid.Visibility = Visibility.Collapsed;
			Test_View.Visibility = Visibility.Visible;
			Test_Question.Text = $"{questionNumber + 1}:{shuffleTerms.Keys.ToList() [questionNumber]}";
			answerNumeber = r.Next(1, 4);
			string [] questions = new string [3];
			for (int i = 0; i < 3; i++)
			{
				string question;
				do
				{
					question = shuffleTerms.Values.ToList() [r.Next(0, shuffleTerms.Count)];
				} while (shuffleTerms.Values.ToList() [questionNumber].ToString() == question || questions.Contains(question));
				questions [i] = question;
			}

			switch (answerNumeber)
			{
				case 1:
					Test_Answer_1.Content = shuffleTerms.Values.ToList() [questionNumber];
					Test_Answer_2.Content = questions [0];
					Test_Answer_3.Content = questions [1];
					Test_Answer_4.Content = questions [2];
					break;
				case 2:
					Test_Answer_1.Content = questions [0];
					Test_Answer_2.Content = shuffleTerms.Values.ToList() [questionNumber];
					Test_Answer_3.Content = questions [1];
					Test_Answer_4.Content = questions [2];
					break;
				case 3:
					Test_Answer_1.Content = questions [0];
					Test_Answer_2.Content = questions [1];
					Test_Answer_3.Content = shuffleTerms.Values.ToList() [questionNumber];
					Test_Answer_4.Content = questions [2];
					break;
				case 4:
					Test_Answer_1.Content = questions [0];
					Test_Answer_2.Content = questions [1];
					Test_Answer_3.Content = questions [2];
					Test_Answer_4.Content = shuffleTerms.Values.ToList() [questionNumber];
					break;
				default:
					break;
			}
			if (questionNumber + 1 < shuffleTerms.Count)
			{
				questionNumber++;
			}
			else
			{
				End_Screen_Title.Text = $"You got a score of: {correct}/{shuffleTerms.Count}";
				Test_View.Visibility = Visibility.Collapsed;
				Test_End_Screen.Visibility = Visibility.Visible;
			}
		}

		public MainWindow()
		{
			InitializeComponent();
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
			if (pathToCSV != "No user input")
			{
				if (Test_Selection.IsChecked == true)
				{
					Test();
					Test_Score.Text = $" Score: \n {correct}/{shuffleTerms.Count}";
				}
				else if (Practice_Selection.IsChecked == true)
				{
					Main_Menu_Grid.Visibility = Visibility.Collapsed;
					Practice_View.Visibility = Visibility.Visible;
				}
				else if (View_Terms_Selection.IsChecked == true)
				{
					Main_Menu_Grid.Visibility = Visibility.Collapsed;
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
				terms.Remove(terms.Keys.First());
				Terms_Box.Text = string.Join(Environment.NewLine, terms).Replace("[", "").Replace("]", "").Replace(",", " :");
				shuffleTerms = Shuffle_Dictionary(terms);
			}
		}

		private void Done_Button_Click(object sender, RoutedEventArgs e)
		{
			Main_Menu_Grid.Visibility = Visibility.Visible;
			Term_View.Visibility = Visibility.Collapsed;
		}

		private void Test_Answer_1_Selection_Checked(object sender, RoutedEventArgs e)
		{
			answerSelected = 1;
			Test_Answer_2.IsChecked = false;
			Test_Answer_3.IsChecked = false;
			Test_Answer_4.IsChecked = false;
		}

		private void Test_Answer_2_Selection_Checked(object sender, RoutedEventArgs e)
		{
			answerSelected = 2;
			Test_Answer_1.IsChecked = false;
			Test_Answer_3.IsChecked = false;
			Test_Answer_4.IsChecked = false;
		}

		private void Test_Answer_3_Selection_Checked(object sender, RoutedEventArgs e)
		{
			answerSelected = 3;
			Test_Answer_1.IsChecked = false;
			Test_Answer_2.IsChecked = false;
			Test_Answer_4.IsChecked = false;
		}

		private void Test_Answer_4_Selection_Checked(object sender, RoutedEventArgs e)
		{
			answerSelected = 4;
			Test_Answer_1.IsChecked = false;
			Test_Answer_2.IsChecked = false;
			Test_Answer_3.IsChecked = false;
		}

		private void Test_Confirm_Button_Click(object sender, RoutedEventArgs e)
		{
			if (answerSelected == answerNumeber && answerSelected != 0)
			{
				correct++;
				Test_Score.Text = $" Score: \n {correct}/{shuffleTerms.Count}";
				Test();
			}
			else if(answerSelected != 0)
			{
				Test_Score.Text = $" Score: \n {correct}/{shuffleTerms.Count}";
				Test();
			}
		}

		private void End_Screen_Done_Click(object sender, RoutedEventArgs e)
		{
			Test_End_Screen.Visibility = Visibility.Collapsed;
			Main_Menu_Grid.Visibility = Visibility.Visible;
		}

		private void End_Screen_Restart_Click(object sender, RoutedEventArgs e)
		{
			questionNumber = 0;
			correct = 0;
			shuffleTerms = Shuffle_Dictionary(terms);
			Test_End_Screen.Visibility = Visibility.Collapsed;
			Test();
			Test_Score.Text = $" Score: \n {correct}/{shuffleTerms.Count}";
		}
	}
}
