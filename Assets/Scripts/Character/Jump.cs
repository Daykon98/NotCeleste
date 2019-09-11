using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Las multiples funcionalidades de salto se hacen en esta función para tenerlo todo controlado,
//incluyendose el doble salto y el salto de pared
public class Jump : MonoBehaviour
{

    private Rigidbody2D rigidBody;
	private Animator animator;
    [SerializeField] private GameObject poundParticle;
    [SerializeField] private GameObject debugCube;
    [SerializeField] private GameObject jumpParticle;

    [SerializeField] private audioController audioController;

    public float lowJumpMultiplier = 2f;
	public float fallMultiplier = 2.5f;
	public float jumpForce = 6f;
	public float fallSpeedLimit = -15f;
    public float ySpeedLimit = 1f; //El limite de velocidad de la y para que se considere que ya no puedes mantener más el salto
    public float wallSpeed = -2.5f;
    public float wallJumpForce = 5f;
    public int numberOfWallJumps = 1; //No está pensado para más de uno del todo, pero doy la posibilidad por si dios sepa


    private int numberOfJumps = 1; 
    private bool isGrounded;
    private int doubleJump;
    private int onWall; //1 = derecha, -1 = izquierda
    private bool wallJumping = false;

    public bool IsGrounded
    {
        get { return isGrounded == true; }
    }

    public bool IsWallJumping
    {
        get { return wallJumping == true; }
    }

    public int DoubleJump
    {
        get { return doubleJump;  }
    }

    private bool jumpRequest;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D> ();
		animator = GetComponent<Animator> ();
        isGrounded = false;
        doubleJump = numberOfJumps;
        onWall = 0;
        animator.SetInteger("Jumps", numberOfJumps);

        animator.SetBool("isGrounded", false);
    }


    // Update is called once per frame
    void Update()
    {
        float ySpeed = rigidBody.velocity.y;
		if (ySpeed < fallSpeedLimit) {
			rigidBody.AddForce (new Vector2 (0, - (ySpeed - fallSpeedLimit)));
		}

   		animator.SetFloat ("SpeedY", rigidBody.velocity.y);

        if (Input.GetButtonDown("Jump")){
            if (onWall != 0 && numberOfWallJumps > 0) //WALL JUMP
            {
                wallJumping = true;
                StartCoroutine("stopWallJumping");
                audioController.playWallJump();
                numberOfWallJumps--;
                animator.SetInteger("WallJump", 0);
                rigidBody.velocity = new Vector2(wallJumpForce * -onWall, wallJumpForce * 1.9f);
                onWall = 0;
                GameObject part = Instantiate(jumpParticle);
                part.transform.position = new Vector2(rigidBody.position.x, rigidBody.position.y);
                animator.SetBool("OnWall", false);

            }
            else if (doubleJump > 0)
                jumpRequest = true;
            else if (rigidBody.velocity.y <= 0)
                jumpRequest = true;
        }


        
    }

    void FixedUpdate() {
		if (jumpRequest) {
            if (isGrounded) //JUMP
            {
                audioController.playJump1();
                rigidBody.velocity = Vector2.up * jumpForce;
                isGrounded = false;
                animator.SetBool("isGrounded", false);
                jumpRequest = false;
            }
            else if (doubleJump > 0) //DOUBLE JUMP
            {
                audioController.playJump2();
                rigidBody.velocity = Vector2.up * jumpForce * 1.5f;
                //Particula del salto
                GameObject part = Instantiate(jumpParticle);
                part.transform.position = new Vector2(rigidBody.position.x, rigidBody.position.y);
                isGrounded = false;
                doubleJump--;
                animator.SetInteger("Jumps", doubleJump);
                jumpRequest = false;
            }
        }

        float ySpeed = rigidBody.velocity.y;
        if (ySpeed > 1f || ySpeed < -3f) //Si detecta caida se quita automaticamente el estado de suelo //Deja un marco en la caida para permitir saltar un poco demasiado tarde
        {
            animator.SetBool("isGrounded", false);
            isGrounded = false;
        }

		if (ySpeed < ySpeedLimit || doubleJump == 0 || IsWallJumping) {
			rigidBody.gravityScale = fallMultiplier;
		} else if (ySpeed > 0 && !Input.GetButton("Jump")) {
			rigidBody.gravityScale = lowJumpMultiplier;
		} else {
			rigidBody.gravityScale = 1f;
		}
	}

    private void OnCollisionStay2D(Collision2D collision)
    {
        int contactCount = collision.contactCount;

        if (rigidBody.velocity.y < 0f && Mathf.Abs(Input.GetAxis("Horizontal")) > 0.3f && collision.gameObject.layer == 8 /*ground*/ && !isGrounded && contactCount == 1)
        {
        ContactPoint2D[] contacts = new ContactPoint2D[collision.contactCount];
            collision.GetContacts(contacts);
            foreach (ContactPoint2D contact in contacts)
            {
                
                if (Vector2.Dot(contact.normal, Vector3.right) > 0.1f || Vector2.Dot(contact.normal, Vector3.left) > 0.1f)
                {
                    numberOfWallJumps = 1;
                    animator.SetInteger("WallJump", 1);
                    onWall = Vector2.Dot(contact.normal, Vector3.right) > 0.1f ? -1 : 1;
                    animator.SetBool("OnWall", true);
                    rigidBody.velocity = new Vector2(rigidBody.velocity.x, wallSpeed);
                    //GameObject cube = Instantiate(debugCube);
                    return;
                }
            }
        }
        

        //Los ifs son exclusivos

        //Soluciona el no detectar una nueva colision si caes desde una pared
        //No entra en collision enter si llega al suelo del mismo objeto, entonces hay que comprobarlo aquí
        if (collision.gameObject.layer == 8 /*ground*/ && !isGrounded && contactCount > 1)
        {
            ContactPoint2D[] contacts = new ContactPoint2D[collision.contactCount];
            collision.GetContacts(contacts);
            foreach (ContactPoint2D contact in contacts)
            {
                if (Vector2.Dot(contact.normal, Vector3.up) > 0.3)
                {
                    contactedGround(contact.point);
                    return;
                }
            }
           
        }
    }
    /* Hay que hace que deje de detectar que está en la pared de alguna manera AQUI O EN UN UPDATE, VEREMOS
    private void OnCollisionExit2D(Collision2D collision)
    {
        onWall = 0;
        animator.SetBool("OnWall", false);
    }*/

    void OnCollisionEnter2D(Collision2D collision)
	{
        ContactPoint2D[] contacts = new ContactPoint2D[collision.contactCount];
        collision.GetContacts(contacts);
        bool pointDown = false;
        foreach (ContactPoint2D contact in contacts)
        {
            //GameObject cube = Instantiate(debugCube);
            //cube.transform.position = contact.point;
            if (Vector2.Dot(contact.normal, Vector3.up) > 0.3)
            {
                pointDown = true;
                break;
            }
        }
    
        if (collision.gameObject.layer == 8 /*ground*/ && pointDown && !isGrounded)
        {
            contactedGround(collision.GetContact(0).point);
		}
	}

    private void contactedGround(Vector3 contactPoint)
    {
        audioController.playGround();
        GameObject part = Instantiate(poundParticle);
        part.transform.position = contactPoint;
        isGrounded = true;
        doubleJump = numberOfJumps;
        animator.SetInteger("WallJump", 1);
        animator.SetInteger("Jumps", doubleJump);
        animator.SetBool("isGrounded", true);
        onWall = 0;
        numberOfWallJumps = 1;
        animator.SetBool("OnWall", false);
        animator.SetBool("DoubleJump", true);

    }

    public void doubleJumped()
    {
        animator.SetBool("DoubleJump", false);
    }

    /*
    public void wallJumped()
    {
        animator.SetBool("WallJump", true);
    }*/

    private IEnumerator stopWallJumping()
    {
        yield return new WaitForSeconds(0.3f);
        wallJumping = false;
    }



}
