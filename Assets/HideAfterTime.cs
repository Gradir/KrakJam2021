using DG.Tweening;
using UnityEngine;

public class HideAfterTime : MonoBehaviour
{
	[SerializeField] private CanvasGroup _cg;
	[SerializeField] private float _afterTime = 6f;
	void Start()
	{
		_cg.DOFade(0, _afterTime).OnComplete(() => gameObject.SetActive(false));

	}
}
