using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{

    public float seconds = 2;
    public float force = 10f;
    private GameObject player;
    private Rigidbody2D rb;
    private Animator animator;
    private bool facingRight = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        beginActivity();
    }

    void beginActivity()
    {
        //StartCoroutine(animate());
    }

    /*public IEnumerator animate()
    {
        animator.SetBool("start", true);
      }*/

    public void impulse()
    {
        bool right = player.transform.position.x >= transform.position.x;
        if (facingRight != right)
            Flip();
        facingRight = right;
        rb.AddForce(new Vector2( right ? force : -force, 0));
        animator.SetBool("start", false);
        //StartCoroutine(animate());


    }

    private void Flip()
    {
        transform.Rotate(new Vector3(0, 180, 0));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
            GameObject.FindGameObjectWithTag("GameController").GetComponent<sceneController>().killPlayer();
        }
}
