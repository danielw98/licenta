using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TowerGunRotation : Tower
{
    public GameObject turret;
    
    private void Update()
    {
        //Quaternion rotation = Quaternion.LookRotation(target.transform.position - _t.transform.position);
        if (!Game.Instance.isPaused && Game.Instance.enemies.Count > 0 && turret != null)
        {
            Quaternion rotation = Quaternion.LookRotation(Game.Instance.enemies[0].transform.position - turret.transform.position);
            rotation.x = 0; //This is for limiting the rotation to the y axis. I needed this for my project so just
            rotation.z = 0;
            turret.transform.rotation = Quaternion.Slerp(turret.transform.rotation, rotation, turnSpeed * Time.deltaTime);
        }
    }
}