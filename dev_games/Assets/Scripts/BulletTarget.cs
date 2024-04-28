using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTarget : MonoBehaviour
{

    private float normalZombiehp;

    void Start()
    {
        normalZombiehp = 10;
    }    
    public void bulletHit()
    {
        if (normalZombiehp <= 0)
            destroyObject();
        normalZombiehp -= 2;
    }
    private void destroyObject()
    {
            Destroy(gameObject);
    }
}
