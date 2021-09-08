using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    //! Before a level changed, there is a small delay.
    [SerializeField]
    private float levelLoadDelay = 2.0f;

    //! Holds the audio clip with the crash sound.
    [SerializeField]
    private AudioClip crashSound = null;

    //! Holds the audio clip with the success sound.
    [SerializeField]
    private AudioClip successSound = null;

    //! Holds the particles displayed in a successfull landing.
    [SerializeField]
    private ParticleSystem successParticles = null;

    //! Holds the particles displayed in a unsuccessfull landing.
    [SerializeField]
    private ParticleSystem crashParticles = null;

    //! Used to chanage the audio sources in situations like landing and crashing.
    private AudioSource myAudioSource = null;

    //! Disable initiating multiple sequences. Otherwise player will hear same sound again and again.
    private bool isTransitioning = false;

    //! This is just for debugging process. We can disable the collusions.
    private bool collisionDisabled = false;

    private void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        RespondToDebugKeys();
    }

    //! Whenever a collusion happened this function will get called.
    //! This function will trigger the success and crash sequences.
    private void OnCollisionEnter( Collision other )
    {

        if( isTransitioning || collisionDisabled )
        {
            return;
        }

        switch( other.gameObject.tag )
        {
            case "Friendly":                
                break;
            
            case "Finish":
                StartSucessSequence();
                break;

            default:                
                StartCrashSequence();
                break;
        }
    }

    //! This is just a function for debugging purposes.
    //! Remove before build.
    //! This allows user to load levels without completing the level
    //! and disable collusions.
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

    //! Start the sucess sequence.
    //! Play sound clip and triggeres the particles.
    //! Also, disabled the movement script.
    //! So, player won't be able to control the rocket.
    //! after a small delay, it will then load the next level.
    private void StartSucessSequence()
    {
        isTransitioning = true;
        myAudioSource.Stop();
        myAudioSource.PlayOneShot( successSound );
        successParticles.Play();
        GetComponent<Movement>().enabled = false;
        // Invoke add a delay to the method.
        Invoke( "LoadNextLevel", levelLoadDelay );
    }

    //! Start the crash sequence.
    //! Play sound clip and triggeres the particles.
    //! Also, disabled the movement script.
    //! So, player won't be able to control the rocket.
    //! after a small delay, it will then load the next level.
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

    //! Load the next level.
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

    //! Reload the level if player crashed the ship.
    private void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene( currentSceneIndex );

    }
    
}
