using UnityEngine;
using System.Collections;


public class GrabWorld : MonoBehaviour {

    bool isMoving = false;

    Vector3 controllerMovementInit;
    Vector3 cameraPositionInit;
    Vector3 cameraDelta = Vector3.zero;
    Vector3 controllerMovementDelta;

    float cameraRotInit;
    float cameraRot;
    float controllerRotDelta;
    float controllerRotInit;
    float cameraRotDelta;
    bool gripPressed = false;

    SteamVR_TrackedObject trackedController;
   
    public GameObject cameraRig;
    
    void Awake()
    {
        trackedController = GetComponent<SteamVR_TrackedObject>();
    }
   
    // Update is called once per frame
    void Update ()
    {

       
        var device = SteamVR_Controller.Input((int)trackedController.index);

        //Grip
        
        if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Grip))
        {
            gripPressed = true;
          
        }
        else if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Grip))
        {
            gripPressed = false;
         
        }
        
       
        if (gripPressed)
      
        {

            if (!isMoving)
            {
                //get initial Controller pose when button pressed
                controllerMovementInit = transform.position;
                controllerRotInit = transform.rotation.eulerAngles.y;

                //get initial Camera Rig pose when button pressed
                cameraPositionInit = cameraRig.transform.position;
                cameraRotInit = cameraRig.transform.rotation.eulerAngles.y;

                //reset deltas
                cameraRotDelta = 0;
                cameraDelta = Vector3.zero;

                isMoving = true;
            }

            //Calculate controller movement minus the movement of parent camera rig
            controllerMovementDelta.x = transform.position.x - controllerMovementInit.x - cameraDelta.x;
            controllerMovementDelta.z = transform.position.z - controllerMovementInit.z - cameraDelta.z;
            controllerRotDelta = transform.rotation.eulerAngles.y - controllerRotInit - cameraRotDelta;

            //update CameraRig
            cameraRig.transform.position = cameraPositionInit - controllerMovementDelta;
            cameraRig.transform.rotation = Quaternion.Euler(0f, cameraRotInit - controllerRotDelta, 0f);

            //update Deltas
            cameraDelta = cameraRig.transform.position - cameraPositionInit;
            cameraRotDelta = cameraRig.transform.rotation.eulerAngles.y - cameraRotInit;


        }
        else
        {
            isMoving = false;
          
        }

    }
}
