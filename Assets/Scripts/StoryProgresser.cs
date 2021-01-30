using System.Linq;
using UnityEngine;

public abstract class StoryProgresser : MonoBehaviour
{
	public bool _progressesStory = true;
	public GameProgress _thisStory;
	public bool _interactible = true;
	public bool _activateAutomatically = false;
	public bool _setInteractibleOnEnable = true;
	[SerializeField] private GameObject[] _objectsToDeactivate;
	public bool _hideOnInteraction = false;
	public string _displayName;

	public StoryProgresser[] _interactiblesToActivate;
	public bool _hideOnStart = true;

	private int interactionCount;
	private BrotherController brother;

	private void Start()
	{
		brother = GetComponent<BrotherController>();
		if (_hideOnStart)
		{
			gameObject.SetActive(false);
		}
	}

	public void TryProgress()
	{
		if (_progressesStory)
		{
			Signals.Get<StoryShouldProgressSignal>().Dispatch(_thisStory, _interactiblesToActivate.Length > 0, interactionCount);
			interactionCount++;

			foreach (var o in _objectsToDeactivate)
			{
				o.SetActive(false);
			}
			foreach (var i in _interactiblesToActivate)
			{
				var agent = i.GetComponent<TurnAgentComponents>();
				if (agent)
				{
					agent.TurnOnOrOff(true);
				}
				i.gameObject.SetActive(true);
				if (i._setInteractibleOnEnable)
				{
					i._interactible = true;
				}
				if (i.GetType() == typeof(Door))
				{
					var door = i as Door;
					door.SwitchTarget();
				}
			}
		}
		if (_hideOnInteraction)
		{
			if (brother)
			{
				brother.FadeAway();
			}
			gameObject.SetActive(false);
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
			if (i == null)
			{
				continue;
			}
			DrawArrow.ForGizmo(transform.position, i.transform.position, Color.white);
		}
	}
}
