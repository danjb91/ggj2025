using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Stonk", menuName = "Scriptable Objects/Stonk")]
public class Stonk : ScriptableObject
{
    [field: SerializeField]
    public float Price { get; set; }
    [field: SerializeField]
    public float UpperBound { get; set; }
    [field: SerializeField]
    public float LowerBound { get; set; }
    // How much the stock bound is affected by sales or purchases
    [field: SerializeField]
    public int TotalShares { get; set; }
    // How much has the stock last changed
    public float Delta { get; set; }

    // How much the stock floor and ceiling can change per tick
    [field: SerializeField]
    public float Volatility { get; set; }

    [field: SerializeField]
    public Color32 Color { get; set; }

    [field: SerializeField]
    public Material Material { get; set; }

    public Stonk() => (Price, UpperBound, LowerBound, TotalShares, Volatility, Color, Delta) = (0f, 0f, 0f, 100, 1f, new Color32(), 0f);
}
