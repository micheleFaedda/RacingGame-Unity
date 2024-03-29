using UnityEngine;
using UnityEngine.UI;
/**COSE CHE STIAMO RINOMINANDO (DA TOGLIERE):
 * torque -> forza
 * a -> accellerazione
 * maxSteerAngle -> angoloMassimoSterzata
 * maxBrakeTorque -> massimaFrenata
 * trustTorque -> forzaEffettiva
 * steer -> sterzata
 * brake -> frenata
 * Wheels -> ruote
 * WCs -> collidersRuote
 * Metodo Go -> Move
 * maxSpeed -> velocitàMassima
 * skidSound -> suonoSgommata
 * wheelHit -> ruotaHit
 * checkForSkid -> CheckSgommata
 * skidTrailPrefab -> tracciaSgommata
 * skidTrails -> sgommataRuote
 * startSkidTrail -> inizioSgommata
 * smokePrefab -> fumo
 * skidSmoke -> sgommataRuote
 * breakeLight -> luciFrenata
 */
public class CarController : MonoBehaviour {
    
    /*Array di GameObjects per le ruote*/
    public GameObject[] ruote;
    
    /*Array di WheelColliders per il colliders delle ruote*/
    public WheelCollider[] collidersRuote;
    
    /*Forza che agisce sulla sfera della ruota, ovvero il punto in cui agisce la forza per ruotare la ruota*/
    public float forza = 200;
    
    /*Massima forza per la frenata*/
    public float massimaFrenata = 500.0f;
    
    /*Angolo massimo di rotazione della ruota (Sterzata)*/
    public float angoloMassimoSterzata = 30.0f;
    
    
    /*dichiaro un oggetto AudioSource per l'audio della sgommata*/
    public AudioSource suonoSgommata;
    //public AudioSource highAccel;

    public Transform tracciaSgommata;
    Transform[] sgommataRuote = new Transform[4];

    public ParticleSystem fumoPrefab;
    ParticleSystem[] fumoRuote = new ParticleSystem[4];

    public GameObject[] luciFrenata;

    public Rigidbody rb;
  //  public float gearLength = 3.0f;
   // public float currentSpeed { get { return rb.velocity.magnitude * gearLength; } }
    //public float lowPitch = 1.0f;
    //public float highPitch = 6.0f;
    //public int numGears = 5;
    //float rpm;
    //int currentGear = 1;
    //float currentGearPerc;
    public float velocitàMassima = 200.0f;

    //public GameObject playerNamePrefab;
    //public Renderer jeepMesh;

    //public string networkName = "";

   // string[] aiNames = { "Adrian", "Lee", "Penny", "Merlin", "Tabytha", "Pauline", "John", "Kia", "Chloe", "Fiona", "Mathew" };

   
    /**
     * Metodo che si occupa di segnare sul terreno la traccia della sgommata
     */
    public void InizioSgommata(int i) {
        
        //inizializzo la traccia della ruota se non ancora presente per la ruota passata come parametro (indice)
        if (sgommataRuote[i] == null) {

            sgommataRuote[i] = Instantiate(tracciaSgommata);
        }
        
        //Rilevo chi è il parent della sgommata che è esattamente la ruota
        sgommataRuote[i].parent = collidersRuote[i].transform;

        sgommataRuote[i].localRotation = Quaternion.Euler(90, 0, 0);
        //Metto la sgommata esattamente sotto la ruota che lo genera tramite la sua posizione
        sgommataRuote[i].localPosition = -Vector3.up * collidersRuote[i].radius;
    }
   
    /**
     * Metodo che si occupa di terminare sul terreno la traccia della sgommata
    */
    public void FineSgommata(int i) {
        
        //se è null vuol dire che è già terminata
        if (sgommataRuote[i] == null) 
            return;
        
        //rilevo la vecchia sgommata della ruota
        Transform vecchiaSgommata = sgommataRuote[i];
        
        //setto a null la sgommata per la ruota
        sgommataRuote[i] = null;
        
        //anche al parent della vecchia sgommata
        vecchiaSgommata.parent = null;
        
        vecchiaSgommata.rotation = Quaternion.Euler(90, 0, 0);
        
        //la sgommata viene eliminata al termine di 40 secondi
        Destroy(vecchiaSgommata.gameObject, 40);
    }
     
     
    // Start is called before the first frame update
    void Start()
    {   
        /*recupero il rigidbody*/
        rb = this.GetComponent<Rigidbody>();
        
        
         for (int i = 0; i < 4; ++i) {
         
             /*istanzio il fumo per la ruota*/
             fumoRuote[i] = Instantiate(fumoPrefab);
             
             /*lo fermo perchè deve comparire solo quando si ha la sgommata*/
             fumoRuote[i].Stop();
         }
 
         luciFrenata[0].SetActive(false);
         luciFrenata[1].SetActive(false);
         
        // GameObject playerName = Instantiate(playerNamePrefab);
        //playerName.GetComponent<NameUIController>().target = rb.gameObject.transform;
/*
        if (this.GetComponent<AIController>().enabled)
            if (networkName != "")
                playerName.GetComponent<Text>().text = networkName;
            else
                playerName.GetComponent<Text>().text = aiNames[Random.Range(0, aiNames.Length)];
        else
            playerName.GetComponent<Text>().text = PlayerPrefs.GetString("PlayerName");

        playerName.GetComponent<NameUIController>().carRend = jeepMesh;
 */
    }
/*
    public void CalculateEngineSound() {

        float gearPercentage = (1 / (float)numGears);
        float targetGearFactor = Mathf.InverseLerp(gearPercentage * currentGear, gearPercentage * (currentGear + 1),
            Mathf.Abs(currentSpeed / maxSpeed));

        currentGearPerc = Mathf.Lerp(currentGearPerc, targetGearFactor, Time.deltaTime * 5.0f);

        var gearNumFactor = currentGear / (float)numGears;
        rpm = Mathf.Lerp(gearNumFactor, 1, currentGearPerc);

        float speedPercentage = Mathf.Abs(currentSpeed / maxSpeed);
        float upperGearMax = (1 / (float)numGears) * (currentGear + 1);
        float downGearMax = (1 / (float)numGears) * currentGear;

        if (currentGear > 0 && speedPercentage < downGearMax) {

            currentGear--;
        }

        if (speedPercentage > upperGearMax && (currentGear < (numGears - 1))) {

            currentGear++;
        }

        float pitch = Mathf.Lerp(lowPitch, highPitch, rpm);
        highAccel.pitch = Mathf.Min(highPitch, pitch) * 0.25f;

    }
*/
  
    /*Quando una ruota entra in collisione questa restituisce un oggetto WheelHit che rappresenta il colpo che subisce la ruota
     *inoltre riproduce anche il suono della sgommata*/
   public void CheckSgommata() {
        
        
        int numeroRuoteSgommano = 0;
        
        /**
         * Per ogni ruota gestico gli hit che riceve la ruota e gestisco il suono e il suo effetto sgommata
         */
        for (int i = 0; i < 4; ++i) {

            WheelHit ruotaHit;
            
            /*ottengo l'hit della ruota corrispondente al collider*/
            collidersRuote[i].GetGroundHit(out ruotaHit);
            
            /*Accedo allo slittamento su entrambi gli assi e controllo che sia effettivamente abbastanza grande, se questo
             *valore è alto allora sta sgommando. Lo metto in valore assoluto perche gestisco solamente lo slittamento in
             *accellerazione e non in decellerazione.*/
            if (Mathf.Abs(ruotaHit.forwardSlip) >= 0.4f || Mathf.Abs(ruotaHit.sidewaysSlip) >= 0.4f) {

                numeroRuoteSgommano++;
                
                if (!suonoSgommata.isPlaying) {
                    suonoSgommata.Play();
                }
                /*inzio della sgommata per la ruota a video (traccia)*/
                 InizioSgommata(i);
                
                 fumoRuote[i].transform.position = collidersRuote[i].transform.position - collidersRuote[i].transform.up * collidersRuote[i].radius;
                 fumoRuote[i].Emit(1);
            } else {

                 FineSgommata(i);
            }
        }
        if (numeroRuoteSgommano == 0 && suonoSgommata.isPlaying) {

            suonoSgommata.Stop();
        }
    }


    /**
     * Questo metodo permette di gestire l'accelerazione, la sterzata e la frenata delle ruote appartenenti alla macchina
     * Si occupa anche del movimento delle mesh tramite quaternioni per evitare artefatti dati dall'interpolazione
     * di rotazioni.
     */
    public void Move(float accelerazione, float sterzata, float frenata) {
        
        /*stabiliamo la percentuale di accelerazione, questo per andare avanti e indietro*/
        accelerazione = Mathf.Clamp(accelerazione, -1, 1);  
        
        /*stabiliamo la percentuale di frenata che viene moltiplicata per la massima forza di frenata applicabile*/
        frenata = Mathf.Clamp(frenata, 0, 1) * massimaFrenata;

        /*stabiliamo la percentuale di sterzata che viene moltiplicato per l'angolo massimo*/
        sterzata = Mathf.Clamp(sterzata, -1, 1) * angoloMassimoSterzata;
       
        
        /*sezione per le luci, se è diverso da 0 allora attiva le luci di stop*/
        if (frenata != 0.0f)
        {
            //attivo lo stop
            luciFrenata[0].SetActive(true); 
            luciFrenata[1].SetActive(true); 
        } else {
            //disattivo lo stop
            luciFrenata[0].SetActive(false);
            luciFrenata[1].SetActive(false);
        }
        
        /*occorre avere una forza corretta in base all'accelerazione che è stata corretta in precedenza*/
        float forzaEffettiva = 0.0f;
        
        
        //if (currentSpeed < velocitàMassima) {
            forzaEffettiva = accelerazione * forza;
        //}
        
        /*for che permette di applicare le corrispettive forze a tutte le ruote (wheelCollider)*/
        for (int i = 0; i < collidersRuote.Length; ++i) {
            
            
            /*motorTorque è un'attributo che pemette di simulare la forza motrice della macchina */
            collidersRuote[i].motorTorque = forzaEffettiva;
            
            /*se sono le prime ruote allora sono quelle anteriori e quindi devono sterzare*/
            if (i < 2) {
                /*motorTorque è un'attributo che pemette di simulare la sterzata della macchina */
                collidersRuote[i].steerAngle = sterzata;
            } else {
                
                /*brakeTorque è un'attributo che pemette di simulare la frenata della macchina*/
                collidersRuote[i].brakeTorque = frenata;
            }
            
            /*Quaternione che rende plausibile la rotazione*/
            Quaternion quaternione;
            
            Vector3 posizione;
            
            /*Le strutture precedenti vengono passate per riferimento, questo per poter salvare il valore aggiornato
             *dato dalla scena*/
            collidersRuote[i].GetWorldPose(out posizione, out quaternione);
            
            /*Applico all'attributo rotation della transform, la rotazione tramite il quaternione anche alla mesh per
             * poter avere coerenza tra movimento del collider e la mesh dato che sono scomposti*/
            ruote[i].transform.rotation = quaternione;
            
            /*Applico all'attributo position della transform la rotazione tramite il quaternione anche alla mesh per
             * poter avere coerenza tra movimento del collider e la mesh dato che sono scomposti*/
            ruote[i].transform.position = posizione;

}
}
}
