using DG.Tweening;
using TMPro;
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
	Console
}

public class GameDirector : MonoBehaviour
{
	[SerializeField] private CanvasGroup _blackCG;
	[SerializeField] private float _timeForShowBlack = 0.4f;
	[SerializeField] private float _timeForHideBlack = 0.3f;
	[SerializeField] private CameraCollider _player;
	[SerializeField] private CanvasGroup _interactionStoryCG;
	[SerializeField] private TextMeshProUGUI _interactionStory;
	[SerializeField] private AudioManager _audioManager;
	[SerializeField] private TextWithSoundDatabase _textWithSoundDatabase;
	private bool _isInInteractionMode;
	private GameProgress _cachedLastProgress;
	private int _iterationsNumber;

	private float fixedDeltaTime;

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

	public void ReactOnStoryProgress(GameProgress progress)
	{
		if (progress != _cachedLastProgress)
		{
			if (_iterationsNumber > 1)
			{
				_cachedLastProgress = progress;
				_iterationsNumber = 0;
			}
			else
			{
				_iterationsNumber++;
			}
		}
		var txt = _textWithSoundDatabase.GetText(progress);
		// ToDo: blocking interaction, wait time
		if (txt != null)
		{
			_interactionStoryCG.DOFade(1, 0.3f).SetId("story");
			var length = txt.Length * 0.1f;
			DOVirtual.DelayedCall(length, FadeOutStory);
			_interactionStory.text = txt;
		}
		var vo = _textWithSoundDatabase.GetVoiceOver(progress);
		if (vo != null)
		{
			_audioManager.PlayVoiceOver(vo);
		}
		else
		{
			_audioManager.ReactOnStoryProgress(progress);
		}
		if (_textWithSoundDatabase.IsInteractionBlocked(progress))
		{
			_player.DisableControl();
			_isInInteractionMode = true;
		}
		Time.timeScale = _textWithSoundDatabase.GetTimeScale(progress);
		Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;

		if (_textWithSoundDatabase.DoesFadeOut(progress))
		{
			_blackCG.alpha = 0;
			ChangeBlackOpacity(true);
			DOVirtual.DelayedCall(_timeForShowBlack, () => ChangeBlackOpacity(false));
		}
	}

	private void FadeOutStory()
	{
		DOTween.Kill("story");
		_interactionStoryCG.DOFade(0, 0.5f);
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
				FadeOutStory();
				_player.EnableControl();
				_audioManager.ReactOnStoryProgress(_cachedLastProgress);
				_isInInteractionMode = false;
			}
		}
	}

}
