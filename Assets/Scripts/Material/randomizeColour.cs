using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomizeColour : MonoBehaviour
{
    [SerializeField] private Color baseColor;
    [SerializeField] private bool unifyVariationWithRed;
    [SerializeField, Range(0f, 1f)] private float redVariation;
    [SerializeField, Range(0f, 1f)] private float greenVariation;
    [SerializeField, Range(0f, 1f)] private float blueVariation;
    [SerializeField] private bool changeOverTime;
    [SerializeField] private float secondsToChange;
    [SerializeField] private float tinyChange;
    public float alpha = 1; 



    private SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();

        randomize();

        if (changeOverTime)
            InvokeRepeating("smallRandomize", secondsToChange, secondsToChange);
    }

    public void randomize()
    {
        float redVaried = Random.Range(-redVariation, redVariation);
        float newRed = Mathf.Clamp(baseColor.r + redVaried, 0f, 1f);
        float newGreen = 0.0f;
        float newBlue = 0.0f;

        if (unifyVariationWithRed)
        {
            newGreen = Mathf.Clamp(baseColor.g + redVaried, 0f, 1f);
            newBlue = Mathf.Clamp(baseColor.b + redVaried, 0f, 1f);
        }
        else
        {
            newGreen = Mathf.Clamp(baseColor.g + Random.Range(-greenVariation, greenVariation), 0f, 1f);
            newBlue = Mathf.Clamp(baseColor.b + Random.Range(-blueVariation, blueVariation), 0f, 1f);
        }
       

        sprite.color = new Color(newRed, newGreen, newBlue, alpha);
        
    }

    public void smallRandomize()
    {
        float redVaried = Random.Range(-redVariation / tinyChange, redVariation / tinyChange);
        float newRed = Mathf.Clamp(sprite.color.r + redVaried, Mathf.Clamp(baseColor.r - redVariation, 0f, 1f), Mathf.Clamp(baseColor.r + redVariation, 0f, 1f));
        float newGreen = 0.0f;
        float newBlue = 0.0f;

        if (unifyVariationWithRed)
        {
            newGreen = Mathf.Clamp(sprite.color.g + redVaried, Mathf.Clamp(baseColor.g - greenVariation, 0f, 1f), Mathf.Clamp(baseColor.g + greenVariation, 0f, 1f));
            newBlue = Mathf.Clamp(sprite.color.b + redVaried, Mathf.Clamp(baseColor.b - blueVariation, 0f, 1f), Mathf.Clamp(baseColor.b + blueVariation, 0f, 1f));
        }
        else
        {
            newGreen = Mathf.Clamp(sprite.color.g + Random.Range(-greenVariation / tinyChange, greenVariation / tinyChange), Mathf.Clamp(baseColor.g - greenVariation, 0f, 1f), Mathf.Clamp(baseColor.g + greenVariation, 0f, 1f));
            newBlue = Mathf.Clamp(sprite.color.b + Random.Range(-blueVariation / tinyChange, blueVariation / tinyChange), Mathf.Clamp(baseColor.b - blueVariation, 0f, 1f), Mathf.Clamp(baseColor.b + blueVariation, 0f, 1f));
        }
        sprite.color = new Color(newRed, newGreen, newBlue, alpha);
    }

}
