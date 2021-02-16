using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMove : MonoBehaviour
{
   
    public CharacterController cc;

    [SerializeField]
    float speed, gravity, jumpForce, baseSpd, sprintSpd, maxSpd;

    [SerializeField]
    private float slopeForce, slopeForceRayLength;

    [SerializeField]
    float groundRayLength, vertMove;
    bool onGround, onAir, jumping, sprint;


    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {   
        //get input
        float x = Input.GetAxis("Horizontal"); //side
        float z = Input.GetAxis("Vertical"); //forward
        jumping = Input.GetKeyDown(KeyCode.Space);
        sprint = Input.GetKey(KeyCode.LeftShift);

        Vector3 move = transform.right * x + transform.forward * z;
        //direction to move based on x-y movement & direction faced.

        //check ground
        GroundCheck();
        AirCheck();

        //on air
        if (!onGround) vertMove -= gravity * Time.deltaTime;
        else vertMove = 0;
        //Debug.Log(" " + vertMove);
        /*
        //jump force
        if (jumping && onGround)
        {
            vertMove += jumpForce; //jump
            Debug.Log("Jumping");
        }*/
        if (onAir)
        {
            vertMove = -3; //start falling
        }

        if (sprint)
        {
            speed += (sprintSpd * 2);
            if (speed >= maxSpd) {
                speed = 6;
            }
        }
        else if(!sprint && !onAir) speed = baseSpd;

        cc.Move(move * speed * Time.deltaTime); //move character controller || "Move" is a function in the CC.
        cc.Move(new Vector3(0, vertMove, 0) * Time.deltaTime);

        if ((vertMove != 0 || speed != 0) && OnSlope()) {
            cc.Move(Vector3.down * cc.height / 2 * slopeForceRayLength);
        }

        Debug.Log(" " + speed);
    }

    void GroundCheck() {
        RaycastHit hit;
        onGround = Physics.SphereCast(transform.position, 0.2f, Vector3.down, out hit, groundRayLength);
    }

    void AirCheck() {
        RaycastHit hit;
        onAir = Physics.SphereCast(transform.position, 1f, Vector3.up, out hit, .8f);
    }

    private bool OnSlope() {
        if (jumping)
            return false;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, cc.height / 2 * slopeForceRayLength))
            if (hit.normal != Vector3.up)
                return true;
        return false;
    }
}
