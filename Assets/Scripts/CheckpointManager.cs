using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

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
    int checkPointCount;
    int checkPointSucc;
    public GameObject checkPointPred;

    void Start() {

        GameObject[] checkPoints = GameObject.FindGameObjectsWithTag("CheckPoint");
        checkPointCount = checkPoints.Length;
        foreach (GameObject c in checkPoints) {

            if (c.name == "0") {
                checkPointPred = c;
                break;
            }
        }
    }

    /*Quando entriamo in un trigger controlliamo se è un checkpoint*/
    private void OnTriggerEnter(Collider other) {

        if (other.gameObject.tag == "CheckPoint") {

            int numeroCheckPointCorrente = int.Parse(other.gameObject.name);
            
            if (numeroCheckPointCorrente == checkPointSucc) {
                
                checkPointPred = other.gameObject;
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
