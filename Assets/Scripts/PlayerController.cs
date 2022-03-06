using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/**COSE CHE STIAMO RINOMINANDO (DA TOGLIERE):
 * b -> frenata
 * a -> accellerazione
 * s -> sterzata
 * ds -> carController
 * Drive -> CarController
 */
public class PlayerController : MonoBehaviour {
    
    /*Mi occorre gestire il funzionamento delle ruote e quindi recupero il CarController*/
    CarController carController;
    public GameObject prefabNomeGiocatore;
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

        if (!GameManager.start) return;

        string text;
        testoPlayer.transform.position = Camera.main.WorldToScreenPoint(this.transform.position + Vector3.up * 1.7f);
        
        //Debug.Log(SceneManager.GetActiveScene().name);
        //Il testo che viene stampato dipenda dalla modalita nella quali si gioca
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

        /*Rilevo la pressione dei tasti per andare avanti/indietro per l'accelerazione*/
        float accelerazione = Input.GetAxis("Vertical");

        /*Rilevo la pressione dei tasti per il movimento orizzontale per la sterzata*/
        float sterzata = Input.GetAxis("Horizontal");

        /*Rilevo la pressione della barra spaziatrice per la frenata*/
        float frenata = Input.GetAxis("Jump");

        //Parte con il tachimetro viecchio
        //GameObject tac = GameObject.FindGameObjectWithTag("Tachimetro");  
        //tac.GetComponent<Tachimetro>().ShowSpeed(carController.velocitaCorrente, 0f, carController.velocitaMassima);
        //tac.GetComponent<Tachimetro>().MostraMarcia();
        
       
        carController.Move(accelerazione, sterzata, frenata);
        carController.CheckSgommata();
        carController.CalcolaSuonoMotore();
    }
}
