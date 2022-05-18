using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerReadyIndicator : MonoBehaviour
{
    UnitPower unitPower;
    private SpriteRenderer sr;

    [SerializeField]
    private Color readyColor;
    [SerializeField]
    private Color cdColor;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        unitPower = GetComponentInParent<UnitPower>();
    }

    // Update is called once per frame
    void Update()
    {
        if (unitPower == null)
            unitPower = GetComponentInParent<UnitPower>();

        if (unitPower.IsPowerReady())
            sr.color = readyColor;
        else
            sr.color = cdColor;
    }
}
