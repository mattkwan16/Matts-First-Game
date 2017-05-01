using UnityEngine;
using System.Collections;

public class BlinkController : MonoBehaviour {

    public ParticleSystem BlinkRing;
    public GameObject Player;
    private bool playRing = false;
    private bool trigRing = false;

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            playRing = !playRing;
            trigRing = true;
        }

        if (trigRing)
        {
            if (playRing)
            {
                BlinkRing.Stop();
                BlinkRing.Play();
            }
            else
            {
                BlinkRing.Stop();
            }
            trigRing = false;
        }

        if(playRing && Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 tempDestination = BlinkRing.transform.position;
            tempDestination.y = 0; // Hold Player to ground
            Player.transform.position = tempDestination;
            BlinkRing.Stop();
            playRing = false;
        }
    }
}
