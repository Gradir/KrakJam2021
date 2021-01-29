using TMPro;
using UnityEngine;

public class StoryShouldProgress : ASignal { }

public class CameraCollider : MonoBehaviour
{
	[SerializeField] private FloatingText _text;
	[SerializeField] private CharacterController _characterController;
	private StoryProgresser storyProgresserInFront;
	private const string interactString = "Interact with ";

	private void OnTriggerEnter(Collider other)
	{
		var s = other.GetComponent<StoryProgresser>();
		if (s != null && s._interactible)
		{
			storyProgresserInFront = s;
			_text.ChangeText(interactString + s._displayName);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		var s = other.GetComponent<StoryProgresser>();
		if (s != null && s._interactible)
		{
			storyProgresserInFront = null;
			_text.ChangeText(null);
		}
	}

	private void Update()
	{
		if (Input.GetKeyUp(KeyCode.E))
		{
			if (storyProgresserInFront != null && storyProgresserInFront.gameObject.activeSelf)
			{
				var _cachedStoryProgresser = storyProgresserInFront;
				storyProgresserInFront = null;

				Debug.Log(string.Format("<color=white><b>{0}</b></color>", "activated"));
				_cachedStoryProgresser.TryProgress();
				if (_cachedStoryProgresser.GetType() == typeof(Door))
				{
					Debug.Log(string.Format("<color=white><b>{0}</b></color>", "transporting to: " + (_cachedStoryProgresser as Door)._teleportTo.gameObject.name));
					// Pause, move camera etc
					_characterController.enabled = false;
					_characterController.transform.position = (_cachedStoryProgresser as Door)._teleportTo.position;
					_characterController.transform.rotation = (_cachedStoryProgresser as Door)._teleportTo.transform.rotation;
					_characterController.enabled = true;
				}
			}
		}
	}
}
