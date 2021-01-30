using DG.Tweening;
using UnityEngine;

public class UIWorldSpaceController : MonoBehaviour
{
	public float speed = 100f;

	[SerializeField] private Transform _holder;
	[SerializeField] private float timeToScale = 0.8f;
	private Vector2 _lastMousePosition = Vector2.zero;
	private Transform spawnedObject;
	private Vector3 fullCircle = new Vector3(0, 360, 0);

	public void SpawnObject(GameObject prefab)
	{
		if (prefab == null)
		{
			return;
		}
		spawnedObject = Instantiate(prefab, _holder).transform;
		spawnedObject.transform.localScale = Vector3.zero;
		spawnedObject.DOScale(1, timeToScale).SetTarget(this);
		spawnedObject.DORotate(fullCircle, 15, RotateMode.FastBeyond360).SetTarget(this);
	}

	public void CleanUp()
	{
		if (spawnedObject == null)
		{
			return;
		}
		DOTween.Kill(this);
		spawnedObject.DOScale(0, 0.25f).OnComplete(Clean);
	}

	private void Clean()
	{
		if (spawnedObject == null)
		{
			return;
		}
		DOTween.Kill(this);
		Destroy(spawnedObject.gameObject);
		spawnedObject = null;
	}
}