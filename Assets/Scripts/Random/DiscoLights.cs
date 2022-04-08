using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoLights : MonoBehaviour
{
    public Light lerpedColor;

    void Update()
    {
        lerpedColor.color = Color.Lerp(Color.red, Color.yellow, Mathf.PingPong(Time.time, 1));
    }
}
