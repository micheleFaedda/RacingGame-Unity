using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Debug = UnityEngine.Debug;

public class CheckpointManager : MonoBehaviour
{
    private CarController carController;

    //Nome del giocatore
    public string playerName;

    //Il numero di giro che la macchina sta facendo
    public int giro = 0;

    //Indice del checkPoint raggiunto
    public int checkPoint = -1;

    //Indice del ckeckPoint non ancora raggiunto
    public int checkPointSucc;

    //Il tempo di quando è entrata nell'ultimo checkPoint
    public float tempoEntrata = 0.0f;

    //Numero della macchina 
    int idMacchina;

    //Flag se la macchina è stata registrata nella classifica
    private bool macchinaRegistrata;

    //Posizione in classifica
    public string position;

    private GameObject primoClassificaTesto;
    private GameObject secondoClassificaTesto;
    private GameObject terzoClassificaTesto;
    private GameObject quartoClassificaTesto;

    //Array di checkPoints per tenere conto del numero di giri fatti
    private GameObject[] checkPoints;


    /*************timer MICHI*********************************************/
    private GameObject timer;
    private Stopwatch stopWatch;

    private string elapsedTime;
    /********************************************************/


    /*********DISTANZA MICHI*********/
    private GameObject distanceCanvas;
    private float distance = 0.0f;
    private Vector3 vecchiaPosizione;

    /***********************/

    void Start()
    {
        checkPoints = GameObject.FindGameObjectsWithTag("CheckPoint");
        primoClassificaTesto = GameObject.FindGameObjectWithTag("Primo");
        secondoClassificaTesto = GameObject.FindGameObjectWithTag("Secondo");
        terzoClassificaTesto = GameObject.FindGameObjectWithTag("Terzo");
        quartoClassificaTesto = GameObject.FindGameObjectWithTag("Quarto");
        //timer = GameObject.FindGameObjectWithTag("Timer");

        if (!(PlayerPrefs.GetString("modalita").Equals("time")))
        {
            distanceCanvas = GameObject.FindGameObjectWithTag("Distance");
        }


        if (gameObject.CompareTag("Player"))
        {
            timer = GameObject.FindGameObjectWithTag("Timer");
            if (timer != null)
            {
                stopWatch = new Stopwatch(); //stanzio un oggetto stopwatch
            }

            carController = this.GetComponent<CarController>(); //ottengo carController
            vecchiaPosizione =
                this.GetComponent<Rigidbody>().position; //inizializzo la vecchia posizione con quella di partenza 
            distance += Vector3.Distance(vecchiaPosizione, this.GetComponent<Rigidbody>().position) /
                        1000f; //inizializzo la distanza

            if (!(PlayerPrefs.GetString("modalita").Equals("time")))
            {
                distanceCanvas.GetComponent<UnityEngine.UI.Text>().text =
                    String.Format("{0:0.000}", distance) + " KM"; //visualizzo a video
            }
        }
    }

    void FixedUpdate()
    {
        if (carController == null)
        {
            carController = this.GetComponent<CarController>();
        }

        //se la macchina non è stata registrata la registo e prendo il suo id
        if (!macchinaRegistrata)
        {
            idMacchina = Classifica.RegisteraMacchina(playerName);
            macchinaRegistrata = true;
            return;
        }

        Classifica.setPosizione(idMacchina, giro, checkPoint, tempoEntrata);
        position = Classifica.GetPosizione(idMacchina);

        if (PlayerPrefs.GetString("modalita").Equals("racing"))
        {
            setClassifica(position);   
        }


        if (gameObject.CompareTag("Player"))
        {
            if (timer != null)
                CurrentTimer(); //visualizzo il tempo corrente

            if (!(PlayerPrefs.GetString("modalita").Equals("time")))
            {
                CurrentDistance(); //visuallizzo la distanza corrente
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "CheckPoint" && (PlayerPrefs.GetString("modalita").Equals("racing") ||
                                                     PlayerPrefs.GetString("modalita").Equals("multiplayer")))
        {
            int numeroCheckPointCorrente = int.Parse(other.gameObject.name);

            //Se la macchina ha raggiunto il checkpoint successivo allora aggiorniamo gli indici
            if (numeroCheckPointCorrente == checkPointSucc)
            {
                checkPoint = numeroCheckPointCorrente;

                tempoEntrata = Time.time;

                //Se il checkPoint da raggiungere era il primo allora aggiorno il numero del giro
                if (checkPoint == 0)
                {
                    giro++;
                    
                    if (PlayerPrefs.GetString("modalita").Equals("racing"))
                    {
                        if (PlayerPrefs.GetInt("num_giri_race") < giro)
                            SceneManager.LoadScene("SceltaModalita");
                    }

                    if (PlayerPrefs.GetString("modalita").Equals("multiplayer"))
                    {
                        if (PlayerPrefs.GetInt("num_giri_multi") < giro)
                        {
                            if (PhotonNetwork.IsConnected)
                            {
                                if (GetComponent<PlayerController>().view == null) return;
                                if (GetComponent<PlayerController>().view.IsMine)
                                {
                                    PlayerPrefs.SetString("cacca", position);

                                    Debug.Log(PhotonNetwork.PlayerList.Length);
                                    if (PhotonNetwork.PlayerList.Length <= 1)
                                    {
                                        PhotonNetwork.CurrentRoom.IsOpen = false;
                                        PhotonNetwork.CurrentRoom.IsVisible = false;
                                    }
                                    
                                    PhotonNetwork.LeaveRoom();
                                    SceneManager.LoadScene("SceltaModalita");
                                }
                            }
                        }
            
                    }
                    
                    if (gameObject.CompareTag("Player") && timer != null)
                    {
                        /*Se questo è il player allora faccio scattare il timer*/
                        if (!stopWatch.IsRunning)
                            startTimer();
                        else
                            stopWatch.Restart();
                    }
                }

                checkPointSucc++;

                //nel caso la macchina raggiunga l'ultimo checkPoint allora il checkpoint da raggiungere diventa il primo
                if (checkPointSucc >= checkPoints.Length)
                    checkPointSucc = 0;
            }
        }
    }

    private void setClassifica(string position)
    {
        switch (position)
        {
            case "First":
                primoClassificaTesto.GetComponent<UnityEngine.UI.Text>().text = playerName;
                break;
            case "Second":
                secondoClassificaTesto.GetComponent<UnityEngine.UI.Text>().text = playerName;
                break;
            case "Third":
                terzoClassificaTesto.GetComponent<UnityEngine.UI.Text>().text = playerName;
                break;
            case "Fourth":
                quartoClassificaTesto.GetComponent<UnityEngine.UI.Text>().text = playerName;
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


        timer.GetComponent<UnityEngine.UI.Text>().text = elapsedTime + ""; //stampo nella UI
    }

    private void CurrentTimer() //stessa cosa di sopra solo che viene stampato il tempo corrente
    {
        if (stopWatch != null)
        {
            TimeSpan ts = stopWatch.Elapsed;

            elapsedTime = String.Format("{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);


            timer.GetComponent<UnityEngine.UI.Text>().text = elapsedTime + "";
        }
    }

    /*Metodo che si occupa di calcolare la distanza percorsa a partire da quella vecchia*/
    private void CurrentDistance()
    {
        distance += Vector3.Distance(vecchiaPosizione, carController.rb.position) /
                    1000f; //calcolo la distanza percorsa
        distanceCanvas.GetComponent<UnityEngine.UI.Text>().text =
            String.Format("{0:0.000}", distance) + " KM"; //stampo a video
        vecchiaPosizione = carController.rb.position; //aggiorno la vecchia posizione 
    }
    
}