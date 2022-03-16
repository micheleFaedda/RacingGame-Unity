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

    //Oggetti per gestire il timer 
    public GameObject timer;
    private Stopwatch stopWatch;
    private string elapsedTime;

    //Oggetti per gestire la distanza
    private GameObject distanceCanvas;
    private float distance = 0.0f;
    private Vector3 vecchiaPosizione;
    
    //Oggetto per il multiplayer
    public PhotonView view;

    void Start()
    {   
        //recupero gli oggetti per la gestione dei giri e della classifica
        checkPoints = GameObject.FindGameObjectsWithTag("CheckPoint");
        primoClassificaTesto = GameObject.FindGameObjectWithTag("Primo");
        secondoClassificaTesto = GameObject.FindGameObjectWithTag("Secondo");
        terzoClassificaTesto = GameObject.FindGameObjectWithTag("Terzo");
        quartoClassificaTesto = GameObject.FindGameObjectWithTag("Quarto");
        
        //recupero il componente per il multiplayer
        view = GetComponent<PhotonView>();
        
        
        //timer = GameObject.FindGameObjectWithTag("Timer");
        
        //Sezione per settare la visualizzazione di distanza e timer nelle modalità multiplayer e race
        if (!(PlayerPrefs.GetString("modalita").Equals("time")))
        {
            if (PhotonNetwork.IsConnected)
            {
                if (view.IsMine)
                {
                    distanceCanvas = GameObject.FindGameObjectWithTag("Distance");
                }
            }
            else
            {
                distanceCanvas = GameObject.FindGameObjectWithTag("Distance");
            }
        }


        if (gameObject.CompareTag("Player"))
        {
            //If per il multiplayer e per settare il timer e distanza in locale
            if (PhotonNetwork.IsConnected)
            {   
                if(view.IsMine)
                    //inizializzo la distanza per il player in modalità multiplayer
                    InitializeDistance();
                
            }
            else //modalità race
            {   
                //inizializzo la distanza per il player in modalità race
                InitializeDistance();
            }
        }
    }

    void FixedUpdate()
    {   
        //se questo risulta essere null allora sono gli NPC e recupero il car controller
        if (carController == null)
        {
            carController = GetComponent<CarController>();
        }

        //se la macchina non è stata registrata la registo e prendo il suo id
        if (!macchinaRegistrata)
        {
            idMacchina = Classifica.RegisteraMacchina(playerName);
            macchinaRegistrata = true;
            return;
        }
        
        //setting della posizione in classifica
        Classifica.setPosizione(idMacchina, giro, checkPoint, tempoEntrata);
        position = Classifica.GetPosizione(idMacchina);

        if (PlayerPrefs.GetString("modalita").Equals("racing"))
        {
            SetClassifica(position);   
        }

        if (PhotonNetwork.IsConnected)
        {
            if (view.IsMine)
            {
                if (gameObject.CompareTag("Player"))
                {
                    if (timer != null)
                        //visualizzo il tempo corrente
                        CurrentTimer();

                    //visuallizzo la distanza corrente
                    CurrentDistance();
                }
            }
        }
        else
        {
            if (gameObject.CompareTag("Player"))
            {
                if (timer != null)
                    //visualizzo il tempo corrente
                    CurrentTimer(); 

                if (!(PlayerPrefs.GetString("modalita").Equals("time")))
                {   
                    //visuallizzo la distanza corrente
                    CurrentDistance(); 
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("CheckPoint") && (PlayerPrefs.GetString("modalita").Equals("racing") ||
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
                    
                    //se siamo in modalita racing allora controllo se sono finiti i giri e setto la posizione raggiunta a fine gara
                    if (PlayerPrefs.GetString("modalita").Equals("racing"))
                    {
                        if (PlayerPrefs.GetInt("num_giri_race") < giro && CompareTag("Player"))
                        {
                            PlayerPrefs.SetString("posizione_gara", position);
                            int guadagnato = 0;
                            switch (position)
                            {
                                case "First":
                                     guadagnato = Menu.firstCoins + 40 * (PlayerPrefs.GetInt("num_giri_race") - 1);
                                    PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + guadagnato );
                                    
                                    break;
                                case "Second":
                                     guadagnato = Menu.secondCoins + 40 * (PlayerPrefs.GetInt("num_giri_race") - 1);
                                    PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + guadagnato );
                                    break;
                                case "Third":
                                     guadagnato = Menu.thirdCoins + 40 * (PlayerPrefs.GetInt("num_giri_race") - 1);
                                    PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + guadagnato );
                                    break;
                                case "Fourth":
                                     guadagnato = Menu.fourthCoins + 40 * (PlayerPrefs.GetInt("num_giri_race") - 1);
                                    PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + guadagnato );
                                    break;
                                    
                            }
                            PlayerPrefs.SetInt("CoinsEarn", guadagnato);
                            PlayerPrefs.SetString("otherResult","true");
                            PlayerPrefs.SetString("timeResult","false");
                            SceneManager.LoadScene("SceltaModalita");
                        }
                    }
                    
                    /*se siamo in modalita multiplayer allora controllo se sono finiti i giri e 
                      */
                    if (PlayerPrefs.GetString("modalita").Equals("multiplayer"))
                    {
                        if (PlayerPrefs.GetInt("num_giri_multi") < giro)
                        {  
                            CloseRoomAndRedirect(position);
                        }
            
                    }
                    
                    //se siamo collegati alla lobby e il player è quello locale allora gestisco il timer
                    if (PhotonNetwork.IsConnected)
                    {  
                        if(view.IsMine)
                            ManageTimer();
                    }
                    else //altrimenti siamo in un'altra modalità con il tag player
                    {
                        ManageTimer();
                    }
                }

                checkPointSucc++;

                //nel caso la macchina raggiunga l'ultimo checkPoint allora il checkpoint da raggiungere diventa il primo
                if (checkPointSucc >= checkPoints.Length)
                    checkPointSucc = 0;
            }
        }
    }
    
    /*Metodo che si occupa di settare la classifica*/
    private void SetClassifica(string position)
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
    
    
    /*Metodo che si occupa di far scattare o resettare il timer in base al tag player*/
    private void ManageTimer()
    {   
        /*Se questo è il player allora faccio scattare il timer*/
        if (gameObject.CompareTag("Player") && timer != null)
        {
            //se non è partito lo faccio partire altrimenti lo resetto
            if (!stopWatch.IsRunning)
            {   
                //lo faccio partire 
                stopWatch.Start(); 
                
                //prendo il suo tempo corrente e lo assegno ad un TimeSpan
                TimeSpan ts = stopWatch.Elapsed; 
                
                //formatto il tempo corrente
                elapsedTime = String.Format("{1:00}:{2:00}:{3:00}",
                    ts.Hours, ts.Minutes, ts.Seconds,
                    ts.Milliseconds / 10); 
                
                //stampo nella UI
                timer.GetComponent<UnityEngine.UI.Text>().text = elapsedTime + ""; 
            }
            else
            {
                stopWatch.Restart();
            }
        }
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
    
    /*Metodo che si occupa di inizializzare la distanza a video*/
    private void InitializeDistance()
    {
        //otttengo l'oggetto timer
        timer = GameObject.FindGameObjectWithTag("Timer");
            
        if (timer != null)
        {
            stopWatch = new Stopwatch(); //stanzio un oggetto stopwatch per il timer
        }
                    
        //Ottengo il carController del Player
        carController = GetComponent<CarController>(); 
                    
        //inizializzo la vecchia posizione con quella di partenza 
        vecchiaPosizione = GetComponent<Rigidbody>().position; 
                   
        //inizializzo la distanza in KM
        distance += Vector3.Distance(vecchiaPosizione, this.GetComponent<Rigidbody>().position) / 1000f;

        if (!(PlayerPrefs.GetString("modalita").Equals("time")))
        {   if(distanceCanvas != null)
                distanceCanvas.GetComponent<UnityEngine.UI.Text>().text =
                 String.Format("{0:0.000}", distance) + " KM"; //visualizzo a video
        }
    }

    
    
    /*Metodo che si occupa di calcolare la distanza percorsa a partire da quella vecchia*/
    private void CurrentDistance()
    {   
        //calcolo la distanza percorsa
        distance += Vector3.Distance(vecchiaPosizione, carController.rb.position) / 1000f; 
        
        
        if(distanceCanvas != null)
            //stampo a video
            distanceCanvas.GetComponent<UnityEngine.UI.Text>().text =
                String.Format("{0:0.000}", distance) + " KM"; 
        
        //aggiorno la vecchia posizione
        vecchiaPosizione = carController.rb.position;  
    }
    
    /*Metodo che si occupa di chiudere la stanza nel caso ci sia solo un player in uscita e di settare i rusultati ottenuti
     in gara*/
    private void CloseRoomAndRedirect(string position)
    {
        if (PhotonNetwork.IsConnected)
        {
            if (GetComponent<PlayerController>().view == null) return;
            
            //se è il player locale
            if (GetComponent<PlayerController>().view.IsMine)
            {

                switch (position)
                {
                    case "First":
                        PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + 1500 );
                        PlayerPrefs.SetInt("CoinsEarn", 1500 );
                        break;
                    case "Second":
                        PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + 1000 );
                        PlayerPrefs.SetInt("CoinsEarn", 1000 );
                        break;
                    case "Third":
                        PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + 500 );
                        PlayerPrefs.SetInt("CoinsEarn", 500 );
                        break;
                                    
                }
                
                //se vi è solo un player chiudo la room
                if (PhotonNetwork.CurrentRoom.PlayerCount <= 1)
                {
                    PhotonNetwork.CurrentRoom.IsOpen = false;
                    PhotonNetwork.CurrentRoom.IsVisible = false;
                }
                
                //setto parametri per la canvas risultati
                PlayerPrefs.SetString("posizione_gara", position);
                PlayerPrefs.SetString("otherResult","true");
                PlayerPrefs.SetString("timeResult","false");
                
                //esco dalla stanza
                PhotonNetwork.LeaveRoom();
                
                //disconnetto il player
                PhotonNetwork.Disconnect();
                
                //carico la scena della modalità 
                SceneManager.LoadScene("SceltaModalita");
            }
        }
    }
    
}