using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmRotation : MonoBehaviour
{
    public int rotationOffset = 90;
    // Update is called once per frame
    void Update()
    {
        //subtracting the position from the player to the mouse position
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        difference.Normalize();         // Normalize the vector meaning that the sum of  the vector will be equal to one

        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;      //find the angle in Degrees

        transform.rotation = Quaternion.Euler(0f, 0f, rotationZ+rotationOffset);
    }
}
