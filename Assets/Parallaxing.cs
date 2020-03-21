using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxing : MonoBehaviour
{
    public Transform[] backgrounds;
    private float[] parallaxScales;
    public float smoothing = 1f;                   //must be more than 0

    private Transform mainCamera;                   //reference to main camera's transform
    private Vector3 previousCamPosition;            //position of cam in the previous frame

    //called before Start. Great for references
    void Awake()
    {
        mainCamera = Camera.main.transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        previousCamPosition = mainCamera.position;
        parallaxScales = new float[backgrounds.Length];

        //assigning the corresponding parallaxScales
        for (int i =0; i < backgrounds.Length; i++)
        {
            parallaxScales[i] = backgrounds[i].position.z*-1;
        }

    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < backgrounds.Length; i++)
        {
            //the parallax is the opposite of camera movement because it's the previous frame multiplied by scale
            float parallax = (previousCamPosition.x - mainCamera.position.x) * parallaxScales[i];

            //set a target x position which is the current x position + the parallax
            float backgroundTargetPosX = backgrounds[i].position.x + parallax;

            //create a target position which is the background's current position with it's target x position
            Vector3 backgroundTargetPosition = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);

            //fade between current and target positions
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPosition, smoothing * Time.deltaTime);
        }
        previousCamPosition = mainCamera.position;
    }
}
