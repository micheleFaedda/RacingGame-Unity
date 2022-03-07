using System;
using System.Diagnostics;
using UnityEngine;

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
    public GameObject checkPointSucc_go;
    int carRego;
    private bool regoSet;
    public string playerName;
    public string position;
    
    /*************CLASSIFICA MICHI*********************************************/
    private GameObject primoClassifica;
    private GameObject secondoClassifica;
    private GameObject terzoClassifica;
    private GameObject quartoClassifica;
    /************************************************************************/
   
    /*************timer MICHI*********************************************/
    private GameObject timer;
    private Stopwatch stopWatch;
    private string elapsedTime;
 /********************************************************/
    
    private CarController carController;

    void Start() {

        GameObject[] checkPoints = GameObject.FindGameObjectsWithTag("CheckPoint");
        
        /*************CLASSIFICA MICHI*********************************************/
         primoClassifica = GameObject.FindGameObjectWithTag("Primo");
         secondoClassifica = GameObject.FindGameObjectWithTag("Secondo");
         terzoClassifica = GameObject.FindGameObjectWithTag("Terzo");
         quartoClassifica = GameObject.FindGameObjectWithTag("Quarto");
         /************************************************************************/
         
         
         /*************CODICE TIMER MICHI*********************************************/
         
           timer = GameObject.FindGameObjectWithTag("Timer");
           if (gameObject.CompareTag("Player"))
           {
               stopWatch = new Stopwatch(); //stanzio un oggetto stopwatch
           }

           /********************************************************/
        checkPointCount = checkPoints.Length;
        foreach (GameObject c in checkPoints) {

            if (c.name == "0") {
                checkPointSucc_go = c;
                break;
            }
        }
    }

    void Update() {

        if (carController == null)
        {
            carController = this.GetComponent<CarController>();
        }

        if (!regoSet) {
            carRego = Leaderboard.RegisterCar(playerName);
            regoSet = true;
            return;
        }
        
        /*if (checkPointSucc > 0)
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
        }*/
        
        Leaderboard.SetPosition(carRego, giro, checkPoint, timeEntered);
        position = Leaderboard.GetPosition(carRego);
        
        setClassifica(position);
        currentTimer();
       

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
                {
                    giro++;
                    
                    if (gameObject.CompareTag("Player"))
                    {    //Se questo è il player allora faccio scattare il timer
                        if (!stopWatch.IsRunning)
                            startTimer();
                        else 
                            stopWatch.Restart();
                        

                    }
                    
                   
                }

                checkPointSucc++;
                
                if (checkPointSucc >= checkPointCount)
                    checkPointSucc = 0;
            }
        }
    }

    private void setClassifica(string position)
    {
        switch (position)
        {
            case "First":
                primoClassifica.GetComponent<UnityEngine.UI.Text>().text = playerName;
                break;
            case "Second":
                secondoClassifica.GetComponent<UnityEngine.UI.Text>().text = playerName;
                break;
            case "Third":
                terzoClassifica.GetComponent<UnityEngine.UI.Text>().text = playerName;
                break;
            case "Fourth":
                quartoClassifica.GetComponent<UnityEngine.UI.Text>().text = playerName;
                break;
        }
        
    }
 
    private void startTimer()
    {   
        
        stopWatch.Start(); //lo faccio partire 
        TimeSpan ts = stopWatch.Elapsed; //prendo il suo tempo corrente e lo assegno ad un TimeSpan
        
        elapsedTime = String.Format("{1:00}:{2:00}:{3:00}",  
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10); //formatto il tempo corrente
              
               
        timer.GetComponent<UnityEngine.UI.Text>().text = elapsedTime +""; //stampo nella UI
    }

    private void currentTimer() //stessa cosa di sopra solo che viene stampato il tempo corrente
    {
        if (stopWatch != null)
        {
            TimeSpan ts = stopWatch.Elapsed;

            elapsedTime = String.Format("{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);


            timer.GetComponent<UnityEngine.UI.Text>().text = elapsedTime;
        }
    }
}
    