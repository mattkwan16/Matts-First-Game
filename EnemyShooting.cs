using UnityEngine;
using System.Collections;

public class EnemyShooting : MonoBehaviour {

    //Drag in the Bullet Emitter from the Component Inspector.
    public GameObject Bullet_Emitter;
    //Drag in the Bullet Prefab from the Component Inspector.
    public GameObject Bullet;
    public GameObject Shell;
    public GameObject Player;
    //Enter the Speed of the Bullet from the Component Inspector.
    public float BULLET_FORWARD_FORCE;
    public float SHELL_FORWARD_FORCE;

    public float timeBetweenBullets = 1.5f;
    public float range = 100f;
    public Vector3 dirOffset;

    float timer;                                    // A timer to determine when to fire.
    Ray shootRay;                                   // A ray from the gun end forwards.
    RaycastHit shootHit;                            // A raycast hit to get information about what was hit.
    int shootableMask;                              // A layer mask so the raycast only hits things on the shootable layer.
    ParticleSystem gunParticles;                    // Reference to the particle system.
    LineRenderer gunLine;                           // Reference to the line renderer.
    AudioSource gunAudio;                           // Reference to the audio source.
    Light gunLight;                                 // Reference to the light component.
    float effectsDisplayTime = 0.2f;                // The proportion of the timeBetweenBullets that the effects will display for.

    void Awake()
    {
        // Create a layer mask for the Shootable layer.
        shootableMask = LayerMask.GetMask("Shootable");

        // Set up the references.
        gunParticles = GetComponent<ParticleSystem>();
        gunLine = GetComponent<LineRenderer>();
        gunAudio = GetComponent<AudioSource>();
        gunLight = GetComponent<Light>();
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        // Add the time since Update was last called to the timer.
        timer += Time.deltaTime;

        // If the Fire1 button is being press and it's time to fire...
        if (timer >= timeBetweenBullets)
        {
            // ... shoot the gun.
            Shoot();
        }
        // If the timer has exceeded the proportion of timeBetweenBullets that the effects should be displayed for...
        if (timer >= .15f * effectsDisplayTime)
        {
            // ... disable the effects.
            DisableEffects();
        }
    }

    public void DisableEffects()
    {
        // Disable the line renderer and the light.
        //gunLine.enabled = false;
        gunLight.enabled = false;
    }

    void Shoot()
    {
        // Reset the timer.
        timer = 0f;

        // Play the gun shot audioclip.
        gunAudio.Play();

        // Enable the light.
        gunLight.enabled = true;

        // Stop the particles from playing if they were, then start the particles.
        gunParticles.Stop();
        gunParticles.Play();

        // Set the shootRay so that it starts at the end of the gun and points forward from the barrel.
        shootRay.origin = transform.position;
        shootRay.direction = transform.forward + dirOffset;

        //The Bullet instantiation happens here.
        GameObject Temporary_Bullet_Handler;
        Temporary_Bullet_Handler = Instantiate(Bullet, Bullet_Emitter.transform.position, Bullet_Emitter.transform.rotation) as GameObject;
        GameObject Temporary_Shell_Handler;
        Temporary_Shell_Handler = Instantiate(Shell, Bullet_Emitter.transform.position, Bullet_Emitter.transform.rotation) as GameObject;

        //Sometimes bullets may appear rotated incorrectly due to the way its pivot was set from the original modeling package.
        //This is EASILY corrected here, you might have to rotate it from a different axis and or angle based on your particular mesh.
        Temporary_Bullet_Handler.transform.Rotate(Vector3.forward * 90);
        Temporary_Shell_Handler.transform.Rotate(Vector3.forward * 90 * Random.value + Vector3.right * Random.value * 90);
        //Retrieve the Rigidbody component from the instantiated Bullet and control it.
        Rigidbody Temporary_RigidBody;
        Rigidbody Temporary_RigidBody2;
        Temporary_RigidBody = Temporary_Bullet_Handler.GetComponent<Rigidbody>();
        Temporary_RigidBody2 = Temporary_Shell_Handler.GetComponent<Rigidbody>();
        //Tell the bullet to be "pushed" forward by an amount set by Bullet_Forward_Force.
        Temporary_RigidBody.AddForce(transform.forward * BULLET_FORWARD_FORCE);
        Temporary_RigidBody2.AddForce(transform.forward * SHELL_FORWARD_FORCE / 4 + transform.right * SHELL_FORWARD_FORCE * Random.value + transform.up * SHELL_FORWARD_FORCE * Random.value * 1.25f);
        //Basic Clean Up, set the Bullets to self destruct after 10 Seconds, I am being VERY generous here, normally 3 seconds is plenty.
        Destroy(Temporary_Bullet_Handler, 3.0f);
        Destroy(Temporary_Shell_Handler, 3.0f);

    }
}
