using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalLightAnimation : MonoBehaviour
{
	[SerializeField] private float _animationSpeed;
	[SerializeField] private bool _beginOnStart;
	private Vector3 yRotation = new Vector3(0, 360, 0);

	private void Start()
	{
		if (_beginOnStart)
		{

		}
	}

	private void StartTween()
	{
		transform.DORotate(yRotation, ).SetEase
	}

	private void StopTween()
	{
		DOTween.Kill(this);
	}
}
