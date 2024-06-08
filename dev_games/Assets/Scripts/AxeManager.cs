using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject axe;
    Rigidbody r;
    void Awake() { 
        r = axe.GetComponent<Rigidbody>(); 
    }

    public void disableRigidBodyAxe()
    {
        r.detectCollisions = !r.detectCollisions;
    }
}
