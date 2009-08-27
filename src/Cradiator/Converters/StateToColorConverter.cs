using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Cradiator.Model;

namespace Cradiator.Converters
{
	[ValueConversion(typeof(string), typeof(Color))]
	public class StateToColorConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			switch (value.ToString())
			{
				case ProjectStatus.SUCCESS: 
					return Colors.LimeGreen;

				case ProjectStatus.BUILDING:
					return Colors.Yellow;

				case ProjectStatus.FAILURE:
				case ProjectStatus.EXCEPTION:
					return Colors.Red;

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
