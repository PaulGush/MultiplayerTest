using System;
using _Project.Scripts.Input;
using Mirror;
using UnityEngine;

namespace _Project.Scripts
{
    public class CharacterLocomotion : NetworkBehaviour
    {
        [SerializeField] private InputReader m_inputReader;
        [SerializeField] private float m_moveSpeed = 5f;
        
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
        }
    }
}