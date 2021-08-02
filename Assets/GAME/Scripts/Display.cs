using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Display : MonoBehaviour
{

}
public abstract class DisplayRatioBar : Display
{
    [SerializeField] Slider slider;

    float _ratio;
    public float ratio
    {
        get
        {
            return _ratio;
        }
        set
        {
            if (value < 0)
            {
                _ratio = 0;
            }
            else
            {
                _ratio = value;
            }
            slider.value = _ratio;
        }
    }
}
