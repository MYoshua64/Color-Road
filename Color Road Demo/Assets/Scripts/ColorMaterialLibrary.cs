using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ColorMaterialLibrary : MonoBehaviour
{
    public static ColorMaterialLibrary instance;

    private void Awake()
    {
        instance = this;
    }

    [System.Serializable]
    public class ColorToMaterial
    {
        public BallColor color;
        public Material material;
    }

    [System.Serializable]
    public class BallColorToColor
    {
        public BallColor ballColor;
        public Color color;
    }

    [SerializeField] List<ColorToMaterial> libraryNormal;
    [SerializeField] List<ColorToMaterial> libraryMultiplier;
    [SerializeField] List<BallColorToColor> libraryColors;

    public Material GetMaterialByColor(BallColor _color, bool multiplierBall = false)
    {
        ColorToMaterial _data;
        if (multiplierBall)
        {
            _data = libraryMultiplier.First(ball => ball.color == _color);
        }
        else
        {
            _data = libraryNormal.First(ball => ball.color == _color);
        }
        return _data.material;
    }

    public Color GetColorByBallColor(BallColor _color)
    {
        BallColorToColor _data = libraryColors.First(item => item.ballColor == _color);
        return _data.color;
    }
}
