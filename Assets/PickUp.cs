using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PickUp : MonoBehaviour
{
    public GameObject sledgehammer;
    public GameObject hammer;
    public GameObject axe;
    public GameObject flaregun;
    public GameObject testtube;
    public GameObject radio;
    public GameObject invisibleBox;

    public GameObject blackOut; // for game over screen (lose)
    public GameObject whiteOut; // for game over screen (win)
    public GameObject youLose;
    public GameObject youWin;
    public GameObject bombDied;
    public GameObject batteryDied;
    public GameObject flareSave;
    public GameObject instructions;
    public GameObject instructionsClose;

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
    bool holdingHammer = false;
    bool holdingAxe = false;
    bool swungSledgehammer = false;
    bool swungHammer = false;
    bool swungAxe = false;
    bool holdingFlaregun = false;
    bool holdingTesttube = false;
    bool radioTuned = false;
    bool armed = false;

    // for updating text
    public Text tool;
    public Text instruction;

    // crash crate (holding flare gun)
    public MeshRenderer wholeCrate;
    public BoxCollider boxCollider;
    public GameObject fracturedCrate;
    public AudioSource crashAudioClip;

    // radio audio
    public AudioSource music;
    public AudioSource tuning;


    void Start()
    {
        // hiding game over content
        blackOut.SetActive(false);
        whiteOut.SetActive(false);
        youLose.SetActive(false);
        youWin.SetActive(false);
        bombDied.SetActive(false);
        batteryDied.SetActive(false);
        flareSave.SetActive(false);

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

    void gameOverLoseBomb()
    {
        blackOut.SetActive(true);
        youLose.SetActive(true);
        bombDied.SetActive(true);
    }

    void gameOverWin()
    {
        whiteOut.SetActive(true);
        youWin.SetActive(true);
        flareSave.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.E)) // closing instructions
        {
            instructions.SetActive(false);
            instructionsClose.SetActive(false);
        }
        // get positions of player and objects to make sure they are within range before allowing player to pick them up
        Vector3 playerPosition;
        Vector3 sledgehammerPosition;
        Vector3 hammerPosition;
        Vector3 axePosition;
        Vector3 flaregunPosition;
        Vector3 testtubePosition;
        Vector3 radioPosition;

        playerPosition = this.transform.position;
        sledgehammerPosition = sledgehammer.transform.position;
        hammerPosition = hammer.transform.position;
        axePosition = axe.transform.position;
        flaregunPosition = flaregun.transform.position;
        testtubePosition = testtube.transform.position;
        radioPosition = radio.transform.position;

        // RADIO
        // if player is within range, let them tune the radio by pressing Q key
        if (Vector3.Distance(playerPosition, radioPosition) < range)
        {
            instruction.text = "Press Q to tune radio"; // update instruction text
            instructions.SetActive(false);
            instructionsClose.SetActive(false);
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
        // END RADIO

        // TEST TUBE
        // if player is within range, let them pick up the test tube by pressing Q key
        if (Vector3.Distance(playerPosition, testtubePosition) < range)
        {
            // instruction to pick up test tube
            instruction.text = "Press Q to pick up test tube"; // update instruction text

            if (Input.GetKeyUp(KeyCode.Q))
            {
                Debug.Log("picked up test tube");
                // unparenting all other objects
                sledgehammer.transform.parent = null; 
                hammer.transform.parent = null; 
                axe.transform.parent = null; 
                flaregun.transform.parent = null; 

                // making test tube child of player
                testtube.transform.parent = this.transform;

                // positioning/angling the test tube
                testtube.transform.localPosition = new Vector3(0.15f, 0.5f, 0.5f);
                testtube.transform.localRotation = Quaternion.Euler(-20, 0, 0);
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
                testtube.transform.parent = null; // unparenting the testube
                testtube.transform.localPosition = new Vector3(0.15f, 0.2f, 0.5f); // position it randomly just so it's out of view (screen goes black anyway)
                Invoke("Explode", 1); // trigger explosion after 1 second
                Invoke("gameOverLoseBomb", 2); // black out screen (game over)
            }
        }
        // END TEST TUBE

        // SLEDGEHAMMER
        // if player is within range, let them pick up sledgehammer by pressing Q key
        if (Vector3.Distance(playerPosition, sledgehammerPosition) < range)
        {
            // instruction to pick up sledgehammer
            instruction.text = "Press Q to pick up sledgehammer"; // update instruction text

            if (Input.GetKeyUp(KeyCode.Q))
            {
                Debug.Log("picked up sledgehammer");
                // unparenting all other objects
                testtube.transform.parent = null;
                hammer.transform.parent = null;
                axe.transform.parent = null;
                flaregun.transform.parent = null;

                // making sledgehammer child of player
                sledgehammer.transform.parent = this.transform; 
                sledgehammer.transform.localPosition = new Vector3(0.15f, 0.2f, 0.5f); // positioning the sledgehammer
                sledgehammer.transform.localRotation = Quaternion.Euler(-150, 8, -80); // angling it
                holdingSledgehammer = true; // make boolean true
                armed = true;
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
            swungSledgehammer = true; // make boolean true

            // if player has swing sledgehammer within range of crate containing flaregun, destroy invisible box so that they can break crate open
            if ((Vector3.Distance(playerPosition, flaregunPosition) < range) && swungSledgehammer)
            {
                Debug.Log("destroyed invisible box");
                GameObject.Destroy(invisibleBox);
            }
        }
        // END SLEDGEHAMMER

        // HAMMER
        // if player is within range, let them pick up hammer by pressing Q key
        if (Vector3.Distance(playerPosition, hammerPosition) < range)
        {
            // instruction to pick up hammer
            instruction.text = "Press Q to pick up hammer"; // update instruction text

            if (Input.GetKeyUp(KeyCode.Q))
            {
                Debug.Log("picked up hammer");
                // unparenting all other objects
                testtube.transform.parent = null;
                sledgehammer.transform.parent = null;
                axe.transform.parent = null;
                flaregun.transform.parent = null;

                // making hammer child of player
                hammer.transform.parent = this.transform;
                // positioning/angling the hammer
                hammer.transform.localPosition = new Vector3(0.15f, 0.2f, 0.5f); 
                hammer.transform.localRotation = Quaternion.Euler(-150, 8, -80); 
                holdingHammer = true; // make boolean true
                armed = true;
                tool.text = "Hammer"; // update tool text
            }

            if (holdingHammer)
            {
                instruction.text = "";
            }
        }

        // if player is holding hammer and they press R key, swing it
        if (Input.GetKeyUp(KeyCode.R) && holdingHammer)
        {
            Debug.Log("swung hammer");
            instruction.text = ""; // empty instruction text
            swungHammer = true; // make boolean true

            // if player has swung hammer within range of crate containing flaregun, destroy invisible box so that they can break crate open
            if ((Vector3.Distance(playerPosition, flaregunPosition) < range) && swungHammer)
            {
                Debug.Log("destroyed invisible box");
                GameObject.Destroy(invisibleBox);
            }
        }
        // END HAMMER

        // AXE
        // if player is within range, let them pick up axe by pressing Q key
        if (Vector3.Distance(playerPosition, axePosition) < range)
        {
            // instruction to pick up hammer
            instruction.text = "Press Q to pick up axe"; // update instruction text

            if (Input.GetKeyUp(KeyCode.Q))
            {
                Debug.Log("picked up axe");
                // unparenting all other objects
                testtube.transform.parent = null;
                hammer.transform.parent = null;
                sledgehammer.transform.parent = null;
                flaregun.transform.parent = null;

                // making axe child of player
                axe.transform.parent = this.transform;
                // positioning/angling the axe
                axe.transform.localPosition = new Vector3(0.15f, 0.2f, 0.5f);
                axe.transform.localRotation = Quaternion.Euler(-150, 8, -80);
                holdingAxe = true; // make boolean true
                armed = true;
                tool.text = "Axe"; // update tool text
            }

            if (holdingAxe)
            {
                instruction.text = "";
            }
        }

        // if player is holding axe and they press R key, swing it
        if (Input.GetKeyUp(KeyCode.R) && holdingAxe)
        {
            Debug.Log("swung axe");
            instruction.text = ""; // empty instruction text
            swungAxe = true; // make boolean true

            // if player has swung axe within range of crate containing flaregun, destroy invisible box so that they can break crate open
            if ((Vector3.Distance(playerPosition, flaregunPosition) < range) && swungAxe)
            {
                Debug.Log("destroyed invisible box");
                GameObject.Destroy(invisibleBox);
            }
        }
        // END AXE

        // if played is armed and within range, let them destroy box nd pick up flaregun
        if ((Vector3.Distance(playerPosition, flaregunPosition) < range) && boxCollider.enabled == true && armed)
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
                // unparenting all other tools
                sledgehammer.transform.parent = null;
                hammer.transform.parent = null;
                axe.transform.parent = null;
                testtube.transform.parent = null;

                // sledgehammer.GetComponent<Rigidbody>().useGravity = true; // applying gravity so it falls to ground 
                Debug.Log("picked up flaregun");
                flaregun.transform.parent = this.transform; // making flaregun child of player
                flaregun.transform.localPosition = new Vector3(0.16f, 0.2f, 0.8f); // positioning the flaregun
                flaregun.transform.localRotation = Quaternion.Euler(45, 180, 20); // angling it
                tool.text = "Flare gun"; // update tool text
                holdingFlaregun = true;
            }
            if (holdingFlaregun)
            {
                instruction.text = "Click to shoot & R to reload."; // update instruction text
                if (Input.GetMouseButtonDown(0)) // if primary mouse button is clicked, flaregun has been shot and game ends (win)
                {
                    instruction.text = "";
                    Invoke("gameOverWin", 3); // white out screen (game over)
                }
            }
        }
    }
}
