using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StockChartManager : MonoBehaviour
{
	[SerializeField]
	private StockSimulation stockSimulation;

	[SerializeField]
	private PrefabPool stockChartPool;

	private readonly List<StockChart> stockCharts = new List<StockChart>();

	private float storedChartMax = 50.0f;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		FindStockSimulation();
	}

    private void FindStockSimulation()
    {
		if (stockSimulation != null) return;

		stockSimulation = GameManager.Instance?.stockSim;

		if (stockSimulation == null) return;

        stockSimulation.OnSimulationTick += OnStockSimulationTick;
		stockSimulation.StockAdded += OnStockAdded;
		stockSimulation.StockCrashed += OnStockCrashed;

		foreach (StockInstance stockInstance in stockSimulation.CurrentStocks)
		{
			OnStockAdded(null, stockInstance);
		}
    }

    void OnStockSimulationTick()
	{
		if (stockSimulation == null)
			return;

		float chartMax = 50.0f;
		foreach (StockChart stockChart in stockCharts)
		{
			float historicalMax = stockChart.MaxHistoricalValue;
			if (historicalMax > chartMax)
			{
				chartMax = historicalMax;
			}
		}

		storedChartMax = chartMax;

		foreach (StockChart stockChart in stockCharts)
		{
			stockChart.MaxChartValue = chartMax;
			stockChart.UpdateGraph();
		}
	}

	private void OnStockAdded(object sender, StockInstance e)
	{
		GameObject newStockChartGO = stockChartPool.Pool.Get();
		newStockChartGO.transform.SetParent(this.transform, false);
		StockChart newStockChart = newStockChartGO.GetComponent<StockChart>();
		newStockChart.RepresentedStock = e;
		newStockChart.MaxChartValue = storedChartMax;
		stockCharts.Add(newStockChart);
	}

	private void OnStockCrashed(object sender, StockInstance e)
	{
		StockChart chart = stockCharts.Find(stockChartInstance => stockChartInstance.RepresentedStock == e);
		stockCharts.RemoveAll(stockChartInstance => stockChartInstance.RepresentedStock == e);

		stockChartPool.Pool.Release(chart.gameObject);
	}

	private void Update()
	{
		FindStockSimulation();
	}
}
