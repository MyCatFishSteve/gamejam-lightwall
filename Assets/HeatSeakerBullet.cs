using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatSeakerBullet : MonoBehaviour
{
    public HeatSeakerTurret hst;

    public void OnCollisionEnter(Collision collision)
    {
        hst.BulletCollision();
    }
}
