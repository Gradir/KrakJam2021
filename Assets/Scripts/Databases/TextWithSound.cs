using UnityEngine;

[CreateAssetMenu(menuName = "Databases/Text and Sound")]
public class TextWithSound : ScriptableObject
{
	public bool fadeout = false;
	public bool blockInteraction;
	public float waitTime;
	public GameProgress id;
	public string text;
	public AudioClip voiceOver;
}
