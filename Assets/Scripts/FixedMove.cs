using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedMove : MonoBehaviour
{
    private Rigidbody2D rb;

    public float velocityX = 0;
    public float velocityY = 0;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(velocityX, velocityY);
    }

}
