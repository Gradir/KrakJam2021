using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum MusicType
{
	Loop1,
	Loop2,
	Loop3,
	Loop4,
	Loop5,
	Interaction1,
	Interaction2,
	Interaction3,
	Interaction4,
	Interaction5,
	Interaction6,
	Interaction7,
	Interaction8,
	Interaction9,
	Interaction10
}


public class AudioManager : MonoBehaviour
{
	private Dictionary<GameProgress, MusicType> musicTypeByProgress = new Dictionary<GameProgress, MusicType>();
	[SerializeField] private GameDirector _gameDirector;
	[SerializeField] private AudioMixer _mixer;
	[SerializeField] private AudioSource _musicSource;
	[SerializeField] private AudioSource _sfxSource;
	[SerializeField] private float _fadeInDuration = 1.2f;
	[SerializeField] private float _fadeOutDuration = 0.8f;

	private MusicType _currentMusicType;

	private void Start()
	{
		// Fill dictionary here

		Signals.Get<StoryShouldProgressSignal>().AddListener(ReactOnStoryProgress);
	}

	private void ReactOnStoryProgress()
	{
		var newType = musicTypeByProgress[_gameDirector.GetCurrentProgress()];
		if (newType != _currentMusicType)
		{
			Sequence newSequence = DOTween.Sequence();
			newSequence.Append(_musicSource.DOFade(0, _fadeInDuration));
			newSequence.AppendCallback(() => ChangeMusicTrack(newType));
			newSequence.Append(_musicSource.DOFade(1, _fadeOutDuration));
			newSequence.Play();
		}
	}

	public void ChangeMusicTrack(MusicType musicType)
	{
		// scriptable database of tracks
		//_musicSource.clip = music
	}
}
