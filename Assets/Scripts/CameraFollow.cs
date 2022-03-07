using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //Il punto nel quale si deve posizionare rispetto alla macchina
    public Vector3 offset;
    
    //La macchina che deve seguire
    public Transform target;
    
    public float translateSpeed;
    
    public float rotationSpeed;

    private void FixedUpdate()
    {
        HandleTranslation();
        HandleRotation();
    }
   
    private void HandleTranslation()
    {
        var targetPosition = target.TransformPoint(offset);
        transform.position = Vector3.Lerp(transform.position, targetPosition, translateSpeed * Time.deltaTime);
    }
    private void HandleRotation()
    {
        var direction = target.position - transform.position;
        var rotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }
}