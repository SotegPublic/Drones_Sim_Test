using UnityEngine;
using Zenject;

public interface IPoolableObjectFactory : IFactory<GameObject, Vector3, Quaternion, Transform, IPoolableObject>
{
}
