using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour
{

    public float destroyOnXSeconds = 1.0f;

    void Awake()
    {
        StartCoroutine(Remove());
    }

    private IEnumerator Remove()
    {
        yield return new WaitForSeconds(destroyOnXSeconds);
        Destroy(this.gameObject);
    }
}
