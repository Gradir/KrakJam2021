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
	TheEnd
}

public class GameDirector : MonoBehaviour
{
	[SerializeField] private CanvasGroup _blackCG;
	[SerializeField] private float _timeForShowBlack = 0.4f;
	[SerializeField] private float _timeForHideBlack = 0.3f;
	[SerializeField] private CanvasGroup _interactionStoryCG;
	[SerializeField] private TextMeshProUGUI _interactionStory;
	[SerializeField] private AudioManager _audioManager;
	[SerializeField] private TextWithSoundDatabase _textWithSoundDatabase;

	public float GetFadeTime()
	{
		return _timeForShowBlack;
	}

	private GameProgress _gameProgress;

	private void Start()
	{
		Signals.Get<StoryShouldProgressSignal>().AddListener(ReactOnStoryProgress);
		ChangeBlackOpacity(false);
	}

	public GameProgress GetCurrentProgress()
	{
		return _gameProgress;
	}

	public void ReactOnStoryProgress(GameProgress progress)
	{
		_gameProgress = progress;
		var txt = _textWithSoundDatabase.GetText(progress);
		// ToDo: blocking interaction, wait time
		if (txt != null)
		{
			_interactionStoryCG.DOFade(1, 0.3f);
			var length = txt.Length * 0.1f;
			DOVirtual.DelayedCall(length, () => _interactionStoryCG.DOFade(0, 0.5f));
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

		if (_textWithSoundDatabase.DoesFadeOut(progress))
		{
			_blackCG.alpha = 0;
			ChangeBlackOpacity(true);
			DOVirtual.DelayedCall(_timeForShowBlack, () => ChangeBlackOpacity(false));
		}
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

}
