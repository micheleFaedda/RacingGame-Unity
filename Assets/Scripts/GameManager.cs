using System.Collections;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] countDownElements;
    
    //Serve per non far partire le macchine prima della fine del count down
    public static bool start = false;
    
    void Start()
    {
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
            if(item == countDownElements.Last()) start = true;
        }
    }

}