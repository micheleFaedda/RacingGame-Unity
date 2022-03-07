using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Nel gioco sono pochi i punti dove la macchina potrebbe uscire dal piano, ma ci sono.
 * Nel caso accadesse lo gestiamo con il game over
 */
public class WorldLimit : MonoBehaviour
{
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene(0);
        }
    }
}
