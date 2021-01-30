using UnityEngine;

[CreateAssetMenu(menuName = "Databases/Text and Sound")]
public class TextWithSound : ScriptableObject
{
	public bool fadeout = false;
	public bool fadeBackIn = true;
	public bool blockInteraction;
	public float waitTime;
	public GameProgress id;
	public string text;
	public string text2;
	public string text3;
	public AudioClip voiceOver;
	public AudioClip voiceOver2;
	public AudioClip voiceOver3;
	public bool customFootsteps = false;
	public AudioClip[] footsteps;
	public float _timeScaleFactor = 1f;
	public GameObject _interactionModel;
}
