using System;
using System.Globalization;
using System.Windows.Data;

namespace Cradiator.Converters
{
	[ValueConversion(typeof(string), typeof(int))]
	public class ImageSizeConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var currentMessage = value as string;
			return string.IsNullOrEmpty(currentMessage) ? 0 : 15;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}