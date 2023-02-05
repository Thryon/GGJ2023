using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;
using UnityEngine.Serialization;

namespace KinematicCharacterController
{
    public class Player : MonoBehaviour
    {
        public CharacterController Character;
        public CharacterCamera CharacterCamera;
        public WaterReservoir WaterReservoir;
        public Inventory Inventory;
        public PlayerUpgrader PlayerUpgrader;

        [SerializeField] private float fireRate = 100f;

        private const string MouseXInput = "Mouse X";
        private const string MouseYInput = "Mouse Y";
        private const string MouseScrollInput = "Mouse ScrollWheel";
        private const string HorizontalInput = "Horizontal";
        private const string VerticalInput = "Vertical";

        private bool inWaterZone;
        private WaterSource currentWaterSource;
        
        public bool InWaterZone => inWaterZone;

        public WaterSource CurrentWaterSource => currentWaterSource;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;

            // Tell camera to follow transform
            CharacterCamera.SetFollowTransform(Character.CameraFollowPoint);

            // Ignore the character's collider(s) for camera obstruction checks
            CharacterCamera.IgnoredColliders.Clear();
            CharacterCamera.IgnoredColliders.AddRange(Character.GetComponentsInChildren<Collider>());
            
            GlobalEvents.Instance.RegisterEvent(GlobalEventEnum.OnUpgradeMenuOpened, OnUpgradeMenuOpened);
            GlobalEvents.Instance.RegisterEvent(GlobalEventEnum.OnUpgradeMenuClosed, OnUpgradeMenuClosed);
        }

        private void OnUpgradeMenuClosed()
        {
            isUpgradeMenuOpen = false;
        }

        private void OnUpgradeMenuOpened()
        {
            isUpgradeMenuOpen = true;
        }

        private void OnDestroy()
        {
        }
        

        private float waterRefillTimer = 0f;
        private bool isRefilling = false;
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if(!isUpgradeMenuOpen)
                    Cursor.lockState = CursorLockMode.Locked;
            }

            if (inWaterZone)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    isRefilling = true;
                    waterRefillTimer = 0f;
                }

                if (Input.GetKeyUp(KeyCode.E))
                {
                    isRefilling = false;
                }
            }
            else
            {
                isRefilling = false;
            }
        

            if(isRefilling)
            {
                waterRefillTimer += Time.deltaTime;
                float fireRateInterval = fireRate * 100f;
                while (waterRefillTimer >= fireRateInterval)
                {
                    Debug.Log(CurrentWaterSource.currentAmount);
                    if (CurrentWaterSource.currentAmount > 0 && !WaterReservoir.IsFull())
                    {
                        CurrentWaterSource.UseWater(1);
                        if (WaterReservoir.RefillWater(1) > 0)
                        {
                            isRefilling = false;
                            break;
                        }

                        waterRefillTimer -= fireRateInterval;
                    }
                    else
                    {
                        isRefilling = false;
                        break;
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if(isUpgradeMenuOpen)
                    CloseUpgradeMenu();
                else
                    OpenUpgradeMenu();
            }
            
            HandleCharacterInput();
        }

        private bool isUpgradeMenuOpen = false;
        void OpenUpgradeMenu()
        {
            Time.timeScale = 0.2f;
            GlobalEvents.Instance.SendEvent(GlobalEventEnum.OpenUpgradeMenu);
            Cursor.lockState = CursorLockMode.None;
        }

        void CloseUpgradeMenu()
        {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            GlobalEvents.Instance.SendEvent(GlobalEventEnum.CloseUpgradeMenu);
        }

        private void FixedUpdate()
        {
            if (Input.GetMouseButton(1))
            {
                WaterReservoir.RefillWater(5);
            }
        }

        private void LateUpdate()
        {
            // Handle rotating the camera along with physics movers
            if (CharacterCamera.RotateWithPhysicsMover && Character.Motor.AttachedRigidbody != null)
            {
                CharacterCamera.PlanarDirection = Character.Motor.AttachedRigidbody.GetComponent<PhysicsMover>().RotationDeltaFromInterpolation * CharacterCamera.PlanarDirection;
                CharacterCamera.PlanarDirection = Vector3.ProjectOnPlane(CharacterCamera.PlanarDirection, Character.Motor.CharacterUp).normalized;
            }

            HandleCameraInput();
        }

        private void HandleCameraInput()
        {
            // Create the look input vector for the camera
            float mouseLookAxisUp = Input.GetAxisRaw(MouseYInput);
            float mouseLookAxisRight = Input.GetAxisRaw(MouseXInput);
            Vector3 lookInputVector = new Vector3(mouseLookAxisRight, mouseLookAxisUp, 0f);

            // Prevent moving the camera while the cursor isn't locked
            if (Cursor.lockState != CursorLockMode.Locked)
            {
                lookInputVector = Vector3.zero;
            }

            // Input for zooming the camera (disabled in WebGL because it can cause problems)
            float scrollInput = -Input.GetAxis(MouseScrollInput);
#if UNITY_WEBGL
        scrollInput = 0f;
#endif

            // Apply inputs to the camera
            CharacterCamera.UpdateWithInput(Time.deltaTime, scrollInput, lookInputVector);

            // Handle toggling zoom level
            if (Input.GetMouseButtonDown(1))
            {
                CharacterCamera.TargetDistance = (CharacterCamera.TargetDistance == 0f) ? CharacterCamera.DefaultDistance : 0f;
            }
        }

        private void HandleCharacterInput()
        {
            PlayerCharacterInputs characterInputs = new PlayerCharacterInputs();

            // Build the CharacterInputs struct
            characterInputs.MoveAxisForward = Input.GetAxisRaw(VerticalInput);
            characterInputs.MoveAxisRight = Input.GetAxisRaw(HorizontalInput);
            characterInputs.CameraRotation = CharacterCamera.Transform.rotation;
            characterInputs.JumpDown = Input.GetKeyDown(KeyCode.Space);
            characterInputs.CrouchDown = Input.GetKeyDown(KeyCode.C);
            characterInputs.CrouchUp = Input.GetKeyUp(KeyCode.C);

            // Apply inputs to character
            Character.SetInputs(ref characterInputs);
        }



        public void EnterWaterSource(WaterSource waterSource)
        {
            currentWaterSource = waterSource;
            inWaterZone = true;
        }

        public void LeaveWaterSource(WaterSource waterSource)
        {
            currentWaterSource = null;
            inWaterZone = false;
        }

        public void SetFireRate(float f)
        {
            fireRate = f;
        }
    }
}