using UnityEngine;
using Zenject;

public interface IPoolableObjectFactory : IFactory<GameObject, Vector2, Quaternion, Transform, IPoolableObject>
{
}
