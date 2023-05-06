using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public Transform activeObjs;
    public Transform inactiveObjs;

    public virtual GameObject PoolItem(GameObject Prefab)
    {
        foreach (Transform child in inactiveObjs)
        {
            if (!child.gameObject.activeSelf && Prefab.name == child.name.Substring(0, Prefab.name.Length))
            {
                ActivateObject(child.gameObject);
                return child.gameObject;
            }
        }

        return InitFromPrefab(Prefab);
    }
    protected virtual GameObject InitFromPrefab(GameObject Prefab)
    {
        GameObject nEnemy = GameObject.Instantiate(Prefab);
        nEnemy.name = Prefab.name;
        ActivateObject(nEnemy);
        return nEnemy;
    }
    public virtual void ActivateObject(GameObject gOb)
    {
        gOb.transform.SetParent(activeObjs);
        gOb.SetActive(true);
    }
    public virtual void DeactivateObject(GameObject gOb)
    {
        if (gOb == null) return;
        gOb.transform.SetParent(inactiveObjs);
        gOb.SetActive(false);

    }


    public int GetNActiveObjects()
    {
        return activeObjs.childCount;
    }
}
