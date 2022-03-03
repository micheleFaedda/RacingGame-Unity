using UnityEngine;
using UnityEngine.SceneManagement;

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
