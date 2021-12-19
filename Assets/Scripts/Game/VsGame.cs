using SharedGame;
using BattleInput;
using System;
using System.IO;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace PlayerVsGameSpace {

    using static VSGameConstants;

    public static class VSGameConstants {
        public const int MAX_SHIPS = 4;
        public const int MAX_PLAYERS = 64;

        public const int INPUT_THRUST = (1 << 0);
        public const int INPUT_BREAK = (1 << 1);
        public const int INPUT_ROTATE_LEFT = (1 << 2);
        public const int INPUT_ROTATE_RIGHT = (1 << 3);
        public const int INPUT_FIRE = (1 << 4);
        public const int INPUT_BOMB = (1 << 5);
        public const int MAX_BULLETS = 30;

        public const float PI = 3.1415926f;
        public const int STARTING_HEALTH = 100;
        public const float ROTATE_INCREMENT = 3f;
        public const float SHIP_RADIUS = 15f;
        public const float SHIP_THRUST = 0.06f;
        public const float SHIP_MAX_THRUST = 4.0f;
        public const float SHIP_BREAK_SPEED = 0.6f;
        public const float BULLET_SPEED = 5f;
        public const int BULLET_COOLDOWN = 8;
        public const int BULLET_DAMAGE = 10;
    }

    [Serializable]
    public class Fighter {
        public Vector2 position;
        public Vector2 velocity;
        public float radius;
        public float heading;
        public int health;
        public int cooldown;
        public int score;

        public void Serialize(BinaryWriter bw) {
            bw.Write(position.x);
            bw.Write(position.y);
            bw.Write(velocity.x);
            bw.Write(velocity.y);
            bw.Write(radius);
            bw.Write(heading);
            bw.Write(health);
            bw.Write(cooldown);
            bw.Write(score);
        }

        public void Deserialize(BinaryReader br) {
            position.x = br.ReadSingle();
            position.y = br.ReadSingle();
            velocity.x = br.ReadSingle();
            velocity.y = br.ReadSingle();
            radius = br.ReadSingle();
            heading = br.ReadSingle();
            health = br.ReadInt32();
            cooldown = br.ReadInt32();
            score = br.ReadInt32();
        }

        // @LOOK Not hashing bullets.
        public override int GetHashCode() {
            int hashCode = 1858597544;
            hashCode = hashCode * -1521134295 + position.GetHashCode();
            hashCode = hashCode * -1521134295 + velocity.GetHashCode();
            hashCode = hashCode * -1521134295 + radius.GetHashCode();
            hashCode = hashCode * -1521134295 + heading.GetHashCode();
            hashCode = hashCode * -1521134295 + health.GetHashCode();
            hashCode = hashCode * -1521134295 + cooldown.GetHashCode();
            hashCode = hashCode * -1521134295 + score.GetHashCode();
            return hashCode;
        }
    }

    [Serializable]
    public struct VsGame : IGame {
        public int Framenumber { get; private set; }

        public int Checksum => GetHashCode();

        public Fighter[] _fighters;

        public static Rect _bounds = new Rect(0, 0, 640, 480);

        private IList<IController> _controllers;

        public void Serialize(BinaryWriter bw) {
            bw.Write(Framenumber);
            bw.Write(_fighters.Length);
            for (int i = 0; i < _fighters.Length; ++i) {
                _fighters[i].Serialize(bw);
            }
        }

        public void Deserialize(BinaryReader br) {
            Framenumber = br.ReadInt32();
            int length = br.ReadInt32();
            if (length != _fighters.Length) {
                _fighters = new Fighter[length];
            }
            for (int i = 0; i < _fighters.Length; ++i) {
                _fighters[i].Deserialize(br);
            }
        }

        public NativeArray<byte> ToBytes() {
            using (var memoryStream = new MemoryStream()) {
                using (var writer = new BinaryWriter(memoryStream)) {
                    Serialize(writer);
                }
                return new NativeArray<byte>(memoryStream.ToArray(), Allocator.Persistent);
            }
        }

        public void FromBytes(NativeArray<byte> bytes) {
            using (var memoryStream = new MemoryStream(bytes.ToArray())) {
                using (var reader = new BinaryReader(memoryStream)) {
                    Deserialize(reader);
                }
            }
        }

        private static float DegToRad(float deg) {
            return PI * deg / 180;
        }

        private static float Distance(Vector2 lhs, Vector2 rhs) {
            float x = rhs.x - lhs.x;
            float y = rhs.y - lhs.y;
            return Mathf.Sqrt(x * x + y * y);
        }

        /*
         * InitGameState --
         *
         * Initialize our game state.
         */

        public VsGame(int num_players) {
            var w = _bounds.xMax - _bounds.xMin;
            var h = _bounds.yMax - _bounds.yMin;
            var r = h / 4;
            Framenumber = 0;
            _fighters = new Fighter[num_players];
            _controllers = new List<IController>();
            for (int i = 0; i < _fighters.Length; i++) {
                _fighters[i] = new Fighter();
                int heading = 0;
                float cost, sint, theta;

                theta = (float)heading * PI / 180;
                cost = Mathf.Cos(theta);
                sint = Mathf.Sin(theta);

                _fighters[i].position.x = (w / 2) + r * cost;
                _fighters[i].position.y = (h / 2) + r * sint;
                _fighters[i].heading = heading;
                _fighters[i].health = STARTING_HEALTH;
                _fighters[i].radius = SHIP_RADIUS;
            }
        }

        public void GetShipAI(int i, out float heading, out float thrust, out int fire) {
            heading = 0.1f;
            thrust = 0;
            fire = 0;
        }

        public void ParseShipInputs(long inputs, int i, out float heading, out float thrust, out int fire) {
            var ship = _fighters[i];

            GGPORunner.LogGame($"parsing ship {i} inputs: {inputs}.");

            if ((inputs & INPUT_ROTATE_RIGHT) != 0) {
                // heading = (ship.heading - ROTATE_INCREMENT) % 360;
                heading = SHIP_MAX_THRUST;
            }
            else if ((inputs & INPUT_ROTATE_LEFT) != 0) {
                // heading = (ship.heading + ROTATE_INCREMENT + 360) % 360;
                heading = -SHIP_MAX_THRUST;
            }
            else {
                heading = 0;
            }

            if ((inputs & INPUT_THRUST) != 0) {
                thrust = -SHIP_MAX_THRUST;
            }
            else if ((inputs & INPUT_BREAK) != 0) {
                thrust = SHIP_MAX_THRUST;
            }
            else {
                thrust = 0;
            }

            fire = (int)(inputs & INPUT_FIRE);
        }

        public void LogInfo(string filename) {
            Debug.Log(filename);
        }

        // public void LogInfo(string filename) {
        //     string fp = "";
        //     fp += "GameState object.\n";
        //     fp += string.Format("  bounds: {0},{1} x {2},{3}.\n", _bounds.xMin, _bounds.yMin, _bounds.xMax, _bounds.yMax);
        //     fp += string.Format("  num_fighters: {0}.\n", _fighters.Length);
        //     for (int i = 0; i < _fighters.Length; i++) {
        //         var ship = _fighters[i];
        //         fp += string.Format("  ship {0} position:  %.4f, %.4f\n", i, ship.position.x, ship.position.y);
        //         fp += string.Format("  ship {0} velocity:  %.4f, %.4f\n", i, ship.velocity.x, ship.velocity.y);
        //         fp += string.Format("  ship {0} radius:    %d.\n", i, ship.radius);
        //         fp += string.Format("  ship {0} heading:   %d.\n", i, ship.heading);
        //         fp += string.Format("  ship {0} health:    %d.\n", i, ship.health);
        //         fp += string.Format("  ship {0} cooldown:  %d.\n", i, ship.cooldown);
        //         fp += string.Format("  ship {0} score:     {1}.\n", i, ship.score);
        //         for (int j = 0; j < ship.bullets.Length; j++) {
        //             fp += string.Format("  ship {0} bullet {1}: {2} {3} -> {4} {5}.\n", i, j,
        //                     ship.bullets[j].position.x, ship.bullets[j].position.y,
        //                     ship.bullets[j].velocity.x, ship.bullets[j].velocity.y);
        //         }
        //     }
        //     File.WriteAllText(filename, fp);
        // }

        public void Update(long[] inputs, int disconnect_flags) {
            Framenumber++;
            for (int i = 0; i < _fighters.Length; i++) {
                float thrust, heading;
                int fire;

                if ((disconnect_flags & (1 << i)) != 0) {
                    GetShipAI(i, out heading, out thrust, out fire);
                }
                else {
                    ParseShipInputs(inputs[i], i, out heading, out thrust, out fire);
                }
                MoveFighter(i, heading, thrust, fire);

                if (_fighters[i].cooldown != 0) {
                    _fighters[i].cooldown--;
                }
            }
        }

        public void MoveFighter(int index, float heading, float thrust, int fire) {
            var fighter = _fighters[index];
            fighter.velocity.x = heading;
            fighter.velocity.y = thrust;
            fighter.position.x += fighter.velocity.x;
            fighter.position.y += fighter.velocity.y;
        }

        public long ReadInputs(int id) {
            if (id == 0 || id == 1) {
                return GetControllerInput(id);
            } else {
                throw new ArgumentException("reading non existent player id");
            }
        }

        private long GetControllerInput(int id) {
            if (id >= _controllers.Count) {
                // Move this out to local runners later
                ControllerReader[] controllers = GameObject.FindObjectsOfType<ControllerReader>();
                if (id >= controllers.Length) {
                    LogInfo("Controller out of range");
                }
                _controllers = new List<IController>(controllers);
                return 0;
            } else {
                return _controllers[id].GetCurrentInput();
            }
        }

        // public long ReadInputs(int id) {
        //     long input = 0;

        //     if (id == 0) {
        //         if (UnityEngine.Input.GetKey(UnityEngine.KeyCode.UpArrow)) {
        //             input |= INPUT_THRUST;
        //         }
        //         if (UnityEngine.Input.GetKey(UnityEngine.KeyCode.DownArrow)) {
        //             input |= INPUT_BREAK;
        //         }
        //         if (UnityEngine.Input.GetKey(UnityEngine.KeyCode.LeftArrow)) {
        //             input |= INPUT_ROTATE_LEFT;
        //         }
        //         if (UnityEngine.Input.GetKey(UnityEngine.KeyCode.RightArrow)) {
        //             input |= INPUT_ROTATE_RIGHT;
        //         }
        //         if (UnityEngine.Input.GetKey(UnityEngine.KeyCode.RightControl)) {
        //             input |= INPUT_FIRE;
        //         }
        //         if (UnityEngine.Input.GetKey(UnityEngine.KeyCode.RightShift)) {
        //             input |= INPUT_BOMB;
        //         }
        //     }
        //     else if (id == 1) {
        //         if (UnityEngine.Input.GetKey(UnityEngine.KeyCode.W)) {
        //             input |= INPUT_THRUST;
        //         }
        //         if (UnityEngine.Input.GetKey(UnityEngine.KeyCode.S)) {
        //             input |= INPUT_BREAK;
        //         }
        //         if (UnityEngine.Input.GetKey(UnityEngine.KeyCode.A)) {
        //             input |= INPUT_ROTATE_LEFT;
        //         }
        //         if (UnityEngine.Input.GetKey(UnityEngine.KeyCode.D)) {
        //             input |= INPUT_ROTATE_RIGHT;
        //         }
        //         if (UnityEngine.Input.GetKey(UnityEngine.KeyCode.F)) {
        //             input |= INPUT_FIRE;
        //         }
        //         if (UnityEngine.Input.GetKey(UnityEngine.KeyCode.G)) {
        //             input |= INPUT_BOMB;
        //         }
        //     }

        //     return input;
        // }

        public void FreeBytes(NativeArray<byte> data) {
            if (data.IsCreated) {
                data.Dispose();
            }
        }

        public override int GetHashCode() {
            int hashCode = -1214587014;
            hashCode = hashCode * -1521134295 + Framenumber.GetHashCode();
            foreach (var ship in _fighters) {
                hashCode = hashCode * -1521134295 + ship.GetHashCode();
            }
            return hashCode;
        }
    }
}