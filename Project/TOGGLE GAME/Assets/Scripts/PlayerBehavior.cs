using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;



public class PlayerBehavior : MonoBehaviour
{
    private static PlayerBehavior instance;
    public static PlayerBehavior Instance => instance;

    [SerializeField] private KeyCode rightKey;
    [SerializeField] private KeyCode leftKey;
    [SerializeField] private KeyCode jumpKey;
    [SerializeField] private KeyCode secondaryJumpkey;
    [SerializeField] private KeyCode interactionKey;

    [Space(30), SerializeField]
    private float horAcc;
    [SerializeField] private float horVelocityMax;
    [SerializeField] private float jumpForce;
    [SerializeField] private bool Debug_EnableOnStart;
    [SerializeField] private float maxGravitalSpeed = 50f;
    [SerializeField] private float wallSlideSpeed = 10f;
    [SerializeField] private Vector3 WalljumpForce;
    [SerializeField] private LayerMask groundLayer;

    [Space(30), SerializeField]
    private Transform footRCO;
    [SerializeField] private Transform frontTopRCO;
    [SerializeField] private Transform frontBottomRCO;
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject sceneControl;

    private bool inputEnabled = false;

    private Rigidbody2D rBody;
    private SimpleSoundModule soundModule;

    private bool cliffHanged = false;
    private bool wallSliding = false;
    private bool grounded = false;
    private bool landingSoundFlag = false;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        rBody = GetComponent<Rigidbody2D>();
        soundModule = GetComponent<SimpleSoundModule>();
    }

    private void OnEnable()
    {
        inputEnabled = Debug_EnableOnStart;
    }

    public void FixedUpdate()
    {
        if (inputEnabled)
        {
            if (!cliffHanged)
            {
                if (Input.GetKey(rightKey))
                {
                    rBody.velocity += new Vector2(horAcc, 0);
                    transform.rotation = Quaternion.Euler(Vector3.up * 0f);
                    if (rBody.velocity.x > horVelocityMax)
                        rBody.velocity = new Vector2(horVelocityMax, rBody.velocity.y);
                }
                else if (Input.GetKey(leftKey))
                {
                    rBody.velocity += new Vector2(-horAcc, 0);
                    transform.rotation = Quaternion.Euler(Vector3.up * 180f);
                    if (rBody.velocity.x < -horVelocityMax)
                        rBody.velocity = new Vector2(-horVelocityMax, rBody.velocity.y);
                }
                else
                {
                    rBody.velocity = Vector2.Lerp(rBody.velocity, new Vector2(0f, rBody.velocity.y), 0.3f);//new Vector2(0f, rBody.velocity.y);
                }
            }
        }
    }

    float specialJumpTimer = 0f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(inputEnabled)
        if (collision.gameObject.tag == "Ghost")
        {
                sceneControl.SetActive(true);
        }
    }

    public void Update()
    {
        bool jumpKeyDown = Input.GetKeyDown(jumpKey) || Input.GetKeyUp(secondaryJumpkey);

        specialJumpTimer += Time.deltaTime;

        if (Physics2D.Raycast(footRCO.position, Vector2.down, 0.2f, groundLayer))
        {
            Debug.DrawLine(footRCO.position, footRCO.position + Vector3.down * 0.2f);
            grounded = true;
        }
        else
        {
            grounded = false;
        }

        if (grounded && landingSoundFlag == true && rBody.velocity.y < 3f)
        {
            landingSoundFlag = false;
            soundModule.Play("Land");
        }


        if (-maxGravitalSpeed > rBody.velocity.y)
        {
            rBody.velocity = new Vector2(rBody.velocity.x, -maxGravitalSpeed);
        }

        Debug.DrawLine(frontBottomRCO.position, frontBottomRCO.position + transform.right * 0.1f);
        Debug.DrawLine(frontTopRCO.position, frontTopRCO.position + transform.right * 0.1f);

        if (inputEnabled)
        {
            if (jumpKeyDown)
            {
                if (grounded)
                {
                    rBody.velocity = new Vector2(rBody.velocity.x, jumpForce);

                    wallSliding = false;
                    cliffHanged = false;

                    if (landingSoundFlag == false)
                    {
                        landingSoundFlag = true;
                    }
                }

                if (specialJumpTimer < 0.3f)
                {
                    Debug.Log("Special_Jump");
                    wallSliding = false;
                    cliffHanged = false;
                    anim.SetBool("CliffHang", false);

                    if (Physics2D.Raycast(frontBottomRCO.position, transform.right, 0.1f, groundLayer))
                    {
                        rBody.velocity = new Vector3(-transform.right.x * WalljumpForce.x, WalljumpForce.y);
                    }
                    else
                    {
                        rBody.velocity = new Vector3(transform.right.x * WalljumpForce.x, WalljumpForce.y);
                    }
                    // rBody.AddForce((Vector2.up * jumpForce) + additinalWalljumpForce);
                    //transform.rotation *= Quaternion.Euler(Vector3.up * 180f);
                }

                if (cliffHanged)
                {
                    Debug.Log("Special_Jump");
                    rBody.WakeUp();
                    wallSliding = false;
                    cliffHanged = false;
                    anim.SetBool("CliffHang", false);

                    if (Physics2D.Raycast(frontBottomRCO.position, transform.right, 0.1f, groundLayer))
                    {
                        rBody.velocity = new Vector3(0f, WalljumpForce.y);
                    }
                    else
                    {
                        rBody.velocity = new Vector3(0f, WalljumpForce.y);
                    }
                }
            }

            if (!grounded)
            {
                if (rBody.velocity.y < 0f)
                {
                    if (Physics2D.Raycast(frontBottomRCO.position, transform.right, 0.1f, groundLayer))
                    {
                        if (Physics2D.Raycast(frontTopRCO.position, transform.right, 0.1f, groundLayer))
                        {
                            wallSliding = true;
                            specialJumpTimer = 0f;
                            rBody.velocity = new Vector2(rBody.velocity.x, 0f);
                        }
                        else
                        {
                            anim.SetBool("CliffHang", true);
                            cliffHanged = true;
                            specialJumpTimer = 0f;
                            rBody.Sleep();
                        }

                    }
                    else
                    {
                        wallSliding = false;
                    }
                }
            }
            else
            {
                wallSliding = false;
                cliffHanged = false;
            }
        }

        anim.SetBool("Grounded", grounded);
        anim.SetFloat("HorSpeedAbs", Mathf.Abs(rBody.velocity.x));
        anim.SetFloat("VertSpeedAbs", Mathf.Abs(rBody.velocity.y));
        anim.SetFloat("VertSpeed", rBody.velocity.y);
    }
    public void setMovementEnabled(bool value)
    {
        inputEnabled = value;
    }

    public void OnGhostCaught()
    {

    }

    public void StunPlayer(float time)
    {
        StopStunCoroutine();
        StartCoroutine("Cor_StunPlayer", time);
    }

    private void StopStunCoroutine()
    {
        StopCoroutine("Cor_StunPlayer");
        inputEnabled = true;
    }

    private IEnumerator Cor_StunPlayer(float time)
    {
        anim.SetBool("Stun", true);
        inputEnabled = false;

        yield return new WaitForSeconds(time);

        inputEnabled = true;
        anim.SetBool("false", true);

        yield return null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "NextGate")
        {
            collision.GetComponent<EndingPortal>().GoToEnding();
        }
    }
}
