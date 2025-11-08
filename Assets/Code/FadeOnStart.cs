using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class FadeOnStart : MonoBehaviour
{
	private CanvasGroup cg;
	[SerializeField] private float _time = 1.5f;

	private void OnEnable()
	{
		cg = GetComponent<CanvasGroup>();
		cg.alpha = 0;
		cg.DOFade(1, _time);
	}
}
