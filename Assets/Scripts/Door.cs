using UnityEngine;

public class Door : StoryProgresser
{
	public Transform _teleportTo;
	public Transform _newTargetAfterInteraction;

	public override void OnDrawGizmosSelected()
	{
		DrawArrow.ForGizmo(transform.position, _teleportTo.transform.position, Color.blue);
		base.OnDrawGizmos();
	}

	public void SwitchTarget()
	{
		_teleportTo = _newTargetAfterInteraction;
	}
}
