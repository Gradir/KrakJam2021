using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactible : MonoBehaviour
{
	public bool _hideOnStart = true;

	private void Start()
	{
		
		if (_hideOnStart)
		{
			gameObject.SetActive(false);
		}
	}
}
