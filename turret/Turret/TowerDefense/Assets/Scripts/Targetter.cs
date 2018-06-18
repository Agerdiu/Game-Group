using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targetter : MonoBehaviour {
    public Transform turret;
    public Collider attachedCollider;
    public List<GameObject> enemys = new List<GameObject>();
    public GameObject currentEnemy;
    public float idleWaitTime = 2.0f;
    protected float SearchTimer = 0.0f;
    protected float WaitTimer = 0.1f;
    protected float XRotationCorrectionTime;
    protected float CurrentRotationSpeed;
    public bool onlyYTurretRotation;
    public float searchRate;
    public float idleRotationSpeed = 60f;
    public float idleCorrectionTime = 2.0f;
    public Vector2 turretXRotationRange = new Vector2(0, 359);
    public GameObject affector;
    public float fireRate=0.1f;
    private float fireTime;
    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Enemy")
        {
            enemys.Add(col.gameObject);
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Enemy")
        {
            if (col.gameObject == currentEnemy)
                currentEnemy = null;
            enemys.Remove(col.gameObject);
        }
    }
    public GameObject GetTarget()
    {
        return currentEnemy;
    }
    public List<GameObject> GetAllTargets()
    {
        return enemys;
    }
    protected virtual GameObject GetNearestTargetable()
    {
        int length = enemys.Count;
        if (length == 0)
            return null;
        GameObject nearest = null;
        float distance = float.MaxValue;
        for (int i = 0; i < length; i++)
        {
            GameObject enemy = enemys[i];
            if(enemy==null)/*or enemy is dead*/
            {
                enemys.RemoveAt(i);
                continue;
            }
            float currentDistance = Vector3.Distance(transform.position, enemy.transform.position);
            if (currentDistance < distance)
            {
                distance = currentDistance;
                nearest = enemy;
            }
        }
        return nearest;
    }
    protected virtual void AimTurret()
    {
        if (turret == null)
        {
            return;
        }

        if (currentEnemy == null) // do idle rotation
        {
            if (WaitTimer > 0)
            {
                WaitTimer -= Time.deltaTime;
                if (WaitTimer <= 0)
                {
                    CurrentRotationSpeed = (Random.value * 2 - 1) * idleRotationSpeed;
                }
            }
            else
            {

                Vector3 euler = turret.rotation.eulerAngles;
                euler.x = Mathf.Lerp(Wrap180(euler.x), 0, XRotationCorrectionTime);
                XRotationCorrectionTime = Mathf.Clamp01((XRotationCorrectionTime + Time.deltaTime) / idleCorrectionTime);
                euler.y += CurrentRotationSpeed * Time.deltaTime;
                turret.eulerAngles = euler;
            }
        }
        else
        {
            WaitTimer = idleWaitTime;

            Vector3 targetPosition = currentEnemy.transform.position;
            if (onlyYTurretRotation)
            {
                targetPosition.y = turret.position.y;
            }
            Vector3 direction = targetPosition - turret.position;
            Quaternion look = Quaternion.LookRotation(direction, Vector3.up);
            Vector3 lookEuler = look.eulerAngles;
            // We need to convert the rotation to a -180/180 wrap so that we can clamp the angle with a min/max
            float x = Wrap180(lookEuler.x);
            lookEuler.x = Mathf.Clamp(x, turretXRotationRange.x, turretXRotationRange.y);
            look.eulerAngles = lookEuler;
            turret.rotation = look;
        }
    }

    /// <summary>
    /// A simply function to convert an angle to a -180/180 wrap
    /// </summary>
    static float Wrap180(float angle)
    {
        angle %= 360;
        if (angle < -180)
        {
            angle += 360;
        }
        else if (angle > 180)
        {
            angle -= 360;
        }
        return angle;
    }
    protected virtual void Attack()
    {
        if (!currentEnemy)
            return;
        if (enemys.Count > 0)
        {
            GameObject bullect = GameObject.Instantiate(affector,turret);
            bullect.GetComponent<Affector>().SetTarget(currentEnemy.transform);

            fireTime = 0.0f;
            //bullet.GetComponent<Bullet>().SetTarget(currentEnemy.transform);
        }
    }
    protected virtual void Update()
    {
        SearchTimer -= Time.deltaTime;
        if (SearchTimer <= 0.0f && currentEnemy == null && enemys.Count > 0)
        {
            currentEnemy= GetNearestTargetable();
            if (currentEnemy != null)
            {                
                SearchTimer = searchRate;
            }
        }
        AimTurret();
        fireTime += Time.deltaTime;
 
        if (currentEnemy != null&&fireRate!=0)
        {
            if (fireTime >= 1.0f / fireRate)
            {
                Attack();
            }
        }
    }
}
