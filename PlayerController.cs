using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public const int PLAYER_INDEX = 0;

    public float SPEED_MULT;
    private Rigidbody rb;
    private Vector3 move;
    public float ROT_X_OFFSET = 0f;
    Transform transformCenter;
    int transformCenterIndex;
    Animator animator;

    int floorMask;                      // A layer mask so that a ray can be cast just at gameobjects on the floor layer.
    float camRayLength = 200f;          // The length of the ray from the camera into the scene.

    public Transform upperBody;

    void Awake()
    {
        // Create a layer mask for the floor layer.
        floorMask = LayerMask.GetMask("Floor");
    }

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        animator = transform.GetChild(PLAYER_INDEX).GetComponent<Animator>();
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).transform.name == "SoldierCenter")
            {
                transformCenterIndex = i;
                Debug.Log("found soldier center");
            }
        }
    }

    // Occurs before every physics update
    void FixedUpdate()
    {
        move.x = Input.GetAxisRaw("Horizontal");
        move.z = Input.GetAxisRaw("Vertical");
        move.y = 0;
        move = move.normalized * SPEED_MULT * Time.deltaTime;
        rb.MovePosition(transform.position + move);
        if (move.x != 0 || move.z != 0) // aka if the player is moving
        {
            if (!animator.GetBool("isRunning"))
            {
                animator.SetTrigger("trigRunning");
            }
            animator.SetBool("isRunning", true);
        }
        else
        {
            if (animator.GetBool("isRunning"))
            {
                animator.SetTrigger("trigIdle");
            }
            animator.SetBool("isRunning", false);
        }

        Turning();
    }
	
	// Update is called once per frame
	void Update () {
        if(Input.GetMouseButtonDown(0)) // left click
        {
            animator.SetTrigger("trigShoot");
        }

        // Display where the player is pointing the mouse by projecting a line
        //Ray ray = Camera.main.ScreenPointToRay(new Vector3(200, 200, 0));
        //Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(camRay.origin, camRay.direction * camRayLength, Color.magenta);
    }

    void Turning()
    {
        // Create a ray from the mouse cursor on screen in the direction of the camera.
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Create a RaycastHit variable to store information about what was hit by the ray.
        RaycastHit floorHit;

        // Perform the raycast and if it hits something on the floor layer...
        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
        {
            // Create a vector from the player to the point on the floor the raycast from the mouse hit.
            transformCenter = transform.GetChild(transformCenterIndex).transform;
            Vector3 playerToMouse = floorHit.point - transform.position;//transformCenter.position; //transform.position;

            // Ensure the vector is entirely along the floor plane.
            playerToMouse.y = 0f;

            // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);

            // Set the player's rotation to this new rotation.
            rb.MoveRotation(newRotation);
        }
    }
}
