using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangingLight : MonoBehaviour
{
    Light sceneLight;

    // Start is called before the first frame update
    void Start()
    {
        sceneLight = GetComponent<Light>();
        PlayerBall.instance.OnColorChanged += ChangeLight;
    }

    void ChangeLight(BallColor newColor)
    {
        sceneLight.color = ColorMaterialLibrary.instance.GetColorByBallColor(newColor);
    }
}
