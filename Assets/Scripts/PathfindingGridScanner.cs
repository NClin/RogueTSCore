using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingGridScanner
{
    float offsetX = 0.5f;
    float offsetY = 0.5f;

    public IEnumerator ScanAtEndOfFrame(int height, int width)
    {
        yield return new WaitForEndOfFrame();
        Scan(height, width);
    }

    private void Scan(int height, int width)
    {
        AstarPath.active.data.gridGraph.SetDimensions(width, height, 1f);
        var center = new Vector3(width / 2 - offsetX, height / 2 - offsetY, 0);
        AstarPath.active.data.gridGraph.center = center;
        AstarPath.active.Scan();
    }

}
