using UnityEngine;

public class UIWorldSpaceController : MonoBehaviour
{
	public float speed = 100f;

	[SerializeField] private Transform _holder;
	private Vector2 _lastMousePosition = Vector2.zero;
	private Transform spawnedObject;


	public void SpawnObject(GameObject prefab)
	{
		spawnedObject = Instantiate(prefab, _holder).transform;
	}

	void Update()
	{
		if (spawnedObject == null)
		{
			return;
		}
		Vector2 _currentMousePosition = (Vector2)Input.mousePosition;
		Vector2 mouseDelta = _currentMousePosition - _lastMousePosition;
		mouseDelta *= speed * Time.deltaTime;

		_lastMousePosition = _currentMousePosition;

		spawnedObject.Rotate(0f, mouseDelta.x * -1f, 0f, Space.World);
	}

	public void CleanUp()
	{
		Destroy(spawnedObject.gameObject);
		spawnedObject = null;
	}
}
