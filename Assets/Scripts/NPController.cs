using UnityEngine;
using System.Collections;

public class NPController : MonoBehaviour
{
    
    private CarController carController;
    
    public float steeringSensitivity = 0.01f;

    //Posizione del wayPoint che la macchina non ha ancora raggiunto
    public Vector3 targetSucc;
    
    //Posizione dell'ultimo wayPoint che la macchina non ha raggiunto (serve solo per la coroutine)
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
        //Andiamo a popolare l'array di waypoints
        waypoints = new Transform[circuito.transform.childCount];
        for (int i = 0; i < circuito.transform.childCount; i++)
        {
            waypoints[i] = circuito.transform.GetChild(i);
        }
        
        carController = this.GetComponent<CarController>();
        
        //Inizialmente il targetSucc è il primo nell'array
        targetSucc = waypoints[wpDaRaggiungere].transform.position;
        
        //Start della coroutine che si occupa di correggere gli NPC
        corutine = WaitAndRepositioning(10f);
        StartCoroutine(corutine);
    }

    void FixedUpdate()
    {

        Vector3 localTarget = carController.rb.gameObject.transform.InverseTransformPoint(targetSucc);

        float distanzaCorrente = Vector3.Distance(targetSucc, carController.rb.transform.position);

        float targetAngle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;

        //blocco il valore di starzata tra -1 e 1
        float sterzata = Mathf.Clamp(targetAngle * steeringSensitivity, -1.0f, 1.0f) * Mathf.Sign(carController.VelocitaCorrente());
        
        
        //Nel caso stia per raggiungere il waypoint accelerazione e frenata cambiano nell'if successivo
        float accelerazione = 1f;
        float frenata = 0;

        //Nel caso stia per raggiungere il waypoint, la sua velocità sia abbastanza elevata
        //e si incontra un punto di frenata, si frena per non mancare quello successivo
        if(distanzaCorrente < 5 && carController.rb.velocity.magnitude > 9f  && waypoints[wpDaRaggiungere].tag.Equals("frenata"))
        {
            frenata = 0.8f;
            carController.rb.velocity -= carController.rb.velocity * 0.06f;
        }
        
        
        //Nel caso abbia un'angolo elevato verso il prossimo target e una velocità elevata, allora diminuisco la sua velocità
        if (Mathf.Abs(targetAngle) > 36f && carController.rb.velocity.magnitude > 14f)
        {
            frenata = 0.4f;
            carController.rb.velocity -= carController.rb.velocity * 0.025f;
        }
        
        carController.Move(accelerazione, sterzata, frenata);

        //Due if separati per non far frenare le macchine troppo tardi
        //NPC non deve per forza prendere il wapoint in modo preciso, basta che si aviccii abastanza
        if(distanzaCorrente < 4){
            wpDaRaggiungere++;
            
            //Nel caso abbia fatto un ggiro completo il target diventa di nuovo il primo waypoint
            if(wpDaRaggiungere >= waypoints.Length)
                wpDaRaggiungere=0;
            
            //Setto la posizione del prossimo wp da raggiungere
            targetSucc = waypoints[wpDaRaggiungere].transform.position;
            
        }
        
        carController.CheckSgommata();
        carController.SuonoMotore();
    }
    
    /*
     * Si occupa di riposizionare gli NPC dopo un certo tempo nel caso questi non siano riusciti a raggiungere
     * un waypoint
     */
    private IEnumerator WaitAndRepositioning(float t)
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
                //Debug.Log("DEBUG GIRATO: \nSONO LA MACCHINA: "+ carController.name);
                
            }
        }
        
        
    }
      
}
