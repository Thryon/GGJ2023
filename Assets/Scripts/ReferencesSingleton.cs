using System.Collections;
using System.Collections.Generic;
using KinematicCharacterController;
using UnityEngine;

public class ReferencesSingleton : MonoBehaviour
{

    public static ReferencesSingleton Instance = null;

    public GameObject treeRef;
    public Player player;
    
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
