using System;
using UnityEngine;
using Utils;
using Time = UnityEngine.Time;
using Vector3 = UnityEngine.Vector3;

namespace Ships
{
    public class OrbitalFlight : MonoBehaviour
    {

        public float speed;
        protected Position CurrentDirection = new Position(0, 0);
        protected Position PreviousDirection = new Position(0, 0);
        protected float DeltaTimeSpeed = 0;
        protected AudioSource AudioSource;

        public virtual void Start()
        {
            CurrentDirection = Utils.Direction.North;
        }

        public void MoveWithMe(Transform t)
        {
            _move(t, DeltaTimeSpeed);
        }

        public virtual void FixedUpdate()
        {
            _setNewDirection();
            _orbitalFlight();
        }

        protected virtual void _orbitalFlight()
        {
            DeltaTimeSpeed = speed * Time.fixedDeltaTime;
            _move(transform, DeltaTimeSpeed);
        }

        protected void _move(Transform t, float moveSpeed)
        {
            t.RotateAround(
                Vector3.zero, 
                Vector3.right,
                moveSpeed * CurrentDirection.y * -1
            );
            t.RotateAround(
                Vector3.zero, 
                Vector3.forward,
                moveSpeed * CurrentDirection.x * -1
            );
        }

        protected virtual void _setNewDirection()
        {
            CurrentDirection = Utils.Direction.North;
        }

    }
}
