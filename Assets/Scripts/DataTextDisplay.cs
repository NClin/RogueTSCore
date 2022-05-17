using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataTextDisplay : MonoBehaviour
{
    [SerializeField]
    private PlayerRes playerRes;
    [SerializeField]
    private Text resourceCount;

    void Update()
    {
        resourceCount.text = playerRes.GetCurrentData().ToString();
    }
}
