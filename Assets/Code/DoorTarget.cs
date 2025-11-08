using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTarget : MonoBehaviour
{
	private void OnDrawGizmos()
	{
		Gizmos.DrawIcon(transform.position, "DoorTarget.png");
	}
}
