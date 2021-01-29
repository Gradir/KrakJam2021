using DG.Tweening;
using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
	[SerializeField] private float _lengthBase = 10f;
	[SerializeField] private float _lengthRandom = 2f;
	[SerializeField] private float _maxDistance = 100f;
	[SerializeField] private CanvasGroup _cg;
	[SerializeField] private TextMeshProUGUI _text;
	[SerializeField] private RectTransform _textRectTransform;
	private Vector2 _textStartPosition;

	private void Start()
	{
		if (_text == null)
		{
			_text = GetComponent<TextMeshProUGUI>();
		}
		if (_textRectTransform == null)
		{
			_textRectTransform = _text.GetComponent<RectTransform>();
		}
		_textStartPosition = _textRectTransform.anchoredPosition;
	}

	public void ChangeText(string txt)
	{
		if (txt != null && txt != string.Empty)
		{
			_cg.DOFade(1, 0.25f);
			_textRectTransform.anchoredPosition = _textStartPosition;
			_text.text = txt;
			//_revealer.RestartWithText(txt);
			_text.DOColor(Color.black, _lengthBase).SetLoops(-1, LoopType.Yoyo).SetTarget(this);
			StartAnimation();
		}
		else
		{
			_cg.DOFade(0, 0.15f);
			_text.text = string.Empty;
			StopAnimation();
		}
	}

	private void StartAnimation()
	{
		var l = _lengthBase + Random.Range(-_lengthRandom, _lengthRandom);
		var x = Random.Range(-_maxDistance, _maxDistance);
		var y = Random.Range(-_maxDistance, _maxDistance);
		var target = _textStartPosition + new Vector2(x, y);
		_textRectTransform.DOAnchorPos(target, l)
			.SetTarget(this)
			.OnComplete(StartAnimation);

	}

	private void StopAnimation()
	{
		DOTween.Kill(this);
	}
}
