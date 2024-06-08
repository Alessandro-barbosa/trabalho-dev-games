using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeManager : MonoBehaviour
{

    private int treeLife = 10;
    private GameObject treeGameObject;
    MeshRenderer meshTree = null;

    // Start is called before the first frame update
    void Start()
    {
        treeGameObject = gameObject;
        meshTree = treeGameObject.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "axe")
        {
            Debug.Log("BATEU NA ARVORE");
            hitTree();
        }
    }

    public void hitTree()
    {
        if(treeLife >= 0)
        {
            treeLife -= 5;
        }
        else
        {
            meshTree.enabled = false;
        }
    }
}
