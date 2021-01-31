using DG.Tweening;
using UnityEngine;

public enum GameProgress
{
	Entrance,
	Room_Childs,
	Interaction_ToyCar,
	Room_HappyTimes,
	Interaction_Tent,
	Room_Girl,
	Interaction_Girl,
	Room_Corridor,
	Interaction_Corridor,
	Room_Brother,
	Interaction_Picture,
	Room_Accident,
	Interaction_BrokenCar,
	Room_Transfusion,
	Interaction_Transfusion,
	Room_Death,
	Interaction_Coffin,
	Room_Squat,
	Interaction_Squat,
	Room_Window,
	Interaction_Window,
	Room_Tunnel,
	Interaction_Tunnel,
	Room_Exit,
	Interaction_Exit,
	Room_Hospital,
	TheEnd,
	None,
	Console,
	Tunnel1,
	Tunnel2,
	TheEnd1,
	TheEnd2,
	TheEnd3,
	Entrance1
}

public class GameDirector : MonoBehaviour
{
	[SerializeField] private CanvasGroup _blackCG;
	[SerializeField] private float _timeForShowBlack = 0.4f;
	[SerializeField] private float _timeForHideBlack = 0.3f;
	[SerializeField] private CameraCollider _player;
	[SerializeField] private CanvasGroup _interactionStoryCG;
	[SerializeField] private FloatingText _interactionStory;
	[SerializeField] private AudioManager _audioManager;
	[SerializeField] private TextWithSoundDatabase _textWithSoundDatabase;
	[SerializeField] private UIWorldSpaceController _closeUpModelHolder;
	private bool _isInInteractionMode;
	private GameProgress _cachedLastProgress;
	private GameProgress _currentProgress;
	private int _iterationsNumber;
	private float fixedDeltaTime;
	private bool cached;

	void Awake()
	{
		// Make a copy of the fixedDeltaTime, it defaults to 0.02f, but it can be changed in the editor
		this.fixedDeltaTime = Time.fixedDeltaTime;
	}

	public float GetFadeTime()
	{
		return _timeForShowBlack;
	}

	private void Start()
	{
		Signals.Get<StoryShouldProgressSignal>().AddListener(ReactOnStoryProgress);
		DOVirtual.DelayedCall(2f, () => ChangeBlackOpacity(false));
	}

	public void ReactOnStoryProgress(GameProgress progress, bool activatesSomething, int interactionCount)
	{
		cached = !cached;
		if (cached == false)
		{
			_cachedLastProgress = progress;
		}
		_currentProgress = progress;

		_audioManager.ReactOnStoryProgress(progress, activatesSomething);
		var progressData = _textWithSoundDatabase.GetInteractionData(progress);
		if (progressData != null)
		{

			if (progressData.blockInteraction)
			{
				_player.HideFloatingText();
				_player.DisableControl();
				_isInInteractionMode = true;

				var closeUpModel = progressData._interactionModel;
				var rotate = progressData.rotateModel;
				if (closeUpModel != null)
				{
					_closeUpModelHolder.SpawnObject(closeUpModel, rotate);
				}
			}

			Time.timeScale = progressData._timeScaleFactor;
			Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
			if (progressData.customFootsteps)
			{
				var footsteps = progressData.footsteps;
				if (footsteps != null && footsteps.Length > 0)
				{
					_player.ChangeFootstepSounds(footsteps);
				}
			}

			if (progressData.fadeout)
			{
				_blackCG.alpha = 0;
				ChangeBlackOpacity(true);
				if (progressData.fadeBackIn)
				{
					DOVirtual.DelayedCall(_timeForShowBlack, () => ChangeBlackOpacity(false));
				}
			}

			var txt = _textWithSoundDatabase.GetText(progressData, interactionCount);
			if (txt != null && txt.Length != 0)
			{
				_interactionStoryCG.alpha = 0;
				FadeInStory();
				var length = txt.Length * 0.15f;
				Debug.Log(length);
				DOVirtual.DelayedCall(length, FadeOutStory).SetId("story");
				_interactionStory.ChangeText(txt);
			}

			var vo = _textWithSoundDatabase.GetVoiceOver(progressData, interactionCount);
			if (vo != null)
			{
				_audioManager.PlayVoiceOver(vo);
			}
		}
	}

	private void FadeInStory()
	{
		DOTween.Kill("story");
		_interactionStoryCG.DOFade(1, 0.35f).SetId("story");
	}

	private void FadeOutStory()
	{
		DOTween.Kill("story");
		_interactionStoryCG.DOFade(0, 0.5f).SetId("story");
	}

	public void ChangeBlackOpacity(bool showBlack)
	{
		if (showBlack)
		{
			_blackCG.DOFade(1, _timeForShowBlack);
		}
		else
		{
			_blackCG.DOFade(0, _timeForHideBlack);
		}
	}

	private void Update()
	{
		if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
		{
			if (_isInInteractionMode)
			{
				if (_cachedLastProgress != null)
				{
					_audioManager.ReactOnStoryProgress(_cachedLastProgress, false);
				}
				else
				{
					_audioManager.ReactOnStoryProgress(_currentProgress, false);
				}
				_audioManager.StopVoiceOver();
				_player.EnableCollider();
				FadeOutStory();
				_player.EnableControl();
				_isInInteractionMode = false;
				_closeUpModelHolder.CleanUp();
			}
		}
	}

}
