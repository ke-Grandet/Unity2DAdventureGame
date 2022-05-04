using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("узуж")]
    public Image healthMask;

    public static HealthBar Instance;

    private float originalWidth;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        originalWidth = healthMask.rectTransform.rect.width;
    }
    
    public void ChangeValue(float percent)
    {
        healthMask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, percent * originalWidth);
    }

}
