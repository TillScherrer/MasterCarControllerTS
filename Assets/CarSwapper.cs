using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CarSwapper : MonoBehaviour
{
    [SerializeField]
    GameObject[] cars;
    [SerializeField]
    Rigidbody[] rbs;

    [SerializeField]
    GameObject landingGhost;
    [SerializeField]
    CinemachineVirtualCamera cinemachine;
    [SerializeField]
    CameraFollow camFollowSkript;

    [Space(20)]
    [SerializeField]
    bool measureSpeedAndLanding = false;
    [SerializeField]
    TextMeshProUGUI speedText;
    [SerializeField]
    TextMeshProUGUI landingText;




    Car[] carsCarComponent = null;
    //Rigidbody[] rbs = null;

    int indexOfCurrentCar = 0;
    int indexOfCurrentGearMode = 0;


    float maxSpeedOfCurrentCar = 0;
    float startSpeed = 0;
    float timer = 0;
    float endSpeed = 0;
    bool isCurrentlyCounting = false;





    // Start is called before the first frame update
    void Start()
    {
        carsCarComponent = new Car[cars.Length];
        for(int i = 0; i< cars.Length; i++)
        {
            //Debug.Log("iteratio: " + i);
            //rbs[i] = cars[i].GetComponent<Rigidbody>();
            carsCarComponent[i] = cars[i].GetComponent<Car>();
            maxSpeedOfCurrentCar = carsCarComponent[i].SpeedupCurve.topSpeed;
        }
    }

    // Update is called once per frame
    void Update()
    {
        int carIndexToDoChange = -1;
        if (Input.GetKeyDown(KeyCode.Alpha1)) carIndexToDoChange = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2)) carIndexToDoChange = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3)) carIndexToDoChange = 2;
        if (Input.GetKeyDown(KeyCode.Alpha4)) carIndexToDoChange = 3;
        if (Input.GetKeyDown(KeyCode.Alpha5)) carIndexToDoChange = 4;
        if (Input.GetKeyDown(KeyCode.Alpha6)) carIndexToDoChange = 5;

        if (carIndexToDoChange != -1 && carIndexToDoChange<=cars.Length && carIndexToDoChange != indexOfCurrentCar)
        {
           

            rbs[carIndexToDoChange].velocity = rbs[indexOfCurrentCar].velocity;
            rbs[carIndexToDoChange].angularVelocity = rbs[indexOfCurrentCar].angularVelocity;
            cars[carIndexToDoChange].transform.position = cars[indexOfCurrentCar].transform.position;
            cars[carIndexToDoChange].transform.rotation = cars[indexOfCurrentCar].transform.rotation;


            cars[carIndexToDoChange].SetActive(true);
            carsCarComponent[carIndexToDoChange].GearShiftMode = indexOfCurrentGearMode == 0 ? GearShiftMode.Automatic : indexOfCurrentGearMode == 1 ? GearShiftMode.ManualOneClick : GearShiftMode.ManualClickUpDown;
            cinemachine.LookAt = cars[carIndexToDoChange].transform;
            camFollowSkript.player = cars[carIndexToDoChange].transform;
            camFollowSkript.Rb = rbs[carIndexToDoChange];
            camFollowSkript.CustomGravity = cars[carIndexToDoChange].GetComponent<CustomGravityReciver>();
            maxSpeedOfCurrentCar = carsCarComponent[carIndexToDoChange].SpeedupCurve.topSpeed;

            cars[indexOfCurrentCar].SetActive(false);
            indexOfCurrentCar = carIndexToDoChange;
        }

        int gearmodeChangeIndex = -1;
        if (Input.GetKeyDown(KeyCode.Alpha7)) gearmodeChangeIndex = 0;
        if (Input.GetKeyDown(KeyCode.Alpha8)) gearmodeChangeIndex = 1;
        if (Input.GetKeyDown(KeyCode.Alpha9)) gearmodeChangeIndex = 2;
        if (gearmodeChangeIndex != -1) 
        {
            carsCarComponent[indexOfCurrentCar].GearShiftMode = gearmodeChangeIndex == 0 ? GearShiftMode.Automatic: gearmodeChangeIndex == 1 ? GearShiftMode.ManualOneClick: GearShiftMode.ManualClickUpDown ;
            indexOfCurrentGearMode = gearmodeChangeIndex;
        }

        //activate or deactivate landing ghost
        if (Input.GetKeyDown(KeyCode.Y)) landingGhost.SetActive(true);
        if (Input.GetKeyDown(KeyCode.X)) landingGhost.SetActive(false);







        if (!measureSpeedAndLanding) return;

        //meansre Speed and time
        if (Input.GetKeyDown(KeyCode.W))
        {
            isCurrentlyCounting = true;
            startSpeed = rbs[indexOfCurrentCar].velocity.magnitude;
            timer = 0;
        }
        


        if (isCurrentlyCounting)
        {
            timer += Time.deltaTime;
            speedText.text = "time: " + timer;
            float currentCarSpeed = rbs[indexOfCurrentCar].velocity.magnitude;

            if (Input.GetKeyUp(KeyCode.W) || currentCarSpeed>= maxSpeedOfCurrentCar)
            {

                //HIER TEXTAUSGABE
                speedText.text = "The car took " + timer + "s to reach " + currentCarSpeed + "m/s from " + startSpeed + "ms";
                isCurrentlyCounting = false;
            }
        }

        landingText.text = "Previous landing upward to ground-normal offsetAngle is: " + carsCarComponent[indexOfCurrentCar].previousOffsetAngleFromGroundOnLanding + "�";






    }






}