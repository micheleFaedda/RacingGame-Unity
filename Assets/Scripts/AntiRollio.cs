using UnityEngine;

public class AntiRollio : MonoBehaviour {

    public float antiRollio = 5000.0f;
    public WheelCollider colliderSinistroAnteriore;
    public WheelCollider colliderSinistroPosteriore;
    public WheelCollider colliderDestroAnteriore;
    public WheelCollider colliderDestroPosteriore;
    public GameObject centroMassa;
    Rigidbody rb;

    void Start() {

        rb = this.GetComponent<Rigidbody>(); 
        rb.centerOfMass = centroMassa.transform.localPosition;//Se non usiamo questo script questo si puo portare in car controller
    }

    void GroundWheels(WheelCollider colliderSX, WheelCollider colliderDX) {

        WheelHit hit;
        float lavoroSinistro = 1.0f;
        float lavoroDestro = 1.0f;

        /* Per ogni ruota scopriamo se è a contatto con il terreno.
        Se è a contatto con il terreno, allora otteniamo il lavoro in base al punto in cui si ha il contatto 
        e in proporzione con la lunghezza della sospensione*/

        bool flagContattoSinistro = colliderSX.GetGroundHit(out hit);
        if (flagContattoSinistro) {
            lavoroSinistro = (-colliderSX.transform.InverseTransformPoint(hit.point).y - colliderSX.radius) / colliderSX.suspensionDistance;
        }

        bool flagContattoDestro = colliderDX.GetGroundHit(out hit);
        if (flagContattoDestro) {
            lavoroDestro = (-colliderDX.transform.InverseTransformPoint(hit.point).y - colliderDX.radius) / colliderDX.suspensionDistance;
        }

        float forzaAntiRollio = (lavoroSinistro - lavoroDestro) * antiRollio;

        /* Applichiamo una forza verso l'alto e una forza "anti-rollio" verso il basso sulle ruote per non far ribaltare la macchina */

        if (flagContattoSinistro) {
            rb.AddForceAtPosition(colliderSX.transform.up * -forzaAntiRollio, colliderSX.transform.position);
        }

        if (flagContattoDestro) {
            rb.AddForceAtPosition(colliderDX.transform.up * forzaAntiRollio, colliderDX.transform.position);
        }
    }

    void FixedUpdate() {
        GroundWheels(colliderSinistroAnteriore, colliderDestroAnteriore);
        GroundWheels(colliderSinistroPosteriore, colliderDestroPosteriore);
    }
}
