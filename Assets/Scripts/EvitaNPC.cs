using UnityEngine;


/*
avoidPath ->
avoidTime -> tempo
wanderDistance -> minDistanza
avoidLength ->

otherCar -> 
otherCarLocalTarget ->
otherCarAngle ->

OnCollisionExit ->
OnCollisionStay ->
*/
public class EvitaNPC : MonoBehaviour {

    public float avoidPath = 0.0f;
    public float avoidTime = 0.0f;
    public float minDistanza = 4.0f;
    public float avoidLength = 1.0f;

    private void OnCollisionExit(Collision collision) {
        if (collision.gameObject.tag != "npc") return;
        avoidTime = 0.0f;
        Debug.Log("Collisione");
    }

    private void OnCollisionStay(Collision collision) {

        if (collision.gameObject.tag != "npc") return;

        Rigidbody otherCar = collision.rigidbody;
        avoidTime = Time.time + avoidLength;

        Vector3 otherCarLocalTarget = transform.InverseTransformPoint(otherCar.gameObject.transform.position);
        float otherCarAngle = Mathf.Atan2(otherCarLocalTarget.x, otherCarLocalTarget.z);
        avoidPath = minDistanza * -Mathf.Sign(otherCarAngle);
    }
}
