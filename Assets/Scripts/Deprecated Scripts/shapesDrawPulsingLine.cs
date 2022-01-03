using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

[ExecuteAlways]
public class shapesDrawPulsingLine : ImmediateModeShapeDrawer
{

	float t = 0;

	public float frequency = 1;
	public float thickness = 5;
	public Vector3 start;
	public Vector3 end;

	public bool drawing = false;

	public override void DrawShapes(Camera cam)
	{

		float tSin = Mathf.Abs(Mathf.Sin(t * frequency));


		using (Draw.Command(cam))
		{

			if (drawing == false) return;

			Draw.Thickness = thickness * tSin;
			Draw.Line(start, end, Color.blue);
		}

	}

    private void Awake()
    {
		Draw.LineGeometry = LineGeometry.Volumetric3D;
		Draw.ThicknessSpace = ThicknessSpace.Pixels;
	}


    private void Start()
    {
		Draw.Matrix = transform.localToWorldMatrix;
	}

	private void Update()
    {
		if (drawing)
		{
			drawing = Validate();
		}

		t += Time.deltaTime;
		Draw.Matrix = transform.localToWorldMatrix;
	}

	public void DrawPulsingLine(Vector3 start, Vector3 end)
    {
		this.start = start;
		this.end = end;
	}

	private bool Validate()
    {
		if (start == null || end == null)
		{
			return false;
		}
		else return true;
    }

}
