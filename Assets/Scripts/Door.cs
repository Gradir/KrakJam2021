using UnityEngine;

public class Door : MonoBehaviour
{
	public Transform _teleportTo;
	public bool _progressesStory = false;
	public Interactible[] _interactiblesToActivate;

	public void OnPassedThrough()
	{
		if (_progressesStory)
		{
			Signals.Get<StoryShouldProgress>().Dispatch();
			foreach (var i in _interactiblesToActivate)
			{
				i.gameObject.SetActive(true);
			}
		}
	}
}
