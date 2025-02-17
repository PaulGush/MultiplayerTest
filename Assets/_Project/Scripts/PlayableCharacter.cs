using Mirror;
using UnityEngine;

namespace _Project.Scripts
{
    public class PlayableCharacter : NetworkBehaviour
    {
        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();
            
            Debug.Log("I am the local player");
        }
    }
}
