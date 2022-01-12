using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coroutineAnalysis : MonoBehaviour
{
    
    void Start()
    {
        StartCoroutine(Counting());   
    }

    private IEnumerator Counting()
    {
        int count = 0;
        Debug.Log("atrium");

        while (count < 10)
        {
            count++;
            Debug.Log(count);
            yield return null;
        }

        Debug.Log("exit");
        yield return null;
    }
}
