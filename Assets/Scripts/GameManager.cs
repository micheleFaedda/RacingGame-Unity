using System.Collections;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject[] countDownElements;
    public GameObject[] macchine;
    
    public GameObject[] wayPoints;
    private Vector3[] posizioni_npc;
    public GameObject timePoints;
    public GameObject distanza;
    public GameObject coins;

    //Serve per non far partire le macchine prima della fine del count down
    public static bool start = false;

    void Awake()
    {

        GameObject m;
        switch (PlayerPrefs.GetString("modalita"))
        {
            case "time":
                GameObject.FindGameObjectWithTag("Classifica").SetActive(false);
                distanza.SetActive(false);
                coins.SetActive(true);
                timePoints.SetActive(true);
                foreach (GameObject wp in wayPoints)
                {
                    wp.SetActive(false);
                }
                timePoints.SetActive(true);
                m = Instantiate (macchine[PlayerPrefs.GetInt("macchina_giocatore")], new Vector3 (1.528828f, 0, 240f), Quaternion.identity * Quaternion.Euler(0, -90, 0)) as GameObject;
                m.tag = "Player";
                m.AddComponent<TimeCheckpointManager>();
                break;
            case "racing":
                posizioni_npc = new Vector3[3]{new Vector3(-2.92f,0,232), new Vector3(7.1f,0,241), new Vector3(13.7f,0,231.5f)};
                
                timePoints.SetActive(false);
                coins.SetActive(false);
                m = Instantiate (macchine[PlayerPrefs.GetInt("macchina_giocatore")], new Vector3 (22.7f, 0, 241f), Quaternion.identity * Quaternion.Euler(0, -90, 0)) as GameObject;
                m.tag = "Player";
                m.GetComponent<CheckpointManager>().enabled = true;
                m.GetComponent<CheckpointManager>().playerName = PlayerPrefs.GetString("player_name");

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
                GameObject.FindGameObjectWithTag("Classifica").SetActive(false);
                coins.SetActive(false);
                foreach (GameObject wp in wayPoints)
                {
                    wp.SetActive(false);
                }
                timePoints.SetActive(false);

                int pos_multiplayer;

                if (PhotonNetwork.PlayerList.Length == 1)
                {
                    pos_multiplayer = 0;
                }
                else
                {
                    pos_multiplayer = 1;
                }


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

        StartCoroutine(CountDown());
    }

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
}