using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Scriptable Product")]
public class ScriptableProduct : ScriptableObject {
    public ProductType Type;
    public int Points;
    public ProductController Prefab;
    public GameObject CartPrefab;
}

public enum ProductType
{
    None,
    LoafOfBread,
    WaterBottle,
    Banana,
    ToilerRoll
}