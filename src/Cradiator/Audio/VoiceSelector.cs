using System.Linq;

namespace Cradiator.Audio
{
	public class VoiceSelector
	{
		readonly ISpeechSynthesizer _speechSynth;

		public VoiceSelector(ISpeechSynthesizer speechSynth)
		{
			_speechSynth = speechSynth;
		}

		public void SelectInstalledVoice(string voiceName)
		{
			var matchingVoiceName = GetClosestMatchingInstalledVoice(voiceName).Name;
			_speechSynth.SelectVoice(matchingVoiceName);
		}

		public CradiatorInstalledVoice GetClosestMatchingInstalledVoice(string voiceName)
		{
			if (string.IsNullOrEmpty(voiceName)) return _speechSynth.SelectedVoice;

			var selectedVoices =
				from voice in _speechSynth.GetInstalledVoices()
				where voice.Name.ToLower().Contains(voiceName.ToLower())
				orderby voice.Name
				select voice;

			return selectedVoices.Any() ? selectedVoices.First() : _speechSynth.SelectedVoice;
		}
	}
}