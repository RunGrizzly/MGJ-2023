using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ColorSet
{
    public Color MainColor;
    public Color SecColor;
    public Color TertiaryColor;
    public Texture2D Face;
}

[CreateAssetMenu(fileName = "New Color Set", menuName = "Color Sets/New Color Set", order = 1)]
public class TrainColorSet : ScriptableObject
{
    public List<ColorSet> ColorSets;
}