using UnityEngine;

public class StrawScaleManager : MonoBehaviour
{
    static float minMoneyRange = 0.0f;
    static float maxMoneyRange = 1000.0f;

    static float minScale = 2.0f;
    static float maxScale = 4.0f;

    public int owner = 0;
    private void LateUpdate()
    {
        float money = Mathf.Max(minMoneyRange, (float)GameManager.Instance.stockSim.currentMoney[owner]);
        float moneyRatio = money / maxMoneyRange;
        Debug.Log("Owner " + owner + " has money " + money);
        float scale = minScale + (maxScale - minScale) * moneyRatio;

        gameObject.transform.localScale = new Vector3(scale, gameObject.transform.localScale.y, scale);

    }

}
