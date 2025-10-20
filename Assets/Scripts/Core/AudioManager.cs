using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip shootSfx;
    public AudioClip enemyDeathSfx;
    public AudioClip pauseMenuSfx;

    public void OnEnable()
    {
        EnemyBehaviour.OnEnemyDeath += PlayEnemyDeath;
        ShipBehaviour.OnShoot += PlayShoot;
        InGameMenuManager.OnPauseMenuToggled += PlayPauseMenu;
        BuffManager.OnBuffAdded += PlayBuffAdded;
        BuffManager.OnBuffRemoved += PlayBuffRemoved;
    }

    public void OnDisable()
    {
        EnemyBehaviour.OnEnemyDeath -= PlayEnemyDeath;
        ShipBehaviour.OnShoot -= PlayShoot;
        InGameMenuManager.OnPauseMenuToggled -= PlayPauseMenu;
        BuffManager.OnBuffAdded -= PlayBuffAdded;
        BuffManager.OnBuffRemoved -= PlayBuffRemoved;
    }

    protected void PlaySfx(AudioClip clip)
    {
        if (clip != null)
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
    }

    protected void PlayShoot() { PlaySfx(shootSfx); }
    protected void PlayEnemyDeath() { PlaySfx(enemyDeathSfx); }

    protected void PlayPauseMenu(bool active) { PlaySfx(pauseMenuSfx); }

    protected void PlayBuffAdded(ActiveBuff buff)
    {
        PlaySfx(buff.buff.GetBuffActiveSfx());
    }
    
    protected void PlayBuffRemoved(ActiveBuff buff)
    {
        PlaySfx(buff.buff.GetBuffDeactiveSfx());
    }
}
