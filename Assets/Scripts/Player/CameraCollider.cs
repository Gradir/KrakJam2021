using TMPro;
using UnityEngine;
using DG.Tweening;

public class StoryShouldProgressSignal : ASignal<GameProgress> { }

public class CameraCollider : MonoBehaviour
{
	[SerializeField] private FloatingText _text;
	[SerializeField] private Camera _thisCamera;
	[SerializeField] private LookWithMouse _mouseLook;
	[SerializeField] private Collider _thisCollider;
	[SerializeField] private CharacterController _characterController;
	[SerializeField] private GameDirector gameDirector;
	[SerializeField] private float _zoomSpeed = 5f;
	private StoryProgresser storyProgresserInFront;
	private const string interactString = "Interact with ";
	private float baseFOV;

	private void Start()
	{
		if (_thisCamera == null)
		{
			_thisCamera = GetComponent<Camera>();
		}
		baseFOV = _thisCamera.fieldOfView;
	}

	private void OnTriggerEnter(Collider other)
	{
		var s = other.GetComponent<StoryProgresser>();
		if (s != null && s._interactible)
		{
			if (s._activateAutomatically)
			{
				s.TryProgress();
				if (s is Door)
				{
					DisableControl();
					DOVirtual.DelayedCall(gameDirector.GetFadeTime(), () => TransportPlayer((s as Door)._teleportTo));
				}
			}
			else
			{
				storyProgresserInFront = s;
				_text.ChangeText(s._displayName);
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		var s = other.GetComponent<StoryProgresser>();
		if (s != null && s._interactible)
		{
			storyProgresserInFront = null;
			_text.ChangeText(null);
		}
	}

	public void HideFloatingText()
	{
		_thisCollider.enabled = false;
		storyProgresserInFront = null;
		_text.ChangeText(null);
	}

	public void EnableCollider()
	{
		_thisCollider.enabled = true;
	}

	private void Update()
	{
		if (Input.GetKeyUp(KeyCode.E))
		{
			if (storyProgresserInFront != null && storyProgresserInFront.gameObject.activeSelf)
			{
				var _cachedStoryProgresser = storyProgresserInFront;
				storyProgresserInFront = null;

				Debug.Log(string.Format("<color=white><b>{0}</b></color>", "activated"));
				_cachedStoryProgresser.TryProgress();
				if (_cachedStoryProgresser.isActiveAndEnabled == false)
				{
					_text.ChangeText(null);
				}
				if (_cachedStoryProgresser.GetType() == typeof(Door))
				{
					Debug.Log(string.Format("<color=white><b>{0}</b></color>", "transporting to: " + (_cachedStoryProgresser as Door)._teleportTo.gameObject.name));
					// Pause, move camera etc
					DisableControl();
					DOVirtual.DelayedCall(gameDirector.GetFadeTime(), () => TransportPlayer((_cachedStoryProgresser as Door)._teleportTo));
				}
			}
		}
	}

	void LateUpdate()
	{
		if (Input.GetMouseButton(1))
		{
			Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 35, Time.deltaTime * _zoomSpeed);
		}
		else
		{
			Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, baseFOV, Time.deltaTime * _zoomSpeed);
		}
	}

		private void TransportPlayer(Transform t)
	{
		_characterController.transform.position = t.position;
		_characterController.transform.rotation = t.rotation;
		EnableControl();
	}

	public void DisableControl()
	{
		_mouseLook.enabled = false;
		_characterController.enabled = false;
	}
	public void EnableControl()
	{
		_mouseLook.enabled = true;
		_characterController.enabled = true;
	}
}
