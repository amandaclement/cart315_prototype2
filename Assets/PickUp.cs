using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PickUp : MonoBehaviour
{
    public GameObject sledgehammer;
    public GameObject flaregun;
    public GameObject testtube;
    public GameObject radio;
    public GameObject invisibleBox;
    public GameObject blackOut; // for game over screen

    // particle systems to create explosion
    public ParticleSystem trailsBlack;
    public ParticleSystem trailsWhite;
    public ParticleSystem shower;
    public ParticleSystem fireball;
    public ParticleSystem dust;
    public ParticleSystem shockwave;
    public ParticleSystem smokeBlack;

    int range = 2; // to measure distance between player and various objects in scene

    // booleans to track player actions
    bool holdingSledgehammer = false;
    bool swungSledgehammer = false;
    bool swing = false;
    bool holdingFlaregun = false;
    bool holdingTesttube = false;
    bool radioTuned = false;

    // for updating text
    public Text tool;
    public Text instruction;

    // crash crate
    public MeshRenderer wholeCrate;
    public BoxCollider boxCollider;
    public GameObject fracturedCrate;
    public AudioSource crashAudioClip;

    // radio audio
    public AudioSource music;
    public AudioSource tuning;


    void Start()
    {
        blackOut.SetActive(false);

        // turning off particle systems for explosion
        trailsBlack.Stop();
        trailsWhite.Stop();
        shower.Stop();
        fireball.Stop();
        dust.Stop();
        shockwave.Stop();
        smokeBlack.Stop();
    }

    void Explode()
    {
      // play all particle systems to create explosion (trigger when test tube is thrown into pot)
      trailsBlack.Play();
      trailsWhite.Play();
      shower.Play();
      fireball.Play();
      dust.Play();
      shockwave.Play();
      smokeBlack.Play();
    }

    void gameOver()
    {
        blackOut.SetActive(true);
    }

    void Update()
    {
        // get positions of player and objects to make sure they are within range before allowing player to pick them up
        Vector3 playerPosition;
        Vector3 sledgehammerPosition;
        Vector3 flaregunPosition;
        Vector3 testtubePosition;
        Vector3 radioPosition;

        playerPosition = this.transform.position;
        sledgehammerPosition = sledgehammer.transform.position;
        flaregunPosition = flaregun.transform.position;
        testtubePosition = testtube.transform.position;
        radioPosition = radio.transform.position;

        // if player is within range, let them tune the radio by pressing Q key
        if (Vector3.Distance(playerPosition, radioPosition) < range)
        {
            instruction.text = "Press Q to tune radio"; // update instruction text
            if (Input.GetKeyUp(KeyCode.Q))
            {
                Debug.Log("tuning radio");
                tuning.mute = true; // mute tuning sound
                music.mute = false; // unmute music
                radioTuned = true;
            }
            if (radioTuned)
            {
                instruction.text = "";
            }
        } else { instruction.text = "";  }

        // if player is within range, let them pick up the test tube by pressing Q key
        if (Vector3.Distance(playerPosition, testtubePosition) < range)
        {
            // instruction to pick up test tube
            instruction.text = "Press Q to pick up test tube"; // update instruction text

            if (Input.GetKeyUp(KeyCode.Q))
            {
                Debug.Log("picked up test tube");
                testtube.transform.parent = null; // unparenting the sledgehammer
                flaregun.transform.parent = null; // unparenting the flaregun
                testtube.transform.parent = this.transform; // making test tube child of player
                testtube.transform.localPosition = new Vector3(0.15f, 0.5f, 0.5f); // positioning the test tube
                testtube.transform.localRotation = Quaternion.Euler(-20, 0, 0); // angling it
                holdingTesttube = true; // make boolean true
                tool.text = "Test tube"; // update tool text
            }

            if (holdingTesttube)
            {
                instruction.text = "Press R to throw into smoking pot"; // update instruction text
            }

            if (Input.GetKeyUp(KeyCode.R) && holdingTesttube)
            {
                instruction.text = "";
                // trigger explosion here
                testtube.transform.parent = null; // unparenting the testube
                testtube.transform.localPosition = new Vector3(0.15f, 0.2f, 0.5f); // position it randomly just so it's out of view (screen goes black anyway)
                Invoke("Explode", 1); // trigger explosion after 1 second
                Invoke("gameOver", 3); // black out screen (game over)
            }
        }
        // if player is within range, let them pick up sledgehammer by pressing Q key
        if (Vector3.Distance(playerPosition, sledgehammerPosition) < range)
        {
            // instruction to pick up sledgehammer
            instruction.text = "Press Q to pick up sledgehammer"; // update instruction text

            if (Input.GetKeyUp(KeyCode.Q))
            {
                Debug.Log("picked up sledgehammer");
                testtube.transform.parent = null; // unparenting the test tube
                flaregun.transform.parent = null; // unparenting the flare gun
                sledgehammer.transform.parent = this.transform; // making sledgehammer child of player
                sledgehammer.transform.localPosition = new Vector3(0.15f, 0.2f, 0.5f); // positioning the sledgehammer
                sledgehammer.transform.localRotation = Quaternion.Euler(-150, 8, -80); // angling it
                holdingSledgehammer = true; // make boolean true
                tool.text = "Sledgehammer"; // update tool text
            }

            if (holdingSledgehammer)
            {
                instruction.text = "";
            }
        }

        // if player is holding sledgehammer and they press R key, swing it
        if (Input.GetKeyUp(KeyCode.R) && holdingSledgehammer)
        {
            Debug.Log("swung sledgehammer");
            instruction.text = ""; // empty instruction text

            //sledgehammer.transform.position = Vector3.Lerp(rotA, rotB, Mathf.PingPong(Time.time, 1));
            swungSledgehammer = true; // make boolean true
            swing = true;

            // if player has swing sledgehammer within range of crate containing flaregun, destroy invisible box so that they can break crate open
            if ((Vector3.Distance(playerPosition, flaregunPosition) < range) && swungSledgehammer)
            {
                Debug.Log("destroyed invisible box");
                GameObject.Destroy(invisibleBox);
            }
        }

        // if box has been destroyed and player is within range, let them pick up flaregun by pressing E key
        if ((Vector3.Distance(playerPosition, flaregunPosition) < range) && boxCollider.enabled == true && holdingSledgehammer)
        {
            instruction.text = "Press R to destroy box"; // update instruction text
            if (Input.GetKeyUp(KeyCode.R))
            {
                    boxCollider.enabled = false;
                    wholeCrate.enabled = false;
                    fracturedCrate.SetActive(true);
                    crashAudioClip.Play();
            }
        }

        if ((Vector3.Distance(playerPosition, flaregunPosition) < range) && boxCollider.enabled == false)
        {
            instruction.text = "Press Q to pick up flare gun"; // update instruction text

            if (Input.GetKeyUp(KeyCode.Q))
            {
                instruction.text = "Press R to load"; // update instruction text
                sledgehammer.transform.parent = null; // unparenting the sledgehammer
                testtube.transform.parent = null; // unparenting the test tube
                // sledgehammer.GetComponent<Rigidbody>().useGravity = true; // applying gravity so it falls to ground 
                Debug.Log("picked up flaregun");
                flaregun.transform.parent = this.transform; // making flaregun child of player
                flaregun.transform.localPosition = new Vector3(0.16f, 0.2f, 0.8f); // positioning the flaregun
                flaregun.transform.localRotation = Quaternion.Euler(45, 180, 20); // angling it
                tool.text = "Flare gun"; // update tool text
                holdingFlaregun = true;
            }
            if (Input.GetKeyUp(KeyCode.R) && holdingFlaregun)
            {
                instruction.text = "Use cursor to aim & click to shoot"; // update instruction text
            }
        }
    }
}
