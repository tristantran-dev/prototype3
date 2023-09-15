using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleMake : MonoBehaviour
{
    // Start is called before the first frame update
    public LineRenderer circleRenderer;
    void Start()
    {
      drawCircle(100,5);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void drawCircle(int steps, float radius){
        circleRenderer.positionCount = steps;

        for (int currentstep = 0; currentstep < steps; ++currentstep){
          float circumferenceProgress = (float) currentstep/steps;
          float currentradian = circumferenceProgress * 2 * Mathf.PI;
          float xScale = Mathf.Cos(currentradian) * radius;
          float yScale = Mathf.Sin(currentradian) * radius;

          Vector3 currentPosition = new Vector3(xScale,yScale,0);
          circleRenderer.SetPosition(currentstep, currentPosition);

        }
    }
}
