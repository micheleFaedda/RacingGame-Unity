using UnityEngine;
using UnityEngine.UI;

public class TextController : MonoBehaviour {
    public Text playerText;
    
    public Transform target;
    CheckpointManager cpManager;

    int carRego;
    bool regoSet = false;

    void Start() {
        this.transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
        playerText = this.GetComponent<Text>();
    }

    void Update() {

        if (!regoSet) {
            carRego = Leaderboard.RegisterCar(playerText.text);
            regoSet = true;
            return;
        }

        this.transform.position = Camera.main.WorldToScreenPoint(target.position + Vector3.up * 1.7f);
        if (cpManager == null) {
            cpManager = target.GetComponent<CheckpointManager>();
        }

        Leaderboard.SetPosition(carRego, cpManager.giro, cpManager.checkPoint, cpManager.timeEntered);
        string position = Leaderboard.GetPosition(carRego);

        playerText.text = position + " " + cpManager.giro + " (" + cpManager.checkPoint + ")";
    }
}
