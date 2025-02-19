using _Project.Scripts.Input;
using Mirror;
using UnityEngine;

namespace _Project.Scripts
{
    public class PlayableCharacter : NetworkBehaviour
    {
        [SerializeField] private InputReader m_inputReader;
        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();
            
            if (this.isOwned)
            {
                m_inputReader.EnablePlayerActions();
                EventSubscriptions();
            }
        }

        [Client]
        private void EventSubscriptions()
        {
            m_inputReader.Move += OnMove;
            m_inputReader.Jump += OnJump;
        }
        
        [Client]
        private void OnJump(bool arg0)
        {
            
        }
        
        [Client]
        private void OnMove(Vector2 arg0)
        {
            
        }
    }
}
