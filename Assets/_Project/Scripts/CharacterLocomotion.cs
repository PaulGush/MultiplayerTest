using _Project.Scripts.Input;
using Mirror;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

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

        [SerializeField] private Transform m_cameraFollowTarget; 
        [SerializeField] private CinemachineCamera m_camera;
        
        private CinemachineOrbitalFollow m_orbitalFollow;

        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();

            Cursor.lockState = CursorLockMode.Locked;

            if (isOwned)
            {   
                CinemachineCameraSetup();
            }
        }

        private void CinemachineCameraSetup()
        {
            m_camera = new GameObject().AddComponent<CinemachineCamera>();
            m_camera.name = "Cinemachine Camera";
                
            m_camera.Follow = m_cameraFollowTarget;
            m_camera.LookAt = m_cameraFollowTarget;

            m_camera.Lens.FieldOfView = 60;

            m_orbitalFollow = m_camera.AddComponent<CinemachineOrbitalFollow>();
            
            m_orbitalFollow.HorizontalAxis.Recentering.Enabled = true;
            m_orbitalFollow.VerticalAxis.Recentering.Enabled = true;
            m_orbitalFollow.VerticalAxis.Range = new Vector2(-20, 60);
            m_orbitalFollow.Radius = 4;
                
            m_camera.AddComponent<CinemachineRotationComposer>();
            m_camera.AddComponent<CinemachineInputAxisController>();
        }

        private void Update()
        {
            if (isOwned)
            {
                HandleLocalPlayerLocomotionInput();
            }
        }

        [Client]
        private void HandleLocalPlayerLocomotionInput()
        {
            //If we own this character, then we update the locomotion with the local player's input (bypassing any need for server/client communication)
            
            Vector2 direction = m_inputReader.Direction;
            Vector3 move = m_camera.transform.forward * direction.y + m_camera.transform.right * direction.x;
            move.y = 0; // Ensure the movement is only on the XZ plane
            transform.Translate(move * (Time.deltaTime * m_moveSpeed), Space.World);
            
            if (move.magnitude > 0)
            {
                Quaternion toRotation = Quaternion.LookRotation(move, Vector3.up);
                transform.rotation = Quaternion.Euler(Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, toRotation.eulerAngles.y, ref m_rotationVelocity, m_rotationSmoothTime));
            }
        }
        
        
    }
}