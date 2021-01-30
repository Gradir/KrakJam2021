using DG.Tweening;
using UnityEngine;

public class HideAfterTime : MonoBehaviour
{
	[SerializeField] private CanvasGroup _cg;
	void Start()
	{
		_cg.DOFade(0, 3f).OnComplete(() => gameObject.SetActive(false));

	}
}
