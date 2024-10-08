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
    private FauxGravityAttractor[] attractors;

    public float weight=1f;
    // Start is called before the first frame update
    void Start()
    {
        myTransform=transform;
        body=GetComponent<Rigidbody>();
        body.constraints=RigidbodyConstraints.FreezeRotation;
        body.useGravity=false;
        halveGravity=false;
        attractors=FindObjectsOfType<FauxGravityAttractor>();
    }

    // Update is called once per frame
    void Update()
    {
        FindAttractor();
        attractor.Attract(myTransform, insideGravityField, rotationSpeed, weight);
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

    public void FindAttractor(){
        FauxGravityAttractor currentAttractor=attractor;
        FauxGravityAttractor newAttractor=currentAttractor;
        float currentDistance=Vector3.Distance(transform.position,currentAttractor.transform.position);;
        float minDistance=currentDistance;
        float d;
        for(int i=0;i<attractors.Length;i++){
            if(attractors[i]!=currentAttractor){
                d=Vector3.Distance(transform.position,attractors[i].transform.position);
                if(d<currentDistance*0.5 && d<minDistance){
                    newAttractor=attractors[i];
                    minDistance=d;
                }
            }
        }
        if(newAttractor!=currentAttractor){
            ChangeAttractor(newAttractor);
            Debug.Log("new attractor");
            Debug.Log(newAttractor);
            insideGravityField=false;
        }
    }
}
