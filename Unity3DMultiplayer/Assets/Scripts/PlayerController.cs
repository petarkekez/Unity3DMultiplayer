using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;

public class PlayerController : NetworkBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawn;

    private DateTime lastBulletFired = DateTime.Now;
    private const int fireRatePerSecond = 4;
    private TimeSpan minTimeBetweenSohots = new TimeSpan(TimeSpan.TicksPerSecond / fireRatePerSecond);

    public void Start()
        {
        
        }

    public override void OnStartLocalPlayer()
    {
        //base.OnStartLocalPlayer();
        GetComponent<MeshRenderer>().material.color = Color.blue;

        Camera camera = gameObject.GetComponentInChildren<Camera>(true);

        if (camera != null)
        {
            var cameras = Component.FindObjectsOfType<Camera>();
            foreach (var cameraItem in cameras)
            {
                cameraItem.enabled = false;
            }

            camera.enabled = true;
        }
        //gameObject.ge
        //var camera = gameObject.AddComponent<Camera>();
    }
    void Update()
    {
        if (!isLocalPlayer)
            return;

        float x = 0;
        float z = 0;

        if (Application.isMobilePlatform)
        {
            x = (Input.acceleration.x) * Time.deltaTime * 150.0f;
            z = (-1 * Input.acceleration.z - 0.75f) * Time.deltaTime * 10.0f;
            if (z > 5) z = 5;
            if (z < -5) z = -5;
        }
        else
        {
            x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
            z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;
        }
        

        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);

        bool fire = false;

        if (Input.GetKeyDown(KeyCode.Space))
            fire = true;

        if(Input.touches.Length > 0)
            foreach (var touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    fire = true;
                    break;
                }
            }

        if(fire &&  DateTime.Now - lastBulletFired > minTimeBetweenSohots)
        {
            lastBulletFired = DateTime.Now;
            CmdFire();
        }
        
    }
    

    [Command]
    void CmdFire()
    {
        // Create the Bullet from the Bullet Prefab
        var bullet = (GameObject)Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);

        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 6;

        NetworkServer.Spawn(bullet);

        Destroy(bullet, 2.0f);
    }
}
