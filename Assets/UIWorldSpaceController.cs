using UnityEngine;

public class UIWorldSpaceController : MonoBehaviour
{
	public float speed = 100f;

	private Vector2 _lastMousePosition = Vector2.zero;
	private Transform spawnedObject;


	public void SpawnObject(GameObject prefab)
	{
		spawnedObject = Instantiate(prefab, transform).transform;
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

		if (Input.GetMouseButton(0))
		{
			spawnedObject.Rotate(0f, mouseDelta.x * -1f, 0f, Space.World);
		}
	}
}
