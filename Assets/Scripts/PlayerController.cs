using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public InterpretArduino interpretArduino;
    
    public float moveSpeed=0f;
    private Vector3 moveDir;
    private Rigidbody body;

    public float initSpeed=1f;
    public float acceleration=5f;
    public float maxSpeed=10f;
    public float deceleration=5f;

    public float jumpForce=5f;

    private Vector3 lastDir;
    private Vector3 moveVector;

    private bool grounded=true;
    private bool slow=false;

    public float slowFactor=0.5f;

    public Transform cameraCloseTransform;
    public Transform cameraFarTransform;

    private GameObject camera;


    private float initHeight=10f;
    private float maxHeight=20f;

    private FauxGravityBody fauxGravityBody;

    private bool reachedPeak=false;

    public float cameraLerpSpeed=10f;

    public bool moveCamera=false;

    public float airResistance=1f;

    public float drag;

    public float gravityModifier=1f;

    void Start()
    {
        body=GetComponent<Rigidbody>();
        moveVector=Vector3.zero;
        camera=Camera.main.gameObject;
        fauxGravityBody=GetComponent<FauxGravityBody>();
        drag=body.drag;
    }

    // Update is called once per frame
    void Update()
    {

        SetMoveVector();

        Jump();

        

        // Distance between current attractor and body
        float dis=Vector3.Distance(transform.position,fauxGravityBody.attractor.transform.position);


        if(!reachedPeak && dis>=maxHeight){
            // Did I reach peak of jump
            reachedPeak=true;
            Debug.Log("reached peak");
        }
 
    }

    public void SetMoveVector(){
        //slow=Input.GetMouseButton(1);

        moveDir=GetInputVector();

        float acc=acceleration;
        float ms=maxSpeed;
        float dec=deceleration;

        // Slowing down movement
        if(slow){
            float k=slowFactor;
            acc=acc*k;
            ms=ms*k;
            dec=dec*k;
        }

        if(moveDir.magnitude>0f && grounded){
            if(moveVector.magnitude<0.05f){
                //Minimum speed at smallest joystick movement
                moveVector=moveDir*initSpeed;
            }else{
                //Accelerating over time
                moveVector+=moveDir*acc*Time.deltaTime;
            }
        }
        //Slowing down if no input OR going over maximum speed
        if(moveDir.magnitude==0f || moveVector.magnitude>moveDir.magnitude*ms){
            if(moveVector.magnitude>0f){
                moveVector-=dec*moveVector.normalized*Time.deltaTime;
            }
        }

        moveVector=Vector3.ClampMagnitude(moveVector,ms);

        if(fauxGravityBody.insideGravityField){
            body.drag=drag;
        }else{
            body.drag=0;
        }

    }

    public void Jump(){
        //Handling Jump when key down
        if(Input.GetKeyDown(KeyCode.Space) && grounded){
            body.velocity=transform.up*jumpForce;
            body.velocity+=moveVector;
            Debug.Log("jump");
        }

        //Handling continuous jump button press
        // Only halfing gravity if still going up
        if(Input.GetKey(KeyCode.Space) && transform.InverseTransformDirection(body.velocity).y>0){
            Debug.Log("pressing space while going up");
            fauxGravityBody.halveGravity=true;
        }else{
            fauxGravityBody.halveGravity=false;
        }
    }

    public Vector3 GetInputVector(){
        //return new Vector3(Input.GetAxisRaw("Horizontal"),0,Input.GetAxisRaw("Vertical")).normalized;
        Vector2 input = interpretArduino.GetCurrentInput();
        Debug.Log(input);
        return new Vector3(input.y, 0, input.x);
    }

    public void SetCameraPosition(){
        if (moveCamera){
        //Lerping the camera to player position
            if(slow){
                camera.transform.position=Vector3.Lerp(camera.transform.position,cameraCloseTransform.transform.position,cameraLerpSpeed*Time.deltaTime);
                camera.transform.rotation=Quaternion.Lerp(camera.transform.rotation,cameraCloseTransform.transform.rotation,cameraLerpSpeed*Time.deltaTime);
            }else{
                camera.transform.position=Vector3.Lerp(camera.transform.position,cameraFarTransform.transform.position,cameraLerpSpeed*Time.deltaTime);
                camera.transform.rotation=Quaternion.Lerp(camera.transform.rotation,cameraFarTransform.transform.rotation,cameraLerpSpeed*Time.deltaTime);
            }
        }
    }

    void FixedUpdate() {
        // Moving the body (except for jump)
        body.MovePosition(body.position+transform.TransformDirection(moveVector)*Time.deltaTime);

        //Vector3 velocity=body.velocity-body.velocity.normalized*airResistance;
        
        // if(Mathf.Sign(velocity.x)!=Mathf.Sign(body.velocity.x)){
        //     velocity.x=0f;
        // }
        // if(Mathf.Sign(velocity.y)!=Mathf.Sign(body.velocity.y)){
        //     velocity.y=0f;
        // }
        // if(Mathf.Sign(velocity.z)!=Mathf.Sign(body.velocity.z)){
        //     velocity.z=0f;
        // }

        //body.velocity=velocity;

        SetCameraPosition();

    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag=="Planet"){
            grounded=true;
        }
        reachedPeak=false;
    }

    private void OnCollisionExit(Collision other) {
        if(other.gameObject.tag=="Planet"){
            grounded=false;
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
