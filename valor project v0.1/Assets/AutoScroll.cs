using UnityEngine;
using UnityEngine.UI;

public class AutoScroll : MonoBehaviour
{
    private void OnRectTransformDimensionsChange()
    {
        GetComponentInParent<ScrollRect>().verticalNormalizedPosition = 0;
    }
}
