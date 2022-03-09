using System;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
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

public class CheckpointManager : MonoBehaviour
{

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

    private GameObject primoClassifica;
    private GameObject secondoClassifica;
    private GameObject terzoClassifica;
    private GameObject quartoClassifica;

    private CarController carController;

    void Start()
    {

        GameObject[] checkPoints = GameObject.FindGameObjectsWithTag("CheckPoint");

        primoClassifica = GameObject.FindGameObjectWithTag("Primo");
        secondoClassifica = GameObject.FindGameObjectWithTag("Secondo");
        terzoClassifica = GameObject.FindGameObjectWithTag("Terzo");
        quartoClassifica = GameObject.FindGameObjectWithTag("Quarto");

        checkPointCount = checkPoints.Length;
        foreach (GameObject c in checkPoints)
        {

            if (c.name == "0")
            {
                checkPointSucc_go = c;
                break;
            }
        }
    }

    void Update()
    {

        if (carController == null)
        {
            carController = this.GetComponent<CarController>();
        }

        if (!regoSet)
        {
            carRego = Leaderboard.RegisterCar(playerName);
            regoSet = true;
            return;
        }

        Leaderboard.SetPosition(carRego, giro, checkPoint, timeEntered);
        position = Leaderboard.GetPosition(carRego);

        setClassifica(position);

    }

    /*Quando entriamo in un trigger controlliamo se è un checkpoint*/
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "CheckPoint")
        {

            int numeroCheckPointCorrente = int.Parse(other.gameObject.name);

            if (numeroCheckPointCorrente == checkPointSucc)
            {

                checkPointSucc_go = other.gameObject;
                checkPoint = numeroCheckPointCorrente;
                timeEntered = Time.time;

                if (checkPoint == 0)
                {
                    giro++;
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
}