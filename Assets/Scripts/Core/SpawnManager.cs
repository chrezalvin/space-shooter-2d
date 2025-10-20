using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject boundary;

    protected List<GameObject> m_spawnedObjects;

    protected BoxCollider2D m_boundaryCollider;
    protected Bounds m_boundary;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        m_boundaryCollider = boundary.GetComponent<BoxCollider2D>();

        if (m_boundaryCollider == null)
            Debug.LogError("Boundary object does not have a BoxCollider2D component.");

        m_boundary = m_boundaryCollider.bounds;
        m_spawnedObjects = new List<GameObject>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        // checks for any spawned object that is out of boundary
        for (int iii = m_spawnedObjects.Count - 1; iii >= 0; --iii)
        {
            GameObject obj = m_spawnedObjects[iii];
            if (obj == null || !m_boundary.Contains(obj.transform.position))
            {
                if (obj != null)
                    Destroy(obj);
                m_spawnedObjects.RemoveAt(iii);
            }
        }
    }

    // Returns a random point on the edge of the rectangle defined by the given boundaries
    protected Vector2 GetRandomPointOnEdge(Bounds bounds)
    {
        int side = Random.Range(0, 4); // 0: left, 1: right, 2: top, 3: bottom
        Vector2 point;
        switch (side)
        {
            case 0: // left
                point = new Vector2(bounds.min.x, Random.Range(bounds.min.y, bounds.max.y));
                break;
            case 1: // right
                point = new Vector2(bounds.max.x, Random.Range(bounds.min.y, bounds.max.y));
                break;
            case 2: // top
                point = new Vector2(Random.Range(bounds.min.x, bounds.max.x), bounds.max.y);
                break;
            case 3: // bottom
                point = new Vector2(Random.Range(bounds.min.x, bounds.max.x), bounds.min.y);
                break;
            default:
                point = Vector2.zero; // should never reach here
                break;
        }

        return point;
    }
    
    protected GameObject SpawnObject(GameObject obj, Vector2 from, Vector2 to)
    {
        Vector2 direction = (to - from).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        Quaternion rotation = Quaternion.Euler(0f, 0f, angle);

        GameObject spawnedObj = Instantiate(obj, from, rotation);

        m_spawnedObjects.Add(spawnedObj);

        return spawnedObj;
    }
}
