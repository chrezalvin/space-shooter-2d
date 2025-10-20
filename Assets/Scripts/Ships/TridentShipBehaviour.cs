using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TridentShipBehaviour : ShipBehaviour
{
    public GameObject piercingBulletPrefab;
    public EvolutionSystem evolutionSystem;
    public override void ApplyEvolve(BuffManager buffManager, BuffDatabase buffDatabase)
    {
        evolutionSystem.Evolve(buffManager, buffDatabase);
        base.ApplyEvolve(buffManager, buffDatabase);
    }

    public override void TryShoot()
    {
        if (m_nextFireTime > 0f)
            return;

        foreach (Transform firePoint in mainFirePoint)
            Instantiate(m_ship.isEvolved ? piercingBulletPrefab : bulletPrefab, firePoint.position, firePoint.rotation);

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
}
