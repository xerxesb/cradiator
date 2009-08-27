using System;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace Cradiator.Views
{
	public partial class MessageWindow : Window
	{
		const double PercentageOfPollFrequency = 1.2;

		public MessageWindow(int pollFrequency, string message)
		{
			InitializeComponent();

			var timer = new DispatcherTimer
			{
				Interval = TimeSpan.FromSeconds(pollFrequency / PercentageOfPollFrequency)
			};

			timer.Tick += Timer_Tick;
			timer.Start();

			Message.Text = message;
		}
	
		private void Timer_Tick(object sender, EventArgs e)
		{
			((DispatcherTimer)sender).Stop();
			CloseWithFade();
		}

		private void CloseWithFade()
		{
			Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
			{
				var story = (Storyboard) FindResource("FadeAway");
				story.Completed += FadeAway_Completed;
                BeginStoryboard(story);
			}));
		}

		private void FadeAway_Completed(object sender, EventArgs e)
		{
			Dispatcher.Invoke(DispatcherPriority.Normal, new Action(Close));
		}
	}
}