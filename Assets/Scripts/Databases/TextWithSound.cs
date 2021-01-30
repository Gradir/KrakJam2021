using UnityEngine;

[CreateAssetMenu(menuName = "Databases/Text and Sound")]
public class TextWithSound : ScriptableObject
{
	public bool fadeout = false;
	public bool blockInteraction;
	public float waitTime;
	public GameProgress id;
	public string text;
	public string text2;
	public string text3;
	public AudioClip voiceOver;
	public float _timeScaleFactor = 1f;
}
