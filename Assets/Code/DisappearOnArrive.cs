using UnityEngine;

public class DisappearOnArrive : Target
{
	public override void ReactOnArrive(GameObject arrived)
	{
		arrived.SetActive(false);
	}
}
