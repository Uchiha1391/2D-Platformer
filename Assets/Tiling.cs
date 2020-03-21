using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(SpriteRenderer))]

public class Tiling : MonoBehaviour
{
    public int offsetX = 2;                 //offset so no weird errors arise

    //for checking if instantiating buddies is needed
    public bool hasARightBuddy = false;     
    public bool hasALeftBuddy = false;

    public bool reverseScale = false;       //if object's not tilable
    
    private float spriteWidth = 0f;         //width of out element

    private Camera mainCamera;
    private Transform myTransform;

    void Awake()
    {
        mainCamera = Camera.main;
        myTransform = transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer sRenderer = GetComponent<SpriteRenderer>();
        spriteWidth = sRenderer.sprite.bounds.size.x;

    }

    // Update is called once per frame
    void Update()
    {
        if(!hasALeftBuddy || !hasARightBuddy)
        {
            //calculate camera's extent, which is half of the width, of what the camera can see in world coordinates
            float camHorizontalExtent = mainCamera.orthographicSize * Screen.width / Screen.height;

            //calculate x position where the camera can see the edge of the sprite
            float edgeVisiblePositonRight = (myTransform.position.x + spriteWidth / 2) - camHorizontalExtent;
            float edgeVisiblePositionLeft = (myTransform.position.x + -spriteWidth / 2) + camHorizontalExtent;

            //checking if we can see the edge of the element and then calling MakeNewBuddy if we can
            if(mainCamera.transform.position.x>=edgeVisiblePositonRight-offsetX&&!hasARightBuddy)
            {
                MakeNewBuddy(1);
                hasARightBuddy = true;
            }
            else if(mainCamera.transform.position.x<=edgeVisiblePositionLeft+offsetX&&!hasALeftBuddy)
            {
                MakeNewBuddy(-1);
                hasALeftBuddy = true;
            }
        }
    }

    //create a buddy on the side required
    void MakeNewBuddy(int rightOrLeft)
    {
        //calcuating the new position for our new buddy
        Vector3 newPosition = new Vector3(myTransform.position.x+spriteWidth*rightOrLeft,myTransform.position.y,myTransform.position.z);
        //Instantiating our new buddy and storing him in a variable
        Transform newBuddy =Instantiate(myTransform, newPosition, myTransform.rotation) as Transform;

        //if not tilable, reverse x size of our object to get rid of ugly scenes
        if(reverseScale==true)
        {
            newBuddy.localScale = new Vector3(newBuddy.localScale.x * -1, newBuddy.localScale.y, newBuddy.localScale.z);
        }

        newBuddy.parent = myTransform.parent;
        if(rightOrLeft>0)
        {
            newBuddy.GetComponent<Tiling>().hasALeftBuddy = true;
        }
        else
        {
            newBuddy.GetComponent<Tiling>().hasARightBuddy = true;
        }
    }
}
