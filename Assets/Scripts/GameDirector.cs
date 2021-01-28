using System.Collections;
using System.Collections.Generic;
using UnityEngine;

	public enum GameProgress
	{
		FirstInteraction,
		SecondInteraction,
		ThirdInteraction,
		FourthInteraction
	}

public class GameDirector : MonoBehaviour
{
	private GameProgress _gameProgress;
	private int _progressCount;

	private void Start()
	{
		
	}

	public void ReactOnStoryProgress()
	{
		_progressCount++;
		_gameProgress = (GameProgress)_progressCount;
	}

}
