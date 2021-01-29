using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

// To be deleted
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
	//private Dictionary<GameProgress, MusicType> _musicTypeByProgress = new Dictionary<GameProgress, MusicType>();
	[SerializeField] private MusicByGameProgressDatabase _musicDatabase;
	[SerializeField] private AudioMixer _mixer;
	[SerializeField] private AudioSource _musicSource;
	[SerializeField] private AudioSource _voiceOverSource;
	[SerializeField] private float _fadeInDuration = 1.2f;
	[SerializeField] private float _fadeOutDuration = 0.8f;

	private MusicType _currentMusicType;
	private GameProgress _currentStory;
	private MusicByGameProgress _cachedMusicProgress;

	public void ReactOnStoryProgress(GameProgress _thisStory)
	{
		var musicProgress = _musicDatabase.GetMusicScriptable(_thisStory);
		if (musicProgress != null)
		{
			if (_thisStory != _currentStory && musicProgress != _cachedMusicProgress)
			//var newType = _musicTypeByProgress[_thisStory];
			//if (newType != _currentMusicType)
			{
				_cachedMusicProgress = musicProgress;
				Sequence newSequence = DOTween.Sequence();
				newSequence.Append(_musicSource.DOFade(0, _fadeInDuration));
				newSequence.AppendCallback(() => ChangeMusicTrack(_thisStory));
				newSequence.Append(_musicSource.DOFade(1, _fadeOutDuration));
				newSequence.Play();
			}
		}
	}

	public void PlayVoiceOver(AudioClip clip)
	{
		_voiceOverSource.clip = clip;
		_voiceOverSource.Play();
	}

	public void ChangeMusicTrack(GameProgress progress)
	{
		var m = _musicDatabase.GetMusic(progress);
		if (m!= null)
		{
			_musicSource.clip = m;
		}
		_musicSource.Play();
	}
}
