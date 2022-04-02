using System;
using System.Collections;
using System.Collections.Generic;
using MiscUtil.Collections.Extensions;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Ships
{
    /// <summary>
    /// The list of possible states the enemy ship can be in.
    /// </summary>
    public enum EnemyShipStates
    {
        Idle,
        Repositioning,
        AttackingEarth,
        Fleeing,
        Resisting,
        Dying
    }

    /// <summary>
    /// Controls the transition of states an enemy ship may be in.
    ///
    /// The purpose of this class is to ensure a consistent behaviour for the enemy ships, avoiding that logic from
    /// different parts of the code violate the expected behaviour.
    ///
    /// It exposes available transitions an throws errors if an illegal transition is attempted.
    /// </summary>
    public class EnemyShipStateMachine
    {

        private EnemyShipStates _state = EnemyShipStates.Idle;

        public EnemyShipStates GetState()
        {
            return _state;
        }
        
        private void _throwInvalidState()
        {
            throw new Exception($"Invalid state {_state}");
        }

        public void Reposition()
        {
            if (_state == EnemyShipStates.Dying) _throwInvalidState();
            _state = EnemyShipStates.Repositioning;
        }

        public void AttackEarth()
        {
            if (_state != EnemyShipStates.Repositioning) _throwInvalidState();
            _state = EnemyShipStates.AttackingEarth;
        }

        public void Flee()
        {
            if (_state == EnemyShipStates.Dying) _throwInvalidState();
            _state = EnemyShipStates.Fleeing;
        }

        public void Resist()
        {
            if (_state != EnemyShipStates.Fleeing) _throwInvalidState();
            _state = EnemyShipStates.Resisting;
        }

        public override string ToString()
        {
            return $"StateMachine {_state.ToString()}";
        }

    }
}