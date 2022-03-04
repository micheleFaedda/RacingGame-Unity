using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] countDownElements;
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
        }
    }

}