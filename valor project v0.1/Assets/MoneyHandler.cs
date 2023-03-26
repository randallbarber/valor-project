using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class MoneyHandler : MonoBehaviour
{
    public float money = 0f;
    public TMP_Text label;
    public void addMoney(float amount)
    {
        money += amount;
        label.text = "Money: " + money.ToString();
    }
}
