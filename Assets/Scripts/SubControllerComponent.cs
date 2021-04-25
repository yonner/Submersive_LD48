using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public enum States { underWater, onSurface}

public enum FocusStates { Nothing, Close, Good, Far}

public enum AngleState { Nothing, Poor, Average, Good}

[Serializable]
public class ShotScore
{
    public FocusStates focusState;
    public AngleState angleState;

    public int score;
    void CalculateScore(int fishIndex)
    {

    }
}

public class SubControllerComponent : MonoBehaviour
{
    float diveSpeed;

    float turnRotation;

    public States state = States.onSurface;

    bool bobDown = true;

    float currentBobTimer;

    bool ascending = false;

    Rigidbody m_Rigidbody;

    public Camera shootCamera;

    public Light torch;
    
    [Header("Variables")]
    public float BobTimer = .5f;

    public float DiveSpeed = .002f;

    public float BobDiveSpeed = .002f;

    public float acceleration = .3f;

    private bool fishIsInRange = false;
    private GameObject fishTarget;
    private FocusStates fishTargetFocusState = FocusStates.Nothing;
    private AngleState fishTargetAngleState = AngleState.Nothing;

    private float distFromCentre;

    private int slotIndex = 0;

    private float shotCooldown = 0f;

    private float maxDiveSpeed = 0.06f;

    [Header("Screen shots")]
    //public List<Texture2D> ScreenShots;

    [Header("Screen shots Scores")]
    public ShotScore[] ShotScores;

    public GameObject[] shotSlots;

    // Start is called before the first frame update
    void Start()
    {
        currentBobTimer = BobTimer;

        m_Rigidbody = GetComponent<Rigidbody>();

        //ScreenShots = new List<Texture2D>();

        //ShotScores = new List<ShotScoreS>();

        ShotScores = new ShotScore[3];

    }

    // Update is called once per frame
    void Update()
    {
        if (shotCooldown > 0)
        {
            shotCooldown -= Time.deltaTime;
        }

        if (shotCooldown < 0)
        {
            shotCooldown = 0f;
        }

        if (transform.position.y < -0.1)
        {
            state = States.underWater;
        }

        if (fishIsInRange)
        {
            float dist = Vector3.Distance(fishTarget.transform.position, transform.position);

            if (dist > 14f)
            {
                fishTargetFocusState = FocusStates.Nothing;
                fishTargetAngleState = AngleState.Nothing;
                fishTarget = null;
                fishIsInRange = false;
            } else {

                if (dist > 10 && dist < 12.2)
                {
                    fishTargetFocusState = FocusStates.Good;
                }

                if (dist < 10)
                {
                    fishTargetFocusState = FocusStates.Close;
                }

                if (dist > 12.2)
                {
                    fishTargetFocusState = FocusStates.Far;
                }

                Vector3 toOther = fishTarget.transform.forward;

                // So a value < -70 and > -110 will result in a good shot
                // > 70 and < 110
                // etc..
                var ang = Vector3.Angle(toOther, transform.forward);

                if (ang > 60 && ang < 120)
                {
                    Debug.Log("Good angle ");

                    fishTargetAngleState = AngleState.Good;
                }

                if (ang > 120 && ang < 130)
                {
                    Debug.Log("Average angle ");

                    fishTargetAngleState = AngleState.Average;
                }

                if (ang > 40 && ang < 60)
                {
                    Debug.Log("Average angle ");

                    fishTargetAngleState = AngleState.Average;
                }

                if (ang > 0 && ang < 40)
                {
                    Debug.Log("poor angle ");

                    fishTargetAngleState = AngleState.Poor;
                }

                if (ang > 130 && ang < 180)
                {
                    Debug.Log("poor angle ");

                    fishTargetAngleState = AngleState.Poor;
                }
            }

        }

        // calculate distance from 0 (middle point)  ignore the down 
        distFromCentre = Vector3.Distance(Vector3.zero, new Vector3(transform.position.x, 0, transform.position.z) /* transform.position*/);

        //Debug.Log($"Current dist = {distFromCentre}");
    }

    public void Reset()
    {
        transform.position = Vector3.zero;
        acceleration = 0;
        turnRotation = 0;
        state = States.onSurface;
    }

    public float GetDistFromCenter()
    {
        return distFromCentre;
    }

    public Vector2 Coords()
    {
        return new Vector2(this.transform.position.x, this.transform.position.z);
    }

    public FocusStates CurrentFishFocusState()
    {
        return fishTargetFocusState;
    }

    public AngleState CurrentFishAngleState()
    {
        return fishTargetAngleState;
    }

    public bool IsFishInFOV()
    {
        return fishTarget != null ? true : false;
    }    

    public bool IsUsingCamera()
    {
        return shootCamera.gameObject.activeSelf;
    }

    public bool IsOnSurface()
    {
        return state == States.onSurface ? true : false;
    }

    private void FixedUpdate()
    {
        if (state == States.onSurface)
        {
            if (bobDown)
            {
                Vector2 myVectorD = Vector2.zero;
                myVectorD.y -= BobDiveSpeed;

                transform.Translate(myVectorD);

                currentBobTimer -= Time.deltaTime;

                if (currentBobTimer < 0)
                {
                    currentBobTimer = BobTimer;
                    bobDown = false;
                }
            }
            
            if (!bobDown)
            {
                Vector2 myVectorU = Vector2.zero;
                myVectorU.y += BobDiveSpeed;

                transform.Translate(myVectorU);

                currentBobTimer -= Time.deltaTime;

                if (currentBobTimer < 0)
                {
                    currentBobTimer = BobTimer;
                    bobDown = true;
                }
            }
        }

        if (state == States.underWater)
        {
            if (ascending)
            {
                if (transform.position.y > -0.12f)
                {
                    ascending = false;

                    diveSpeed = 0;

                    transform.position = new Vector3(transform.position.x, 0, transform.position.z);

                    state = States.onSurface;

                }
            }
        }

        Vector2 myVector = Vector2.zero;

        myVector.y += diveSpeed;

        transform.Translate(myVector);


        transform.position -= transform.forward * Time.deltaTime * acceleration;

        transform.Rotate(Vector3.up, turnRotation);
    }

    public void OnTorch(InputAction.CallbackContext context)
    {

        if (context.ReadValueAsButton())
        {
            torch.gameObject.SetActive(true);
        }
        else
        {
            torch.gameObject.SetActive(false);
        }

    }

    public void OnFullStop(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton())
        {
            //m_Rigidbody.velocity = Vector3.zero;
            acceleration = 0;
            turnRotation = 0;

        }

    }

    public void OnFire(InputAction.CallbackContext context)
    {

        if (context.ReadValueAsButton())
        {
            shootCamera.gameObject.SetActive(true);
        }
        else
        {
            shootCamera.gameObject.SetActive(false);
        }

    }

    public void OnMove(InputAction.CallbackContext context)
    {
       
        Vector2 inputVector = context.ReadValue<Vector2>();

        if (inputVector.x < -.7)
        {
            //Debug.Log($"yes!! OnMoveHorizontal > y");

            if (turnRotation > -1.3)
            {
                turnRotation += -.3f;
            }
        }

        if (inputVector.x > .7)
        {
            //Debug.Log($"yes!! OnMoveHorizontal < y");

            if (turnRotation < 1.3)
            {
                turnRotation += .3f;
            }
        }

        if (inputVector.y > .7)
        {

            //m_Rigidbody.velocity = -transform.forward * acceleration;
            acceleration += .2f;
        }

        if (inputVector.y < -.7)
        {

            //m_Rigidbody.velocity = transform.forward * acceleration;
            acceleration -= .2f;
        }

        
    }

    public void OnMoveVertical(InputAction.CallbackContext context)
    {
        Debug.Log($"yes!! ");

        Vector2 inputVector = context.ReadValue<Vector2>();

        //Vector2 inputVector = Input.GetAxis("Vertical");

        Debug.Log($"yes!! ${inputVector}");

        // need to determine if pressing betten up/down & left/right?


        if ((inputVector.y < -.7) && (transform.position.y > -198))
        {
            ascending = false;

            if (diveSpeed > maxDiveSpeed * -1)
            {
                diveSpeed -= DiveSpeed;
            }
        }

        if (state == States.underWater)
        {

            Debug.Log($" val {transform.position.y} i {inputVector.y}");

            if (inputVector.y > .7)
            {
                ascending = true;

                if (diveSpeed < maxDiveSpeed)
                {
                    diveSpeed += DiveSpeed;
                }
            }

        }
    }

    public void OnTakePicture(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton())
        {

            if (IsUsingCamera() && shotCooldown == 0)
            {
                shotCooldown = .5f;

                RenderTexture screenShotRT = new RenderTexture(800, 600, 24);

                shootCamera.targetTexture = screenShotRT;

                Texture2D screenShot = new Texture2D(800, 600, TextureFormat.RGB24, false);

                shootCamera.Render();

                RenderTexture.active = screenShotRT;

                screenShot.ReadPixels(new Rect(0, 0, 800, 600), 0, 0);

                shootCamera.targetTexture = null;

                RenderTexture.active = null;

                Destroy(screenShotRT);

                screenShot.Apply();

                shotSlots[slotIndex].GetComponent<RawImage>().texture = screenShot;

                ShotScore score = new ShotScore() {
                    angleState = fishTargetAngleState,
                    focusState = fishTargetFocusState
                };

                ShotScores[slotIndex] = score;

                slotIndex++;
                if (slotIndex > 2)
                {
                    slotIndex = 0;
                }

            }
        }
    }

    /// <summary>
    /// Collision detection with camera cone
    /// </summary>
    /// <param name="other"></param>

    private void OnTriggerEnter(Collider other)
    {

        fishIsInRange = true;
        fishTarget = other.gameObject;

        Debug.Log("Trigger has been entered. ");
    }

    public void OnQuitGame(InputAction.CallbackContext context)
    {

        Application.Quit();
    }

    private void OnTriggerExit(Collider other)
    {
        fishIsInRange = false;
        fishTarget = null;

        fishTargetFocusState = FocusStates.Nothing;
        fishTargetAngleState = AngleState.Nothing;

        Debug.Log("Trigger has been exitted. ");
    }
}
