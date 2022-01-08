using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetDefaultParent : MonoBehaviour
{
    [SerializeField]
    private string defaultParentName;
    void Start()
    {
        GameObject parentObj = GameObject.Find(defaultParentName);

        if (parentObj == null)
        {
            return;
        }
        else
        {
            Transform parent = GameObject.Find(defaultParentName).transform;
            
            if (transform.parent == null)
            {
                transform.SetParent(parent);
            }
        }
    }
}
