using UnityEngine;
using Utils;
using Time = UnityEngine.Time;

namespace Ships
{
    /// <summary>
    ///     Orbital Flight class is responsible for controlling the flight in orbit around Earth
    ///     It does so by leveraging Unity's RotateAround method. This choice allowed for rapid development but it has some
    ///     unfortunate consequences:
    ///     - speed is measured in angles
    ///     - the actual speed depends both on the angle speed and the distance from Earth. Luckily there is only one object
    ///     (the Moon) travelling at a different distance from Earth.
    ///     - It can be tricky to position objects manually.
    /// </summary>
    public class OrbitalFlight : MonoBehaviour
    {
        public float speed;
        protected AudioSource AudioSource;
        protected Position CurrentDirection = new Position(0, 0);
        protected float DeltaTimeSpeed;
        protected Position PreviousDirection = new Position(0, 0);

        public virtual void Start()
        {
            CurrentDirection = Direction.North;
        }

        public virtual void Update()
        {
            // Gather of input happen here
            _setNewDirection();
        }

        public void FixedUpdate()
        {
            // Actual movement happen here. This game does not use physic engine, but there is no need for movement to
            // be computed faster.
            _orbitalFlight();
        }

        /// <summary>
        ///     Allow a transform t to follow current movement and rotation
        /// </summary>
        /// <param name="t">Transform that will follow this.transform</param>
        public void MoveWithMe(Transform t)
        {
            _move(t, DeltaTimeSpeed);
        }


        /// <summary>
        ///     Default orbital flight sets the speed and rotate around Vector3.zero by DeltaTimeSpeed angles.
        /// </summary>
        protected virtual void _orbitalFlight()
        {
            DeltaTimeSpeed = speed * Time.fixedDeltaTime;
            _move(transform, DeltaTimeSpeed);
        }

        /// <summary>
        ///     Rotate transform t around position zero at an angle of moveSpeed in the CurrentDirection
        /// </summary>
        /// <param name="t">The transform to be rotated</param>
        /// <param name="moveSpeed">The angle that determines how much to rotate.</param>
        protected void _move(Transform t, float moveSpeed)
        {
            t.RotateAround(
                Vector3.zero,
                Vector3.right,
                moveSpeed * CurrentDirection.Y * -1
            );
            t.RotateAround(
                Vector3.zero,
                Vector3.forward,
                moveSpeed * CurrentDirection.X * -1
            );
        }

        /// <summary>
        ///     Placeholder function to set new direction.
        ///     This function should collect user input for the player ship or compute the new direction for NPC.
        /// </summary>
        protected virtual void _setNewDirection()
        {
            CurrentDirection = Direction.North;
        }
    }
}