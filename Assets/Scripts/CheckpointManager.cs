using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;




/*
lap -> giro 
checkPoint ->
checkPointCount ->
nextCheckPoint -> checkPointSucc
cps -> checkPoints
lastCP -> checkPointPred
thisCPNumber -> numeroCheckPointCorrente
*/

public class CheckpointManager : MonoBehaviour {

    public int giro = 0;
    public int checkPoint = -1;
    public float timeEntered = 0.0f;
    private float timeGosted = 0.0f;
    int checkPointCount;
    int checkPointSucc;
    public GameObject checkPointSucc_go;
    int carRego;
    private bool regoSet;
    public string playerName;
    public string position;
    private bool gostato = false;

    private CarController ds;

    void Start() {

        GameObject[] checkPoints = GameObject.FindGameObjectsWithTag("CheckPoint");
        checkPointCount = checkPoints.Length;
        foreach (GameObject c in checkPoints) {

            if (c.name == "0") {
                checkPointSucc_go = c;
                break;
            }
        }
    }

    void Update() {

        if (ds == null)
        {
            ds = this.GetComponent<CarController>();
        }

        if (!regoSet) {
            carRego = Leaderboard.RegisterCar(playerName);
            regoSet = true;
            return;
        }
       Debug.Log(timeGosted + ".." + 5 +Time.time);
        if (checkPointSucc > 0)
        {
            if (Time.time > 20 + timeEntered && timeGosted  + 20 < Time.time )
            {
                timeGosted = Time.time;
                
                this.transform.position =  checkPointSucc_go.transform.position + Vector3.up * 2.0f;
                this.transform.rotation = checkPointSucc_go.transform.rotation;
                //circuit.waypoints[currentTrackerWP].transform.position + Vector3.up * 2 +
                //new Vector3(Random.Range(-1.0f, 1.0f), 0.0f, Random.Range(-1.0f, 1.0f));
                //tracker.transform.position = cpm.lastCP.transform.position;
                //ds.rb.gameObject.layer = 8;
                //this.GetComponent<Ghost>().enabled = true;
                //Invoke("ResetLayer", 3);
            }
        }
        
        Leaderboard.SetPosition(carRego, giro, checkPoint, timeEntered);
        position = Leaderboard.GetPosition(carRego);

    }
    
    /*Quando entriamo in un trigger controlliamo se è un checkpoint*/
    private void OnTriggerEnter(Collider other) {

        if (other.gameObject.tag == "CheckPoint") {

            int numeroCheckPointCorrente = int.Parse(other.gameObject.name);
            
            if (numeroCheckPointCorrente == checkPointSucc) {
                
                checkPointSucc_go = other.gameObject;
                checkPoint = numeroCheckPointCorrente;
                timeEntered = Time.time;

                if (checkPoint == 0) 
                    giro++;
                                
                checkPointSucc++;
                
                if (checkPointSucc >= checkPointCount)
                    checkPointSucc = 0;
            }
        }
    }
}
