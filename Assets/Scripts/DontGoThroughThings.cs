using UnityEngine;
using System.Collections;
 
public class DontGoThroughThings : MonoBehaviour 
{
    public LayerMask layerMask; //make sure we aren't in this layer 
    public float skinWidth = 0.9f; //probably doesn't need to be changed 
 
    private float minimumExtent; 
    private float partialExtent; 
    private float sqrMinimumExtent; 
    private Vector3 previousPosition; 
 
 
    //initialize values 
    void Awake() 
    { 
       previousPosition = rigidbody.position; 
       minimumExtent = Mathf.Min(Mathf.Min(collider.bounds.extents.x, collider.bounds.extents.y), collider.bounds.extents.z); 
       partialExtent = minimumExtent * (1.0f - skinWidth); 
       sqrMinimumExtent = minimumExtent * minimumExtent; 
    } 
 
    void FixedUpdate() 
    { 
       //have we moved more than our minimum extent? 
       Vector3 movementThisStep = rigidbody.position - previousPosition; 
       float movementSqrMagnitude = movementThisStep.sqrMagnitude;
 
       if (movementSqrMagnitude > sqrMinimumExtent) 
        { 
          float movementMagnitude = Mathf.Sqrt(movementSqrMagnitude);
          RaycastHit hitInfo; 
 
          //check for obstructions we might have missed 
          if (Physics.Raycast(previousPosition, movementThisStep, out hitInfo, movementMagnitude, layerMask.value)) 
             rigidbody.position = hitInfo.point - (movementThisStep/movementMagnitude)*partialExtent; 
       } 
 
       previousPosition = rigidbody.position; 
    }
}