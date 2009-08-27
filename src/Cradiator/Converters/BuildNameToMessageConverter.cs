using System;
using System.Globalization;
using System.Windows.Data;

namespace Cradiator.Converters
{
	[ValueConversion(typeof(string), typeof(string))]
	public class BuildNameToMessageConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			var projectName = values[0].ToString().Replace("_", " ");
			var message = values[1].ToString();
			
			return message.Trim().Length == 0 ? 
				projectName : string.Format("{0}\n{1}", projectName, message);
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
