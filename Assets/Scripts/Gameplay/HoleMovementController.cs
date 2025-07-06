using EdwinGameDev.EventSystem;
using EdwinGameDev.Input;
using UnityEngine;

namespace EdwinGameDev.Gameplay
{
    public class HoleMovementController : MonoBehaviour
    {
        [SerializeField] private PlayerSettings playerSettings;
        [SerializeField] private FloatingJoystick joystick;
        [SerializeField] private Rigidbody holeRigidbody;
        [SerializeField] private Hole hole;

        private float speed;
        private float turnSpeed;

        private bool CanMove { get; set; } = true;

        private void Setup()
        {
            speed = playerSettings.MoveSpeed;
            turnSpeed = playerSettings.TurnSpeed;
        }

        private void OnEnable()
        {
            hole.OnIncreaseHoleSize += IncreaseSpeed;
            GlobalEventDispatcher.AddSubscriber<Events.GameStarted>(OnGameStarted);
            GlobalEventDispatcher.AddSubscriber<Events.GameEnded>(OnGameEnded);
        }

        private void OnDisable()
        {
            hole.OnIncreaseHoleSize -= IncreaseSpeed;
            GlobalEventDispatcher.RemoveSubscriber<Events.GameStarted>(OnGameStarted);
            GlobalEventDispatcher.RemoveSubscriber<Events.GameEnded>(OnGameEnded);
        }

        private void Update()
        {
            if (!CanMove)
            {
                return;
            }

            MoveWithTouch();
        }

        private void OnGameStarted(Events.GameStarted _)
        {
            Setup();
            CanMove = true;
        }

        private void OnGameEnded(Events.GameEnded _)
        {
            CanMove = false;
        }

        private void MoveWithTouch()
        {
            Vector3 direction = new(joystick.Horizontal, 0, joystick.Vertical);

            if (direction.magnitude < 0.1f)
            {
                holeRigidbody.velocity = Vector3.zero;
                return;
            }

            holeRigidbody.velocity = direction.normalized * speed;

            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            holeRigidbody.rotation =
                Quaternion.Slerp(holeRigidbody.rotation, toRotation, turnSpeed * Time.fixedDeltaTime);

            GlobalEventDispatcher.Publish(new Events.HoleMoved());

            transform.hasChanged = false;
        }

        private void IncreaseSpeed()
        {
            speed += 1f;
        }
    }
}