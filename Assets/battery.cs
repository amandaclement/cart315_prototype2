using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// script used to deplete flashlight battery life
    // when it reaches 0, the flashlight burns out

public class battery : MonoBehaviour
{
    // public GameObject batteryLifeObject;
    private int batteryLife = 100;
    public Text life;
    public Light flashLight;

    public GameObject blackOut;
    public GameObject youLose;
    public GameObject batteryDied;

    void gameOverLoseBattery()
    {
        blackOut.SetActive(true);
        youLose.SetActive(true);
        batteryDied.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("depleteBattery", 2f, 2f); // 2f for speed of battery depletion
        blackOut.SetActive(false);
        youLose.SetActive(false);
        batteryDied.SetActive(false);
    }

    void depleteBattery()
    {
        if (batteryLife > 0) // gradually decrease until it reaches 0
        {
            batteryLife -= 1;
        }
        if (batteryLife == 0) // at zero, flashLight intensity goes to 0
        {
            Invoke("gameOverLoseBattery", 1); // black out screen (game over)
        }
        life.text = batteryLife.ToString() + "%"; // displaying flashlight battery life on screen
    }
}