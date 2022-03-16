using UnityEngine;

public class CarAnimation : MonoBehaviour
{
    //Posizione iniziale e finale delle macchine nella scena di selezione
    [SerializeField] private Vector3 finalPosition;
    private Vector3 initialPosition;

    private void Awake()
    {
        //setto la posizione iniziale
        initialPosition = transform.position;
    }

    private void Update()
    {
        //interpolo tra i due punti in ingresso con valore interpolante  
        transform.position = Vector3.Lerp(transform.position, finalPosition, 0.1f);
    }

    private void OnDisable()
    {
        //faccio ritornare alla posizione iniziale
        transform.position = initialPosition;
    }
}