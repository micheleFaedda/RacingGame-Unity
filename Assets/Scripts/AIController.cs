using UnityEngine;
using System.Collections;
public class AIController : MonoBehaviour
{

    public Circuit circuit;
    public float steeringSensitivity = 0.01f;
    CarController ds;
    Vector3 targetSucc;
    Vector3 targetPrec;
  
    int currentWP = 0;
    int precWP = 0;
    private IEnumerator corutine;
    

    float finishSteer;
    
    float lastTimeMoving = 0.0f;
    CheckpointManager cpm;

    void Start()
    {    
        ds = this.GetComponent<CarController>();
        targetSucc = circuit.waypoints[currentWP].transform.position;
        
        
        /*CODICE PER RIPOSIZIONAMENTO MICHI*/
        corutine = WaitAndRepositioning(5f);
        StartCoroutine(corutine);
        
        /**********************************/
     

    }

    void Update()
    {
        
        
        if (cpm == null)
        {

            cpm = ds.rb.GetComponent<CheckpointManager>();
        }
        
        Vector3 localTarget = ds.rb.gameObject.transform.InverseTransformPoint(targetSucc);
        float distanceToTarget = Vector3.Distance(targetSucc, ds.rb.transform.position);

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
            
            targetSucc = circuit.waypoints[currentWP].transform.position;
            
   

        }
        ds.CheckSgommata();
        ds.CalcolaSuonoMotore();
    }
    /*********NUOVA COROUTINE MICHI**************/
    private IEnumerator WaitAndRepositioning(float t)
    {
        while (true)
        {
            targetPrec = targetSucc;
            yield return new WaitForSeconds(t);
           

            Debug.Log("Punto:" + targetSucc );
            Debug.Log("PuntoSucc:" + targetPrec );
          
            if (targetPrec == targetSucc && currentWP > 0)
                
            {
                this.transform.position =  circuit.waypoints[currentWP].transform.position + Vector3.up * 2.0f;
                this.transform.rotation = circuit.waypoints[currentWP].transform.rotation;
                targetPrec = circuit.waypoints[currentWP].transform.position;
             
                
            }
        }
    }

    

}
