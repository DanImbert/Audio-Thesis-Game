using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadableComponent : MonoBehaviour
{
    public GameObject interfacePrefab;
   public void OnPlayerRead()
    {
        if(interfacePrefab!=null && ReadingPanel.main!=null)
        {
            ReadingPanel.main.ReadItem(interfacePrefab);
        }
    }
}
