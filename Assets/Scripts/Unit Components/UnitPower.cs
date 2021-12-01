using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPower : MonoBehaviour
{
    private IPower powerScript;

    private void Start()
    {
        powerScript = new Teleport(); // test entry, to be constructed elsewhere.
    }

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
        if (Input.GetKeyDown(KeyCode.F))
        {
            var clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2Int tile = new Vector2Int(Mathf.RoundToInt(clickPos.x), Mathf.RoundToInt(clickPos.y));
            Use(tile);
        }
    }

}
