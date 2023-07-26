using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public float range = 10.0f;
    public float turnSpeed = 1f;
    public float fireRate = 0.5f;
    public float damage = 1.0f;

    public float cost = 10f;
    [HideInInspector]
    public int upgradeLevel = 1;

}