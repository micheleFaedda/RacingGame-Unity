using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] countDownElements;
    public GameObject[] macchine;
    public GameObject[] wayPoints;
    //private Transform[] posizioni_npc;
    public GameObject timePoints;

    //Serve per non far partire le macchine prima della fine del count down
    public static bool start = false;

    void Awake()
    {
        GameObject m;
        switch (PlayerPrefs.GetString("modalita"))
        {
            case "time":
                GameObject.FindGameObjectWithTag("Classifica").SetActive(false);
                foreach (GameObject wp in wayPoints)
                {
                    wp.SetActive(false);
                }
                timePoints.SetActive(true);
                m = Instantiate (macchine[PlayerPrefs.GetInt("macchina_giocatore")], new Vector3 (1.528828f, 0, 240f), Quaternion.identity) as GameObject;
                m.tag = "Player";
                m.AddComponent<TimeCheckpointManager>();
                break;
            case "racing":
                timePoints.SetActive(false);
                m = Instantiate (macchine[PlayerPrefs.GetInt("macchina_giocatore")], new Vector3 (1.528828f, 0, 240f), Quaternion.identity) as GameObject;
                m.tag = "Player";
                m.GetComponent<CheckpointManager>().enabled = true;
                m.GetComponent<CheckpointManager>().playerName = PlayerPrefs.GetString("player_name");

                int index_wp = 0;
                for (int i = 0; i < macchine.Length; i++)
                {
                    if (i != PlayerPrefs.GetInt("macchina_giocatore"))
                    {
                        GameObject npc = Instantiate (macchine[i], new Vector3 (40f + i * 10, 0, 240f), Quaternion.identity) as GameObject;
                        npc.GetComponent<CheckpointManager>().enabled = true;
                        npc.GetComponent<NPController>().circuito = wayPoints[index_wp];
                        index_wp++;
                    }
                }
                
                break;
            case "multiplayer":
                foreach (GameObject wp in wayPoints)
                {
                    wp.SetActive(false);
                }
                timePoints.SetActive(false);
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