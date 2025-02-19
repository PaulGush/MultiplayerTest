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
    
    [SyncVar] private float m_speed;
    [SyncVar] private float m_motionSpeed;
    [SyncVar] private bool m_jump;
    [SyncVar] private bool m_grounded;
    
    private void Update()
    {
        if (isOwned)
        {
            CMD_LocalPlayerInputToServer();
            UpdateLocalAnimatorWithLocalInput();
        }
        else
        {
            RPC_ServerUpdateToClients();
        }
    }

    [Command]
    private void CMD_LocalPlayerInputToServer()
    {
        //If this character is owned by the local player, then we update the server with the local player's input
        
        m_speed = m_inputReader.Direction.magnitude;
        m_motionSpeed = m_inputReader.Direction.magnitude;
        m_jump = m_inputReader.IsJumpKeyPressed;
        m_grounded = true;
    }
    
    [ClientRpc]
    private void RPC_ServerUpdateToClients()
    {
        //If this character is not owned by the local player, then we update the clients with the whatever the server last stored as input from that player 
        
        m_animator.SetFloat(Speed, m_speed);
        m_animator.SetFloat(MotionSpeed, m_motionSpeed);
        m_animator.SetBool(Jump, m_jump);
        m_animator.SetBool(Grounded, m_grounded);
    }

    [Client]
    private void UpdateLocalAnimatorWithLocalInput()
    {
        m_animator.SetFloat(Speed, m_inputReader.Direction.magnitude);
        m_animator.SetFloat(MotionSpeed, m_inputReader.Direction.magnitude);
        m_animator.SetBool(Jump, m_inputReader.IsJumpKeyPressed);
        m_animator.SetBool(Grounded, true);
    }
}
