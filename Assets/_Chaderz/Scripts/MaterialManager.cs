using UnityEngine;

public class MaterialManager : MonoBehaviour
{
	[SerializeField] Material _shellMat;

	private void Update()
	{
		_shellMat.SetFloat("_UnscaledTime", Time.unscaledTime);
		_shellMat.SetFloat("_TimeScale", Time.timeScale);
	}

	private void OnDestroy()
	{
		// Reset (avoid git changes)
		_shellMat.SetFloat("_UnscaledTime", 0f);
		_shellMat.SetFloat("_TimeScale", 0f);
	}
}
