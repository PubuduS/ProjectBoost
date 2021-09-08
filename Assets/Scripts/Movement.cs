using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    //! Holds the value for main thrust.
    [SerializeField]
    private float mainThrust = 1000.0f;

    //! Holds the value for rotation thrust.
    [SerializeField]
    private float rotationThrust = 10.0f;

    //! Holds the audio clip for main engine.
    [SerializeField]
    AudioClip mainEngine = null;

    //! Holds the particles for main engine.
    [SerializeField]
    ParticleSystem mainEngineParticles = null;

    //! Holds the particles for left engine.
    [SerializeField]
    ParticleSystem leftThrusterParticles = null;

    //! Holds the particles for right engine.
    [SerializeField]
    ParticleSystem rightThrusterParticles = null;

    //! This will make our ship react to the physics system.
    private Rigidbody myRigidbody = null;

    //! Used to change the audio clips.
    private AudioSource myAudioSource = null;

    //! Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myAudioSource = GetComponent<AudioSource>();
    }

    //! Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();       
    }

    //! Start the thrusting as long as user holds space button.
    //! Otherwise, stop the thrusting.
    private void ProcessThrust()
    {
        if( Input.GetKey( KeyCode.Space ) )
        {
            StartThrusting();

        }
        else
        {
            StopThrusting();
        }
    }

    //! Process the rotation based on the right and left keys.
    private void ProcessRotation()
    {
        if( Input.GetKey( KeyCode.A ) )
        {
            LeftRotate();

        }
        else if( Input.GetKey( KeyCode.D ) )
        {
            RightRotate();
        }
        else
        {
            StopRotating();
        }
    }

    //! Add left rotation.
    //! Play the left rotaion particles under left engine.
    private void LeftRotate()
    {
        ApplyRotation( rotationThrust );

        if ( !leftThrusterParticles.isPlaying )
        {
            leftThrusterParticles.Play();
        }
    }

    //! Add right rotation.
    //! Play the right rotaion particles under right engine.
    private void RightRotate()
    {
        ApplyRotation( -rotationThrust );

        if ( !rightThrusterParticles.isPlaying )
        {
            rightThrusterParticles.Play();
        }
    }

    //! Stop the rotation when pleyer stop pressing left/right keys.
    private void StopRotating()
    {
        rightThrusterParticles.Stop();
        leftThrusterParticles.Stop();
    }

    //! Start thrusting.
    //! Start the audio clip for thrusting.
    //! Start the main engine particles.
    private void StartThrusting()
    {
        myRigidbody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);

        if ( !myAudioSource.isPlaying )
        {
            myAudioSource.PlayOneShot( mainEngine );
        }

        if ( !mainEngineParticles.isPlaying )
        {
            mainEngineParticles.Play();
        }
    }

    //! Stop the thrusting.
    //! Stop the thrusting audio clip.
    //! Stop the main engine particles. 
    private void StopThrusting()
    {
        myAudioSource.Stop();
        mainEngineParticles.Stop();
    }

    //! Process the rotation.
    //! We freeze the rotation so we can manually rotate.
    //! After that we unfreeze it and yeild the rotation to the physics system.
    private void ApplyRotation( float rotationThisFrame )
    {
        // Freezing rotation so we can manually rotate.
        myRigidbody.freezeRotation = true;

        transform.Rotate( Vector3.forward * rotationThisFrame * Time.deltaTime );

        // Unfreezing rotation so the physics system can take over.
        myRigidbody.freezeRotation = false;
    }
}
