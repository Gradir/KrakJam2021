using DG.Tweening;
using UnityEngine;

public class Door : StoryProgresser
{
	public Transform _teleportTo;
	public Transform _newTargetAfterInteraction;
	public float _timeForColorChange = 5f;
	public GameProgress _nextProgress;
	public bool startAnimationOnInteract = false;
	public Transform transformToMove;
	public AudioSource aus;

	[SerializeField] private Color _newColor;

	private void Start()
	{
		aus = GetComponent<AudioSource>();
	}

	public override void TryProgress()
	{
		if (aus != null)
		{
			aus.Play();
		}
		if (startAnimationOnInteract)
		{
			transformToMove.DOLocalMoveX(transform.position.x - 2, 2f);
		}
		base.TryProgress();
	}

	public override void OnDrawGizmosSelected()
	{
		if (_teleportTo != null)
		{
			DrawArrow.ForGizmo(transform.position, _teleportTo.transform.position, Color.blue);
		}
		base.OnDrawGizmos();
	}

	public void SwitchTarget()
	{
		if (_newTargetAfterInteraction == null)
		{
			return;
		}
		GetComponent<MeshRenderer>().material.DOColor(_newColor, "_BaseColor", _timeForColorChange);
		_thisStory = _nextProgress;
		_teleportTo = _newTargetAfterInteraction;
	}
}
