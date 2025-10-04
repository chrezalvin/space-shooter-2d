using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    public float removeAfter = 2f;
    public float scale = 1f;

    public void Init(float scale)
    {
        this.scale = scale;
        this.transform.localScale = new Vector3(scale, scale, 1f);

        // delete after 2s
        Destroy(this.gameObject, removeAfter);
    }

    // // Start is called before the first frame update
    // void Start()
    // {
    //     // delete after 2s
    //     Destroy(this.gameObject, 2f);
    // }

    // Update is called once per frame
    void Update()
    {
        
    }
}
