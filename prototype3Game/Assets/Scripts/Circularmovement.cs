using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Circularmovement : MonoBehaviour
{
    // float radius = 10f;
    [SerializeField]
    Transform rotationCenter;
    [SerializeField] float rotationRadius = 5f;
    [SerializeField] float angularSpeed = 2f;
    [SerializeField] private GameObject goalFx;
    private GameObject smallCircle;
    private CircleMake circleMakeScript;
    public AudioSource backgroundSound;
    public AudioSource hitSound;
    public AudioSource failSound;
    public AudioSource missSound;
    public int health = 3;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    public static Circularmovement main;
    [SerializeField] float baseAngularSpeed = 2f; // base speed
    [SerializeField] float speedIncreaseFactor = 0.1f;

    float posx, posy, angle = 0f;
    int missedNotes = 0;
    bool clockwise = true;
    bool canIncrementMissedNotes = true;
    bool wasHit = true;
    bool overlapping = false;

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        circleMakeScript = FindObjectOfType<CircleMake>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Image img in hearts)
        {
            img.sprite = emptyHeart;
        }
        for (int i = 0; i < health; i++)
        {
            hearts[i].sprite = fullHeart;
        }


        Debug.Log("washit is" + wasHit);
        Debug.Log("overlapping is" + overlapping);
        wasHit = false;

        if (Input.GetKeyDown("space"))
        {
            clockwise = !clockwise;
        }

        float targetAngle = angle + (clockwise ? -1 : 1) * angularSpeed * Time.deltaTime;
        angle = Mathf.LerpAngle(angle, targetAngle, Time.deltaTime * angularSpeed);

        posx = rotationCenter.position.x + Mathf.Cos(angle) * rotationRadius;
        posy = rotationCenter.position.y + Mathf.Sin(angle) * rotationRadius;
        transform.position = new Vector2(posx, posy);

        if (!clockwise)
        {
            angle = angle + (Time.deltaTime * angularSpeed);
        }
        else
        {
            angle = angle - (Time.deltaTime * angularSpeed);
        }

        if (health == 0)
        {
            if (backgroundSound.isPlaying)
            {
                backgroundSound.Stop();
                failSound.Play();
            }
            LevelManager.main.loser();
        }

        if (Input.GetKeyDown("space") && overlapping)
        {
            // Handle hit
            wasHit = true;
            hitSound.Play();
            GameObject fx = Instantiate(goalFx, transform.position, Quaternion.identity);
            Destroy(fx, 0.5f);
            HandleHit();
        }
    }

    private void HandleHit()
    {
        // Your hit handling logic
        LevelManager.main.IncreasePoints();
        LevelManager.main.UpdateCombo(true);
        Destroy(smallCircle);
        circleMakeScript.SpawnRandomCircle();
    }

    public void UpdateSpeed(int combo)
    {
        angularSpeed = baseAngularSpeed + (combo * speedIncreaseFactor);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("SmallCircle"))
        {
            overlapping = true;
            smallCircle = collision.gameObject;
        }

        else if (collision.gameObject.CompareTag("AvoidCircle"))
        {
            health--;
            missSound.Play();
            LevelManager.main.UpdateCombo(false);
            smallCircle = collision.gameObject;
            Destroy(smallCircle);
            circleMakeScript.avoidSpawned = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("SmallCircle"))
        {
            overlapping = false;  // Reset overlapping on exit

            if (!wasHit && canIncrementMissedNotes && other.gameObject.activeInHierarchy)
            {
                /*if (other.gameObject.CompareTag("AvoidCircle"))
                {
                    // Logic for successfully avoiding the circle (if needed)
                }*/
                // Handle miss only if wasHit is false
                HandleMiss(other.gameObject);
            }

            wasHit = false;  // Reset wasHit on exit
        }
    }
    private void HandleMiss(GameObject missedObject)
    {
        health -= 1;
        missSound.Play();
        LevelManager.main.UpdateCombo(false);
        Debug.Log("Missed a circle: " + missedNotes);
        StartCoroutine(MissedNotesCooldown());
    }

    IEnumerator MissedNotesCooldown()
    {
        canIncrementMissedNotes = false;
        yield return new WaitForSeconds(0.5f); // Adjust the cooldown time as necessary
        canIncrementMissedNotes = true;
    }
}