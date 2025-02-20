using _Project.Scripts.Input;
using Mirror;
using UnityEngine;

public class CharacterAnimator : NetworkBehaviour
{
    [SerializeField] private Animator m_animator;
    [SerializeField] private InputReader m_inputReader;
    
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int MotionSpeed = Animator.StringToHash("MotionSpeed");
    private static readonly int Jump = Animator.StringToHash("Jump");
    private static readonly int Grounded = Animator.StringToHash("Grounded");
    
    private void Update()
    {
        if (isOwned)
        {
            UpdateLocalAnimatorWithLocalInput();
        }
    }

    [Client]
    private void UpdateLocalAnimatorWithLocalInput()
    {
        //If we own this character, then we update the animator with the local player's input (bypassing any need for server/client communication)
        
        m_animator.SetFloat(Speed, m_inputReader.Direction.magnitude * 6, 0.1f, Time.deltaTime);
        m_animator.SetFloat(MotionSpeed, m_inputReader.Direction.magnitude * 3, 0.1f, Time.deltaTime);
        m_animator.SetBool(Jump, m_inputReader.IsJumpKeyPressed);
        m_animator.SetBool(Grounded, true);
    }
}
