using UnityEngine;

[CreateAssetMenu(menuName = "Databases/Text and Sound")]
public class TextWithSound : ScriptableObject
{
	public TextId id;
	public string text;
	public AudioClip voiceOver;
}
