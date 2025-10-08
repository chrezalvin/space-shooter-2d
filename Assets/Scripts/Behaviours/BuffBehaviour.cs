using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffBehaviour : MonoBehaviour
{
    public float moveSpeed = 3f;

    public void Init(float speed)
    {
        moveSpeed = speed;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // speed already set in Init or default value
    }

    // Update is called once per frame
    void Update()
    {
        // move forward
        this.transform.position += moveSpeed * Time.deltaTime * this.transform.up;
    }

    public void Collected()
    {
        Destroy(this.gameObject);
    }
}
