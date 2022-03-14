using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChosenCar : MonoBehaviour
{
    [SerializeField] private GameObject[] carModel;
    private void Awake()
    {
        ChooseCarModel(SaveManager.instance.currentCar);
    }

    private void ChooseCarModel(int i)
    {
        Instantiate(carModel[i], transform.position, Quaternion.identity, transform);
    }
}
