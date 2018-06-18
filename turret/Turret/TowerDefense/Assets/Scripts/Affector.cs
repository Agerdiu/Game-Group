using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Affector : MonoBehaviour {
    public Transform epicenter;
    public float speed;
    public GameObject explosionEffectPrefab;
    public GameObject flyParticleSys;
    public GameObject attackPrefab;
    public float distanceArriveTarget = 1.2f;
    private Transform target;
    public float ParticleSysduration=0.2f;
    public void SetTarget(Transform _target)
    {
        this.target = _target;
    }
    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            Die();
            return;
        }        
        transform.LookAt(target.position);
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        Vector3 dir = target.position - transform.position;
        GameObject effect = GameObject.Instantiate(flyParticleSys, transform.position, transform.rotation);
        Destroy(effect, ParticleSysduration);
        if (dir.magnitude < distanceArriveTarget)
        {
            //target.GetComponent<Enemy>().TakeDamage(damage);
            Die();
        }
    }
    public GameObject initialize()
    {
        return GameObject.Instantiate(gameObject,epicenter);
    }
    void Die()
    {
        GameObject effect = GameObject.Instantiate(explosionEffectPrefab, transform.position, transform.rotation);
        Destroy(effect, 1.5f);
        Destroy(this.gameObject);
    }
}
