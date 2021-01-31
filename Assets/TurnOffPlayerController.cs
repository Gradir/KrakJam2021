using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class TurnOffPlayerController : MonoBehaviour
{
	private void OnTriggerStay(Collider other)
	{
		var cont = other.GetComponent<FirstPersonController>();
		if (cont)
		{
			cont.GetComponent<CameraCollider>().DisableControl();
		}
	}
}
