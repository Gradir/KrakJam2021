using TMPro;
using UnityEngine;

public class StoryShouldProgress : ASignal { }

public class CameraCollider : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI _text;
	[SerializeField] private CharacterController _characterController;
	private StoryProgresser storyProgresserInFront;

	private void OnTriggerEnter(Collider other)
	{
		var s = other.GetComponent<StoryProgresser>();
		if (s != null)
		{
			storyProgresserInFront = s;
			_text.text = "Interact with " + s._displayName;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		var s = other.GetComponent<StoryProgresser>();
		if (s != null)
		{
			s = null;
			_text.text = string.Empty;
		}
	}

	private void Update()
	{
		if (Input.GetKeyUp(KeyCode.E))
		{
			if (storyProgresserInFront != null && storyProgresserInFront.gameObject.activeSelf)
			{
				Debug.Log(string.Format("<color=white><b>{0}</b></color>", "activated"));
				storyProgresserInFront.TryProgress();
				if (storyProgresserInFront.GetType() == typeof(Door))
				{
					Debug.Log(string.Format("<color=white><b>{0}</b></color>", "is door"));
					// Pause, move camera etc
					_characterController.enabled = false;
					_characterController.transform.position = (storyProgresserInFront as Door)._teleportTo.position;
					_characterController.enabled = true;
				}
			}
		}
	}
}
