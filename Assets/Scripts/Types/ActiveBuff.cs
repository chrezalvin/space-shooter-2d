using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveBuff
{
    public Buff buff;
    public float totalDuration;
    public float remainingTime;
    public bool ignoreTimer;

    public ActiveBuff(Buff buff, float duration, bool ignoreTimer = false)
    {
        this.buff = buff;
        this.totalDuration = duration;
        this.remainingTime = duration;
        this.ignoreTimer = ignoreTimer;
    }
}
