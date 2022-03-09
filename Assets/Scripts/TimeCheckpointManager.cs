using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeCheckpointManager : MonoBehaviour
{
    private CarController carController;

    //Il numero di giro che la macchina sta facendo
    public int giro = 0;

    //Indice del checkPoint raggiunto
    public int checkPoint = -1;

    //Indice del ckeckPoint non ancora raggiunto
    public int checkPointSucc;

    //Numero totale di coins raccolti
    private int numCoins = 0;

    //Testo del numero di coins raccolti
    private GameObject testoCoins;

    //Il timer visualizzato sullo schermo
    private GameObject testoTimer;

    //Tempo rimasto al giocatore
    private float tempoRimasto = 40;

    //Array di collezionabili
    private GameObject[] timePoints;

    //Array di checkPoints per tenere conto del numero di giri fatti
    private GameObject[] checkPoints;

    //Colore del timer quando è sopra una certa soglia
    private Color normalTimerColor = new Color32(253, 158, 0, 255);

    void Start()
    {
        checkPoints = GameObject.FindGameObjectsWithTag("CheckPoint");
        timePoints = GameObject.FindGameObjectsWithTag("TimePoint");

        testoTimer = GameObject.FindGameObjectWithTag("Timer");
        testoCoins = GameObject.FindGameObjectWithTag("Coins");
    }

    void Update()
    {
        if (carController == null)
        {
            carController = this.GetComponent<CarController>();
        }

        //Il timer parte solo se il gioco è iniziato
        if (GameManager.start)
        {
            //Aggiorniamo il tempo
            tempoRimasto -= 1 * Time.deltaTime;
            testoTimer.GetComponent<UnityEngine.UI.Text>().text = tempoRimasto.ToString("0");

            //Se rimandono meno di 5 secondi allora cambio colore
            if (tempoRimasto < 5)
            {
                testoTimer.GetComponent<UnityEngine.UI.Text>().color = Color.red;
            }
            else
            {
                testoTimer.GetComponent<UnityEngine.UI.Text>().color = normalTimerColor;
            }

            //Se il tempo è finito si esce dal gioco
            if (tempoRimasto < 0)
            {
                SceneManager.LoadScene(0);
            }
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "CheckPoint")
        {
            int numeroCheckPointCorrente = int.Parse(other.gameObject.name);

            //Se la macchina ha raggiunto il checkpoint successivo allora aggiorniamo gli indici
            if (numeroCheckPointCorrente == checkPointSucc)
            {
                checkPoint = numeroCheckPointCorrente;

                //Se il checkPoint da raggiungere era il primo allora aggiorno il numero del giro e riabilitiamo i timepoints
                if (checkPoint == 0)
                {
                    giro++;
                    enableAll();
                }

                checkPointSucc++;

                //nel caso la macchina raggiunga l'ultimo checkPoint allora il checkpoint da raggiungere diventa il primo
                if (checkPointSucc >= checkPoints.Length)
                    checkPointSucc = 0;
            }
        }

        //Se la macchina ha raggiunto un collezionabile allora aggiorno il punteggio e do del tempo aggiuntivo
        if (other.gameObject.tag == "TimePoint")
        {
            tempoRimasto += 10;
            
            //A ogni giro si da piu punteggio
            numCoins += 100 * giro;
            testoCoins.GetComponent<UnityEngine.UI.Text>().text = "Coins: " + numCoins;
            
            //Il collezionabile viene disabilitato (verra abilitato al prossimo giro)
            other.gameObject.SetActive(false);
        }
    }

    /*Funzione che abilita tutti i timepoints*/
    private void enableAll()
    {
        foreach (GameObject cube in timePoints)
        {
            cube.SetActive(true);
        }
    }
}