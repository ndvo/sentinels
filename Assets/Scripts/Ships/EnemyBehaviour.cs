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
        public string state;

        public float sentinelSensorSize;
        public float navigationSensorSize;
        public readonly EnemyShipStateMachine StateMachine = new EnemyShipStateMachine();
        private AttackEarth _attack;
        private Transform _attackSensorTransform;
        private SimpleSensor _navigationSensor;

        private Transform _navigationSensorTransform;
        private SimpleSensor _sentinelDetector;

        private float _timeAfterEscape;

        private void Start()
        {
            _attack = GetComponent<AttackEarth>();
            _timeAfterEscape = fleeTime;
            _navigationSensorTransform = transform.Find("NavigationSensor");
            if (_navigationSensorTransform is { })
            {
                _navigationSensor = _navigationSensorTransform.GetComponent<SimpleSensor>();
                _navigationSensor.sensorScale =
                    new Vector3(navigationSensorSize, navigationSensorSize, navigationSensorSize);
            }

            _attackSensorTransform = transform.Find("AttackSensor");
            if (_attackSensorTransform is null) return;
            _sentinelDetector = _attackSensorTransform.GetComponent<SimpleSensor>();
            _sentinelDetector.sensorScale = new Vector3(sentinelSensorSize, sentinelSensorSize, sentinelSensorSize);
        }

        private void Update()
        {
            var newState = StateMachine.GetState();
            state = newState.ToString();
            if (newState == EnemyShipStates.Dying) return;
            if (newState != EnemyShipStates.Fleeing)
                if (_sentinelDetector.blocked)
                {
                    StateMachine.Flee();
                    if (
                        resistance < 5
                        || Vector3.Distance(_sentinelDetector.blocking.transform.position, transform.position) < 20f
                    )
                        StateMachine.Resist();
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
                    if (!_navigationSensor.blocked) break;
                    if (Random.value < attackProbability / 100)
                    {
                        StateMachine.AttackEarth();
                        _attack.SetTarget(_navigationSensor.blocking.transform);
                    }

                    break;
                case EnemyShipStates.Idle:
                    _attack.StopAttack();
                    if (Random.value < idleProbability / 100) StateMachine.Reposition();
                    break;
                case EnemyShipStates.Resisting:
                    _attack.StopAttack();
                    if (!_sentinelDetector.blocked) StateMachine.Reposition();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            state = StateMachine.GetState().ToString();
        }
    }
}