using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PointerUpSlider : Slider, IPointerUpHandler
{
    public Action<float> OnSliderPointerUp;

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        OnSliderPointerUp?.Invoke(value);
    }
}
