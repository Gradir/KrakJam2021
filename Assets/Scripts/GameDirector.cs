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
	TheEnd
}

public class GameDirector : MonoBehaviour
{
	[SerializeField] private CanvasGroup _blackCG;
	[SerializeField] private float _timeForShowBlack = 0.4f;
	[SerializeField] private float _timeForHideBlack = 0.3f;
	private GameProgress _gameProgress;
	private int _progressCount;

	private void Start()
	{
		ChangeBlackOpacity(false);
	}

	public GameProgress GetCurrentProgress()
	{
		return _gameProgress;
	}

	public void ReactOnStoryProgress()
	{
		_blackCG.alpha = 1;
		ChangeBlackOpacity(true);
		DOVirtual.DelayedCall(_timeForShowBlack, () => ChangeBlackOpacity(false));
		_progressCount++;
		_gameProgress = (GameProgress)_progressCount;
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
