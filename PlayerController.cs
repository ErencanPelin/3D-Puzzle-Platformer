using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // [HideInInspector]
    public Vector3 input; //stores the input globally
    public float targetRotation; //holds the rotation intended for the player
    float ref_turnSmoothVelocity; //just to store rotation velocity value

    public int points = 0;
    public Text pointsText;    
    private bool onJumpable;
    private bool onGround;
    private bool running;
    public float rotateSpeed = 0.2f;
    public Transform camPivot;
    private Animator anim;
    private Rigidbody rb;

    public float jumpForce = 3f;
    public bool underwater = false;
    public ParticleSystem waterSplash;
    public ParticleSystem waterRipple;
    public ParticleSystem groundLand;
    public ParticleSystem groundRun;

    public float health = 100f;
    public Slider healthSlider;
    public bool canTakeDamage = true;
    public Material playerNormal;
    public Material playerHurt;
    public GameObject modelMesh;
    public GameObject playerRag;

    public GameObject gameOverPanel;

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            if (health > 5)
            {
                if (canTakeDamage)
                {
                    StartCoroutine(TakeDamage());
                }
            }
            else 
            {
                if (canTakeDamage)
                {
                    health = 0;
                    healthSlider.value = health;
                    //kill
                    gameOverPanel.SetActive(true);
                    gameOverPanel.transform.GetChild(0).GetComponent<Text>().text =
                        "<color=#FF0000>Game Over...</color>" + "\n" +
                        "<size=7>" +
                        "<color=#888888>" +
                        "You Found: " +
                        "</color>" + 
                        "<color=#FFA900>" + 
                        points + 
                        " stars</color>" +
                        "</size>";
                    Instantiate(playerRag, this.transform.position, Quaternion.identity);
                    Destroy(this.gameObject);

                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                }
            }
        }
    }

    IEnumerator TakeDamage() 
    {
        health -= 5f;
        canTakeDamage = false;
        modelMesh.GetComponent<Renderer>().material = playerHurt;
        yield return new WaitForSeconds(3f);
        canTakeDamage = true;
        modelMesh.GetComponent<Renderer>().material = playerNormal;
    }

    IEnumerator WaterSplash()
    {
        waterSplash.Play();
        yield return new WaitForSeconds(1f);
        waterSplash.Stop();
    }

    IEnumerator GroundLand()
    {
        groundLand.Play();
        yield return new WaitForSeconds(1f);
        groundLand.Stop();
    }


    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Water"))
        {
            StartCoroutine(WaterSplash());
        }
        else if (col.gameObject.CompareTag("ground") && !underwater)
        {
            StartCoroutine(GroundLand());
        }
        else if (col.gameObject.CompareTag("flag")) 
        {
            //complete game
            gameOverPanel.SetActive(true);
            gameOverPanel.transform.GetChild(0).GetComponent<Text>().text =
                "<color=#00FF00>You Did It!...</color>" + "\n" +
                "<size=7>" +
                "<color=#888888>" +
                "You Found: " +
                "</color>" +
                "<color=#FFA900>" +
                points +
                " stars</color>" +
                "</size>";
            //reset variables
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            running = false;
            onGround = true;
            onJumpable = false;
            this.GetComponent<Animator>().enabled = false;
            this.GetComponent<PlayerController>().enabled = false;
        }
    }

    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "ground")
        {
            onJumpable = true;
            onGround = true;
        }
        if (col.gameObject.CompareTag("unjumpable"))
        {
            onGround = true;
            onJumpable = false;
        }
        else if (col.gameObject.tag == "Water")
        {
            underwater = true;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("ground"))
        {
            onJumpable = false;
            onGround = false;
        }
        else if (col.gameObject.CompareTag("unjumpable")) 
        {
            onGround = false;
            onJumpable = false;
        }
        else if (col.gameObject.CompareTag("Water"))
        {
            underwater = false;
        }
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        pointsText.text = "<color=white>Stars: </color>" + points;
        healthSlider.value = health;
    }

    private void FixedUpdate()
    {
        input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")); //get the input
        Vector3 inputDir = input.normalized; //normalises the input to determine the direction

        float jump = Input.GetAxisRaw("Jump");

        if (onJumpable && jump > 0)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        //rotate the player to the direction they are walking IF THEY ARE INPUTTING A DIRECTION
        //this rotation setting function works perfectly, we can keep this and use it no matter how we set the movement.
        if (inputDir != Vector3.zero)
        {
            targetRotation = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg + camPivot.transform.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref ref_turnSmoothVelocity, rotateSpeed);
        }

        if (onGround && Input.GetMouseButtonDown(0)) 
        {
            StartCoroutine(Attack());
        }

        running = (input.magnitude > 0.1f) ? true : false;
        anim.SetBool("running", running);
        anim.SetBool("onGround", onGround);

        if (running && underwater)
        {
            waterRipple.enableEmission = true;
            groundRun.enableEmission = false;
            anim.speed = 0.6f;
        }
        else if (running && !underwater)
        {
            waterRipple.enableEmission = false;
            if (onJumpable)
            {
                groundRun.enableEmission = true;
                anim.speed = 1.2f;
            }
            else
            {
                groundRun.enableEmission = false;
                anim.speed = 1.2f;
            }

        }
        else if (!running)
        {
            waterRipple.enableEmission = false;
            groundRun.enableEmission = false;
            anim.speed = 1f;
        }
    }

    IEnumerator Attack() 
    {
        anim.SetBool("Attack", true);
        yield return new WaitForSeconds(0.25f);
        anim.SetBool("Attack", false);
    }
}
