using System.Collections.Generic;
using UnityEngine;

namespace Gamepackage
{
    public class GameObjectPool<TDef> where TDef : UnityEngine.Component
    {
        private GameObject PoolFolder;
        private TDef OverlayTilePrefab;
        private Queue<TDef> _overlayTilePool = new Queue<TDef>();
        private int _poolInitialSize;
        private int _poolStepSize;

        public GameObjectPool(string prefabName, GameObject folder, int poolInitialSize = 20, int poolStepSize = 1)
        {
            OverlayTilePrefab = Resources.Load<TDef>(prefabName);
            _poolInitialSize = poolInitialSize;
            _poolStepSize = poolStepSize;
            PoolFolder = folder;
        }

        public void Init()
        {
            AddEntriesToPool(_poolInitialSize);
        }

        public TDef CheckOut()
        {
            if (_overlayTilePool.Count == 0)
            {
                AddEntriesToPool(_poolStepSize);
            }
            var result = _overlayTilePool.Dequeue();
            result.transform.parent = PoolFolder.transform;
            result.gameObject.SetActive(true);
            return result;
        }

        public void CheckIn(TDef spriteRenderer)
        {
            spriteRenderer.gameObject.SetActive(false);
            spriteRenderer.gameObject.transform.parent = PoolFolder.transform;
            spriteRenderer.transform.position = Vector3.zero;
            _overlayTilePool.Enqueue(spriteRenderer);
        }

        private void AddEntriesToPool(int NumberToAdd)
        {
            for (int i = 0; i < NumberToAdd; i++)
            {
                var go = GameObject.Instantiate(OverlayTilePrefab);
                var spriteRenderer = go.GetComponent<TDef>();
                CheckIn(spriteRenderer);
            }
        }
    }
}
