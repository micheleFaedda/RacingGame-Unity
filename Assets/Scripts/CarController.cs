using UnityEngine;

/*
 * Usato per gestire sia il movimento del player sia il movimento delle macchine NPC
 * attraverso il metodo Move()
 */
public class CarController : MonoBehaviour
{
    //Array per le mesh delle ruote
    public GameObject[] ruote;

    //Array di WheelColliders per il colliders delle ruote
    public WheelCollider[] collidersRuote;

    //Forza che agisce sul punto in basso alla ruota (momento torcente)
    public float forza = 200;

    //Massima forza per la frenata
    public float massimaFrenata = 500.0f;

    //Angolo massimo di rotazione della ruota (Sterzata)
    public float angoloMassimoSterzata = 40.0f;

    //L'audio della sgommata
    public AudioSource suonoSgommata;

    //L'audio dell'accelerazione
    public AudioSource audioAccelerazione;

    //L'audio per quando non è in movimento
    public AudioSource audioIdle;

    //Prefab della traccia che verra isatanzaiata per ogni ruota
    public Transform tracciaSgommata;
    
    //ogni ruota ha una suo traccia quando slitta
    private Transform[] sgommateRuote = new Transform[4];

    //Prefab del fumo che verra isatanzaiato per ogni ruota
    public ParticleSystem fumoPrefab;
    
    //Ogni ruota quando slitta produce un efetto particellare
    private ParticleSystem[] fumiRuote = new ParticleSystem[4];

    //Le due luci di stop della macchina
    public GameObject[] luciFrenata;

    public Rigidbody rb;

    public float velocitaMassima = 200.0f;
    
    public GameObject centroMassa;

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        rb.centerOfMass = centroMassa.transform.localPosition;

        for (int i = 0; i < 4; ++i)
        {
            //istanzio e fermo il fumo per la ruota (deve comparire solo quando si ha la sgommata)
            fumiRuote[i] = Instantiate(fumoPrefab);
            fumiRuote[i].Stop();
        }

        //inizialmente le luci di stop sono disattivate
        luciFrenata[0].SetActive(false);
        luciFrenata[1].SetActive(false);

        if (this.gameObject.CompareTag("Player"))
        {
            this.GetComponent<PlayerController>().enabled = true;
            this.GetComponent<NPController>().enabled = false;
            //audioAccelerazione.volume = ;
            //suonoSgommata.volume = ;
        }
        else
        {
            this.GetComponent<PlayerController>().enabled = false;
            this.GetComponent<NPController>().enabled = true;
            audioAccelerazione.volume = 0.35f;
            suonoSgommata.volume = 0.1f;
        }
    }

    public float VelocitaCorrente()
    {
        return rb.velocity.magnitude * 3;
    }

    /**
     * Metodo che si occupa di segnare sul terreno la traccia della sgommata per la ruota con indice 'i'
     */
    public void InizioSgommata(int i)
    {
        //inizializzo la traccia della ruota se non ancora presente per la ruota passata come parametro (indice)
        if (sgommateRuote[i] == null)
        {
            sgommateRuote[i] = Instantiate(tracciaSgommata);
        }

        //Assegno come parent della traccia della routa 'i' il collider di quella ruota
        sgommateRuote[i].parent = collidersRuote[i].transform;

        sgommateRuote[i].localRotation = Quaternion.Euler(90, 0, 0);
        
        //Metto la sgommata esattamente sotto la ruota che lo genera tramite la sua posizione
        sgommateRuote[i].localPosition = -Vector3.up * collidersRuote[i].radius;
    }

    /**
     * Metodo che si occupa di terminare sul terreno la traccia della sgommata  per la ruota con indice 'i'
    */
    public void FineSgommata(int i)
    {
        //se è null vuol dire che la sgommata a questa ruota non è stata neanche assegnata e quindi per evitare errore esco direttamente
        if (sgommateRuote[i] == null)
            return;

        //rilevo la vecchia sgommata della ruota
        Transform vecchiaSgommata = sgommateRuote[i];

        //setto a null la sgommata per la ruota
        sgommateRuote[i] = null;

        //anche al parent della vecchia sgommata
        vecchiaSgommata.parent = null;

        vecchiaSgommata.rotation = Quaternion.Euler(90, 0, 0);

        //la sgommata viene eliminata al termine di 20 secondi
        Destroy(vecchiaSgommata.gameObject, 20);
    }

    /*
     * Attiviamo il suono di Idle o il suono di accelerazione in funzione della velocità della macchina
     */
    public void SuonoMotore()
    {
        if (VelocitaCorrente() <= 2.0f)
        {
            if (!audioIdle.isPlaying)
                audioIdle.Play();

            audioAccelerazione.Stop();
            
            //calcolo il suono in base alla distanza dell'NPC con il player
            SuonoInBaseADistanza(audioIdle);
        }
        else
        {
            audioIdle.Stop();

            if (!audioAccelerazione.isPlaying)
            {
                audioAccelerazione.Play();
                
                
            }
            //calcolo il suono in base alla distanza dell'NPC con il player
            SuonoInBaseADistanza(audioAccelerazione);
        }
    }
    
    /*Quando una ruota entra in collisione questa restituisce un oggetto WheelHit che rappresenta il colpo che subisce la ruota
     *inoltre riproduce anche il suono della sgommata*/
    public void CheckSgommata()
    {
        int numeroRuoteSgommano = 0;

        //Per ogni ruota gestico gli hit che riceve la ruota e gestisco il suono e il suo effetto sgommata
        for (int i = 0; i < 4; ++i)
        {
            WheelHit ruotaHit;

            //ottengo l'hit della ruota corrispondente al collider
            collidersRuote[i].GetGroundHit(out ruotaHit);

            /*Accedo allo slittamento delle 4 ruote su entrambi gli assi e controllo che sia effettivamente abbastanza grande, se questo
             *valore è alto allora sta sgommando. Il controllo viene fatto è sia in avanti, sia laterale*/
            //Debug.Log(this.transform.position.y);
            if (Mathf.Abs(ruotaHit.forwardSlip) >= 0.5f || Mathf.Abs(ruotaHit.sidewaysSlip) >= 0.5f)
            {
                numeroRuoteSgommano++;
                
                //Visto che è stato rilevato lo slittamento nel caso il suono non fosse ancora attivo, allora lo rendo attivo
                if (!suonoSgommata.isPlaying)
                {
                    suonoSgommata.Play();

                }
                
                //calcolo il suono in base alla distanza dell'NPC con il player
                SuonoInBaseADistanza(suonoSgommata);
                
                //inzio della sgommata per la ruota 'i' a video (traccia)
                InizioSgommata(i);

                fumiRuote[i].transform.position = collidersRuote[i].transform.position -
                                                  collidersRuote[i].transform.up * collidersRuote[i].radius;
                
                // 1 -> numero particelle ogni volta che si entra in questa funzione
                fumiRuote[i].Emit(1);
            }
            else
            {
                FineSgommata(i);
            }
        }
        
        //Attiviamo il suono della sgommata solo quando le ruote che stanno sgommando sono piu di 1 se no lo disattiviamo
        if (numeroRuoteSgommano == 0)
        {
            suonoSgommata.Stop();
        }
    }
    
    /**
     * Questo metodo permette di gestire l'accelerazione, la sterzata e la frenata delle ruote appartenenti alla macchina
     */
    public void Move(float accelerazione, float sterzata, float frenata)
    {
        //Non faccio partire la macchina prima del countdown
        if (!GameManager.start) return;

        //blocco il valore di accelerazione tra -1 e 1
        accelerazione = Mathf.Clamp(accelerazione, -1, 1) * forza;

        //blocco il valore di frenata tra 0 e 1 (la macchina se frena non puo tornae indietro)
        frenata = Mathf.Clamp(frenata, 0, 1) * massimaFrenata;

        //blocco il valore di sterzata tra -1 e 1
        sterzata = Mathf.Clamp(sterzata, -1, 1) * angoloMassimoSterzata;

        
        if (frenata != 0.0f)
        {
            luciFrenata[0].SetActive(true);
            luciFrenata[1].SetActive(true);
        }
        else
        {
            luciFrenata[0].SetActive(false);
            luciFrenata[1].SetActive(false);
        }

        //Modifichiamo l'altezza del suono di accelerazione in funzione della velocità della macchina
        // +0.5f perche se no è roppo basso
        audioAccelerazione.pitch = (VelocitaCorrente() / velocitaMassima) + 0.5f;

        //applicchiamo la forza a tutte le ruote (wheelCollider)
        for (int i = 0; i < collidersRuote.Length; ++i)
        {
            //motorTorque è un'attributo che pemette di simulare la forza motrice della macchina 
            collidersRuote[i].motorTorque = accelerazione;

            //Debug.Log(collidersRuote[i].motorTorque);

            //se sono le prime ruote allora sono quelle anteriori e quindi devono sterzare
            if (i < 2)
            {
                //steerAngle è un'attributo che pemette di simulare la sterzata della macchina 
                collidersRuote[i].steerAngle = sterzata;
            }
            else
            {
                //brakeTorque è un'attributo che pemette di simulare la frenata della macchina
                //collidersRuote[i].brakeTorque = frenata;
            }
            
            collidersRuote[i].brakeTorque = frenata;
            
            Quaternion quaternione;
            Vector3 posizione;

            //Le strutture precedenti vengono passate per riferimento
            collidersRuote[i].GetWorldPose(out posizione, out quaternione);

            /*Applico all'attributo rotation della transform, la rotazione tramite il quaternione anche alla mesh per
             * poter avere coerenza tra movimento del collider e la mesh dato che sono scomposti*/
            ruote[i].transform.rotation = quaternione;

            /*Applico all'attributo position della transform la rotazione tramite il quaternione anche alla mesh per
             * poter avere coerenza tra movimento del collider e la mesh dato che sono scomposti*/
            ruote[i].transform.position = posizione;
        }
    }
    
    /*Metodo che si occupa di calcolare il suono in base alla distanza degli NPC*/
    private void SuonoInBaseADistanza(AudioSource audio)
    {
        if (audio.isPlaying)
        {
            if (!gameObject.CompareTag("Player"))
            {
                float distanza =  Mathf.Abs(Vector3.Distance(rb.position,
                    GameObject.FindWithTag("Player").gameObject.transform.position));

                audio.volume = 1f -  (distanza / 100);
                audio.volume -= 0.25f;
            }
        }
    }
}