using TMPro;
using UnityEngine;

public class StoryShouldProgress : ASignal { }

public class CameraCollider : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI _text;
	private Interactible _interInFront;
	private Door _doorInFront;

	private void OnTriggerEnter(Collider other)
	{
		var door = other.GetComponent<Door>();
		var inter = other.GetComponent<Interactible>();
		if (inter != null)
		{
			_interInFront = inter;
			_text.text = "Interact with" + inter.gameObject.name;
		}
		else if (door != null)
		{
			_text.text = "Door!";
			_doorInFront = door;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		var door = other.GetComponent<Door>();
		var inter = other.GetComponent<Interactible>();
		if (inter != null)
		{
			_interInFront = null;
			_text.text = string.Empty;
		}
		else if (door != null)
		{
			_text.text = string.Empty;
			_doorInFront = null;
		}
	}

	private void Update()
	{
		if (Input.GetKeyUp(KeyCode.E))
		{
			if (_interInFront != null && _interInFront.gameObject.activeSelf)
			{

			}
			else if (_doorInFront != null)
			{
				_doorInFront.OnPassedThrough();
				// Pause, move camera etc
				transform.parent.transform.position = _doorInFront._teleportTo.position;
			}
		}
	}
}
