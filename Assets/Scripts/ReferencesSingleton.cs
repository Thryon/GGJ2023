using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferencesSingleton : MonoBehaviour
{

    public static ReferencesSingleton Instance = null;

    public GameObject treeRef;
    
    void Start()
    {
        if (Instance != null)
            Destroy(this);
        else
            Instance = this;

    }

    public void RegisterTreeRef(GameObject _tree)
    {
        treeRef = _tree;
    }
}
