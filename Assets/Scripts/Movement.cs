using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    [SerializeField]
    private float mainThrust = 1000.0f;

    [SerializeField]
    private float rotationThrust = 10.0f;

    [SerializeField]
    AudioClip mainEngine = null;

    [SerializeField]
    ParticleSystem mainEngineParticles = null;

    [SerializeField]
    ParticleSystem leftThrusterParticles = null;

    [SerializeField]
    ParticleSystem rightThrusterParticles = null;


    private Rigidbody myRigidbody = null;
    private AudioSource myAudioSource = null;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();       
    }

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

    private void LeftRotate()
    {
        ApplyRotation( rotationThrust );

        if ( !leftThrusterParticles.isPlaying )
        {
            leftThrusterParticles.Play();
        }
    }

    private void RightRotate()
    {
        ApplyRotation( -rotationThrust );

        if ( !rightThrusterParticles.isPlaying )
        {
            rightThrusterParticles.Play();
        }
    }

    private void StopRotating()
    {
        rightThrusterParticles.Stop();
        leftThrusterParticles.Stop();
    }

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

    private void StopThrusting()
    {
        myAudioSource.Stop();
        mainEngineParticles.Stop();
    }

    private void ApplyRotation( float rotationThisFrame )
    {
        // Freezing rotation so we can manually rotate.
        myRigidbody.freezeRotation = true;

        transform.Rotate( Vector3.forward * rotationThisFrame * Time.deltaTime );

        // Unfreezing rotation so the physics system can take over.
        myRigidbody.freezeRotation = false;
    }
}
