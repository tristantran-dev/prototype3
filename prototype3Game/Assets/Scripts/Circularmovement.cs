using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    float posx, posy, angle = 0f;
    int missedNotes = 0;
    bool clockwise = true;
    bool canIncrementMissedNotes = true;
    bool wasHit = true;
    bool overlapping = false;

    private void Start()
    {
        circleMakeScript = FindObjectOfType<CircleMake>();
    }

    // Update is called once per frame
    void Update()
    {
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

        if (missedNotes >= 3)
        {
            LevelManager.main.loser();
        }

        /*if (Input.GetKeyDown("space") && overlapping)
        {
            Debug.Log("space clicked and in circle");
            //kind of off...
            LevelManager.main.IncreasePoints();
            *//*GameObject fx = Instantiate(goalFx, transform.position, Quaternion.identity);
            Destroy(fx, 0.5f);*//*
            Debug.Log("change washit to true");
            wasHit = true;
            Debug.Log("wasHit is " + wasHit);
            Destroy(smallCircle);
            circleMakeScript.SpawnRandomCircle();
            wasHit = false;
            //overlapping = false;
        }*/
        if (Input.GetKeyDown("space") && overlapping)
        {
            // Handle hit
            wasHit = true;
            GameObject fx = Instantiate(goalFx, transform.position, Quaternion.identity);
            Destroy(fx, 0.5f);
            HandleHit();
        }
    }

    private void HandleHit()
    {
        // Your hit handling logic
        LevelManager.main.IncreasePoints();
        Destroy(smallCircle);
        circleMakeScript.SpawnRandomCircle();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("SmallCircle"))
        {
            overlapping = true;
            smallCircle = collision.gameObject;
        }
    }

    /* private void OnTriggerStay2D(Collider2D other)
     {
         Debug.Log("Trigger entered with: " + other.gameObject.tag);
         //overlapping = true;
         if (Input.GetKeyDown("space") && other.gameObject.CompareTag("SmallCircle"))
         {
             Debug.Log("space clicked and in circle");
             //kind of off...
             LevelManager.main.IncreasePoints();
             GameObject fx = Instantiate(goalFx, transform.position, Quaternion.identity);
             Destroy(fx, 0.5f);
             Debug.Log("change washit to true");
             wasHit = true;
             Debug.Log("wasHit is " + wasHit);
             Destroy(other.gameObject);
             circleMakeScript.SpawnRandomCircle();
         }
     }*/

    private void OnTriggerExit2D(Collider2D other)
    {
        /*Debug.Log("wasHit is " + wasHit);
        if (other.gameObject.CompareTag("SmallCircle") && canIncrementMissedNotes && !wasHit && other.gameObject.activeInHierarchy)
        {
            Debug.Log("collision out");
            missedNotes += 1;
            Debug.Log("missed a circle" + missedNotes);
            StartCoroutine(MissedNotesCooldown());
        }
        if (other.gameObject.CompareTag("SmallCircle"))
        {
            Debug.Log("change washit to false");
            wasHit = false;
            overlapping = false;
        }
        *//* Debug.Log("change washit to false");
         wasHit = false;*/

        if (other.gameObject.CompareTag("SmallCircle"))
        {
            overlapping = false;  // Reset overlapping on exit

            if (!wasHit && canIncrementMissedNotes && other.gameObject.activeInHierarchy)
            {
                // Handle miss only if wasHit is false
                HandleMiss(other.gameObject);
            }

            wasHit = false;  // Reset wasHit on exit
        }
    }
    private void HandleMiss(GameObject missedObject)
    {
        missedNotes += 1;
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