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

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		if (stockSimulation == null)
			return;

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

		foreach (StockChart stockChart in stockCharts)
		{
			stockChart.UpdateGraph();
		}
	}

	private void OnStockAdded(object sender, StockInstance e)
	{
		GameObject newStockChartGO = stockChartPool.Pool.Get();
		newStockChartGO.transform.SetParent(this.transform);
		StockChart newStockChart = newStockChartGO.GetComponent<StockChart>();
		newStockChart.RepresentedStock = e;
		stockCharts.Add(newStockChart);
	}

	private void OnStockCrashed(object sender, StockInstance e)
	{
		stockCharts.RemoveAll(stockChartInstance => stockChartInstance.RepresentedStock == e);
	}
}
