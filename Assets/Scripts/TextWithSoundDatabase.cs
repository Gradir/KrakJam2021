using System.Linq;
using UnityEngine;


public enum TextId
{
	Discretion,
	ToyCar,
	ProgressBackwards,
	HappyTimes,
	Girl,
	BrotherArgument,
	Accident,
	Transfusion,
	Death,
	PsychicBreakdown,
	SuicideAttempt,
	Tunnel,
	HesAwake
}

[CreateAssetMenu(menuName = "Databases/Text and Sound Database")]
public class TextWithSoundDatabase : ScriptableObject
{
	[SerializeField] private TextWithSound[] textWithSounds;

	public string GetText(TextId id)
	{
		var s = textWithSounds.Where(x => x.id == id).First().text;
		if (s != null)
		{
			return s;
		}
		else
		{
			return string.Empty;
		}
	}

	public AudioClip GetVoiceOver(TextId id)
	{
		var vo = textWithSounds.Where(x => x.id == id).First().voiceOver;
		if (vo != null)
		{
			return vo;
		}
		else
		{
			return null;
		}
	}
}
