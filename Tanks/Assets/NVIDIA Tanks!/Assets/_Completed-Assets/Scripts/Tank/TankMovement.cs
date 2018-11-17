using UnityEngine;

namespace Complete
{
    public class TankMovement : MonoBehaviour
    {
        public int m_PlayerNumber = 1;              // Used to identify which tank belongs to which player.  This is set by this tank's manager.
        public float m_Speed = 12f;                 // How fast the tank moves forward and back.
        public float m_TurnSpeed = 180f;            // How fast the tank turns in degrees per second.
        public AudioSource m_MovementAudio;         // Reference to the audio source used to play engine sounds. NB: different to the shooting audio source.
        public AudioClip m_EngineIdling;            // Audio to play when the tank isn't moving.
        public AudioClip m_EngineDriving;           // Audio to play when the tank is moving.
		public float m_PitchRange = 0.2f;           // The amount by which the pitch of the engine noises can vary.

        
        private string m_MovementAxisName;          // The name of the input axis for moving forward and back.
        private string m_TurnAxisName;              // The name of the input axis for turning.
        private Rigidbody m_Rigidbody;              // Reference used to move the tank.
        private float m_MovementInputValue;         // The current value of the movement input.
        private float m_TurnInputValue;             // The current value of the turn input.
        private float m_OriginalPitch;              // The pitch of the audio source at the start of the scene.
        private ParticleSystem[] m_particleSystems; // References to all the particles systems used by the Tanks

        private float m_DistanceTraveled = 0.0f;   // Total distance traveled by tank

        private void Awake ()
        {
            m_Rigidbody = GetComponent<Rigidbody> ();
        }


        private void OnEnable ()
        {
            // When the tank is turned on, make sure it's not kinematic.
            m_Rigidbody.isKinematic = false;

            // Also reset the input values.
            m_MovementInputValue = 0f;
            m_TurnInputValue = 0f;

            // We grab all the Particle systems child of that Tank to be able to Stop/Play them on Deactivate/Activate
            // It is needed because we move the Tank when spawning it, and if the Particle System is playing while we do that
            // it "think" it move from (0,0,0) to the spawn point, creating a huge trail of smoke
            m_particleSystems = GetComponentsInChildren<ParticleSystem>();
            for (int i = 0; i < m_particleSystems.Length; ++i)
            {
                m_particleSystems[i].Play();
            }
        }


        private void OnDisable ()
        {
            // When the tank is turned off, set it to kinematic so it stops moving.
            m_Rigidbody.isKinematic = true;

            // Stop all particle system so it "reset" it's position to the actual one instead of thinking we moved when spawning
            for(int i = 0; i < m_particleSystems.Length; ++i)
            {
                m_particleSystems[i].Stop();
            }
        }


        private void Start ()
        {
            // The axes names are based on player number.
            m_MovementAxisName = "Vertical" + m_PlayerNumber;
            m_TurnAxisName = "Horizontal" + m_PlayerNumber;

            // Store the original pitch of the audio source.
            m_OriginalPitch = m_MovementAudio.pitch;
        }


        private void Update ()
        {
            // Store the value of both input axes.
            

            EngineAudio ();
        }


        private void EngineAudio ()
        {
            // If there is no input (the tank is stationary)...
          
            {
                // ... and if the audio source is currently playing the driving clip...
                
                // Otherwise if the tank is moving and if the idling clip is currently playing...
               
               
                    // ... change the clip to driving and play.
                   
        }


        private void FixedUpdate ()
        {
            // Adjust the rigidbodies position and orientation in FixedUpdate.
            
        }


        private void Move ()
        {
            // Create a vector in the direction the tank is facing with a magnitude based on the input, speed and the time between frames.
           
            // Add to total distance traveled


            // Apply this movement to the rigidbody's position.
            
        }


        private void Turn ()
        {
            // Determine the number of degrees to be turned based on the input, speed and time between frames.
           

            // Make this into a rotation in the y axis.
            

            // Apply this rotation to the rigidbody's rotation.

        }

    }
}