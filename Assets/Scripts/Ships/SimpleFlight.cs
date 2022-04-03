using System;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace Ships
{
    /// <summary>
    ///     SimpleFlight is the most basic flight for the Enemy Ships.
    ///     It navigates the maze by simply choosing a random direction when a station is hit.
    ///     SimpleFlight was created as a replacement for BinaryTreeSearchFlight, but it cannot provide the same features,
    ///     notably the ability to set a target goal and move there.
    /// </summary>
    public class SimpleFlight : ShipFlight
    {
        private EnemyBehaviour _behaviour;
        private SimpleSensor _sensor;
        private SimpleSensor _sentinelDetector;

        public override void Start()
        {
            CurrentDirection = new[]
            {
                Direction.North, Direction.East, Direction.South, Direction.West
            }[Random.Range(0, 4)];
            var sensorObj = transform.Find("SimpleSensor");
            if (sensorObj != null) _sensor = sensorObj.GetComponent<SimpleSensor>();
            var sentinelDetectorObj = transform.Find("NavigationSensor");
            if (sentinelDetectorObj != null) _sentinelDetector = sentinelDetectorObj.GetComponent<SimpleSensor>();
            _behaviour = GetComponent<EnemyBehaviour>();
            speed = _behaviour.movementSpeed;
        }

        protected override void _orbitalFlight()
        {
            // Allow for a chance of changing directions during offboard
            // this ensures that the enemy ships will eventually find the board.
            if (OffBoard && Random.value < 0.1f) _setNewDirection();
            base._orbitalFlight();
        }

        /// <summary>
        ///     Set a new direction randomly according to the ship states.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        protected override void _setNewDirection()
        {
            if (_behaviour is null)
                _setDirectionRepositioning();
            else
                switch (_behaviour.StateMachine.GetState())
                {
                    case EnemyShipStates.Repositioning:
                    case EnemyShipStates.Idle:
                        _setDirectionRepositioning();
                        break;
                    case EnemyShipStates.Dying:
                        return;
                    case EnemyShipStates.Fleeing:
                    case EnemyShipStates.Resisting:
                        _setDirectionFleeing();
                        break;
                    case EnemyShipStates.AttackingEarth:
                        CurrentDirection = new Position(0, 0);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
        }

        /// <summary>
        ///     Choose a new direction by avoiding hitting a space station
        /// </summary>
        private void _setDirectionRepositioning()
        {
            if (_sensor.blocked || Random.value < 0.01f)
                CurrentDirection = CurrentDirection.X != 0
                    ? new Position(0, Random.value > 0.5f ? 1 : -1)
                    : new Position(Random.value > 0.5f ? 1 : -1, 0);
        }

        /// <summary>
        ///     Choose a new direction by going in the opposite direction of the Sentinel
        /// </summary>
        private void _setDirectionFleeing()
        {
            if (_sensor.blocked)
            {
                _setDirectionRepositioning();
                return;
            }

            var avoid = _sentinelDetector.blocking.transform.position;
            var opposite = avoid - transform.position;
            var oppositeX = opposite.x;
            var oppositeZ = opposite.z;
            CurrentDirection = oppositeX > oppositeZ
                ? new Position(oppositeX > 0 ? 1 : 0, 0)
                : new Position(0, oppositeZ > 0 ? 1 : 0);
        }

        public override void GoToStartPosition(Transform t = null)
        {
        }
    }
}