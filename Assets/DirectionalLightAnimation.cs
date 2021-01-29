using DG.Tweening;
using UnityEngine;

public class DirectionalLightAnimation : MonoBehaviour
{
	[SerializeField] private float _length;
	[SerializeField] private bool _beginOnStart;
	private Vector3 yRotation = new Vector3(0, 360, 0);

	private void Start()
	{
		if (_beginOnStart)
		{
			StartTween();
		}
	}

	public void StartTween()
	{
		transform.DORotate(yRotation, _length).SetEase(Ease.Linear);
	}

	public void StopTween()
	{
		DOTween.Kill(this);
	}
}
