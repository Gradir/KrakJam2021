using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraCollider : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI _text;
	private Door _doorInFront;

	private void OnTriggerEnter(Collider other)
	{
		var door = other.GetComponent<Door>();
		if (door != null)
		{
			_text.text = "Door!";
			_doorInFront = door;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		var door = other.GetComponent<Door>();
		if (door != null)
		{
			_text.text = string.Empty;
			_doorInFront = null;
		}
	}

	private void Update()
	{
		if (Input.GetKeyUp(KeyCode.E))
		{
			if (_doorInFront != null)
			{
				transform.parent.transform.position = _doorInFront._teleportTo.position;
			}
		}
	}
}
