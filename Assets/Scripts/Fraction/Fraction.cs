using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Fraction: IEquatable<Fraction>
{
    public Action<int> OnGetResource;
    
    [SerializeField] private string _fractionName;
    [SerializeField] private BaseView _fractonBase;
    [SerializeField] private Material _fractionMaterial;
    [SerializeField] private Material _fractionPathMaterial;

    private int _resourcesCount;

    public int ResourcesCount => _resourcesCount;
    public BaseView FractonBase => _fractonBase;
    public Material FractionMaterial => _fractionMaterial;
    public Material FractionPathMaterial => _fractionPathMaterial;
    public string FractionName => _fractionName;

    public void GetResource()
    {
        _resourcesCount++;
        OnGetResource?.Invoke(ResourcesCount);
    }

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
