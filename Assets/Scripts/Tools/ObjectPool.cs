using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPool
{
    const int POOLSIZE = 32;

    public string sObjectName = "ObjPool";
    public GameObject tObjectTemplate = null;

    GameObject tPoolObject = null;
    Stack<GameObject> tObjects;

    public ObjectPool()
    {
        tPoolObject = new GameObject(sObjectName);
        Init();
    }

    public void Init()
    {
        tObjects = new Stack<GameObject>();
        IncreasePool();
    }

    public virtual void InitObject(GameObject tObject)
    {
        tObject.transform.position = Vector3.zero;
        tObject.transform.rotation = Quaternion.identity;
    }

    public void IncreasePool()
    {
        for (int i = 0; i < POOLSIZE; ++i)
        {
            GameObject obj = GameObject.Instantiate(tObjectTemplate) as GameObject;
            obj.transform.SetParent(tPoolObject.transform);
            obj.SetActive(false);
            tObjects.Push(obj);
        }
    }

    public GameObject GetObject()
    {
        GameObject tNewObject = null;
        int iObjCount = tObjects.Count;

        if (iObjCount <= 0)
        {
            IncreasePool();
        }

        iObjCount = tObjects.Count;
        if (iObjCount > 0)
        {
            tNewObject = tObjects.Pop();
        }

        if (tNewObject != null)
        {
            ObjectPoolMember tOPM = tNewObject.AddComponent<ObjectPoolMember>();
            tOPM.SetPool(this);

            InitObject(tNewObject);

            tNewObject.SetActive(true);
        }

        return tNewObject;
    }

    public void RemoveObject(GameObject tObject)
    {
        tObject.SetActive(false);
        tObjects.Push(tObject);
    }
}
