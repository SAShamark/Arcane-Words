using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Services.ObjectPooling
{
    public class ObjectPool<T> where T : MonoBehaviour, IPoolable
    {
        private List<T> _includedPool;
        private List<T> _excludedPool;

        private readonly DiContainer _diContainer;
        private readonly T _prefab;
        private readonly Transform _container;

        public ObjectPool(T prefab, int count, Transform container)
        {
            _prefab = prefab;
            _container = container;
            CreatePool(count);
        }

        public ObjectPool(T prefab, int count, Transform container, DiContainer diContainer)
        {
            _diContainer = diContainer;
            _prefab = prefab;
            _container = container;
            CreatePool(count);
        }

        private void CreatePool(int count)
        {
            _excludedPool = new List<T>();
            _includedPool = new List<T>();

            for (int i = 0; i < count; i++)
            {
                CreateObject(false);
            }
        }

        private T CreateObject(bool isActiveByDefault)
        {
            T createdObject = _diContainer != null
                ? _diContainer.InstantiatePrefabForComponent<T>(_prefab, _container)
                : Object.Instantiate(_prefab, _container);

            createdObject?.Reset();

            if (isActiveByDefault)
            {
                createdObject?.gameObject.SetActive(true);
                _includedPool.Add(createdObject);
            }
            else
            {
                createdObject?.gameObject.SetActive(false);
                _excludedPool.Add(createdObject);
            }

            return createdObject;
        }

        private bool HasFreeElement(out T element)
        {
            if (_excludedPool.Count > 0)
            {
                element = _excludedPool[0];
                element.gameObject.SetActive(true);
                _includedPool.Add(element);
                _excludedPool.RemoveAt(0);
                return true;
            }

            element = null;
            return false;
        }

        public void TurnOffObject(T element)
        {
            element.gameObject.SetActive(false);
            _includedPool.Remove(element);
            _excludedPool.Add(element);
        }

        public T GetFreeElement()
        {
            return HasFreeElement(out T element) ? element : CreateObject(true);
        }
    }
}