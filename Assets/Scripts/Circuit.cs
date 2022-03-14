﻿using UnityEngine;

public class Circuit : MonoBehaviour
{
    public Transform[] waypoints;

    private void Start()
    {
        waypoints = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            waypoints[i] = transform.GetChild(i);
        }
    }
    
    //Da qui in poi questa roba si puo anche togliliere e cercare di integrare questo script in AI controller
    private void OnDrawGizmos()
    {

        DrawGizmos(false);
    }

    private void OnDrawGizmosSelected()
    {

        DrawGizmos(true);
    }

    void DrawGizmos(bool selected)
    {

        if (selected == false) return;

        if (waypoints.Length > 1)
        {

            Vector3 prev = waypoints[0].position;
            for (int i = 1; i < waypoints.Length; ++i)
            {

                Vector3 next = waypoints[i].position;
                Gizmos.DrawLine(prev, next);
                prev = next;
            }
            Gizmos.DrawLine(prev, waypoints[0].position);
        }
    }

}