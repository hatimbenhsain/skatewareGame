using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

// Planet / Attractor
public class FauxGravityAttractor : MonoBehaviour
{
    // Start is called before the first frame update
    public float gravity=-10f;
    private float gravityModifier;
    private PlayerController playerController;
    void Start()
    {
        playerController=FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        gravityModifier=playerController.gravityModifier;
    }

    public void Attract(Transform body, bool insideGravityField=true, float rotationSpeed=50f, float weight=1f){
        Vector3 gravityUp=(body.position-transform.position).normalized;
        Vector3 bodyUp=body.up;

        float g=gravity*gravityModifier;

        // Halfing gravity when holding jump button
        if(body.GetComponent<FauxGravityBody>().halveGravity){
            g=g*0.5f;
        }

        //Lowering gravity if outside field
        if(!insideGravityField){
            g=g*0.5f;
        }

        body.GetComponent<Rigidbody>().AddForce(gravityUp*g*weight);

        Quaternion targetRotation=Quaternion.FromToRotation(bodyUp,gravityUp)*body.rotation;
        body.rotation=Quaternion.Slerp(body.rotation,targetRotation,rotationSpeed*Time.deltaTime);
    }
}
