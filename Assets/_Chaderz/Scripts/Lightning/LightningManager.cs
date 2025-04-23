using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningManager : MonoBehaviour
{
    [SerializeField]
    private float _timeBetweenNew, _range;

    [SerializeField]
    private GameObject _lightning;

    void Start()
    {
        StartCoroutine(LightningCoroutine());
    }

    private IEnumerator LightningCoroutine()
    {
        while (true)
        {
            Vector2 vec = Random.insideUnitCircle;
            vec *= Random.Range(0, _range);
            Vector3 pos = new Vector3(vec.x, 0, vec.y);

            Instantiate(_lightning, pos, Quaternion.identity);

            yield return new WaitForSeconds(_timeBetweenNew);
        }
    }
}
