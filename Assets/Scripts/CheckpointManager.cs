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
    
    public float timeEntered = 0.0f;
    
    int carRego;
    private bool regoSet;
    public string position;

    private GameObject primoClassifica;
    private GameObject secondoClassifica;
    private GameObject terzoClassifica;
    private GameObject quartoClassifica;
    
    //Array di checkPoints per tenere conto del numero di giri fatti
    private GameObject[] checkPoints;

    void Start()
    {
        checkPoints = GameObject.FindGameObjectsWithTag("CheckPoint");
        primoClassifica = GameObject.FindGameObjectWithTag("Primo");
        secondoClassifica = GameObject.FindGameObjectWithTag("Secondo");
        terzoClassifica = GameObject.FindGameObjectWithTag("Terzo");
        quartoClassifica = GameObject.FindGameObjectWithTag("Quarto");
    }

    void Update()
    {

        if (carController == null)
        {
            carController = this.GetComponent<CarController>();
        }

        if (!regoSet)
        {
            carRego = Leaderboard.RegisterCar(playerName);
            regoSet = true;
            return;
        }

        Leaderboard.SetPosition(carRego, giro, checkPoint, timeEntered);
        position = Leaderboard.GetPosition(carRego);

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
                timeEntered = Time.time;

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
                primoClassifica.GetComponent<UnityEngine.UI.Text>().text = playerName;
                break;
            case "Second":
                secondoClassifica.GetComponent<UnityEngine.UI.Text>().text = playerName;
                break;
            case "Third":
                terzoClassifica.GetComponent<UnityEngine.UI.Text>().text = playerName;
                break;
            case "Fourth":
                quartoClassifica.GetComponent<UnityEngine.UI.Text>().text = playerName;
                break;
        }

    }
}