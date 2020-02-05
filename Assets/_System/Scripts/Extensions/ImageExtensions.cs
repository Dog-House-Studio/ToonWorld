using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Image Extensions is a script that contains
/// various extension methods for the image
/// class.
/// 
/// Note : I have intentionally not wrapped this
/// class inside a namespace so that this extension
/// is available by default.
/// </summary>
public static class ImageExtensions
{
    //This sets the alpha of the image.
    //This is helpful for setting image transparency in
    //unity events.
    public static void SetAlphaTransparency(this Image image, float value)
    {
        value = Mathf.Clamp01(value);
        Color c = image.color;
        c.a = value;
        image.color = c;
    }
}
