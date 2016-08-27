using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("LudumDareResources/Utility/Object Pools")]
public class ObjectPools : MonoBehaviour {

	static Dictionary<string, List<PoolItem>> pools = new Dictionary<string, List<PoolItem>>();
	static Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();

	public class PoolItem
	{
		public GameObject GameObject;
		public Transform Transform;
		public MonoBehaviour Script;

		public static PoolItem Create<T>(GameObject obj) where T : MonoBehaviour
		{
			return new PoolItem()
			{
				GameObject = obj,
				Transform = obj.transform,
				Script = obj.GetComponent<T>()
			};
		}
	}

	[System.Serializable]
	public class PoolDefinition
	{
		public string Name;
		public GameObject Prefab;
	}

	public PoolDefinition[] PoolDefinitions;

	#region Unity Lifecycle
	void Awake()
	{
		CleanupPools();
		SetupPools();
	}
	#endregion

	#region Utility Methods
	void SetupPools()
	{
		for (int i = 0; i < PoolDefinitions.Length; i++)
		{
			if (!pools.ContainsKey(PoolDefinitions[i].Name))
				pools[PoolDefinitions[i].Name] = new List<PoolItem>();

			if (!prefabs.ContainsKey(PoolDefinitions[i].Name))
				prefabs[PoolDefinitions[i].Name] = PoolDefinitions[i].Prefab;
		}
	}

	void CleanupPools()
	{
		int i = 0;
		foreach (var pool in pools.Values)
		{
			i = 0;
			while (i < pools.Count)
			{
				if (pool[i] == null)
					pool.RemoveAt(i);
				else
					i++;
			}
		}
	}
	#endregion

	static PoolItem tmpPI;
	public static T Spawn<T>(string poolName, Vector3 position, Transform parent) where T : MonoBehaviour
	{
        T result = null;

		// Try to get the object from the pool
		foreach (var pi in pools[poolName])
		{
			if (!pi.GameObject.activeSelf)
			{
				pi.Transform.parent = parent;
				pi.Transform.position = position;
				pi.GameObject.SetActive(true);
				result = pi.Script as T;
                break;
			}
		}

        // Otherwise, instanitate
        if (result == null)
        {
            tmpPI = PoolItem.Create<T>((GameObject)Instantiate(prefabs[poolName]));
            Debug.Log(tmpPI);
            tmpPI.Transform.parent = parent;
            tmpPI.Transform.position = position;
            pools[poolName].Add(tmpPI);
            result = tmpPI.Script as T;
        }

        if (result is IObjectPoolable)
            ((IObjectPoolable)result).Initialize();

        return result;
	}

	public static void Despawn(MonoBehaviour obj)
	{
        if (obj is IObjectPoolable)
            ((IObjectPoolable)obj).Disable();

        obj.gameObject.SetActive(false);
	}
}

public interface IObjectPoolable
{
    void Initialize();
    void Disable();
}
