using UnityEngine;
using Utils;
using Time = UnityEngine.Time;

namespace Ships
{
    /// <summary>
    ///     Controls the Sentinel Flight, concentrating the gathering of navigation user input.
    ///     It uses Unity's Axis Input System to allow the game to be easily ported to other platforms if needed.
    /// </summary>
    public class SentinelFlight : ShipFlight
    {
        public GameObject help;
        private GameManager _gameManager;
        private float _showHelp;
        private float _standardSpeed;

        public override void Start()
        {
            _standardSpeed = speed;
            _gameManager = GameObject.Find("/GameManager").GetComponent<GameManager>();
            base.Start();
        }

        /// <summary>
        ///     Gather user input related to setting a new direction
        /// </summary>
        protected override void _setNewDirection()
        {
            PreviousDirection = new Position(CurrentDirection.X, CurrentDirection.Y);
            var horizontal = Input.GetAxisRaw("Horizontal");
            var vertical = Input.GetAxisRaw("Vertical");
            horizontal = horizontal > 0 ? 1 : horizontal < 0 ? -1 : 0;
            vertical = vertical > 0 ? -1 : vertical < 0 ? 1 : 0;
            if (horizontal != 0 || vertical != 0) CurrentDirection = new Position((int) horizontal, (int) vertical);
            speed = Input.GetAxisRaw("Jump") > 0
                ? 12
                : _standardSpeed;
            // Practice mode help
            if (_showHelp != 0 || (horizontal == 0 && vertical == 0) || !(Random.value < 0.001f)) return;
            _gameManager.ShowHelp(help);
            _showHelp += Time.deltaTime;
            if (!(_showHelp > 60)) return;
            _showHelp = 0;
            help.SetActive(false);
        }
    }
}