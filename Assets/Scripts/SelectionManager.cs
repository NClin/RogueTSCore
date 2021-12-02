using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Selects and deselects selectable units based on input.
/// </summary>
public class SelectionManager : MonoBehaviour
{
    private List<SelectableUnit> selectedUnits;

    void Start()
    {
        selectedUnits = new List<SelectableUnit>();
    }

    void Update()
    {
        ProcessInput();
    }

    private void AddToSelection(SelectableUnit toSelect)
    {
        if (!selectedUnits.Contains(toSelect))
        {
            selectedUnits.Add(toSelect);
            toSelect.selected = true;
        }
    }

    private void RemoveFromSelection(SelectableUnit toRemove)
    {
        if (selectedUnits.Contains(toRemove))
        {
            toRemove.selected = false;
            selectedUnits.Remove(toRemove);
        }
    }

    private void ClearSelection()
    {
        if (selectedUnits.Count > 0)
        {

            for (int i = selectedUnits.Count - 1; i >= 0; i--)
            {
                RemoveFromSelection(selectedUnits[i]);
            }
        }
    }

    private void ProcessInput()
    {
        // Toggle selection on single unit.
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition)))
            {
                RaycastHit hitInfo;
                Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);

                if (hitInfo.collider.gameObject.GetComponent<SelectableUnit>())
                {
                    var clicked = hitInfo.collider.gameObject.GetComponent<SelectableUnit>();
                    if (selectedUnits.Contains(clicked) && Input.GetKey(KeyCode.LeftShift))
                    {
                        RemoveFromSelection(clicked);
                    }
                    else
                    { 
                        if (!Input.GetKey(KeyCode.LeftShift)) { ClearSelection(); }

                        AddToSelection(hitInfo.collider.gameObject.GetComponent<SelectableUnit>());
                    }
                }
            }
            else
            {
                ClearSelection();
            }
        }

        // Give order to selected units.
        if (Input.GetMouseButtonDown(1))
        {
            if (selectedUnits.Count > 0)
            {
                foreach (SelectableUnit unit in selectedUnits)
                {
                    if (unit.GetComponent<MoveableUnit>())
                    {
                        unit.GetComponent<MoveableUnit>().MoveTo(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                    }
                }
            }
        }

    }

}
