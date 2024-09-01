using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuffRotation : MonoBehaviour
{
    public float rotationSpeed = 20f;
    public bool rotateX=false;
    public bool rotateY=true;
    public bool rotateZ=false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rotation=new Vector3();
        if(rotateY){
            rotation.y=Time.deltaTime*rotationSpeed;
        }
        if(rotateZ){
            rotation.z=Time.deltaTime*rotationSpeed;
        }
        if(rotateX){
            rotation.x=Time.deltaTime*rotationSpeed;    
        }
        transform.Rotate(rotation);
    }
}
