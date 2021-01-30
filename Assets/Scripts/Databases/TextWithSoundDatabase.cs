using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Databases/Text and Sound Database")]
public class TextWithSoundDatabase : ScriptableObject
{
	[SerializeField] private TextWithSound[] textWithSounds;

	public float GetTimeScale(GameProgress id)
	{
		var s = textWithSounds.Where(x => x.id == id).FirstOrDefault();
		if (s != null)
		{
			return s._timeScaleFactor;
		}
		return 1f;
	}

	public bool IsInteractionBlocked(GameProgress id)
	{
		var s = textWithSounds.Where(x => x.id == id).FirstOrDefault();
		if (s != null)
		{
			return s.blockInteraction;
		}
		return false;
	}

	public bool DoesFadeOut(GameProgress id)
	{
		var s = textWithSounds.Where(x => x.id == id).FirstOrDefault();
		if (s != null)
		{
			return s.fadeout;
		}
		return false;
	}

	public float GetWaitTime(GameProgress id)
	{
		var s = textWithSounds.Where(x => x.id == id).FirstOrDefault();
		if (s != null)
		{
			return s.waitTime;
		}
		return 0;
	}

	public string GetText(GameProgress id)
	{
		var s = textWithSounds.Where(x => x.id == id).FirstOrDefault();
		if (s != null)
		{
			return s.text;
		}
		else
		{
			return null;
		}
	}

	public AudioClip GetVoiceOver(GameProgress id)
	{
		var vo = textWithSounds.Where(x => x.id == id).FirstOrDefault();
		if (vo != null)
		{
			return vo.voiceOver;
		}
		else
		{
			return null;
		}
	}
}
