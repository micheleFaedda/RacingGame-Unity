using UnityEngine;
using System.Collections;

/*
 * Gestione degli NPC attraverso una serie di waypoints.
 *  Passa i parametri di accelerazione, fretata e starzata
 *  al CarController della macchina NPC (metodo Move(float, float, float))
 */
public class NPController : MonoBehaviour
{
    
    private CarController carController;

    //Posizione del wayPoint che la macchina non ha ancora raggiunto
    public Vector3 targetSucc;
    
    //Posizione dell'ultimo wayPoint che la macchina non ha raggiunto (serve per la coroutine)
    public Vector3 targeSucctOld;

    //Indice del wayPoint che la macchina non ha ancora raggiunto
    public int wpDaRaggiungere = 0;

    //Pardre dei waypoint nella scena
    public GameObject circuito;
    
    //Array di waypoint preso dal GameObject circuito
    private Transform[] waypoints;
    
    //Per risposizionare gli NPC
    private IEnumerator corutine;
    
    void Start()
    {    
        carController = this.GetComponent<CarController>();
        
        //Andiamo a popolare l'array di waypoints
        waypoints = new Transform[circuito.transform.childCount];
        for (int i = 0; i < circuito.transform.childCount; i++)
        {
            waypoints[i] = circuito.transform.GetChild(i);
        }
        
        //Inizialmente il targetSucc è il primo nell'array
        targetSucc = waypoints[wpDaRaggiungere].transform.position;
        
        //Start della coroutive che si occupa di correggere gli NPC
        corutine = Riposizionamento(5f);
        StartCoroutine(corutine);
    }

    void Update()
    {
        
        //rb è il centro
        Vector3 localTarget = carController.rb.gameObject.transform.InverseTransformPoint(targetSucc);

        //Distanza della macchina dal WP che deve raggiungere
        float distanza = Vector3.Distance(targetSucc, carController.rb.transform.position);

        //Atan2 restituisce radianti, convertiamo in gradi con Rad2Deg  
        float targetAngle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;

        //blocco il valore di starzata tra -1 e 1
        float sterzata = Mathf.Clamp(targetAngle * 0.01f, -1.0f, 1.0f) * Mathf.Sign(carController.VelocitaCorrente());
        
        //Nel caso stia per raggiungere il waypoint accelerazione e frenata cambiano nell'if successivo
        float accelerazione = 1f;
        float frenata = 0;

        //Nel caso stia per raggiungere il waypoint si frena per non mancare quello successivo
        if(distanza < 5){
            frenata = 0.8f;
            accelerazione = 0.1f;
        }
        
        //Due if separati per non far frenare le macchine troppo tardi
        
        //NPC non deve per forza prendere il wapoint in modo preciso, basta che si aviccii abastanza
        if(distanza < 4){
            wpDaRaggiungere++;
            
            //Nel caso abbia fatto un ggiro completo il target diventa di nuovo il primo waypoint
            if (wpDaRaggiungere >= waypoints.Length)
                wpDaRaggiungere = 0;

            //Setto la posizione del prossimo wp da raggiungere
            targetSucc = waypoints[wpDaRaggiungere].transform.position;
        }
        
        //La macchina viene mossa dal CarController
        carController.Move(accelerazione, sterzata, frenata);
        carController.CheckSgommata();
        carController.SuonoMotore();
    }
    
    /*
     * Si occupa di riposizionare gli NPC dopo un certo tempo nel caso questi non siano riusciti a raggiungere
     * un waypoint
     */
    private IEnumerator Riposizionamento(float t)
    {
        while (true)
        {
            targeSucctOld = targetSucc;
            yield return new WaitForSeconds(t);

            /*Se il waypoint da raggiungere dopo un tot di secondi è sempre quello,
             allora riposiziono la macchina all'ultimo checkpoint che aveva daggiunto*/
            if (targeSucctOld == targetSucc && wpDaRaggiungere > 0)//wpDaRaggiungere>0 perche all'inizio le macchina ci mettono qualcosa a raggiungere il primo wp
            {
                //Per riposizionare la macchina non basta settare la posizione, serve anche la rotazione per metterlo nella direzone giusta
                this.transform.position =  waypoints[wpDaRaggiungere-1].transform.position + Vector3.up * 1.2f;
                this.transform.rotation = waypoints[wpDaRaggiungere-1].transform.rotation;
            }
        }
    }
}
