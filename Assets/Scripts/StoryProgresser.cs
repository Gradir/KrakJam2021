using UnityEngine;
using System.Collections;

public abstract class StoryProgresser : MonoBehaviour
{
	public bool _progressesStory = false;
	public string _displayName;
	public Interactible[] _interactiblesToActivate;
	public bool _hideOnStart = true;

	private void Start()
	{
		if (_hideOnStart)
		{
			gameObject.SetActive(false);
		}
	}

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

	public void OnDrawGizmos()
	{
		Gizmos.DrawIcon(transform.position, "Interactible.png");
	}

	public virtual void OnDrawGizmosSelected()
	{
		if (!_progressesStory)
		{
			return;
		}
		foreach (var i in _interactiblesToActivate)
		{
			DrawArrow.ForGizmo(transform.position, i.transform.position, Color.white);
		}
	}
}
