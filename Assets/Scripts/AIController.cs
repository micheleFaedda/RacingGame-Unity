using UnityEngine;
using System.Collections;

public class AIController : MonoBehaviour
{

    //public Circuit circuito;
    private CarController carController;
    
    public float steeringSensitivity = 0.01f;

    private IEnumerator corutine;
    
    public Vector3 targetSucc;
    public Vector3 targetPrec;

    public int wpDaRaggiungere = 0;

    public GameObject circuito;
    private Transform[] waypoints;
    
    
    void Start()
    {    
        
        waypoints = new Transform[circuito.transform.childCount];
        for (int i = 0; i < circuito.transform.childCount; i++)
        {
            waypoints[i] = circuito.transform.GetChild(i);
        }
        
        carController = this.GetComponent<CarController>();
        targetSucc = waypoints[wpDaRaggiungere].transform.position;
        corutine = WaitAndRepositioning(5f);
        StartCoroutine(corutine);
    }

    void Update()
    {

        Vector3 localTarget = carController.rb.gameObject.transform.InverseTransformPoint(targetSucc);

        float distanceToTarget = Vector3.Distance(targetSucc, carController.rb.transform.position);

        float targetAngle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;

        float steer = Mathf.Clamp(targetAngle * steeringSensitivity, -1.0f, 1.0f) * Mathf.Sign(carController.VelocitaCorrente());
        float accel = 1f;
        float brake = 0;

        if(distanceToTarget < 5){
            brake = 0.8f;
            accel = 0.1f;
        }
        
        carController.Move(accel, steer, brake);

        if(distanceToTarget < 4){
            wpDaRaggiungere++;
            if(wpDaRaggiungere >= waypoints.Length)
                wpDaRaggiungere=0;
            
            targetSucc = waypoints[wpDaRaggiungere].transform.position;
        }
        
        carController.CheckSgommata();
        carController.SuonoMotore();
    }
    
    private IEnumerator WaitAndRepositioning(float t)
    {
        while (true)
        {
            targetPrec = targetSucc;
            yield return new WaitForSeconds(t);

            if (targetPrec == targetSucc && wpDaRaggiungere > 0)
            {
                this.transform.position =  waypoints[wpDaRaggiungere-1].transform.position + Vector3.up * 1.2f;
                this.transform.rotation = waypoints[wpDaRaggiungere-1].transform.rotation;
                targetPrec = waypoints[wpDaRaggiungere-1].transform.position;
            }
        }
    }
}
