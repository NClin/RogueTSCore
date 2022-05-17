using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPower : MonoBehaviour
{
    private IPower powerScript;


    private void Update()
    {
        powerScript.OnUpdate();
        DoPowerInput();
    }

    public void Use(Vector2Int targetTile)
    {
        powerScript.OnUse(targetTile, gameObject);
    }

    private void DoPowerInput()
    {
        var selectable = GetComponent<SelectableUnit>();

        if (powerScript == null) return;
        if (selectable == null) return;

        if (Input.GetKeyDown(KeyCode.F) && selectable.selected)
        {
            Debug.Log("got heal order");
            var clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2Int tile = new Vector2Int(Mathf.RoundToInt(clickPos.x), Mathf.RoundToInt(clickPos.y));
            Use(tile);
        }
    }

    public void SetPower(IPower power)
    {
        powerScript = power;
    }

}
