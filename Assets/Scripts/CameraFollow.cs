using System;
using UnityEngine;
using Photon.Pun;

public class CameraFollow : MonoBehaviour
{
    public Vector3 offset;
    private Transform target;
    public float translateSpeed;
    public float rotationSpeed;

    private void FixedUpdate()
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
                        this.target = player.transform;
                        return;
                    }
                }

                return;
            }
            else
            {
                if (GameObject.FindGameObjectWithTag("Player") == null) return;
                target = GameObject.FindGameObjectWithTag("Player").transform;
            }
            
        }
        HandleTranslation();
        HandleRotation();
    }
   
    private void HandleTranslation()
    {
        var targetPosition = target.TransformPoint(offset);
        transform.position = Vector3.Lerp(transform.position, targetPosition, translateSpeed * Time.deltaTime);
    }
    private void HandleRotation()
    {
        var direction = target.position - transform.position;
        var rotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }
}