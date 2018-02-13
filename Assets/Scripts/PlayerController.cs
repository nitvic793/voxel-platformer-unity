using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public bool isRunning = false;
    public bool isDead = false;
    private bool jump = false;
    private bool doubleJump = false;
    private bool doubleJumpFlag = true;
    private Animator animator;
    public GameObject decoyPrefab;
    public float health = 100;
    public float smooth = 1f;
    public float speed = 1.0F;
    public float jumpSpeed = 8.0F;
    public float jumpForce = 300F;
    public float gravity = 20.0F;
    private Quaternion lookLeft;
    private Quaternion lookRight;
    private Vector3 moveDirection = Vector3.zero;
    public bool isGrounded = true;
    private Rigidbody playerRigidBody = null;
    private int groundLayerMask;
    GameObject decoyInstance = null;
    public float moveForce = 365f;
    public float maxSpeed = 5f;
    public float cameraPullback = -8F;

    public int decoyExplode = 0;
    public int dragonPower = 0;
    public GameObject dragonPrefab = null;
    public GameObject dragonInstance = null;

    public GameObject mainCamera = null;
    // Use this for initialization
    void Start()
    {
        groundLayerMask = LayerMask.GetMask("Ground");
        playerRigidBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        lookRight = transform.rotation;
        lookLeft = lookRight * Quaternion.Euler(0, 180, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0F)
        {
            isDead = true;
            animator.SetBool("Death", isDead);

            return;
        }

        CheckIfGrounded();
        CheckForDragon();
        isRunning = false;
        moveDirection = new Vector3(-(Input.GetAxis("Vertical")), 0, Input.GetAxis("Horizontal"));
        if (Input.GetKey(KeyCode.D))
        {
            transform.rotation = lookRight;
            isRunning = true;
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.rotation = lookLeft;
            isRunning = true;
            moveDirection = transform.TransformDirection(-moveDirection);
            moveDirection *= speed;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            jump = true;
        }

        if (Input.GetKeyDown(KeyCode.Space) && !isGrounded && !doubleJump && doubleJumpFlag)
        {
            doubleJump = true;
            doubleJumpFlag = false;
        }

        if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftShift))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "Ground")
                {
                    if (decoyInstance != null)
                    {
                        Destroy(decoyInstance);
                    }
                    decoyInstance = Instantiate(decoyPrefab, hit.point, transform.rotation);
                    decoyInstance.transform.position = new Vector3(hit.point.x, hit.point.y, 0F);
                }
            }
        }

        transform.position += moveDirection * Time.deltaTime;
        animator.SetBool("Run", isRunning);
        animator.SetBool("Jump", jump);
        mainCamera.transform.position = new Vector3(transform.position.x + 5, transform.position.y + 2, cameraPullback);
        var posXOffset = transform.position + new Vector3(5, 0, 0);
        mainCamera.transform.LookAt(posXOffset);
    }

    private void FixedUpdate()
    {
        playerRigidBody.transform.position = new Vector3(transform.position.x, transform.position.y, 0.0F);
        if (jump || doubleJump)
        {
            if (jump)
                playerRigidBody.AddForce(new Vector3(0, jumpForce, 0));
            else
                playerRigidBody.AddForce(new Vector3(0, jumpForce / 2, 0));
            //rigidbody.velocity = new Vector3(1, 10);
            jump = doubleJump = false;
        }
    }

    void CheckIfGrounded()
    {
        isGrounded = false;
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 5.0F, groundLayerMask))
        {
            if (hit.transform.tag == "Ground")
            {
                if (Vector3.Distance(hit.point, transform.position) < 0.2F)
                {
                    isGrounded = true;
                    doubleJumpFlag = true;
                }
            }
        }
    }

    void CheckForDragon()
    {
        if(dragonPower>0)
        {
            if(dragonInstance==null)
            {
                var pos = transform.position;
                pos.y += 4F;
                dragonInstance = Instantiate(dragonPrefab, pos, transform.rotation);
            }
            cameraPullback = -16F;
        }
        else
        {
            if(dragonInstance!=null)
            {
                Destroy(dragonInstance);
                dragonInstance = null;
                cameraPullback = -8F;
            }
        }
    }
}
