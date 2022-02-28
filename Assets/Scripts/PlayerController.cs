using UnityEngine;
using UnityEngine.UI;




/**COSE CHE STIAMO RINOMINANDO (DA TOGLIERE):
 * b -> frenata
 * a -> accellerazione
 * s -> sterzata
 * ds -> carController
 * Drive -> CarController
 */
public class PlayerController : MonoBehaviour


{
    /*Mi occorre gestire il funzionamento delle ruote e quindi recupero il CarController*/
    CarController carController;
    

    //float lastTimeMoving = 0.0f;
    //Vector3 lastPosition;
    //Quaternion lastRotation;

    //CheckpointManager cpm;
    //float finishSteer;

    /*  void ResetLayer() {

          ds.rb.gameObject.layer = 0;
          this.GetComponent<Ghost>().enabled = false;
      }
      */

    void Start()
    {

        carController = this.GetComponent<CarController>();
        
        //nomeGiocatore.GetComponent<Text>().text = "Lap: 0";
        //this.GetComponent<Ghost>().enabled = false;
        //lastPosition = ds.rb.gameObject.transform.position;
        //lastRotation = ds.rb.gameObject.transform.rotation;
        //finishSteer = Random.Range(-1.0f, 1.0f);
    }

    void Update()
    {
        /*
        if (cpm == null) {

            cpm = ds.rb.GetComponent<CheckpointManager>();
        }
        */

        /*
        if (cpm.lap == RaceMonitor.totalLaps + 1) {

            ds.highAccel.Stop();
            ds.Go(0.0f, finishSteer, 0.0f);
        }
        */

        /*Rilevo la pressione dei tasti per andare avanti/indietro per l'accelerazione*/
        float accelerazione = Input.GetAxis("Vertical");

        /*Rilevo la pressione dei tasti per il movimento orizzontale per la sterzata*/
        float sterzata = Input.GetAxis("Horizontal");

        /*Rilevo la pressione della barra spaziatrice per la frenata*/
        float frenata = Input.GetAxis("Jump");

        /*
        if (ds.rb.velocity.magnitude > 1.0f || !RaceMonitor.racing) {

            lastTimeMoving = Time.time;
        }
        */
        /*
        RaycastHit hit;
        if (Physics.Raycast(ds.rb.gameObject.transform.position, -Vector3.up, out hit, 10)) {

            if (hit.collider.gameObject.tag == "road") {

                lastPosition = ds.rb.gameObject.transform.position;
                lastRotation = ds.rb.gameObject.transform.rotation;
            }
        }
        */

        /*if (Time.time > lastTimeMoving + 4 || carController.rb.gameObject.transform.position.y < -5.0f) {


            carController.rb.gameObject.transform.position = cpm.lastCP.transform.position + Vector3.up * 2;
            carController.rb.gameObject.transform.rotation = cpm.lastCP.transform.rotation;
            carController.rb.gameObject.layer = 8;
            //this.GetComponent<Ghost>().enabled = true;
            Invoke("ResetLayer", 3);
        }*/


        /*if (!RaceMonitor.racing) 
              a = 0.0f;
              */

        GameObject tac = GameObject.FindGameObjectWithTag("Tachimetro");  
        tac.GetComponent<Tachimetro>().ShowSpeed(carController.velocitaCorrente, 0f, carController.velocitaMassima); 
        
        carController.Move(accelerazione, sterzata, frenata);
        carController.CheckSgommata();
        carController.CalcolaSuonoMotore();
    }
}
