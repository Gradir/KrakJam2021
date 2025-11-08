using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class Flicker : MonoBehaviour
{
	[SerializeField] private bool _startAutomatically;
	[Space]
	[SerializeField] private Ease _easing = Ease.InExpo;
	[SerializeField] private float _minIntensity = 10000f;
	[SerializeField] private float _maxIntensity = 1000000f;
	[SerializeField] private float _minTime = 0.1f;
	[SerializeField] private float _maxTime = 0.9f;
	[SerializeField] private int _chanceForABreak = 15;
	[SerializeField] private float _breakMinTime = 0.2f;
	[SerializeField] private float _breakMaxTime = 1f;
	private HDAdditionalLightData _light;

	private bool onBreak;

	private void Start()
	{
		if (_light == null)
		{
			_light = GetComponent<HDAdditionalLightData>();
		}
		if (_startAutomatically && _light != null)
		{
			StartFlicker();
		}
	}

	private void StopBreak()
	{
		onBreak = false;
		StartFlicker();
	}

	private void OnDestroy()
	{
		DOTween.Kill(this);
	}

	private void StartFlicker()
	{
		if (Random.Range(0, 99) < _chanceForABreak)
		{
			onBreak = true;
			var randomBreakTime = Random.Range(_breakMinTime, _breakMaxTime);
			DOVirtual.DelayedCall(randomBreakTime, StopBreak);
		}
		if (onBreak)
		{
			return;
		}
		var randomIndensity = Random.Range(_minIntensity, _maxIntensity);
		var randomTime = Random.Range(_minTime, _maxTime);
		DOTween.To(() => _light.intensity, x => _light.intensity = x, randomIndensity, randomTime).SetEase(_easing).OnComplete(StartFlicker).SetTarget(this);
	}
}
