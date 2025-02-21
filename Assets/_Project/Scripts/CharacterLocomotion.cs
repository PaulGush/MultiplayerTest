using System;
using _Project.Scripts.Input;
using Mirror;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts
{
    public class CharacterLocomotion : NetworkBehaviour
    {
        [SerializeField] private InputReader m_inputReader;
        [SerializeField] private float m_moveSpeed = 5f;
        [SerializeField] private float m_rotationVelocity;
        
        [Tooltip("How fast the character turns to face movement direction")]
        [Range(0.0f, 0.3f)]
        [SerializeField] private float m_rotationSmoothTime = 0.12f;

        [SerializeField] private CinemachineCamera m_camera;
        [SerializeField] private Transform m_cameraTarget;
        
        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();

            m_camera = transform.GetComponentInChildren<CinemachineCamera>();
            Cursor.lockState = CursorLockMode.Locked;
            
            if (!isOwned)
            {
                m_camera.gameObject.SetActive(false);
                m_cameraTarget.gameObject.SetActive(false);
            }
        }

        private void Update()
        {
            if (isOwned)
            {
                UpdateLocalPlayerLocomotion();
            }
        }

        private void LateUpdate()
        {
            if (isOwned)
            {
                UpdateLocalCameraRotation();
            }
        }

        [Client]
        private void UpdateLocalPlayerLocomotion()
        {
            //If we own this character, then we update the locomotion with the local player's input (bypassing any need for server/client communication)
            
            Vector2 direction = m_inputReader.Direction;
            Vector3 move = m_camera.transform.forward * direction.y + m_camera.transform.right * direction.x;
            move.y = 0; // Ensure the movement is only on the XZ plane
            transform.Translate(move * (Time.deltaTime * m_moveSpeed), Space.World);
        }
        
        [Client]
        private void UpdateLocalCameraRotation()
        {
            Vector2 direction = m_inputReader.LookDirection;
            
            //Use input to rotate the camera
            m_cameraTarget.rotation *= Quaternion.Euler(Vector3.up * (direction.x * m_rotationSmoothTime) + Vector3.right * (-direction.y * m_rotationSmoothTime));
        }
    }
}