using UnityEngine;

/*
 * Nel gioco non ci dovrebbero essere punti dove la macchina potrebbe uscire dal piano.
 * Abbiamo comunque preferito di gestire questo caso.
 */
public class WorldLimit : MonoBehaviour
{
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameObject[] checkPoints = GameObject.FindGameObjectsWithTag("CheckPoint");
            other.gameObject.transform.position = checkPoints[0].transform.position;
            other.gameObject.transform.rotation = checkPoints[0].transform.rotation;
        }
    }
}
