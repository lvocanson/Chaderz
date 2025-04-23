using UnityEngine;

public class MaterialManager : MonoBehaviour
{
	[SerializeField] Material _shellMat;

	private void Update()
	{
		_shellMat.SetFloat("_UnscaledTime", Time.unscaledTime);
		_shellMat.SetFloat("_TimeScale", Time.timeScale);
	}
}
