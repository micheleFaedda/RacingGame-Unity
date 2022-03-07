using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    
    CarController carController;
    
    public GameObject prefabNomeGiocatore;
    
    //testo che verra visualizzato sopra la macchina del player (numero di giri e la posizione in classifica)
    private GameObject testoPlayer;

    void Start()
    {
        carController = this.GetComponent<CarController>();
        testoPlayer = Instantiate(prefabNomeGiocatore);
        testoPlayer.transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>());
        testoPlayer.GetComponent<Text>().fontSize = 70;
    }

    void Update()
    {

        string text;
        testoPlayer.transform.position = Camera.main.WorldToScreenPoint(this.transform.position + Vector3.up * 1.7f);
        
        //Debug.Log(SceneManager.GetActiveScene().name);
        
        /*Il testo che viene stampato dipenda dalla modalita nella quali si gioca
         In modalità Time viene visualizzato solo il numero di giri, nella modalità racing anche la posizione in classifica
         */
        if (SceneManager.GetActiveScene().name.Equals("Time"))
        {
            text = "Lap: " + this.GetComponent<TimeCheckpointManager>().giro;
        }
        else
        {
            text = this.GetComponent<CheckpointManager>().position + "\nLap: " +
                   this.GetComponent<CheckpointManager>().giro;
        }
        
        testoPlayer.GetComponent<Text>().text = text;

        //Rilevo la pressione dei tasti per andare avanti/indietro per l'accelerazione
        float accelerazione = Input.GetAxis("Vertical");

        //Rilevo la pressione dei tasti per il movimento orizzontale per la sterzata
        float sterzata = Input.GetAxis("Horizontal");

        //Rilevo la pressione della barra spaziatrice per la frenata
        float frenata = Input.GetAxis("Jump");
        
        /*
         * La macchina viene mossa dal CarController in funzione dei tasti premuti dal giocatore
         */
        carController.Move(accelerazione, sterzata, frenata);
        carController.CheckSgommata();
        carController.SuonoMotore();
    }
}
