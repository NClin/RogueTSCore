using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;
using Pathfinding;

#nullable enable

/// <summary>
/// Selects and deselects selectable units based on input.
/// </summary>
/// 
public class SelectionManager : MonoBehaviour
{
    
    private List<SelectableUnit> selectedUnits;
    private List<SelectableUnit> pathCallbackTargets;
    [SerializeField]
    private float longClickThreshold = 0.05f;
    [SerializeField]
    private float minMouseMovement = 0.3f;
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

    //private FormationStamp formationstamp; // temporarily deprecated?


    void Start()
    {
        selectedUnits = new List<SelectableUnit>();
        selectionBoxes = new List<GameObject>();
        //formationstamp = GetComponent<FormationStamp>();
        //if (formationstamp == null)
        //{
        //    Debug.Log("added formationstamp");
        //    gameObject.AddComponent<FormationStamp>();
        //}
    }

    void Update()
    {
        ProcessInput();
    }

    private void DrawSelectionRect()
    {
        var center = VectorTools.GetBoxCenter(longClickStart, longClickCurrent);
        var extents = VectorTools.GetBoxExtents(longClickStart, longClickCurrent);

        Rect toDraw = new Rect(center - extents / 2, extents);
        using (Draw.Command(Camera.main))
        {
            Draw.Color = Color.green;
            Draw.Matrix = transform.localToWorldMatrix;
            Draw.LineGeometry = LineGeometry.Volumetric3D;
            Draw.RectangleBorder(toDraw, 0.1f);
        }

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
        start.z = 0f;
        end.z = 0f;

        var boxCenter = VectorTools.GetBoxCenter(start, end);
        var boxExtents = VectorTools.GetBoxExtents(start, end);

        var potentials = Physics.OverlapBox(boxCenter, boxExtents/2);

        ClearSelection();

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

        var startTile = VectorTools.GetClosestTileCoordinatesV3(start);
        var endTile = VectorTools.GetClosestTileCoordinatesV3(end);


        ClearSelectionBox();

        if (selectionBoxInstance == null)
        {
            //selectionBoxes.Add(Instantiate(selectionBox, startTile, Quaternion.identity));
        }

        // hurrr
        int minx = startTile.x < endTile.x ? (int)startTile.x : (int)endTile.x;
        int maxx = startTile.x > endTile.x ? (int)startTile.x : (int)endTile.x;

        int miny = startTile.y < endTile.y ? (int)startTile.y : (int)endTile.y;
        int maxy = startTile.y > endTile.y ? (int)startTile.y : (int)endTile.y;



        for (int x = minx; x <= maxx; x++)
        {
            for (int y = miny; y <= maxy; y++)
            {
                //selectionBoxes.Add(Instantiate(selectionBox, new Vector3(x, y, 0), Quaternion.identity));
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
                longClickCurrent.z = 0; // hack
                DrawSelectionRect();
                DrawLongClickBox(longClickStart, longClickCurrent);
            }

            if (mouse0heldTime > longClickThreshold && !longClick)
            {
                Debug.Log("LongLeftStarted");
                longClickStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                longClickStart.z = 0; // hack
                longClick = true;
            } 
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (longClick && Vector3.Distance(longClickStart, longClickCurrent) > minMouseMovement)
            {
                DoLongLeftClick(longClickStart, longClickCurrent);
            }
            else
            {
                DoLeftClick();
            }
            longClick = false;
            mouse0held = false;
            mouse0heldTime = 0;
            ClearSelectionBox();
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
                //formationstamp.StampLine(longClickStart, longClickCurrent);
            }

        }

        if (Input.GetMouseButtonUp(1))
        {
            if (longClick && Vector3.Distance(longClickStart, longClickCurrent) > minMouseMovement)
            {
                StartCoroutine(LineAbreastFormationMovement(longClickStart, longClickCurrent, selectedUnits));

                DoLongRightClick();
            }
            else
            {
                DoRightClick();
            }
            mouse1held = false;
            mouse1heldTime = 0;
            //formationstamp.ClearStamp();
            longClick = false;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            if (selectedUnits.Count > 0)
            {
                foreach (SelectableUnit unit in selectedUnits)
                {
                    if (unit.GetComponent<MovementStripped>())
                    {
                        unit.GetComponent<MovementStripped>().StopOrder();
                    }
                }
            }
        }

    }

    private void DoRightClick()
    {
        if (selectedUnits.Count > 0)
        {
            foreach (SelectableUnit unit in selectedUnits)
            {
                if (unit == null) { continue; }

                if (unit.GetComponent<MovementStripped>())
                {
                    unit.GetComponent<MovementStripped>().MoveTo(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                }
            }
        }
    }

    public void OnPathCallback(Path p)
    {

    }

    private void DoLongRightClick()
    {
        // some kind of formation move order.
    }

    private IEnumerator LineAbreastFormationMovement(Vector3 lineStart, Vector3 lineEnd, List<SelectableUnit> units)
    {
        var linePositions = VectorTools.BesenhamLine(VectorTools.GetClosestTileCoordinatesV2Int(lineStart),
            VectorTools.GetClosestTileCoordinatesV2Int(lineEnd));
        

        Vector2 rowTranslation = VectorTools.GetVectorFormationRowTranslation(lineStart, lineEnd);

        Debug.Log("moving units count: " + units.Count);

        int i = 0;
        int row = 0;
        foreach (SelectableUnit unit in units)
        {
            if (i >= linePositions.Count)
            {
                row++;
                i = 0;
                yield return new WaitForSeconds(0.1f * row);
            }

            unit.GetComponent<MovementStripped>().MoveTo(linePositions[i] + row * rowTranslation);
            i++;
        }


    }

    private void DoLeftClick()
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
}
