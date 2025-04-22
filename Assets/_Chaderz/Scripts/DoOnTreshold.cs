using UnityEngine;
using UnityEngine.Events;

public class DoOnTreshold : MonoBehaviour
{
    public enum OperationEnum : byte
    {
        [InspectorName("==")] Equal = 0,
        [InspectorName("< ")] StrictlyInferior,
        [InspectorName("<=")] InferiorOrEqual,
        [InspectorName("> ")] StrictlySuperior,
		[InspectorName(">=")] SuperiorOrEqual
    }

    [Tooltip("The operation to execute : `value [Operation] Treshold`")]
    public OperationEnum Operation = OperationEnum.Equal;
    public float Treshold = 0f;

    public UnityEvent OnDo;
    public UnityEvent OnNotDo;

	public void TryToDo(float value)
    {
		if (Operation switch
        {
			OperationEnum.Equal => value == Treshold,
			OperationEnum.StrictlyInferior => value < Treshold,
			OperationEnum.InferiorOrEqual => value <= Treshold,
			OperationEnum.StrictlySuperior => value > Treshold,
			OperationEnum.SuperiorOrEqual => value <= Treshold,
            _ => throw new System.NotImplementedException()
		})
        {
            OnDo.Invoke();
        }
        else
        {
            OnNotDo.Invoke();
        }
    }
}
