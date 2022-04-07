//Author: Craig Zeki
//Student ID: zek21003166

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class UnlockNextTeleport : MonoBehaviour, iTeleportPointUnlocker
{
    
    [SerializeField] GameObject teleportToUnlock;

    public void UnlockTeleportPoint()
    {
        //guard
        if (teleportToUnlock == null) return;
        teleportToUnlock.GetComponentInChildren<TeleportPoint>().SetLocked(false);
    }


}
