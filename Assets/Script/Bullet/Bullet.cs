using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //effect
    public GameObject BloodEffect;
    public GameObject ImpactEffect;

    //Get life in IA
    IA iA;


    private void OnCollisionEnter(Collision collision)
    {
        iA = collision.gameObject.GetComponent<IA>();

        if (collision.gameObject.tag == "Ia")
        {
            iA.Life -= 20;
            Instantiate(BloodEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        else
            Instantiate(ImpactEffect, transform.position, transform.rotation);
            Destroy(gameObject);
    }
}
