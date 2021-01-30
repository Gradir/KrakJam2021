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

	public AudioClip[] GetCustomFootsteps(GameProgress id)
	{
		var s = textWithSounds.Where(x => x.id == id).FirstOrDefault();
		if (s != null && s.customFootsteps)
		{
			return s.footsteps;
		}
		return null;
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

	public string GetText(GameProgress id, int interactionCount)
	{
		var s = textWithSounds.Where(x => x.id == id).FirstOrDefault();
		if (s != null)
		{
			if (interactionCount > 0)
			{
				if (interactionCount == 1)
				{
					if (s.text2 != string.Empty)
					{
						return s.text2;
					}
				}
				if (interactionCount > 1)
				{
					if (s.text3 != string.Empty)
					{
						return s.text3;
					}
				}
			}
			return s.text;
		}
		else
		{
			return null;
		}
	}

	public AudioClip GetVoiceOver(GameProgress id, int interactionCount)
	{
		var vo = textWithSounds.Where(x => x.id == id).FirstOrDefault();
		if (vo != null)
		{
			if (interactionCount > 0)
			{
				if (interactionCount == 1)
				{
					if (vo.voiceOver2 != null)
					{
						return vo.voiceOver2;
					}
				}
				if (interactionCount > 1)
				{
					if (vo.voiceOver3 != null)
					{
						return vo.voiceOver3;
					}
				}
			}
			return vo.voiceOver;
		}
		else
		{
			return null;
		}
	}
}
