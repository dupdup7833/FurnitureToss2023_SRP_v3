using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HurricaneVR.Framework.Weapons.Guns;

public class FT_Gun : HVRGunBase
{
    // Start is called before the first frame update

    protected override void Awake()
    {
        base.Awake();
    }



    protected override void Update()
    {
        CheckTriggerHaptics();
        CheckTriggerPull();
        //UpdateTrackedBullets();
        UpdateTriggerAnimation();
        UpdateShooting();
    }


    protected override void FireBullet(Vector3 direction)
    {

        var bullet = Instantiate(BulletPrefab);
        bullet.transform.position = BulletOrigin.position;
        //  bullet.transform.rotation = Quaternion.FromToRotation(bullet.transform.forward, direction) *
        //                                     bullet.transform.rotation;
        bullet.transform.rotation = BulletOrigin.rotation;
       // bullet.transform.Rotate(0, 90, 0);
        bullet.GetComponent<Rigidbody>().AddForce(transform.forward * this.BulletSpeed);
        Destroy(bullet, this.BulletLife);


    }



}
