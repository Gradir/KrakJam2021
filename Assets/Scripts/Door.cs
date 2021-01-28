using UnityEngine;

public class Door : StoryProgresser
{
	public Transform _teleportTo;

	public override void OnDrawGizmosSelected()
	{
		DrawArrow.ForGizmo(transform.position, _teleportTo.transform.position, Color.blue);
		base.OnDrawGizmos();
	}
}
