using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BoxController : MonoBehaviour
{
    [SerializeField] HDRColor blue;
    [SerializeField] HDRColor green;
    [SerializeField] HDRColor red;
    [SerializeField] HDRColor yellow;
    [SerializeField] HDRColor white;
    private BoxColor boxColor;
    private HDRColor HDRColor;


    private Material boxMaterial;

    public HDRColor GetHDRColor { get => HDRColor; }
    public BoxColor GetBoxColor { get => boxColor;}

    private void Awake()
    {
        boxMaterial = GetComponent<MeshRenderer>().material;
        
    }
    public void SetBoxColor(BoxColor Color)
    {
        if(Color == BoxColor.Blue)
        {
            boxMaterial.SetColor("_EmissionColor", blue.ToUnityColor());
            HDRColor = blue;
            boxColor = BoxColor.Blue;
        }
        else if(Color == BoxColor.Green)
        {
            boxMaterial.SetColor("_EmissionColor", green.ToUnityColor());
            HDRColor = green;
            boxColor = BoxColor.Green;
        }
        else if (Color == BoxColor.White)
        {
            boxMaterial.SetColor("_EmissionColor", white.ToUnityColor());
            HDRColor = white;
            boxColor = BoxColor.White;
        }
        else if (Color == BoxColor.Yellow)
        {
            boxMaterial.SetColor("_EmissionColor", yellow.ToUnityColor());
            Debug.Log("color set");
            HDRColor = yellow;
            boxColor = BoxColor.Yellow;
        }
        else if (Color == BoxColor.Red)
        {
            boxMaterial.SetColor("_EmissionColor", red.ToUnityColor());
            HDRColor = red;
            boxColor = BoxColor.Red;
        }
    }
    
}
public enum BoxColor{
    Blue,Green,White,Yellow,Red
}
[Serializable]
public class HDRColor
{
    public Color color;
    [Range(-10f, 10f)]
    public float intensity;

    public HDRColor(Color c, float i)
    {
        color = c;
        intensity = i;
    }

    public Color ToUnityColor()
    {
        return color * intensity;
    }
}