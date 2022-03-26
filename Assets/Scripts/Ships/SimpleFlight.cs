using System;
using UnityEngine.PlayerLoop;
using Utils;
using Random = UnityEngine.Random;

namespace Ships
{
    public class SimpleFlight : ShipFlight
    {
        private SimpleSensor _sensor;
        private SimpleSensor _sentinelDetector;
        private EnemyBehaviour _behaviour;

        public override void Start()
        {
            CurrentDirection = Utils.Direction.North;
            var sensorObj = transform.Find("SimpleSensor");
            if (sensorObj != null)
            {
                _sensor = sensorObj.GetComponent<SimpleSensor>();
            }
            var sentinelDetectorObj = transform.Find("NavigationSensor");
            if (sentinelDetectorObj != null)
            {
                _sentinelDetector = sentinelDetectorObj.GetComponent<SimpleSensor>();
            }
            _behaviour = GetComponent<EnemyBehaviour>();
            speed = 2f + _behaviour.movementSpeed / 10;
        }

        protected override void _setNewDirection()
        {
            if (_behaviour is null)
                _setDirectionRepositioning();
            else switch (_behaviour.StateMachine.GetState())
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
        
        private void _setDirectionRepositioning()
        {
            if (_sensor.blocked)
            {
                CurrentDirection = CurrentDirection.x != 0 
                    ? new Position(0, Random.value > 0.5f ? 1 : -1) 
                    : new Position(Random.value > 0.5f ? 1 : -1, 0);
            }
        }

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
                : new Position(0, oppositeZ > 0 ? 1: 0);
        }
        
    }
}
