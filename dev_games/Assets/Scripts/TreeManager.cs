using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeManager : MonoBehaviour
{

    private int treeLife = 10;
    private GameObject treeGameObject;
    MeshRenderer meshTree = null;
    private GameObject toco;
    private float timer = 0;
    private float treeHitTime = 2f;

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
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
    }

    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "axe")
        {
            // Se o timer estiver zerado, permite registrar o hit
            if (timer <= 0)
            {
                Debug.Log($"bateu na árvore, vida restante: {treeLife}");
                hitTree();
                timer = treeHitTime; // Reinicia o timer
            }
        }
    }

    public void hitTree()
    {
        if(treeLife > 0)
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
