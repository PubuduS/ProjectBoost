using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{

    [SerializeField]
    private float levelLoadDelay = 2.0f;

    [SerializeField]
    AudioClip crashSound = null;

    [SerializeField]
    AudioClip successSound = null;

    [SerializeField]
    ParticleSystem successParticles = null;

    [SerializeField]
    ParticleSystem crashParticles = null;

    private AudioSource myAudioSource = null;

    private bool isTransitioning = false;
    private bool collisionDisabled = false;

    private void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        RespondToDebugKeys();
    }

    private void OnCollisionEnter( Collision other )
    {

        if( isTransitioning || collisionDisabled )
        {
            return;
        }

        switch( other.gameObject.tag )
        {
            case "Friendly":
                Debug.Log("Bumped into a launch pad");
                break;
            
            case "Finish":
                StartSucessSequence();
                break;

            default:
                // Invoke add a delay to the method.
                StartCrashSequence();
                break;
        }
    }

    private void RespondToDebugKeys()
    {
        if ( Input.GetKey( KeyCode.L ) )
        {
            LoadNextLevel();            
        }
        else if ( Input.GetKey( KeyCode.C ) )
        {
            // Toggle collision
            collisionDisabled = !collisionDisabled; 
        }
    }
    private void StartSucessSequence()
    {
        isTransitioning = true;
        myAudioSource.Stop();
        myAudioSource.PlayOneShot( successSound );
        successParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke( "LoadNextLevel", levelLoadDelay );
    }

    private void StartCrashSequence()
    {        
        // todo add particle effect upon crash.
        isTransitioning = true;
        myAudioSource.Stop();
        myAudioSource.PlayOneShot( crashSound );
        crashParticles.Play();
        GetComponent<Movement>().enabled = false;
        Invoke( "ReloadLevel", levelLoadDelay );
    }

    private void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if( nextSceneIndex == SceneManager.sceneCountInBuildSettings )
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene( nextSceneIndex );
    }

    private void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene( currentSceneIndex );

    }
    
}
