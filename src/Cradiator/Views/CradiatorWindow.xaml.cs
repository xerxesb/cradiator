﻿using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Cradiator.Config;
using log4net;

namespace Cradiator.Views
{
	public partial class CradiatorWindow : ICradiatorView, IConfigObserver
	{
		static readonly ILog _log = LogManager.GetLogger(typeof(CradiatorWindow).Name);

		int _pollFrequency;
		bool _isShowProgressConfigured;	// named to avoid confusion with property on this view with same name

		public CradiatorWindow(IConfigSettings configSettings)
		{
			try
			{
				InitializeComponent();
			}
			catch (Exception exception)
			{
				_log.Error(exception);	// usually xaml issues 
				throw;
			}

			_pollFrequency = configSettings.PollFrequency;
			_isShowProgressConfigured = configSettings.ShowProgress;
			configSettings.AddObserver(this);
		}

		public CradiatorPresenter Presenter { get; set; }

		bool ICradiatorView.ShowProgress
		{
			set
			{
				Invoke(() => progressBar.Visibility =
								value && _isShowProgressConfigured ? 
									Visibility.Visible : Visibility.Hidden);
			}
		}

		object ICradiatorView.DataContext
		{
			set { Invoke(() => DataContext = value); }
		}

		void ICradiatorView.AddWindowBinding(InputBinding binding)
		{
			InputBindings.Add(binding);
		}

		void ICradiatorView.AddSettingsLinkBinding(InputBinding binding)
		{
			settingsLink.InputBindings.Add(binding);
		}

		void ICradiatorView.UpdateScreen()
		{
			Presenter.UpdateScreen();
		}

		Window ICradiatorView.Window
		{
			get { return this; }
		}

		void ICradiatorView.ShowMessage(string message)
		{
			// TODO add the dependency on IConfigSettings can move to the MessageWindow
			var messageWindow = new MessageWindow(_pollFrequency, message);
			messageWindow.Show();
			messageWindow.Focus();
		}

		void ICradiatorView.UpdateCountdownTimer(TimeSpan timeRemaining)
        {
        	countdownText.Text = string.Format("{0:00}:{1:00}",
											  timeRemaining.Minutes,
											  timeRemaining.Seconds);
        }

		public void Invoke(Action action)
		{
			Dispatcher.Invoke(DispatcherPriority.Normal, action);
		}

		void ICradiatorView.ShowCountdown(bool show)
		{
			countdownText.Visibility = show ? Visibility.Visible : Visibility.Hidden;
		}

		public void ConfigUpdated(ConfigSettings newSettings)
		{
			_isShowProgressConfigured = newSettings.ShowProgress;
			_pollFrequency = newSettings.PollFrequency;
		}
	}
}