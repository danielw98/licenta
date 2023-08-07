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
    public GameObject turret;

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
            enemiesInRange.Add(other.gameObject);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
            enemiesInRange.Remove(other.gameObject);
    }
}