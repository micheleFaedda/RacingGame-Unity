using System.Collections;
using UnityEngine;

public class NewNCPController : MonoBehaviour
{
    
   private CarController carController;
   CheckpointManager cpm;
    
   public float brakingSensitivity = 1.1f;
   public float steeringSensitivity = 0.01f;
   public float accelSensitivity = 0.3f;
   
   float lastTimeMoving = 0.0f;

    //Posizione del wayPoint che la macchina non ha ancora raggiunto
    public Vector3 targetSucc;

    //Indice del wayPoint che la macchina non ha ancora raggiunto
    public int wpDaRaggiungere = 0;

    //Pardre dei waypoint nella scena
    public GameObject circuito;
    
    //Array di waypoint preso dal GameObject circuito
    private Transform[] waypoints;
    
    //Per risposizionare gli NPC
    private IEnumerator corutine;

    private GameObject tracker;
    public float lookAhead = 10;
    private int currentTrackerWP = 0;
    
    void Start()
    {    
        //Andiamo a popolare l'array di waypoints
        waypoints = new Transform[circuito.transform.childCount];
        for (int i = 0; i < circuito.transform.childCount; i++)
        {
            waypoints[i] = circuito.transform.GetChild(i);
        }
        
        carController = this.GetComponent<CarController>();
        
        //Inizialmente il targetSucc è il primo nell'array
        targetSucc = waypoints[wpDaRaggiungere].transform.position;

        tracker = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        DestroyImmediate(tracker.GetComponent<Collider>());
        //tracker.GetComponent<MeshRenderer>().enabled = false;
        tracker.transform.position = this.transform.position;
        tracker.transform.rotation = this.transform.rotation;
        //this.GetComponent<Ghost>().enabled = false;
        //finishSteer = Random.Range(-1.0f, 1.0f);
    }
    
    void ProgressTracker()
    {
        Debug.DrawLine(carController.rb.gameObject.transform.position, tracker.transform.position);

        if (Vector3.Distance(carController.rb.gameObject.transform.position, tracker.transform.position) > lookAhead)
        {
            return;
        }

        tracker.transform.LookAt(waypoints[currentTrackerWP].transform.position);
        tracker.transform.Translate(0.0f, 0.0f, 1.0f);  // Speed of the tracker;

        if (Vector3.Distance(tracker.transform.position, waypoints[currentTrackerWP].transform.position) < 1.0f)
        {

            currentTrackerWP++;
            if (currentTrackerWP >= waypoints.Length)
            {
                currentTrackerWP = 0;
            }
        }
    }

    void Update()
    {
        ProgressTracker();
        
        if (!GameManager.start)
        {
            lastTimeMoving = Time.time;
            return;
        }

        //Debug.Log("Start");
        
        if (cpm == null)
        {
            cpm = carController.rb.GetComponent<CheckpointManager>();
        }
        
        if (carController.VelocitaCorrente() > 1)
        {
            lastTimeMoving = Time.time;
        }
        
        if (Time.time > lastTimeMoving + 4 || carController.rb.gameObject.transform.position.y < -5.0f)
        {

            carController.rb.gameObject.transform.position = waypoints[currentTrackerWP-2].transform.position;
            tracker.transform.position = carController.rb.gameObject.transform.position;
            //carController.rb.gameObject.transform.position = cpm.lastCP.transform.position + Vector3.up * 2.0f;
            //carController.rb.gameObject.transform.rotation = cpm.lastCP.transform.rotation;
            //circuit.waypoints[currentTrackerWP].transform.position + Vector3.up * 2 +
            //new Vector3(Random.Range(-1.0f, 1.0f), 0.0f, Random.Range(-1.0f, 1.0f));
            //tracker.transform.position = cpm.lastCP.transform.position;
            //carController.rb.gameObject.layer = 8;
            //this.GetComponent<Ghost>().enabled = true;
            //Invoke("ResetLayer", 3);
        }
        
        
        Vector3 localTarget = carController.rb.gameObject.transform.InverseTransformPoint(tracker.transform.position);
        float targetAngle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;
        float sterzata = Mathf.Clamp(targetAngle * steeringSensitivity, -1.0f, 1.0f) * Mathf.Sign(carController.VelocitaCorrente());
        
        float speedFactor = carController.VelocitaCorrente() / carController.velocitaMassima;
        float corner = Mathf.Clamp(Mathf.Abs(targetAngle), 0.0f, 90.0f);
        float cornerFactor = corner / 90.0f;
        
        float frenata = 0;
        
        if (corner > 10 && speedFactor > 0.1f)
        {
            frenata = Mathf.Lerp(0.0f, 1.0f + speedFactor * brakingSensitivity, cornerFactor);
        }
        
        float accelerazione = 1f;
        if (corner > 20.0f && speedFactor > 0.4f)
        { 
            accelerazione = Mathf.Lerp(0.0f, 1.0f * accelSensitivity, 1 - cornerFactor);
        }

        carController.Move(accelerazione, sterzata, frenata);
        carController.CheckSgommata();
        carController.SuonoMotore();
    }
}
