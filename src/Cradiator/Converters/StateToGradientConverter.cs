using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Cradiator.Model;

namespace Cradiator.Converters
{
	[ValueConversion(typeof(string), typeof(Color))]
	public class StateToGradientConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			switch (value.ToString())
			{
				case ProjectStatus.SUCCESS:
					return Colors.LightGreen;

				case ProjectStatus.BUILDING:
					return Color.FromArgb(255, 255, 255, 200);

				case ProjectStatus.FAILURE:
				case ProjectStatus.EXCEPTION:
					return Color.FromArgb(255, 255, 150, 150);

				default:
					return Colors.White;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}
	}
}
