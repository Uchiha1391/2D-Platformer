using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float fireRate = 0;
    public int Damage = 10;
    public LayerMask whatToHit;
    public Transform bulletTrailPrefab;
    public Transform muzzleFlashPrefab;
    public Transform hitPrefab;
    public float effectSpawnRate = 10;

    private float timeToFire = 0;
    private float timeToSpawnEffect=0;
    private Transform firePoint;

    void Awake()
    {
        firePoint = transform.FindChild("FirePoint");
        if(firePoint==null)
        {
            Debug.LogError("Error: No FirePoint Object Found under Pistol *Weapon.cs*");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(fireRate==0)
        {
            if(Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
        else
        {
            if(Input.GetButton("Fire1")&&Time.time>timeToFire)
            {
                timeToFire = Time.time + 1 / fireRate;
                Shoot();
            }
        }
    }

    void Shoot()
    {
        Vector2 mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        Vector2 firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);
        RaycastHit2D hit = Physics2D.Raycast(firePointPosition, mousePosition - firePointPosition, 100,whatToHit);
        
        Debug.DrawLine(firePointPosition, (mousePosition-firePointPosition)*100,Color.cyan);
        if(hit.collider!=null)
        {
            Debug.DrawLine(firePointPosition, hit.point, Color.red);
            Debug.Log("We hit " + hit.collider.name + " and did damage: " + Damage);
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null)
                enemy.DamageEnemy(Damage);
        }
        if (Time.time > timeToSpawnEffect)
        {
            Vector3 hitPosition;
            Vector3 hitNormal;

            if (hit.collider == null)
            {
                hitPosition = (mousePosition - firePointPosition) * 30;
                hitNormal = new Vector3(9999, 9999, 9999);
            }
            else
            {
                hitPosition = hit.point;
                hitNormal = hit.normal;
            }

            Effect(hitPosition,hitNormal);
            timeToSpawnEffect = Time.time + 1 / effectSpawnRate;
        }
    }

    void Effect(Vector3 hitPosition,Vector3 hitNormal)
    {
        Transform bulletTrail = (Transform)Instantiate(bulletTrailPrefab, firePoint.position, firePoint.rotation);

        LineRenderer lineRenderer = bulletTrail.GetComponent<LineRenderer>();

        if(lineRenderer!=null)
        {
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, hitPosition);
        }

        Destroy(bulletTrail.gameObject, 0.04f);

        if(hitNormal != new Vector3(9999,9999,9999))
        {
            Transform hitParticle = (Transform)Instantiate(hitPrefab,hitPosition,Quaternion.FromToRotation(Vector3.right,hitNormal));
            Destroy(hitParticle.gameObject, 1f);
        }

        Transform muzzleFlashClone = (Transform)Instantiate(muzzleFlashPrefab, firePoint.position, firePoint.rotation);
        muzzleFlashClone.parent = firePoint;
        float size = Random.Range(0.6f, 0.9f);
        muzzleFlashPrefab.localScale = new Vector3(size, size, 0);
        Destroy(muzzleFlashClone.gameObject,0.02f);
    }
}
