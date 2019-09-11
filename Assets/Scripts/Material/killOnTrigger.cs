using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killOnTrigger : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.tag == "Player")
        {
            GameObject.FindWithTag("GameController").GetComponent<sceneController>().killPlayer();
        }
    }
}
