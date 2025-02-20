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
        
        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();

            if (!isOwned)
            {
                m_camera.gameObject.SetActive(false);
            }
        }

        private void Update()
        {
            if (isOwned)
            {
                UpdateLocalPlayerLocomotion();
            }
        }
        
        [Client]
        private void UpdateLocalPlayerLocomotion()
        {
            //If we own this character, then we update the locomotion with the local player's input (bypassing any need for server/client communication)
            Vector2 direction = m_inputReader.Direction;
            Vector3 move = new Vector3(direction.x, 0, direction.y);
            transform.Translate(move * (Time.deltaTime * m_moveSpeed), Space.World);
            
            // if there is a move input rotate player when the player is moving
            if (!Equals(m_inputReader.Move, Vector2.zero))
            {
                float targetRotation;

                if (isOwned)
                {
                    targetRotation = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg +
                                      m_camera.transform.eulerAngles.y;
                }
                else
                {
                    targetRotation = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
                }

                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref m_rotationVelocity,
                    m_rotationSmoothTime);

                // rotate to face input direction relative to camera position
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }
        }
    }
}