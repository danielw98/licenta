using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Tower : MonoBehaviour
{
    [HideInInspector]
    public List<GameObject> enemiesInRange;
    public float range = 10.0f;
    public float turnSpeed = 1f;
    public float fireRate = 0.5f;
    public float damage = 1.0f;
    public float sellCost;
    private float lastShotTime;
    public GameObject turret;
    public AudioSource audioFire;

    public GameObject bulletPrefab;

    public float cost = 10f;
    [HideInInspector]
    public int upgradeLevel = 1;

    private void Start()
    {
        GetComponent<SphereCollider>().radius = range * 0.565f;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            enemiesInRange.Add(other.gameObject);
            //EnemyDestructionDelegate del = other.gameObject.GetComponent<EnemyDestructionDelegate>();
            //del.enemyDelegate += OnEnemyDestroy;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(other.gameObject);
            //EnemyDestructionDelegate del = other.gameObject.GetComponent<EnemyDestructionDelegate>();
            //del.enemyDelegate -= OnEnemyDestroy;
        }
    }

    public void Attack(Collider collider)
    {
        Vector3 startPosition = gameObject.transform.position;
        foreach (Transform transform in transform)
            if (transform.CompareTag("Gun"))
                foreach (Transform childTransform in transform.transform)
                    if (childTransform.name == "GunBarrelPoint")
                        startPosition = childTransform.transform.position;
        //Vector3 targetPosition = new Vector3(collider.transform.position.x, collider.bounds.center.y , collider.transform.position.z);
        GameObject newBullet = Instantiate(bulletPrefab);
        audioFire.Play();
        newBullet.transform.position = startPosition;
        BulletBehavior bulletComp = newBullet.GetComponent<BulletBehavior>();
        bulletComp.target = collider.gameObject;
        bulletComp.startPosition = startPosition;
        bulletComp.targetPosition = collider.bounds.center;

        //Animator animator = monsterData.CurrentLevel.visualization.GetComponent<Animator>();
        //animator.SetTrigger("fireShot");
        //AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        //audioSource.PlayOneShot(audioSource.clip);
    }

    void Update()
    {
        if (enemiesInRange?.Count > 0)
        {
            var target = enemiesInRange[0];
            if (target != null)
            {
                if (target.TryGetComponent<Collider>(out var targetCollider))
                {
                    if (!Game.Instance.isPaused & !Game.Instance.isGameOver)
                    {
                        sellCost = (cost / 100f) * 75f;
                        if (Time.time - lastShotTime > fireRate)
                        {
                            Attack(targetCollider);
                            lastShotTime = Time.time;
                        }
                    }
                    // only rotate if enemies in range
                    if (!Game.Instance.isPaused && turret != null)
                    {
                        Quaternion rotation = Quaternion.LookRotation(targetCollider.bounds.center - turret.transform.position);
                        rotation.x = 0;
                        //rotation.z = 0;
                        turret.transform.rotation = Quaternion.Slerp(turret.transform.rotation, rotation, turnSpeed * Time.deltaTime);
                    }
                }
            }
            else
            {
                enemiesInRange.Remove(target);
                //Game.Instance.enemies.Remove(target);
            }
        }
    }
}