using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBehaviour : MonoBehaviour
{
    public float removeAfter = 2f;

    void Start()
    {
        Destroy(this.gameObject, removeAfter);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
