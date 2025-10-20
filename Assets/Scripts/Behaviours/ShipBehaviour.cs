using System;
using System.Collections.Generic;
using UnityEngine;

public class ShipBehaviour : MonoBehaviour
{
    static public Action OnShoot;
    static public Action OnEvolve;

    public GameObject bulletPrefab;
    public GameObject playerShield;
    public List<Transform> mainFirePoint;
    public List<Transform> secondaryFirePoint;

    protected Ship m_ship;

    protected float m_nextFireTime = 0f;

    public void Init(Ship ship)
    {
        m_ship = ship;
        playerShield.SetActive(m_ship.shielded);
    }

    void Update()
    {
        m_nextFireTime -= Time.deltaTime;
    }

    public virtual void TryShoot()
    {
        if (m_nextFireTime > 0f)
            return;

        foreach (Transform firePoint in mainFirePoint)
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        if (m_ship.isMultiShotActive)
        {
            // if triple shot, shoot from secondary fire points as well
            foreach (Transform firePoint in secondaryFirePoint)
                Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }

        Debug.Log("Shoot");

        OnShoot?.Invoke();

        m_nextFireTime = m_ship.GetAttackSpeed();
    }

    public virtual void SetShield(bool active)
    {
        playerShield.SetActive(active);
        m_ship.shielded = active;
    }

    public virtual void SetOverShield(bool active)
    {
        m_ship.isOverShieldActive = active;
    }

    public virtual void ApplyMultiShot(bool active)
    {
        m_ship.isMultiShotActive = active;
    }

    public virtual void ApplyRapidShot(bool active)
    {
        m_ship.isRapidShotActive = active;
    }

    // this is meant to be overriden
    public virtual void ApplyEvolve(BuffManager buffManager, BuffDatabase buffDatabase)
    {
        m_ship.isEvolved = true;
        OnEvolve?.Invoke();
    }

    public Ship GetShip() { return m_ship; }
}
