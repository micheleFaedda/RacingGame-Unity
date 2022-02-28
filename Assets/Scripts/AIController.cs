using UnityEngine;

public class AIController : MonoBehaviour
{

    public Circuit circuit;
    public float steeringSensitivity = 0.01f;
    CarController ds;
    Vector3 target;
    int currentWP = 0;

    float finishSteer;

    void Start()
    {    
        ds = this.GetComponent<CarController>();
        target = circuit.waypoints[currentWP].transform.position;
    }

    void Update()
    {
        Vector3 localTarget = ds.rb.gameObject.transform.InverseTransformPoint(target);
        float distanceToTarget = Vector3.Distance(target, ds.rb.transform.position);

        float targetAngle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;

        float steer = Mathf.Clamp(targetAngle * steeringSensitivity, -1.0f, 1.0f) * Mathf.Sign(ds.velocitaCorrente);
        float accel = 1f;
        float brake = 0;

        if(distanceToTarget < 5){
            brake = 0.8f;
            accel = 0.1f;
        }

        ds.Move(accel, steer, brake);

        if(distanceToTarget < 4){
            currentWP++;
            if(currentWP >= circuit.waypoints.Length)
                currentWP=0;
            
            Debug.Log("waypoint " +currentWP);
            target = circuit.waypoints[currentWP].transform.position;
        }

        ds.CheckSgommata();
        ds.CalcolaSuonoMotore();
    }

}
