using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

public class EntityPool : MonoBehaviour
{
    [ReadOnly(true)] public GameObject PoolingObject;
    [ReadOnly] public List<GameObject> Container = new List<GameObject>();

    [ReadOnly(true)] public int StartAllocSize = 5;

    private void Awake()
    {
        for (int i = 0; i < StartAllocSize; i++)
            AddNewObject();

    }

    public GameObject GetObject()
    {
        for (int i = 0; i < Container.Count; i++)
        {
            if (Container[i].activeSelf == false)
            {
                ResetObject(Container[i]);
                return Container[i];
            }
        }

        return AddNewObject();
    }

    private GameObject AddNewObject()
    {
        GameObject NewObject = Instantiate(PoolingObject, transform);
        Container.Add(NewObject);
        return NewObject;
    }


    private void ResetObject(GameObject _Object)
    {
        _Object.transform.localPosition = PoolingObject.transform.localPosition;
        _Object.transform.localScale = PoolingObject.transform.localScale;
        _Object.transform.localRotation = PoolingObject.transform.localRotation;
    }
}
