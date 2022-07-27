using System;
using BestHTTP.SocketIO3;
using UnityEngine;

namespace SensapexLink
{
    /// <summary>
    /// WebSocket connection manager between the Trajectory Planner and a running Sensapex Link server
    /// </summary>
    public class CommunicationManager : MonoBehaviour
    {
        #region Variables

        // Connection details
        [SerializeField] private string serverIp = "10.18.251.95";
        [SerializeField] private ushort serverPort = 8080;

        // Components
        private SocketManager _connectionManager;

        #endregion

        #region Setup

        void Start()
        {
            // Create connection to server
            _connectionManager = new SocketManager(new Uri("http://" + serverIp + ":" + serverPort));
            _connectionManager.Socket.On("connect", () => Debug.Log(_connectionManager.Handshake.Sid));
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Get manipulators event sender
        /// </summary>
        /// <param name="onSuccessCallback">Callback function to handle incoming manipulator ID's</param>
        /// <param name="onErrorCallback">Callback function to handle errors</param>
        public void GetManipulators(Action<int[]> onSuccessCallback, Action<string> onErrorCallback = null)
        {
            _connectionManager.Socket.ExpectAcknowledgement<GetManipulatorsCallbackParameters>(data =>
            {
                if (data.error == "")
                {
                    onSuccessCallback(data.manipulators);
                }
                else
                {
                    onErrorCallback?.Invoke(data.error);
                    Debug.LogError(data.error);
                }
            }).Emit("get_manipulators");
        }

        /// <summary>
        /// Register a manipulator with the server
        /// </summary>
        /// <param name="manipulatorId">The ID of the manipulator to register</param>
        /// <param name="onSuccessCallback">Callback function to handle a successful registration</param>
        /// <param name="onErrorCallback">Callback function to handle errors</param>
        public void RegisterManipulator(int manipulatorId, Action onSuccessCallback = null, Action<string> onErrorCallback = null)
        {
            _connectionManager.Socket.ExpectAcknowledgement<string>(error =>
            {
                if (error == "")
                {
                    onSuccessCallback?.Invoke();
                }
                else
                {
                    onErrorCallback?.Invoke(error);
                    Debug.LogError(error);
                }
            }).Emit("register_manipulator", manipulatorId);
        }

        /// <summary>
        /// Request the current position of a manipulator
        /// </summary>
        /// <param name="manipulatorId">ID of the manipulator to get the position of</param>
        /// <param name="onSuccessCallback"></param>
        /// <param name="onErrorCallback"></param>
        public void GetPos(int manipulatorId, Action<Vector4> onSuccessCallback, Action<string> onErrorCallback = null)
        {
            _connectionManager.Socket.ExpectAcknowledgement<PositionalCallbackParameters>(data =>
            {
                if (data.error == "")
                {
                    onSuccessCallback(new Vector4(data.position[0], data.position[1], data.position[2], data.position[3]));
                }
                else
                {
                    onErrorCallback?.Invoke(data.error);
                    Debug.LogError(data.error);
                }
            }).Emit("get_pos", manipulatorId);
        }

        /// <summary>
        /// Request a manipulator be moved to a specific position
        /// </summary>
        /// <remarks>Position is defined by a Vector4</remarks>
        /// <param name="manipulatorId">ID of the manipulator to be moved</param>
        /// <param name="pos">Position in μm of the manipulator (in needle coordinates)</param>
        /// <param name="speed">How fast to move the manipulator (in μm/s)</param>
        /// <param name="onSuccessCallback"></param>
        /// <param name="onErrorCallback"></param>
        public void GotoPos(int manipulatorId, Vector4 pos, int speed, Action<Vector4> onSuccessCallback,
            Action<string> onErrorCallback = null)
        {
            _connectionManager.Socket.ExpectAcknowledgement<PositionalCallbackParameters>(data =>
            {
                if (data.error == "")
                {
                    onSuccessCallback(new Vector4(data.position[0], data.position[1], data.position[2], data.position[3]));
                }
                else
                {
                    onErrorCallback?.Invoke(data.error);
                    Debug.LogError(data.error);
                }
            }).Emit("goto_pos", new GotoPositionInputDataFormat(manipulatorId, pos, speed));
        }

        /// <summary>
        /// Request a manipulator be moved to a specific position defined by an array of 4 floats
        /// </summary>
        /// <remarks>Position is defined by an array of 4 floats</remarks>
        /// <param name="manipulatorId">ID of the manipulator to be moved</param>
        /// <param name="pos">Position in μm of the manipulator (in needle coordinates)</param>
        /// <param name="speed">How fast to move the manipulator (in μm/s)</param>
        /// <param name="onSuccessCallback"></param>
        /// <param name="onErrorCallback"></param>
        /// <exception cref="ArgumentException">If the given position is not in an array of 4 floats</exception>
        public void GotoPos(int manipulatorId, float[] pos, int speed, Action<Vector4> onSuccessCallback,
            Action<string> onErrorCallback = null)
        {
            if (pos.Length != 4)
            {
                throw new ArgumentException("Position array must be of length 4");
            }

            GotoPos(manipulatorId, new Vector4(pos[0], pos[1], pos[2], pos[3]), speed, onSuccessCallback, onErrorCallback);
        }

        /// <summary>
        /// Request a manipulator drive down to a specific depth
        /// </summary>
        /// <param name="manipulatorId">ID of the manipulator to move</param>
        /// <param name="depth">Depth in μm of the manipulator (in needle coordinates)</param>
        /// <param name="speed">How fast to drive the manipulator (in μm/s)</param>
        /// <param name="onSuccessCallback"></param>
        /// <param name="onErrorCallback"></param>
        public void DriveToDepth(int manipulatorId, float depth, int speed, Action<float> onSuccessCallback,
            Action<string> onErrorCallback)
        {
            _connectionManager.Socket.ExpectAcknowledgement<DriveToDepthCallbackParameters>(data =>
            {
                if (data.error == "")
                {
                    onSuccessCallback(data.depth);
                }
                else
                {
                    onErrorCallback?.Invoke(data.error);
                    Debug.LogError(data.error);
                }
            }).Emit("drive_to_depth", new DriveToDepthInputDataFormat(manipulatorId, depth, speed));
        }

        /// <summary>
        /// Set the inside brain state of a manipulator
        /// </summary>
        /// <param name="manipulatorId">ID of the manipulator to set the state of</param>
        /// <param name="inside">State to set to</param>
        /// <param name="onSuccessCallback"></param>
        /// <param name="onErrorCallback"></param>
        public void SetInsideBrain(int manipulatorId, bool inside, Action<bool> onSuccessCallback,
            Action<string> onErrorCallback = null)
        {
            _connectionManager.Socket.ExpectAcknowledgement<StateCallbackParameters>(data =>
            {
                if (data.error == "")
                {
                    onSuccessCallback(data.state);
                }
                else
                {
                    onErrorCallback?.Invoke(data.error);
                    Debug.LogError(data.error);
                }
            }).Emit("set_inside_brain", new InsideBrainInputDataFormat(manipulatorId, inside));
        }

        /// <summary>
        /// Request a manipulator to be calibrated
        /// </summary>
        /// <param name="manipulatorId">ID of the manipulator to be calibrated</param>
        /// <param name="callback">Callback function to handle a successful calibration</param>
        /// <param name="error">Callback function to handle an unsuccessful calibration</param>
        public void Calibrate(int manipulatorId, Action callback, Action<string> error = null)
        {
            _connectionManager.Socket.ExpectAcknowledgement<string>(errorMessage =>
            {
                if (errorMessage == "")
                {
                    callback();
                }
                else
                {
                    error?.Invoke(errorMessage);
                    Debug.LogError(errorMessage);
                }
            }).Emit("calibrate", manipulatorId);
        }

        /// <summary>
        /// Bypass calibration requirement of a manipulator
        /// </summary>
        /// <remarks>This method should only be used for testing and NEVER in production</remarks>
        /// <param name="manipulatorId">ID of the manipulator to bypass calibration</param>
        /// <param name="onSuccessCallback"></param>
        /// <param name="onErrorCallback"></param>
        public void BypassCalibration(int manipulatorId, Action onSuccessCallback,
            Action<string> onErrorCallback = null)
        {
            _connectionManager.Socket.ExpectAcknowledgement<string>(error =>
            {
                if (error == "")
                {
                    onSuccessCallback();
                }
                else
                {
                    onErrorCallback?.Invoke(error);
                    Debug.LogError(error);
                }
            }).Emit("bypass_calibration", manipulatorId);
        }

        /// <summary>
        /// Request a write lease for a manipulator
        /// </summary>
        /// <param name="manipulatorId">ID of the manipulator to allow writing</param>
        /// <param name="canWrite">Write state to set the manipulator to</param>
        /// <param name="hours">How many hours a manipulator may have a write lease</param>
        /// <param name="onSuccessCallback"></param>
        /// <param name="onErrorCallback"></param>
        public void SetCanWrite(int manipulatorId, bool canWrite, float hours, Action<bool> onSuccessCallback,
            Action<string> onErrorCallback = null)
        {
            _connectionManager.Socket.ExpectAcknowledgement<StateCallbackParameters>(data =>
            {
                if (data.error == "")
                {
                    onSuccessCallback(data.state);
                }
                else
                {
                    onErrorCallback?.Invoke(data.error);
                    Debug.LogError(data.error);
                }
            }).Emit("set_can_write", new CanWriteInputDataFormat(manipulatorId, canWrite, hours));
        }

        /// <summary>
        /// Request all movement to stop
        /// </summary>
        /// <param name="callback">Callback function to handle stop result</param>
        public void Stop(Action<bool> callback)
        {
            _connectionManager.Socket.ExpectAcknowledgement(callback).Emit("stop");
        }

        #endregion
    }
}