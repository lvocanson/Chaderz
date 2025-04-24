using System.Collections;
using UnityEngine;

public class SlowMotionOnTrigger : MonoBehaviour
{
	public AnimationCurve InterpolationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

	[Min(0f)] public float SlowForce = 10f;
	[Min(0f)] public float LerpTime = 1f;

	private Coroutine _LerpRoutine = null;

	private void OnValidate()
	{
		if (SlowForce < 0f)
		{
			Debug.LogError($"{nameof(SlowForce)} cannot be negative !");
		}

		if (LerpTime < 0f)
		{
			Debug.LogError($"{nameof(LerpTime)} cannot be negative !");
		}
	}

	private void LerpTimeScaleTo(float targetTimeScale)
	{
		if (_LerpRoutine != null)
		{
			StopCoroutine(_LerpRoutine);
		}
		_LerpRoutine = StartCoroutine(Routine(targetTimeScale));

		IEnumerator Routine(float target)
		{
			float elapsed = 0f;
			float startingTimeScale = Time.timeScale;

			while (elapsed < LerpTime)
			{
				float t = elapsed / LerpTime;
				float curveT = InterpolationCurve.Evaluate(t);
				float value = Mathf.Lerp(startingTimeScale, target, curveT);
				Time.timeScale = value;
				Debug.Log(value);

				elapsed += Time.deltaTime;
				yield return null;
			}

			Time.timeScale = target;
			_LerpRoutine = null;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		Debug.LogWarning($"Trigger Enter {other} : {Mathf.Exp(-SlowForce)}");
		LerpTimeScaleTo(Mathf.Exp(-SlowForce));
	}

	private void OnTriggerExit(Collider other)
	{
		Debug.LogWarning($"Trigger Exit {other}");
		LerpTimeScaleTo(1f);
	}

}
