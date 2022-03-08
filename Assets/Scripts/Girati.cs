using UnityEngine;

/*
 * Dopo numerose prove abbiamo visto che la macchina non si capovolge facilmente.
 * Può però essere fatto di proposito (per esempio in discesa dal ponte).
 * Gestiamo un possibile capovolgimento della macchina con questo script.
 */
public class Girati : MonoBehaviour {

    private CarController carController;
    
    //Tempo quando la macchina è ancora nella posizione giusta
    private float tempoOk;

    void Start() {
        carController = this.GetComponent<CarController>();
    }
    void Update() {

        /*Aggiorniamo tempoOk
        Lo facciamo solo nel caso la macchina sia nella posizione che ci soddisfa (velocità adeguata o non capovolta ).
        Se la macchina è in posizione giusta allora transform.up.y > 0, se è capovolta allora è negativa.
        0.5 sarebbe il valore limite ancora accettabile per considerare che è in posizione giusta*/
        if (transform.up.y > 0.5f || carController.VelocitaCorrente() > 1.0f) {
            tempoOk = Time.time;
        }
        //Debug.Log(transform.up.y);
        Debug.Log(transform.forward);
        
        /*Se è trascorso troppo tempo allora mettiamo la macchina in posizione giusta*/
        if (Time.time > tempoOk + 3.0f) {
           this.transform.position += Vector3.up;
           this.transform.rotation = Quaternion.LookRotation(this.transform.forward);
        }
    }
}
