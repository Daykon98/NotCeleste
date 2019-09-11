using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeColour : Notifiable
{
    private sceneController sceneController;
    private EdgeCollider2D edgeCollider;
    private Animator animator;
    public bool isRed;
    public override int priority { get { return 0; } }


    void Awake()
    {
        animator = GetComponent<Animator>();
        edgeCollider = GetComponent<EdgeCollider2D>();
        sceneController = GameObject.FindWithTag("GameController").GetComponent<sceneController>();
        active(sceneController.currentColor == sceneController.SceneColor.red && isRed || sceneController.currentColor == sceneController.SceneColor.blue && !isRed);
    }

    public void active(bool active)
    {
        animator.SetBool("active", active);
        edgeCollider.enabled = active;
    }


    /// NOTIFIABLE INTERFACE

    public override void ChangeColour(bool isNewColorRed)
    {

        active(isRed && !isNewColorRed || !isRed && isNewColorRed);
    }

    public override void initializeNotifiable(bool isNewColorRed)
    {
        ChangeColour(isNewColorRed);
    }
}
