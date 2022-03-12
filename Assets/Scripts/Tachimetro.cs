using Photon.Pun;
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
    
    public PhotonView view;

    private void Update()
    {
        
        if (target == null)
        {
            if (PhotonNetwork.IsConnected)
            {
                GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
                foreach (GameObject player in players)
                {
                    if(PhotonView.Get(player).GetComponent<PlayerController>().view == null) return;
                    if (PhotonView.Get(player).GetComponent<PlayerController>().view.IsMine)
                    {
                        this.target = player.GetComponent<Rigidbody>();
                        return;
                    }
                }

                return;
            }
            else
            {
                if (GameObject.FindGameObjectWithTag("Player") == null) return;
                target = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
            }
            
        }
        
        
        velocita = target.velocity.magnitude * 3.6f;
        testoVelocita.text = ((int) velocita) + "";

        //Modifichamo la rotazione sull'asse z
        freccia.localEulerAngles =
            new Vector3(0, 0, Mathf.Lerp(angoloMinimo, angoloMassimo, velocita / 260));
    }
}