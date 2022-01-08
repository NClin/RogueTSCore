using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

/// <summary>
/// Selects and deselects selectable units based on input.
/// </summary>
/// 
public class SelectionManager : MonoBehaviour
{
    
    private List<SelectableUnit> selectedUnits;
    [SerializeField]
    private float longClickThreshold = 0.05f;
    private bool mouse0held = false;
    private bool mouse1held = false;
    private float mouse0heldTime = 0;
    private float mouse1heldTime = 0;
    private bool longClick = false;
    Vector3 longClickStart; 
    Vector3 longClickCurrent;
    Vector3 longClickEnd;

    [SerializeField]
    GameObject selectionBox;
    private GameObject? selectionBoxInstance;
    List<GameObject> selectionBoxes;

    private FormationStamp formationstamp;


    void Start()
    {
        selectedUnits = new List<SelectableUnit>();
        selectionBoxes = new List<GameObject>();
        formationstamp = GetComponent<FormationStamp>();
        if (formationstamp == null)
        {
            Debug.Log("added formationstamp");
            gameObject.AddComponent<FormationStamp>();
        }
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

    private void DoLongLeftClick(Vector3 start, Vector3 end)
    {
        // Hack z value:
        // Can implement this with ray to tilemap, get a point, start there?
        start.z = -.1f;
        end.z = .1f;

        var boxCenter = VectorTools.GetBoxCenter(start, end);
        var boxExtents = VectorTools.GetBoxExtents(start, end);

        var potentials = Physics.OverlapBox(boxCenter, boxExtents);
        Debug.Log(potentials.Length + " overlaps");
        foreach (Collider potential in potentials)
        {
            if (potential.gameObject.GetComponent<SelectableUnit>() != null)
            {
                AddToSelection(potential.gameObject.GetComponent<SelectableUnit>());
                Debug.Log("selected a unit");
            }
        }
    }

    private void ClearSelectionBox()
    {
        foreach (GameObject box in selectionBoxes)
        {
            Destroy(box);
        }
        selectionBoxes = new List<GameObject>();
    }


        /// <summary>
        /// Very Buggy.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
    private void DrawLongClickBox(Vector3 start, Vector3 end)
    {
        //        Vector3 center = VectorTools.GetBoxCenter(start, end);

        Debug.Log("start = " + start);
        Debug.Log("end = " + end);

        var startTile = VectorTools.GetClosestTileCoordinatesV3(start);
        var endTile = VectorTools.GetClosestTileCoordinatesV3(end);

        Debug.Log("startTile = " + startTile);
        Debug.Log("endTile = " + endTile);

        ClearSelectionBox();

        if (selectionBoxInstance == null)
        {
            selectionBoxes.Add(Instantiate(selectionBox, startTile, Quaternion.identity));
        }

        // hurrr
        int minx = startTile.x < endTile.x ? (int)startTile.x : (int)endTile.x;
        int maxx = startTile.x > endTile.x ? (int)startTile.x : (int)endTile.x;

        int miny = startTile.y < endTile.y ? (int)startTile.y : (int)endTile.y;
        int maxy = startTile.y > endTile.y ? (int)startTile.y : (int)endTile.y;

        Debug.Log("minx = " + minx);
        Debug.Log("maxx = " + maxx);
        Debug.Log("miny = " + miny);
        Debug.Log("maxy = " + maxy);


        for (int x = minx; x <= maxx; x++)
        {
            for (int y = miny; y <= maxy; y++)
            {
                Debug.Log("Drawing at " + new Vector3(x, y, 0));
                selectionBoxes.Add(Instantiate(selectionBox, new Vector3(x, y, 0), Quaternion.identity));
            }
        }



    }

    private void ProcessInput()
    {
        if(Input.GetMouseButton(0))
        {
            mouse0held = true;
            mouse0heldTime += Time.deltaTime;

            if (longClick)
            {
                longClickCurrent = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                DrawLongClickBox(longClickStart, longClickCurrent);
            }

            if (mouse0heldTime > longClickThreshold && !longClick)
            {
                Debug.Log("LongLeftStarted");
                longClickStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                longClick = true;
            } 
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (longClick)
            {
                DoLongLeftClick(longClickStart, longClickCurrent);
            }
            longClick = false;
            mouse0held = false;
            mouse0heldTime = 0;
            ClearSelectionBox();
        }


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

        if (Input.GetMouseButton(1))
        {
            mouse1held = true;
            mouse1heldTime += Time.deltaTime;

            if (mouse1heldTime > longClickThreshold && !longClick)
            {
                longClickStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                longClick = true;
            }

            if (longClick)
            {
                longClickCurrent = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                formationstamp.StampLine(longClickStart, longClickCurrent);
            }

        }

        // Give order to selected units.
        if (Input.GetMouseButtonDown(1))
        {

            if (selectedUnits.Count > 0)
            {
                foreach (SelectableUnit unit in selectedUnits)
                {
                    if (unit.GetComponent<Movement>())
                    {
                        unit.GetComponent<Movement>().MoveTo(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            if (longClick)
            {
                DoLongClick(1);
            }
            mouse1held = false;
            mouse1heldTime = 0;
            formationstamp.ClearStamp();
            longClick = false;
        }

    }

    private void DoLongClick(int mouseButton)
    {
        // some kind of formation move order.
        Debug.Log("formation move order");
    }

}
