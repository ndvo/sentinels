using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Ships
{
    
    public class EnemyBehaviour : MonoBehaviour
    {
        public float resistance = 10f;
        public float firePower = 10f;
        public float drainPower = 10f;
        public float movementSpeed = 10f;
        public float fleeTime = 10f;
        public float attackProbability = 10f;
        public float idleProbability = 10f;
        public readonly EnemyShipStateMachine StateMachine = new EnemyShipStateMachine();
        private SimpleSensor _sentinelDetector;
        private SimpleSensor _sensor;
        public string state;
        
        private float _timeAfterEscape = 0;
        private AttackEarth _attack;
    
        void Start()
        {
            _attack = GetComponent<AttackEarth>();
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

        private void Update()
        {
            var newState = StateMachine.GetState();
            this.state = newState.ToString();
            if (newState == EnemyShipStates.Dying) return;
            if (newState != EnemyShipStates.Fleeing)
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
                    _attack.StopAttack();
                    break;
                case EnemyShipStates.AttackingEarth:
                    break;
                case EnemyShipStates.Fleeing:
                    _attack.StopAttack();
                    _timeAfterEscape += Time.deltaTime;
                    if (_timeAfterEscape > fleeTime)
                    {
                        _timeAfterEscape = 0;
                        StateMachine.Reposition();
                    }
                    break;
                case EnemyShipStates.Repositioning:
                    _attack.StopAttack();
                    if (!_sensor.blocked) break;
                    if (Random.value < attackProbability / 100)
                    {
                        StateMachine.AttackEarth();
                        _attack.SetTarget(_sensor.blocking.transform);
                    }
                    break;
                case EnemyShipStates.Idle:
                    _attack.StopAttack();
                    if (Random.value > idleProbability / 100)
                    {
                        StateMachine.Reposition();
                    }
                    break;
                case EnemyShipStates.Resisting:
                    _attack.StopAttack();
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
