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
    private WoodCanva woodLogCounter;
    private bool canDestroy = true;
    private float respawnTimer = 0;
    private MeshRenderer tocoMesh;

    // Start is called before the first frame update
    void Start()
    {
        treeGameObject = gameObject;
        meshTree = treeGameObject.GetComponent<MeshRenderer>();
        toco = GameObject.FindGameObjectWithTag("toco");
        woodLogCounter = FindObjectOfType<WoodCanva>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        if (!canDestroy)
            respawnTimer += Time.deltaTime;
        respawnTree();
    }

    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "axe" && canDestroy)
        {
            // Se o timer estiver zerado, permite registrar o hit
            if (timer <= 0)
            {
                hitTree();
                timer = treeHitTime; // Reinicia o timer
                Debug.Log($"bateu na árvore, vida restante: {treeLife}");
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
            woodLogCounter.AddLogWood(1);
            Vector3 position = transform.position;
            Quaternion rotation = transform.rotation;
            meshTree.enabled = false;
            canDestroy = false;
            Instantiate(toco, position, rotation);
        }
    }

    public void respawnTree()
    {
        if(respawnTimer >= 5)
        {
            //filhosdeToco();
            meshTree.enabled = true;
            canDestroy = true;
            respawnTimer = 0;
            treeLife = 10;
        }
    }
    private void filhosdeToco()
    {
        foreach(var filhos in toco.GetComponentsInChildren<MeshRenderer>())
        {
            filhos.enabled = false;
        }
    }
}
