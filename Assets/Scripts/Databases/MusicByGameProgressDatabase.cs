using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Databases/MusicByGameProgressDatabase")]
public class MusicByGameProgressDatabase : ScriptableObject
{
	[SerializeField] private MusicByGameProgress[] music;

	public MusicByGameProgress GetMusicScriptable(GameProgress progress)
	{
		return music.Where(x => x.gameProgress == progress).FirstOrDefault();
	}

	public AudioClip GetMusic(GameProgress progress)
	{
		var m = music.Where(x => x.gameProgress == progress).FirstOrDefault().music;
		if (m != null)
		{
			return m;
		}
		else
		{
			return null;
		}
	}
}
