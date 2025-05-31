using System;
using UnityEngine;

[Serializable]
public class Fraction
{
    [SerializeField] private BaseView _fractonBase;
    [SerializeField] private Material _fractionMaterial;

    public BaseView FractonBase => _fractonBase;
    public Material FractionMaterial => _fractionMaterial;
}
