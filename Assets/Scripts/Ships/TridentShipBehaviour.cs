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
        m_ship.isEvolved = true;
    }

    public override void TryShoot()
    {
        if (m_nextFireTime > 0f)
            return;

        foreach (Transform firePoint in mainFirePoint)
            Instantiate(piercingBulletPrefab, firePoint.position, firePoint.rotation);

        if (m_ship.isMultiShotActive)
        {
            // if triple shot, shoot from secondary fire points as well
            foreach (Transform firePoint in secondaryFirePoint)
                Instantiate(piercingBulletPrefab, firePoint.position, firePoint.rotation);
        }

        Debug.Log("Shoot");

        if (shootSfx && Camera.main)
            AudioSource.PlayClipAtPoint(shootSfx, Camera.main.transform.position);

        m_nextFireTime = m_ship.GetAttackSpeed();
    }
}
