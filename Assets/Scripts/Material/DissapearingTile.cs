using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissapearingTile : Notifiable
{
    private BoxCollider2D boxCollider;
    private SpriteRenderer sprite;
    private sceneController sceneController;
    public bool isRed;
    public override int priority { get { return 0; } }


    void Awake()
    {
        
        boxCollider = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        sceneController = GameObject.FindWithTag("GameController").GetComponent<sceneController>();
        active(sceneController.currentColor == sceneController.SceneColor.red && isRed || sceneController.currentColor == sceneController.SceneColor.blue && !isRed);        
    }

    public void active(bool active)
    {
        float alpha = active ? 1f : 0.4f;
        boxCollider.enabled = active;
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, alpha);
    }


    /// NOTIFIABLE INTERFACE
    
    public override void ChangeColour(bool isNewColorRed)
    {
        
        active(isRed && isNewColorRed || !isRed && !isNewColorRed);
    }

    public override void initializeNotifiable(bool isNewColorRed)
    {
        ChangeColour(isNewColorRed);
    }
}
