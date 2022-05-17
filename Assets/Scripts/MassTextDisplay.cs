using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MassTextDisplay : MonoBehaviour
{
    [SerializeField]
    private PlayerRes playerRes;
    [SerializeField]
    private Text resourceCount;

    void Update()
    {
        resourceCount.text = playerRes.GetCurrentMass().ToString();
    }
}
