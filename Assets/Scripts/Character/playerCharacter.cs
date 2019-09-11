using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class playerCharacter : Notifiable {

	private Rigidbody2D rigidBody;
	private Animator animator;
	private Jump jumpController;
    private Renderer rend;
    private EmitRipple emitRipple;
    private bool alternateSteps = true;
    public override int priority { get { return 10; } }

    [SerializeField] private GameObject debugCube;
    [SerializeField] private GameObject deathParticle;
    [SerializeField] private audioController audioController;
    private sceneController sceneController;

    private Shader redChar;
    private Shader blueChar;
    public LayerMask layerMask;


    public float speed = 200.0f;
		
	private bool facingRight;

	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody2D> ();
		animator = GetComponent<Animator> ();
		jumpController = GetComponent<Jump> ();
        rend = GetComponent<Renderer>();
        sceneController = GameObject.FindWithTag("GameController").GetComponent<sceneController>();
        blueChar = rend.material.shader;
        redChar = Shader.Find("Sprites/Default");
        emitRipple = GetComponent<EmitRipple>();
        facingRight = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (!jumpController.IsWallJumping)
        {
            float movementX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
            rigidBody.velocity = new Vector2(movementX, rigidBody.velocity.y);
        }
		float xSpeed = rigidBody.velocity.x;

		animator.SetFloat ("SpeedX", Mathf.Abs(xSpeed));

        //Se utiliza la velocidad de x para controlar la velocidad de la animación si se encuentra andando
		if (jumpController.IsGrounded && Mathf.Abs(xSpeed) > 1)
			animator.speed =  Mathf.Abs(xSpeed) / 9f;
		else {
			animator.speed = 1;
		}

        //Giros basados en la velocidad
		if ((facingRight && xSpeed < 0) || (!facingRight && xSpeed > 0))
			Flip ();
	}

    /*private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(new Vector2(rigidBody.position.x, rigidBody.position.y + 1), 1);
    }*/
    public void die()
    {
        GameObject deathPart = Instantiate(deathParticle);
        deathPart.transform.position = rigidBody.position;
        audioController.playDeath();
        
        //Destroy(this.gameObject);
    }
    private void Flip(){
		facingRight = !facingRight;
		transform.Rotate(0, 180, 0);
	}
    
    public void StepOnGround()
    {
        alternateSteps = !alternateSteps;
        audioController.playStep(alternateSteps);
    }

    public void modifyCollisions(bool activate)
    {
        rigidBody.simulated = activate;
    }
    //NOTIFIABLE INTERFACE

    public override void ChangeColour(bool isNewColorRed)
    {
        //Comprobacion de que no se encuentra sobre ningun objeto
        Vector2 position = new Vector2(transform.position.x, transform.position.y + 0.6f);

        Collider2D hit1 = Physics2D.OverlapPoint(position, layerMask);
        Debug.Log(layerMask.ToString());
        if (hit1 != null && hit1.gameObject.tag == "ColorNotified")
        {
            sceneController.killPlayer();
            return;
        } // Aquí para destruir inmediatemente, en otro lado si queremos dar una oportunidad al jugador


        emitRipple.Emit(new Vector2(rigidBody.position.x, rigidBody.position.y + 1));
        if (!isNewColorRed)
        {
            audioController.playChangeFormBlue();
            emitRipple.changeColor(new Color(0.258f, 0.667f, 0.878f));
            rend.material.shader = blueChar;
        }
        else
        {
            audioController.playChangeFormRed();
            emitRipple.changeColor(new Color(0.761f, 0.369f, 0.078f));
            rend.material.shader = redChar;
        }
    }

    public override void initializeNotifiable(bool isNewColorRed)
    {
        if (isNewColorRed)
        {
            rend.material.shader = redChar;
        }
    }
}

