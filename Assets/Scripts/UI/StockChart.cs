using System;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class StockChart : MonoBehaviour
{
	private RectTransform rectTransform;
	private CanvasRenderer canvasRenderer;
	private Mesh graphMesh;
	private LineRenderer lineRenderer;
	private Image imageSprite;

	Vector3[] worldCorners = new Vector3[4];

	private static int historicalValueCount = 30;
	private Vector3[] linePositions = new Vector3[historicalValueCount];
	private float[] stockHistoricalValues = new float[historicalValueCount];

	public StockInstance RepresentedStock { get; set; }

	public float MaxChartValue { get; set; }

	public float MaxHistoricalValue 
	{
		get
		{
			float max = 0.0f;
			foreach	(float value in stockHistoricalValues)
			{
				if (value > max)
				{
					max = value;
				}
			}

			return max;
		}
	}

	void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
		canvasRenderer = GetComponent<CanvasRenderer>();
		lineRenderer = GetComponent<LineRenderer>();
		imageSprite = GetComponent<Image>();
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

		stockHistoricalValues[0] = RepresentedStock.Price;

		imageSprite.color = RepresentedStock.Color;
		lineRenderer.startColor = RepresentedStock.Color;
		lineRenderer.endColor = RepresentedStock.Color;
		rectTransform.GetWorldCorners(worldCorners);

		for	(int x = 0; x < historicalValueCount; x++)
		{
			float xLerp =(float)x / (float)linePositions.Length;
			linePositions[x].x = xLerp * rectTransform.rect.width; //, Random.Range(0.0f, 10.0f), 0.0f);
			linePositions[x].y = Mathf.Lerp(-242.8f * 2.0f, 242.8f * 2.0f, RescaleStockValue(stockHistoricalValues[x])); //(float)Math.Sin(xLerp + Time.time*5)*242.8f*2.0f;
		}

		lineRenderer.SetPositions(linePositions);
		lineRenderer.BakeMesh(graphMesh);
		canvasRenderer.SetMesh(graphMesh);
	}

	private float RescaleStockValue(float stockValue)
	{
		float value = stockValue / MaxChartValue;
		return value;
	}
}
