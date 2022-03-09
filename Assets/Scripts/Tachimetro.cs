using UnityEngine;
using UnityEngine.UI;

public class Tachimetro : MonoBehaviour
{
    public Rigidbody target;

    public float angoloMinimo; //Angolo della freccia quando la velocita è minima
    public float angoloMassimo; //Angolo della freccia quando la velocita è massima

    public Text testoVelocita;
    public RectTransform freccia;

    private float velocita = 0.0f;

    private void Update()
    {
        velocita = target.velocity.magnitude * 3.6f;

        testoVelocita.text = ((int) velocita) + "";

        //Modifichamo la rotazione sull'asse z
        freccia.localEulerAngles =
            new Vector3(0, 0, Mathf.Lerp(angoloMinimo, angoloMassimo, velocita / 260));
    }
}