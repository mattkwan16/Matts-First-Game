using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{

    const string ENEMY_SHOOTER_TAG = "Enemy Shooter";
    const string PLAYER_TAG = "Player";
    const double ACCURACY_RANGE = 5;
    Transform player;
    Rigidbody enemyRB;


    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag(PLAYER_TAG).transform;
        enemyRB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        if (transform.tag == ENEMY_SHOOTER_TAG)
        {
            enemyShooterAI();
        }
        else
        {
            //Debug.LogError("could not find enemy shooter tag");
        }
    }

    // Shooter that doesn't move, but aims for the player +/- an accuracy
    void enemyShooterAI()
    {
        // Create a vector from the player to the enemy.
        Vector3 enemyToPlayer = player.position - transform.position;
        //Debug.Log("enemy to player: " + enemyToPlayer);

        // Ensure the vector is entirely along the floor plane.
        enemyToPlayer.y = 0f;

        // Create a quaternion (rotation) based on looking down the vector from the player to the enemy.
        Quaternion newRotation = Quaternion.LookRotation(enemyToPlayer);
        //Debug.Log("newRotation " + newRotation);

        // Set the player's rotation to this new rotation.
        enemyRB.MoveRotation(newRotation);
    }



}