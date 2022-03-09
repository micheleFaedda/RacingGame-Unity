using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    private CarController carController;
    
    //Nome del giocatore
    public string playerName;

    //Il numero di giro che la macchina sta facendo
    public int giro = 0;
    
    //Indice del checkPoint raggiunto
    public int checkPoint = -1;
    
    //Indice del ckeckPoint non ancora raggiunto
    public int checkPointSucc;
    
    //Il tempo di quando è entrata nell'ultimo checkPoint
    public float tempoEntrata = 0.0f;
    
    //Numero della macchina 
    int idMacchina;
    
    //Flag se la macchina è stata registrata nella classifica
    private bool macchinaRegistrata;
    
    //Posizione in classifica
    public string position;

    private GameObject primoClassificaTesto;
    private GameObject secondoClassificaTesto;
    private GameObject terzoClassificaTesto;
    private GameObject quartoClassificaTesto;
    
    //Array di checkPoints per tenere conto del numero di giri fatti
    private GameObject[] checkPoints;

    void Start()
    {
        checkPoints = GameObject.FindGameObjectsWithTag("CheckPoint");
        primoClassificaTesto = GameObject.FindGameObjectWithTag("Primo");
        secondoClassificaTesto = GameObject.FindGameObjectWithTag("Secondo");
        terzoClassificaTesto = GameObject.FindGameObjectWithTag("Terzo");
        quartoClassificaTesto = GameObject.FindGameObjectWithTag("Quarto");
    }

    void Update()
    {

        if (carController == null)
        {
            carController = this.GetComponent<CarController>();
        }

        //se la macchina non è stata registrata la registo e prendo il suo id
        if (!macchinaRegistrata)
        {
            idMacchina = Classifica.RegisteraMacchina(playerName);
            macchinaRegistrata = true;
            return;
        }

        Classifica.setPosizione(idMacchina, giro, checkPoint, tempoEntrata);
        position = Classifica.GetPosizione(idMacchina);

        setClassifica(position);

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
                
                tempoEntrata = Time.time;

                //Se il checkPoint da raggiungere era il primo allora aggiorno il numero del giro
                if (checkPoint == 0)
                {
                    giro++;
                }

                checkPointSucc++;

                //nel caso la macchina raggiunga l'ultimo checkPoint allora il checkpoint da raggiungere diventa il primo
                if (checkPointSucc >= checkPoints.Length)
                    checkPointSucc = 0;
            }
        }
    }

    private void setClassifica(string position)
    {
        switch (position)
        {
            case "First":
                primoClassificaTesto.GetComponent<UnityEngine.UI.Text>().text = playerName;
                break;
            case "Second":
                secondoClassificaTesto.GetComponent<UnityEngine.UI.Text>().text = playerName;
                break;
            case "Third":
                terzoClassificaTesto.GetComponent<UnityEngine.UI.Text>().text = playerName;
                break;
            case "Fourth":
                quartoClassificaTesto.GetComponent<UnityEngine.UI.Text>().text = playerName;
                break;
        }

    }
}