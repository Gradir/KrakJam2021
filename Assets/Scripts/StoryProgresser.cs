using UnityEngine;
using System.Collections;

public abstract class StoryProgresser : MonoBehaviour
{
	public bool _progressesStory = false;
	public string _displayName;
	public Interactible[] _interactiblesToActivate;

	public void TryProgress()
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
