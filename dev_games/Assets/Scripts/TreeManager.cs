using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeManager : MonoBehaviour
{

    private int treeLife = 10;
    private GameObject treeGameObject;
    MeshRenderer meshTree = null;
    private GameObject toco;

    // Start is called before the first frame update
    void Start()
    {
        treeGameObject = gameObject;
        meshTree = treeGameObject.GetComponent<MeshRenderer>();
        toco = GameObject.FindGameObjectWithTag("toco");
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
            Vector3 position = transform.position;
            Quaternion rotation = transform.rotation;
            meshTree.enabled = false;
            Instantiate(toco, position, rotation);
        }
    }
}
