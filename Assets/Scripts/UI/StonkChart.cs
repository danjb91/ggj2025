using System.Linq;
using UnityEngine;

public class StonkChart : MonoBehaviour
{
	private CanvasRenderer canvasRenderer;
	private Mesh graphMesh;
	private LineRenderer lineRenderer;
	private Vector3[] linePositions = new Vector3[10];

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Awake()
	{
		canvasRenderer = GetComponent<CanvasRenderer>();
		lineRenderer = GetComponent<LineRenderer>();
		graphMesh = new Mesh();
		canvasRenderer.SetMesh(graphMesh);

		lineRenderer.positionCount = linePositions.Length;
		for	(int x = 0; x < linePositions.Length; x++)
		{
			linePositions[x] = new Vector3(x*50.0f, Random.Range(0.0f, 10.0f), 0.0f);
		}

		lineRenderer.SetPositions(linePositions);
	}

	// Update is called once per frame
	void Update()
	{
		lineRenderer.BakeMesh(graphMesh);
		canvasRenderer.SetMesh(graphMesh);
	}
}
