using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Ships
{
    
    public class EnemyBehaviour : MonoBehaviour
    {
        public float resistance = 10f;
        public float firePower = 10f;
        public float fireSpeed = 10f;
        public float drainPower = 10f;
        public float drainSpeed = 10f;
        public float movementSpeed = 10f;
        public float fleeTime = 10f;
        public float attackProbability = 10f;
        public float idleProbability = 10f;
        
        public readonly EnemyShipStateMachine StateMachine = new EnemyShipStateMachine();
        private SimpleSensor _sentinelDetector;
        private SimpleSensor _sensor;
        
        private float _timeAfterEscape = 0;
    
        void Start()
        {
            _timeAfterEscape = fleeTime;
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
        }

        private void ResumeOperation()
        {
        }

        private void Update()
        {
            // Regardless of 
            var state = StateMachine.GetState();
            if (state == EnemyShipStates.Dying) return;
            if (state != EnemyShipStates.Fleeing)
            {
                if (_sentinelDetector.blocked)
                {
                    StateMachine.Flee();
                    if (
                        resistance < 5 
                        || Vector3.Distance(_sentinelDetector.blocking.transform.position, transform.position) < 20f
                        )
                        StateMachine.Resist();
                }
            }
            switch (StateMachine.GetState())
            {
                case EnemyShipStates.Dying:
                case EnemyShipStates.AttackingEarth:
                    break;
                case EnemyShipStates.Fleeing:
                    _timeAfterEscape += Time.deltaTime;
                    if (_timeAfterEscape > fleeTime)
                    {
                        _timeAfterEscape = 0;
                        StateMachine.Reposition();
                    }
                    break;
                case EnemyShipStates.Repositioning:
                    if (!_sensor.blocked) return;
                    if (Random.value < attackProbability / 1000)
                    {
                        StateMachine.AttackEarth();
                    }
                    break;
                case EnemyShipStates.Idle:
                    if (Random.value < idleProbability / 1000)
                    {
                        StateMachine.Reposition();
                    }
                    break;
                case EnemyShipStates.Resisting:
                    if (!_sentinelDetector.blocked)
                    {
                        StateMachine.Reposition();
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
