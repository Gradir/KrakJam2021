using DG.Tweening;
using UnityEngine;

public class BrotherController : MonoBehaviour
{
	private Material _brotherMaterial;
	private float _texturePanningDuration = 2f;
	private const string _offsetString = "_Offset";

	private void OnEnable()
	{
		StartPanning();
	}

	private void OnDisable()
	{
		StopPanning();
	}

	private void StartPanning()
	{
		if (_brotherMaterial == null)
		{
			_brotherMaterial = GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterial;
		}
		_brotherMaterial.DOFloat(1, _offsetString, _texturePanningDuration).SetEase(Ease.Linear).SetTarget(this).SetLoops(-1, LoopType.Restart);
	}

	private void StopPanning()
	{
		DOTween.Kill(this);
	}
}
