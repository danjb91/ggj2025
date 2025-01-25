using System;
using System.Linq;
using UnityEngine;

public class StockChart : MonoBehaviour
{
	private RectTransform rectTransform;
	private CanvasRenderer canvasRenderer;
	private Mesh graphMesh;
	private LineRenderer lineRenderer;
	Vector3[] worldCorners = new Vector3[4];

	private static int historicalValueCount = 30;
	private Vector3[] linePositions = new Vector3[historicalValueCount];
	private float[] stockHistoricalValues = new float[historicalValueCount];

	public StockInstance RepresentedStock { get; set; }

	void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
		canvasRenderer = GetComponent<CanvasRenderer>();
		lineRenderer = GetComponent<LineRenderer>();
		graphMesh = new Mesh();
		canvasRenderer.SetMesh(graphMesh);

		lineRenderer.positionCount = linePositions.Length;
	}

	public void UpdateGraph()
	{
		for	(int x = historicalValueCount - 1; x > 0; x--)
		{
			stockHistoricalValues[x] = stockHistoricalValues[x - 1];
		}

		stockHistoricalValues[0] = RepresentedStock.Delta;

		rectTransform.GetWorldCorners(worldCorners);

		for	(int x = 0; x < historicalValueCount; x++)
		{
			float xLerp =(float)x / (float)linePositions.Length;
			linePositions[x].x = xLerp * rectTransform.rect.width; //, Random.Range(0.0f, 10.0f), 0.0f);
			linePositions[x].y = stockHistoricalValues[x]*100; //(float)Math.Sin(xLerp + Time.time*5)*100;
		}

		lineRenderer.SetPositions(linePositions);
		lineRenderer.BakeMesh(graphMesh);
		canvasRenderer.SetMesh(graphMesh);
	}
}
