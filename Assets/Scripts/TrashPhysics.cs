using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TrashPhysics : MonoBehaviour{
    private Rigidbody rb;
    private FauxGravityBody fauxGravityBody;

    public float impactStrength = 10f;

    
    void Start(){
        rb = GetComponent<Rigidbody>();
        fauxGravityBody = GetComponent<FauxGravityBody>();
    }

    private void OnCollisionEnter(Collision col){
        if(col.gameObject.transform.parent.tag == "Player"){
            Rigidbody playerRB = col.gameObject.GetComponent<Rigidbody>();
            if(playerRB != null){
                Vector3 normal = col.GetContact(0).normal;
                rb.velocity = normal * impactStrength;
            }
        }
    }

    private void OnTriggerEnter(Collider other){
        if(other.gameObject.tag=="Planet"){
            fauxGravityBody.ChangeAttractor(other.gameObject.GetComponentInParent<FauxGravityAttractor>());
        }
    }

    private void OnTriggerExit(Collider other){
        if(other.gameObject.tag=="Planet"){
            fauxGravityBody.ExitGravityField(other.gameObject.GetComponentInParent<FauxGravityAttractor>());
        }
    }
}
