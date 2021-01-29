using UnityEngine;

[CreateAssetMenu(menuName = "Databases/MusicByGameProgress")]
public class MusicByGameProgress : ScriptableObject
{
	public GameProgress gameProgress;
	public AudioClip music;
}
