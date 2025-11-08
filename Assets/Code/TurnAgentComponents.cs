using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class TurnAgentComponents : MonoBehaviour
{
	[SerializeField] private bool _activateOnStart = false;
	[SerializeField] private AICharacterControl _ai;
	[SerializeField] private Animator _am;
	[SerializeField] private NavMeshAgent _nv;
	[SerializeField] private ThirdPersonCharacter _th;

	public void TurnOnOrOff(bool on)
	{
		_ai.enabled = on;
		_am.enabled = on;
		_nv.enabled = on;
		_th.enabled = on;
	}

	private void Start()
	{
		if (_activateOnStart)
		{
			TurnOnOrOff(true);
		}
	}
}
