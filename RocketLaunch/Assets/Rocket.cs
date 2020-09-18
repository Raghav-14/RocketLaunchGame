using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] float trustRotate = 250.0f;
    [SerializeField] float trustMovment = 50.0f;

    [SerializeField] AudioClip rocketSound;
    [SerializeField] AudioClip newLevelSound;
    [SerializeField] AudioClip deadSound;

    [SerializeField] ParticleSystem rocketParticles;
    [SerializeField] ParticleSystem newLevelParticles;
    [SerializeField] ParticleSystem deadParticles;

    enum State {Alive , Dead , Trasending};//Transending : Moving Next Level
    State state;

    Rigidbody rigidBody;
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        state = State.Alive;
    }

    // Update is called once per frame
    void Update()
    {
        if(state == State.Alive)
        {
            MovmentOfRocket();
            RotationOfRocket();
        }
    }

    private void MovmentOfRocket()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyMovement();
        }
        else
        {
            audioSource.Stop();
            rocketParticles.Stop();
        }
    }

    private void ApplyMovement()
    {
        rigidBody.AddRelativeForce(Vector3.up * trustMovment);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(rocketSound);
            rocketParticles.Play();
        }
    }

    private void RotationOfRocket()
    {
        float rotationAngle = trustRotate * Time.deltaTime;
        rigidBody.freezeRotation = true;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(Vector3.forward * rotationAngle);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(-Vector3.forward * rotationAngle);
        }
        rigidBody.freezeRotation = false;
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (state != State.Alive) { return; }//This code for ..when player is deied then it do not go to other collision messages .hence below loop is stop 
        //due to this code block(State not equal alive means dead)  
      
        if(collision.gameObject.tag == "Finish")
        {
            state = State.Trasending;;
            audioSource.PlayOneShot(newLevelSound);
            newLevelParticles.Play();
            Invoke("LoadNextScene", 2f); //call function
        }
        else if(collision.gameObject.tag == "Unsafe")
        {
            state = State.Dead;
            audioSource.PlayOneShot(deadSound);
            deadParticles.Play();
            Invoke("LoadBeginingScene", 2f); //call function
        }
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene("Level-2");
    }

    private void LoadBeginingScene()
    {
        SceneManager.LoadScene("Level-1");
    }

}
