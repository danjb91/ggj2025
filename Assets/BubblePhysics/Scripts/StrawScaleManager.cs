using UnityEngine;

public class StrawScaleManager : MonoBehaviour
{
    public static float minMoneyRange = 0.0f;
    public static float maxMoneyRange = 1000.0f;

    public static float minScale = 1.0f;
    public static float maxScale = 3.0f;

    public int owner = 0;
    private void LateUpdate()
    {
        float money = Mathf.Max(minMoneyRange, (float)GameManager.Instance.stockSim.currentMoney[owner]);
        float moneyRatio = money / maxMoneyRange;
        float scale = Mathf.Min(maxScale, minScale + (maxScale - minScale) * moneyRatio);

        gameObject.transform.localScale = new Vector3(scale, gameObject.transform.localScale.y, scale);

    }

}
