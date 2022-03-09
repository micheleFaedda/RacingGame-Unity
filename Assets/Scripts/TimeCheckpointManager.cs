using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

public class TimeCheckpointManager : MonoBehaviour {

    public int giro = 0;
    public int checkPoint = -1;
    public float timeEntered = 0.0f;
    int checkPointCount;
    int checkPointSucc;
    public GameObject checkPointSucc_go;
    private GameObject[] timePoints;
    private Color normalTimerColor = new Color32(253,158, 0, 255);
    private int numCoins;
    
    private GameObject timer;
    private GameObject coins;

    private float currentTime = 0f;
    private float startingTime = 1000f;
    
    private CarController carController;

    void Start()
    {

        numCoins = 0;
        currentTime = startingTime;
        
        GameObject[] checkPoints = GameObject.FindGameObjectsWithTag("CheckPoint");
        
        timePoints = GameObject.FindGameObjectsWithTag("TimePoint");
        
        timer = GameObject.FindGameObjectWithTag("Timer"); //Questo mi serve
        coins = GameObject.FindGameObjectWithTag("Coins"); //Questo mi serve

        checkPointCount = checkPoints.Length;
        foreach (GameObject c in checkPoints) {

            if (c.name == "0") {
                checkPointSucc_go = c;
                break;
            }
        }
    }

    void Update()
    {
        if (carController == null)
        {
            carController = this.GetComponent<CarController>();
        }

        //Debug.Log(giro);

        if (GameManager.start)
        {
            currentTime -= 1 * Time.deltaTime;
            timer.GetComponent<UnityEngine.UI.Text>().text = currentTime.ToString("0");

            if (currentTime < 5)
            {
                timer.GetComponent<UnityEngine.UI.Text>().color = Color.red;//Non è un errore (stronzo bastardo)
            }
            else
            {
                timer.GetComponent<UnityEngine.UI.Text>().color = normalTimerColor;
            }

            if (currentTime < 0)
            {
                SceneManager.LoadScene(0);
            }
        }

    }
    
    /*Quando entriamo in un trigger controlliamo se è un checkpoint*/
    private void OnTriggerEnter(Collider other) {

        if (other.gameObject.tag == "CheckPoint") {

            int numeroCheckPointCorrente = int.Parse(other.gameObject.name);
            
            if (numeroCheckPointCorrente == checkPointSucc) {
                checkPointSucc_go = other.gameObject;
                checkPoint = numeroCheckPointCorrente;
                timeEntered = Time.time;
                if (checkPoint == 0)
                {   
                    giro++;
                    
                    enableAll();
                }
                checkPointSucc++;
                //Debug.Log("" +numCoins);
                if (checkPointSucc >= checkPointCount)
                    checkPointSucc = 0;
            }
        }
        
        if (other.gameObject.tag == "TimePoint") {
            currentTime += 10;
                numCoins += (1+int.Parse(other.gameObject.name)) * (giro+1);
                //Debug.Log("" +numCoins);
                coins.GetComponent<UnityEngine.UI.Text>().text = "Coins: " + numCoins;
                other.gameObject.SetActive(false);
        }
    }

    /*Funzione che abilita tutti i timepoints*/
    private void enableAll()
    {
        foreach(GameObject cube in timePoints){
            cube.SetActive(true);
        }
    }

}
