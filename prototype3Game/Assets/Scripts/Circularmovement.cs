using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // float radius = 10f;
    [SerializeField]
    Transform rotationCenter;
    [SerializeField] float roationRadius = 5f;
    [SerializeField] float angularSpeed = 2f;
    private CircleMake circleMakeScript;

    float posx, posy, angle = 0f;
    bool clockwise = true;

    private void Start()
    {
        circleMakeScript = FindObjectOfType<CircleMake>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("space")){
            clockwise = !clockwise;
        }

        posx = rotationCenter.position.x + Mathf.Cos(angle) * roationRadius;
        posy = rotationCenter.position.y + Mathf.Sin(angle) * roationRadius;
        transform.position = new Vector2(posx,posy);

        if(!clockwise){
            angle = angle + Time.deltaTime * angularSpeed;
        }
        else{
            angle = angle - (Time.deltaTime * angularSpeed);
        }

        if (angle >= 360f && !clockwise){
          angle = 0f;
        }
        else if ( angle <= 0f && clockwise){
          angle = 360f;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger entered with: " + other.gameObject.tag);
        if (Input.GetKey("space") && other.gameObject.CompareTag("SmallCircle"))
        {
            Destroy(other.gameObject);
            circleMakeScript.SpawnRandomCircle();
        }
    }
}
