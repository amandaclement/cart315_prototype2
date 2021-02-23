using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// once player gets within close range of radio, allow them to tune it

public class tuneRadio : MonoBehaviour
{
    public GameObject player;
    int range = 1;

    public AudioSource music;
    public AudioSource tuning;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPosition;
        Vector3 radioPosition;

        playerPosition = player.transform.position;
        radioPosition = this.transform.position;

        // once player is within range of radio, give them option to tune it
        if (Vector3.Distance(playerPosition, radioPosition) < range)
        {
            // press E to tune radio
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("tune radio");
                tuning.mute = true; // mute tuning sound
                music.mute = false; // unmute music
            }
        }
    }
}
