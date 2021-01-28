public class Interactible : StoryProgresser
{
	public bool _hideOnStart = true;

	private void Start()
	{

		if (_hideOnStart)
		{
			gameObject.SetActive(false);
		}
	}
}
