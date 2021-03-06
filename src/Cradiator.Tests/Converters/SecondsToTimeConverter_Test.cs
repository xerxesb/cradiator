using System.Windows.Data;
using Cradiator.Converters;
using NUnit.Framework;

namespace Cradiator.Tests.Converters
{
	[TestFixture]
	public class SecondsToTimeConverter_Test
	{
		private readonly IValueConverter _converter = new SecondsToTimeConverter();

		[Test]
		public void _1_minute_shows_nonplural_minute()
		{
			var convertedText = (string) _converter.Convert(60d, null, null, null);
			Assert.That(convertedText, Is.EqualTo("Every 1 minute"));
		}

		[Test]
		public void _1m_15s_shows_nonplural_minute_and_plural_seconds()
		{
			var convertedText = (string) _converter.Convert(75d, null, null, null);
			Assert.That(convertedText, Is.EqualTo("Every 1 minute and 15 seconds"));
		}

		[Test]
		public void _1m_1s_shows_nonplural_minute_and_nonplural_second()
		{
			var convertedText = (string) _converter.Convert(61d, null, null, null);
			Assert.That(convertedText, Is.EqualTo("Every 1 minute and 1 second"));
		}

		[Test]
		public void _2_minutes_shows_plural_minutes()
		{
			var convertedText = (string) _converter.Convert(120d, null, null, null);
			Assert.That(convertedText, Is.EqualTo("Every 2 minutes"));
		}

		[Test]
		public void _5m_15s_shows_plural_minutes_and_plural_seconds()
		{
			var convertedText = (string) _converter.Convert(315d, null, null, null);
			Assert.That(convertedText, Is.EqualTo("Every 5 minutes and 15 seconds"));
		}

		[Test]
		public void hours_converts_to_minutes()
		{
			var convertedText = (string) _converter.Convert((3615d), null, null, null);
			Assert.That(convertedText, Is.EqualTo("Every 60 minutes and 15 seconds"));
		}

		[Test]
		public void shows_whole_seconds_for_fractional_double()
		{
			var convertedText = (string) _converter.Convert(15.333333466666, null, null, null);
			Assert.That(convertedText, Is.EqualTo("Every 15 seconds"));
		}

		[Test]
		public void shows_whole_seconds_for_nonfractional_double()
		{
			var convertedText = (string) _converter.Convert(15d, null, null, null);
			Assert.That(convertedText, Is.EqualTo("Every 15 seconds"));
		}

		[Test]
		public void convertback_should_return_null()
		{
			Assert.That(_converter.ConvertBack(true, null, null, null), Is.Null);
		}
	}
}