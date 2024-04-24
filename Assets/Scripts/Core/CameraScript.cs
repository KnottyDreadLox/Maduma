using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraScript : MonoBehaviour
{
    Vector3 touchStart;
    public float zoomOutMin = 1;
    public float zoomOutMax = 8;


    public Tilemap tilemap; // Reference to your Tilemap
    public float smoothSpeed = 0.125f; // The speed at which the camera moves

    void Start()
    {
       
        CenterCameraOnStartPosition();
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            zoom(difference * 0.01f);
        }
        else if (Input.GetMouseButton(0))
        {
            Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Camera.main.transform.position += direction;
        }
        zoom(Input.GetAxis("Mouse ScrollWheel"));
    }

    void zoom(float increment)
    {
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment, zoomOutMin, zoomOutMax);
    }




    void CenterCameraOnStartPosition()
    {
        // Get the position of the start tile
        Vector3Int startPosition = GameManager.startCoordinates;

        if (startPosition != Vector3Int.zero)
        {
            // Convert the tile position to world position
            Vector3 centerPosition = tilemap.CellToWorld(startPosition);
            // Set the camera's position to the center position
            transform.position = new Vector3(centerPosition.x, centerPosition.y, transform.position.z);
        }
        else
        {
            Debug.LogWarning("Start position not found.");
        }
    }


    void LateUpdate()
    {
        // You may want to add camera follow logic here if you want the camera to track something
    }
}
