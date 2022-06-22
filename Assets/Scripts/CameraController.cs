using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //public Transform player; // Mario's Transform
    public FloatVariable playerXPos;
    public FloatVariable playerYPos;
    public FloatVariable cameraXPos;
    public Transform endLimit; // GameObject that indicates end of map
    public Transform startLimit;
    public Transform ceiling;
    public Transform floor;

    private float offsetX; // initial x-offset between camera and Mario
    private float offsetY; // initial y-offset between camera and Mario
    private float startX; // smallest x-coordinate of the Camera
    private float endX; // largest x-coordinate of the camera
    private float startY;
    private float endY;
    private float viewportHalfWidth;
    private float viewportHalfHeight;
    
    void Start()
    {
        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0,0,0));
        viewportHalfWidth = Mathf.Abs(bottomLeft.x - this.transform.position.x);
        viewportHalfHeight = Mathf.Abs(bottomLeft.y - this.transform.position.y);

        offsetX = this.transform.position.x - playerXPos.value;
        Debug.Log(offsetX);
        offsetY = this.transform.position.y - playerYPos.value;
        Debug.Log(offsetY);

        startX = startLimit.transform.position.x + viewportHalfWidth;
        endX = endLimit.transform.position.x - viewportHalfWidth;

        startY = floor.transform.position.y + viewportHalfHeight;
        endY = ceiling.transform.position.y - viewportHalfHeight;
        
        updateCameraValues();
    }

    void updateCameraValues()
    {
        cameraXPos.SetValue(this.transform.position.x);
    }

    // Update is called once per frame
    void Update()
    {
        updateCameraValues();
        float desiredX = playerXPos.value + offsetX;
        float desiredY = playerYPos.value + offsetY;

        desiredX = Mathf.Clamp(desiredX, startX, endX);

        // Look Down Feature
        if(Input.GetKey("s"))
        {
            desiredY = Mathf.Clamp(desiredY, startY, endY);
            desiredY -= 3;
        }
        desiredY = Mathf.Clamp(desiredY, startY, endY);

        if(desiredX >= startX && desiredX <= endX)
        {
            this.transform.position = new Vector3(desiredX, this.transform.position.y, this.transform.position.z);
        }

        if(desiredY >= startY && desiredY <= endY)
        {
            this.transform.position = new Vector3(this.transform.position.x, desiredY, this.transform.position.z);
        }
        
    }
}
