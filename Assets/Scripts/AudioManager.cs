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
	[SerializeField] private GameDirector _gameDirector;
	[SerializeField] private AudioSource _musicSource;
	[SerializeField] private AudioSource _voiceOverSource;
	[SerializeField] private AudioSource _sfxSource;
	[SerializeField] private AudioClip[] _interactionSounds;
	[SerializeField] private AudioClip[] _interactionSoundsGood;

	private MusicType _currentMusicType;
	private GameProgress _currentStory;
	private MusicByGameProgress _cachedMusicProgress;

	public void ReactOnStoryProgress(GameProgress _thisStory, bool activatesSomething)
	{
//		var sound = GetRandomClip(activatesSomething ? _interactionSoundsGood : _interactionSounds);
		//PlayOneShot(sound);

		var musicProgress = _musicDatabase.GetMusicScriptable(_thisStory);
		if (musicProgress != null)
		{
			if (_thisStory != _currentStory && _cachedMusicProgress != null && musicProgress.music != _cachedMusicProgress.music)
			//var newType = _musicTypeByProgress[_thisStory];
			//if (newType != _currentMusicType)
			{
				_musicSource.loop = musicProgress.loops;
				_currentStory = _thisStory;
				var fadeInTime = _gameDirector.GetFadeTime();
				Sequence newSequence = DOTween.Sequence();
				newSequence.Append(_musicSource.DOFade(0, fadeInTime));
				newSequence.AppendCallback(() => ChangeMusicTrack(_thisStory));
				newSequence.Append(_musicSource.DOFade(1, fadeInTime / 2));
				newSequence.Play();
			}
			_cachedMusicProgress = musicProgress;
		}
	}

	private AudioClip GetRandomClip(AudioClip[] array)
	{
		return array[Random.Range(0, array.Length)];
	}

	public void PlayOneShot(AudioClip clip)
	{
		_sfxSource.PlayOneShot(clip);
	}

	public void PlayVoiceOver(AudioClip clip)
	{
		_voiceOverSource.clip = clip;
		_voiceOverSource.Play();
	}

	public void StopVoiceOver()
	{
		_voiceOverSource.Stop();
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
