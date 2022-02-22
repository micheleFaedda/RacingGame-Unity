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
    
    

    //public AudioSource skidSound;
    //public AudioSource highAccel;

    //public Transform skidTrailPrefab;
    //Transform[] skidTrails = new Transform[4];

    //public ParticleSystem smokePrefab;
    //ParticleSystem[] skidSmoke = new ParticleSystem[4];

    //public GameObject brakeLight;

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

    /*public void StartSkidTrail(int i) {

        if (skidTrails[i] == null) {

            skidTrails[i] = Instantiate(skidTrailPrefab);
        }

        skidTrails[i].parent = collidersRuote[i].transform;
        skidTrails[i].localPosition = -Vector3.up * collidersRuote[i].radius;
    }*/

    /*public void EndSkidTrail(int i) {

        if (skidTrails[i] == null) return;

        Transform holder = skidTrails[i];
        skidTrails[i] = null;
        holder.parent = null;
        Destroy(holder.gameObject, 30);
    }*/

    // Start is called before the first frame update
    void Start() {

       /*
        for (int i = 0; i < 4; ++i) {

            skidSmoke[i] = Instantiate(smokePrefab);
            skidSmoke[i].Stop();
        }

        brakeLight.SetActive(false);
        */
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
 */   }
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
  /*
   public void CheckForSkid() {

        int numSkidding = 0;
        for (int i = 0; i < 4; ++i) {

            WheelHit wheelHit;
            collidersRuote[i].GetGroundHit(out wheelHit);

            if (Mathf.Abs(wheelHit.forwardSlip) >= 0.4f || Mathf.Abs(wheelHit.sidewaysSlip) >= 0.4f) {

                numSkidding++;
                if (!skidSound.isPlaying) {
                    skidSound.Play();
                }
                // StartSkidTrail(i);
                skidSmoke[i].transform.position = collidersRuote[i].transform.position - collidersRuote[i].transform.up * collidersRuote[i].radius;
                skidSmoke[i].Emit(1);
            } else {

                // EndSkidTrail(i);
            }
        }
        if (numSkidding == 0 && skidSound.isPlaying) {

            skidSound.Stop();
        }
    }
*/

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
        /*
         if (frenata != 0.0f) {

            brakeLight.SetActive(true);
      
        } else {
            brakeLight.SetActive(false);
        }
        */
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
