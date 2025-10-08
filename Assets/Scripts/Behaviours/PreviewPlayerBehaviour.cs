using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewPlayerBehaviour : MonoBehaviour
{
    public ShipEntry shipEntry;
    private ShipBehaviour m_shipBehaviour;

    public void Init(ShipEntry entry)
    {
        shipEntry = entry;
        m_shipBehaviour = GetComponent<ShipBehaviour>();
        if (shipEntry == null)
        {
            Debug.LogError("Default ship not found in ShipDatabase.");
            return;
        }

        if (m_shipBehaviour)
        {
            m_shipBehaviour.Init(shipEntry.ship);
        }
        else
            Debug.LogError("Player object does not have a ShipBehaviour component.");

        m_shipBehaviour.SetShield(false);
    }

    public void OnEnable()
    {
        StartCoroutine(ApplyBuffRotation(3f));
    }

    public void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator ApplyBuffRotation(float duration)
    {
        while (true)
        {
            m_shipBehaviour.ApplyMultiShot(false);
            m_shipBehaviour.ApplyRapidShot(false);
            yield return new WaitForSeconds(duration);

            m_shipBehaviour.ApplyRapidShot(true);
            yield return new WaitForSeconds(duration);
            m_shipBehaviour.ApplyMultiShot(true);
            yield return new WaitForSeconds(duration);
        }
    }

    // Update is called once per frame
    void Update()
    {
        m_shipBehaviour.TryShoot();
    }
}
