using System;
using UnityEngine;

[Serializable]
public class Fraction: IEquatable<Fraction>
{
    [SerializeField] private BaseView _fractonBase;
    [SerializeField] private Material _fractionMaterial;

    public BaseView FractonBase => _fractonBase;
    public Material FractionMaterial => _fractionMaterial;

    public bool Equals(Fraction other)
    {
        if (other is null) return false;
        return _fractonBase.GetInstanceID() == other.FractonBase.GetInstanceID() &&
            _fractonBase.name == other.FractonBase.name;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_fractonBase.name, _fractonBase.GetInstanceID());
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as Fraction);
    }
}
