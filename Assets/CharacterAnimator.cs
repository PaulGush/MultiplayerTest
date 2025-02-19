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
        if (!isOwned)
        {
            LocalPlayerUpdateToServer();
        }
        else
        {
            ServerUpdateToClients();
        }
    }

    [Command]
    private void LocalPlayerUpdateToServer()
    {
        m_speed = m_inputReader.Direction.magnitude;
        m_motionSpeed = m_inputReader.Direction.magnitude;
        m_jump = m_inputReader.IsJumpKeyPressed;
        m_grounded = true;
        
        m_animator.SetFloat(Speed, m_inputReader.Direction.magnitude);
        m_animator.SetFloat(MotionSpeed, m_inputReader.Direction.magnitude);
        m_animator.SetBool(Jump, m_inputReader.IsJumpKeyPressed);
        m_animator.SetBool(Grounded, true);
    }
    
    [ClientRpc]
    private void ServerUpdateToClients()
    {
        m_speed = m_inputReader.Direction.magnitude;
        m_motionSpeed = m_inputReader.Direction.magnitude;
        m_jump = m_inputReader.IsJumpKeyPressed;
        m_grounded = true;
    }
}
