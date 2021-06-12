using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatSeakerTurret : MonoBehaviour
{
    public Rigidbody bullet;
    public Transform player;

    //parameters
    public float coolDown = 5.0f;
    public float bulletSpeed = 5.0f;
    public float bulletRotationSpeed = 2.0f;

    public Vector3 bulletStart;
    public float bulletHeightOffset = 0.25f;

    public bool canFire = true;
    public bool bulletActive = true;
    public bool charged = true;

    public float coolDownProg = 0.0f;

    private void Awake()
    {
        bulletStart = bullet.transform.position;
    }

    private void Update()
    {
        if (charged){
            
            bulletActive = true;
        }

        if(bulletActive == true){
            //bullet travelling towards the player
            bullet.transform.position = new Vector3(bullet.transform.position.x, player.transform.position.y + bulletHeightOffset, bullet.transform.position.z);

            Vector3 targetDir = player.position - bullet.transform.position;
            Vector3 newDir = Vector3.RotateTowards(bullet.transform.forward, targetDir, bulletRotationSpeed * Time.deltaTime, 0.0f);

            bullet.transform.rotation = Quaternion.LookRotation(newDir);
            bullet.AddForce(bullet.transform.forward * bulletSpeed);
            bullet.velocity = bullet.velocity.normalized * bulletSpeed;
            bullet.angularVelocity = Vector3.zero;
        }

        //check if turret can see the player
        Ray ray = new Ray(transform.position, (player.transform.position - transform.position));
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, 25.0f)){
            if(hitInfo.transform == player){
                canFire = true;
            }else{
                canFire = false;
            }
        }

        Debug.DrawLine(transform.position, hitInfo.point);
    }

    private void FixedUpdate()
    {
        if (bulletActive == true || canFire == false){
            coolDownProg = Mathf.Clamp(coolDownProg, 0, coolDown - 0.5f);
            return;
        }

        coolDownProg += Time.deltaTime;
        coolDownProg = Mathf.Clamp(coolDownProg, 0, coolDown);

        if(coolDownProg == coolDown){
            charged = true;
        }else{
            charged = false;
        }
    }

    public void BulletCollision()
    {
        ResetBullet();
    }

    private void ResetBullet()
    {
        bullet.transform.position = bulletStart;
        bullet.velocity = Vector3.zero;
        bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        bullet.angularVelocity = Vector3.zero;
        bulletActive = false;
        coolDownProg = 0.0f;
        charged = false;
    }
}
