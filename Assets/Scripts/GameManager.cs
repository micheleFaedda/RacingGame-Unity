using System.Collections;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    //I numeri del count Down
    public GameObject[] countDownElements;
    
    //Le macchine che possono essere istanziate
    public GameObject[] macchine;
    
    //Waypoints per gli npc
    public GameObject[] wayPoints;
    
    //Timepoints per la modalità time
    public GameObject timePoints;
    
    //Testo della distanza sullo schermo
    public GameObject distanza;
    
    //Testo dei coins sullo schermo
    public GameObject coins;
    
    //Posizioni iniziali delle macchine
    private Vector3[] posizioni_npc = new Vector3[3]{new Vector3(-2.92f,0,232), new Vector3(7.1f,0,241), new Vector3(13.7f,0,231.5f)};

    //Testo di attesa dei giocatori quando si è in multiplayer
    public GameObject attesa;
    
    //Materiali per le skybox
    public Material giorno;
    public Material notte;

    public GameObject timerRace;
    public GameObject timerTime;
    
    //Serve per non far partire le macchine prima della fine del count down
    public static bool start = false;
    
    //Serve per far partire il countDown allo stesso tempo per la macchine in multiplayer
    public static bool flag_started_coundown = false;
    
    void Awake()
    {   
       
        
        attesa.SetActive(false);
       
        GameObject m;
        
        //A seconda della modalità abilitiamo solo le cose che ci servono
        switch (PlayerPrefs.GetString("modalita"))
        {
            case "time":
                GiornoNotte(Random.Range(0, 10));
                timerTime.SetActive(true);
                timerRace.SetActive(false);
                GameObject.FindGameObjectWithTag("Classifica").SetActive(false);
                distanza.SetActive(false);
                coins.SetActive(true);
                timePoints.SetActive(true);
                foreach (GameObject wp in wayPoints)
                {
                    wp.SetActive(false);
                }
                timePoints.SetActive(true);
                
                //Viene istanziata la macchina del giocatore e viene settata la sua forza
                m = Instantiate (macchine[PlayerPrefs.GetInt("macchina_giocatore")], new Vector3 (1.528828f, 0, 240f), Quaternion.identity * Quaternion.Euler(0, -90, 0)) as GameObject;
                m.tag = "Player";
                m.AddComponent<TimeCheckpointManager>();
                m.GetComponent<CarController>().forza = PlayerPrefs.GetInt("forza");
                break;
            case "racing":
                GiornoNotte(Random.Range(0, 10));
                timerTime.SetActive(false);
                timerRace.SetActive(true);
                timePoints.SetActive(false);
                coins.SetActive(false);
                
                //Viene istanziata la macchina del giocatore e viene settata la sua forza
                m = Instantiate (macchine[PlayerPrefs.GetInt("macchina_giocatore")], new Vector3 (22.7f, 0, 241f), Quaternion.identity * Quaternion.Euler(0, -90, 0)) as GameObject;
                m.tag = "Player";
                m.GetComponent<CheckpointManager>().enabled = true;
                m.GetComponent<CheckpointManager>().playerName = PlayerPrefs.GetString("player_name");
                m.GetComponent<CarController>().forza = PlayerPrefs.GetInt("forza");

                //Istanziamo le macchine NPC in posizioni prestabilite
                int index_npc = 0;
                for (int i = 0; i < macchine.Length; i++)
                {
                    if (i != PlayerPrefs.GetInt("macchina_giocatore"))
                    {
                        GameObject npc = Instantiate (macchine[i], posizioni_npc[index_npc], Quaternion.identity * Quaternion.Euler(0, -90, 0)) as GameObject;
                        npc.GetComponent<CheckpointManager>().enabled = true;
                        npc.GetComponent<NPController>().circuito = wayPoints[index_npc];
                        index_npc++;
                    }
                }
                
                break;
            case "multiplayer":
                
                if (PhotonNetwork.IsConnected)
                {
                    switch (PhotonNetwork.CurrentRoom.MaxPlayers)
                    {
                        case 2:
                            GameObject.FindGameObjectWithTag("TerzoIntestazione").SetActive(false);
                            GameObject.FindGameObjectWithTag("QuartoIntestazione").SetActive(false);
                            break;
                        case 3: 
                            GameObject.FindGameObjectWithTag("QuartoIntestazione").SetActive(false);
                            break;
                    }
                }
                //a seconda della lunghezza del nome della stanza setto la modalita giorno o notte
                GiornoNotte(PhotonNetwork.CurrentRoom.Name.Length); 
                
                timerTime.SetActive(false);
                timerRace.SetActive(true);
                //timerRace.transform.position = new Vector3(timerRace.transform.position.x, timerRace.transform.position.y + 230f, 0);
                //distanza.transform.position = new Vector3(distanza.transform.position.x, distanza.transform.position.y + 230f, 0);
           
                GameObject.FindGameObjectWithTag("Classifica").SetActive(true);
                coins.SetActive(false);
                foreach (GameObject wp in wayPoints)
                {
                    wp.SetActive(false);
                }
                timePoints.SetActive(false);

                int pos_multiplayer = PhotonNetwork.CurrentRoom.PlayerCount-1;

                //Viene istanziata la macchina del giocatore (la posizione dipenda da quanti giocatori sono gia presenti nella stanza) e viene settata la sua forza
                m = PhotonNetwork.Instantiate(macchine[PlayerPrefs.GetInt("macchina_giocatore")].name, posizioni_npc[pos_multiplayer], Quaternion.identity * Quaternion.Euler(0, -90, 0));
                m.tag = "Player";
                m.GetComponent<CheckpointManager>().playerName = PlayerPrefs.GetString("player_name");
                m.GetComponent<CarController>().forza = PlayerPrefs.GetInt("forza");
                break;
        }
        
        
        foreach (GameObject item in countDownElements)
        {
            item.SetActive(false);
        }

        if (!PhotonNetwork.IsConnected)
        {
            StartCoroutine(CountDown());
        }

    }

    /*
     * Gestiamo il countDown e la partenza sincronizzata dei player in modalità multiplayer
     */
    public void Update()
    {
        if (PhotonNetwork.IsConnected && !flag_started_coundown && PhotonNetwork.PlayerList.Length == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            flag_started_coundown = true;
            attesa.SetActive(false);
            StartCoroutine(CountDown());
        }
        else if(PhotonNetwork.IsConnected && !flag_started_coundown)
        {
            attesa.SetActive(true);
        }
    }

    /*
     * Coroutine per visualizzare i numeri del countdown a scermo all'inizio del gioco
     */
    IEnumerator CountDown()
    {
        yield return new WaitForSeconds(2);
        foreach (GameObject item in countDownElements)
        {
            item.SetActive(true);
            yield return new WaitForSeconds(1);
            item.SetActive(false);
            if (item == countDownElements.Last())
                start = true;
        }
    }

    private void GiornoNotte(int x)
    {
        if (x % 2 == 0)
            RenderSettings.skybox = giorno;
        else 
            RenderSettings.skybox = notte;
        
    }
}