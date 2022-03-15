using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyMusicMenu : MonoBehaviour
{   
    
    private void Awake()
    {   /*faccio si che non venga distrutta tra una scena e l'altra*/
        DontDestroyOnLoad(transform.gameObject);
    }

    private void Update()
    {   
        /*se sono in game la fermo*/
        if (SceneManager.GetActiveScene().name.Equals("Game"))
        {
            this.gameObject.GetComponent<AudioSource>().Stop();
        }   /*se è stata fermata allora la metto in play*/
        else if(!this.gameObject.GetComponent<AudioSource>().isPlaying)
        {   
            this.gameObject.GetComponent<AudioSource>().Play(); 
        }
        
        /*Questo è fatto perchè tornando alla scena del menu inizale viene istanziato
         un altro oggetto music e quindi rimuovo il nuovo oggetto tranne quello iniziale che si 
         trova nella cella 0*/
        GameObject[] music =  GameObject.FindGameObjectsWithTag("Music");

//        Debug.Log("Musiche " +music.Length);

        for (int i = music.Length - 1; i > 0; i--)
        {
            Destroy(music[i]);
        }

    }
}
