using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Body of Actor
public class FauxGravityBody : MonoBehaviour
{
    public FauxGravityAttractor attractor;
    private Rigidbody body;
    private Transform myTransform;
    public bool halveGravity;

    public bool insideGravityField=false;

    public float rotationSpeed=10f;
    // Start is called before the first frame update
    void Start()
    {
        myTransform=transform;
        body=GetComponent<Rigidbody>();
        body.constraints=RigidbodyConstraints.FreezeRotation;
        body.useGravity=false;
        halveGravity=false;
    }

    // Update is called once per frame
    void Update()
    {
        attractor.Attract(myTransform, insideGravityField, rotationSpeed);
    }

    public void ChangeAttractor(FauxGravityAttractor newAttractor){
        attractor=newAttractor;
        insideGravityField=true;
    }

    public void ExitGravityField(FauxGravityAttractor myAttractor){
        if(myAttractor==attractor){
            insideGravityField=false;
        }
    }
}
