using DG.Tweening;
using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
	[SerializeField] float _lengthBase = 10f;
	[SerializeField] float _lengthRandom = 2f;
	[SerializeField] float _maxDistance = 100f;
	[SerializeField] CanvasGroup _cg;
	[SerializeField] TextMeshProUGUI _text;
	[SerializeField] RectTransform _textRectTransform;
	Vector2 _textStartPosition;
	const string FloatingTextId = "FloatingText";

	void Start()
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
		DOTween.Kill(this);
		if (txt != null && txt != string.Empty)
		{
			_text.text = txt;
			_textRectTransform.anchoredPosition = _textStartPosition;
			_cg.DOFade(1, 0.6f).SetTarget(this);
			//_revealer.RestartWithText(txt);
			_text.DOColor(Color.black, _lengthBase).SetLoops(-1, LoopType.Yoyo).SetTarget(this);
			StartAnimation();
		}
		else
		{
			DOTween.Kill(FloatingTextId);
			_cg.DOFade(0, 0.15f).OnComplete(ResetText).SetTarget(this);
		}
	}

	void ResetText()
	{
		_text.text = string.Empty;
	}

	void StartAnimation()
	{
		var l = _lengthBase + Random.Range(-_lengthRandom, _lengthRandom);
		var x = Random.Range(-_maxDistance, _maxDistance);
		var y = Random.Range(-_maxDistance, _maxDistance);
		var target = _textStartPosition + new Vector2(x, y);
		_textRectTransform.DOAnchorPos(target, l)
			.SetTarget(FloatingTextId)
			.OnComplete(StartAnimation);

	}

}
