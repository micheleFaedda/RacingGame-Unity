using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Nel gioco non ci dovrebbero essere punti dove la macchina potrebbe uscire dal piano.
 * Abbiamo preferito comunque di gestire questo (gameOver)
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
