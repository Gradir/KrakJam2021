using DG.Tweening;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class BrotherController : MonoBehaviour
{
	[SerializeField] private AICharacterControl _ai;
	private Material _brotherMaterial;
	private float _texturePanningDuration = 2f;
	private const string _offsetString = "_Offset";
	private const string colorParameter = "_Color";

	private void OnEnable()
	{
		StartPanning();
	}

	private void OnDisable()
	{
		StopPanning();
	}

	public void FadeAway()
	{
		_brotherMaterial.DOColor(Color.black, colorParameter, 6f);
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

	private void Update()
	{
		var t = _ai.target.GetComponent<Target>();
		if (t)
		{
			if (Vector3.Distance(t.transform.position, transform.position) < 0.5f)
			{
				t.ReactOnArrive(gameObject);
			}
		}
	}
}
