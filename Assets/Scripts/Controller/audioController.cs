using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioController : MonoBehaviour
{

    [SerializeField] public AudioSource jump1;
    [SerializeField] public AudioSource jump2;
    [SerializeField] public AudioSource ground;
    [SerializeField] public AudioSource changeForm;
    [SerializeField] public AudioSource death;
    [SerializeField] public AudioSource step;



    public void playJump1()
    {
        jump1.pitch = 1 + Random.Range(-0.02f, 0.15f);
        jump1.Play();
    }

    public void playJump2()
    {
        jump2.pitch = 1 + Random.Range(0, 0.02f);
        jump2.Play();
    }
    public void playGround()
    {
        ground.pitch = 1 + Random.Range(-0.05f, 0.05f);
        ground.Play();
    }

    public void playChangeFormRed()
    {
        changeForm.pitch = 1 + Random.Range(-0.30f, -0.10f);
        changeForm.Play();
    }
    public void playChangeFormBlue()
    {
        changeForm.pitch = 1 + Random.Range(0.10f, 0.30f);
        changeForm.Play();
    }

    public void playDeath()
    { 
        death.pitch = 1 + Random.Range(-0.05f, 0.05f);
        death.Play();
    }

    public void playWallJump()
    {
        jump2.pitch = 1 + Random.Range(-0.15f, -0.01f);
        jump2.Play();
    }

    public void playStep(bool left)
    {
        if (left)
            step.pitch = 1 + Random.Range(-0.06f, -0.01f);
        else
            step.pitch = 1 + Random.Range(0.01f, 0.06f);

        step.Play();
    }
}
