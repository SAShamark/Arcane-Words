using System;
using UnityEngine;

namespace Services.ObjectPooling
{
    public interface IPoolable
    {
        GameObject GameObject { get; }
        event Action<IPoolable> OnReturnToPool;
        void Reset();
    }
}