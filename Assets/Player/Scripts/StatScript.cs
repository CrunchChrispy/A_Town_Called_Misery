using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class StatScript : MonoBehaviour
{
    public LightDetection LightDetection;
    public Slider slider;

    void Start()
    {
        slider.value = .5f;
    }

    void Update()
    {
        slider.value = LightDetection.s_fLightValue;
    }
}
