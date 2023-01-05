using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

/// <summary>
/// Trajectory Planner PlayerPreferences saving/loading
/// 
/// To use this class:
/// 1. define a new float/bool/int/string in the settings
/// 2. link the UI element that this corresponds to
/// 3. add corresponding getter/setter functions
/// 4. in Awake() load the setting and set the ui element
/// 5. if needed, in tpmanager Start() or any other Start() function, call the getter on your setting and do something with that
/// 
/// Note that PlayerPrefs data is not available in Awake() calls in other components!!
/// </summary>
public class Settings : MonoBehaviour
{
    public static Settings Instance;

    #region Probe settings
    // Collision detection
    private static bool s_collisions;
    private const string COLLISIONS_STR = "collisions";
    private const bool COLLISIONS_DEFAULT = true;
    [FormerlySerializedAs("collisionsToggle")][SerializeField] private Toggle _collisionsToggle;
    public UnityEvent DetectCollisionsChangedEvent;

    public static bool DetectCollisions
    {
        get { return s_collisions; }
        set
        {
            s_collisions = value;
            PlayerPrefs.SetInt(COLLISIONS_STR, s_collisions ? 1 : 0);
            Instance.DetectCollisionsChangedEvent.Invoke();
        }
    }

    // Display just the recording region, or the entire length of the probe
    private static bool s_recordingRegionOnly;
    private const string RECREGION_STR = "recordingregion";
    private const bool RECREGION_DEFAULT = true;
    [FormerlySerializedAs("recordingRegionToggle")][SerializeField] private Toggle _recordingRegionToggle;
    public UnityEvent RecordingRegionOnlyChangedEvent;

    public static bool RecordingRegionOnly
    {
        get { return s_recordingRegionOnly; }
        set
        {
            s_recordingRegionOnly = value;
            PlayerPrefs.SetInt(RECREGION_STR, s_recordingRegionOnly ? 1 : 0);
            Instance.RecordingRegionOnlyChangedEvent.Invoke();
        }
    }

    // Convert APML rotation to the probe's axis rotation
    private static bool s_convertAPML2probeAxis;
    private const string APML2PROBE_STR = "apml2probe";
    private const bool APML2PROBE_DEFAULT = false;
    [FormerlySerializedAs("probeAxisToggle")][SerializeField] private Toggle _probeAxisToggle;
    public UnityEvent ConvertAPML2ProbeChangedEvent;

    public static bool ConvertAPML2Probe
    {
        get { return s_convertAPML2probeAxis; }
        set
        {
            s_convertAPML2probeAxis = value;
            PlayerPrefs.SetInt(APML2PROBE_STR, s_convertAPML2probeAxis ? 1 : 0);
            Instance.ConvertAPML2ProbeChangedEvent.Invoke();
        }
    }

    private static bool s_useIBLAngles;
    private const string USEIBLANGLES_STR = "iblangles";
    private const bool USEIBLANGLES_DEFAULT = true;
    [FormerlySerializedAs("iblAngleToggle")][SerializeField] private Toggle _iblAngleToggle;
    public UnityEvent UseIBLAnglesChangedEvent;

    public static bool UseIBLAngles
    {
        get { return s_useIBLAngles; }
        set
        {
            s_useIBLAngles = value;
            PlayerPrefs.SetInt(USEIBLANGLES_STR, s_useIBLAngles ? 1 : 0);
            Instance.UseIBLAnglesChangedEvent.Invoke();
        }
    }

    #endregion

    #region Area settings
    // Use acronyms or full areas
    private static bool s_useAcronyms;
    private const string USEACRONYMS_STR = "acronyms";
    private const bool USEACRONYMS_DEFAULT = true;
    [FormerlySerializedAs("acronymToggle")][SerializeField] private Toggle _acronymToggle;
    public UnityEvent UseAcronymsChangedEvent;

    public static bool UseAcronyms
    {
        get { return s_useAcronyms; }
        set
        {
            s_useAcronyms = value;
            PlayerPrefs.SetInt(USEACRONYMS_STR, s_useAcronyms ? 1 : 0);
            Instance.UseAcronymsChangedEvent.Invoke();
        }
    }

    // Display the 3D area slice
    private static int s_slice3d;
    private const string SHOW3DSLICE_STR = "slice3d";
    private const int SHOW3DSLICE_DEFAULT = 0;
    [FormerlySerializedAs("slice3dDropdown")][SerializeField] private TMP_Dropdown _slice3dDropdown;
    public UnityEvent<int> Slice3DChangedEvent;

    public static int Slice3DDropdownOption
    {
        get { return s_slice3d; }
        set
        {
            s_slice3d = value;
            PlayerPrefs.SetInt(SHOW3DSLICE_STR, s_slice3d);
            Instance.Slice3DChangedEvent.Invoke(s_slice3d);
        }
    }

    // Use Beryl regions instead of ALL CCF regions
    private static bool s_useBeryl;
    private const string USEBERYL_STR = "beryl";
    private const bool USEBERYL_DEFAULT = true;
    [FormerlySerializedAs("useBerylToggle")][SerializeField] private Toggle _useBerylToggle;
    public UnityEvent<bool> UseBerylChangedEvent;

    public static bool UseBeryl
    {
        get { return s_useBeryl; }
        set
        {
            s_useBeryl = value;
            PlayerPrefs.SetInt(USEBERYL_STR, s_useBeryl ? 1 : 0);
            Instance.UseBerylChangedEvent.Invoke(s_useBeryl);
        }
    }

    #endregion

    #region Graphics settings

    // Show the surface coordinate sphere
    private static bool s_showSurfaceCoord;
    private const string SHOWSURFACECOORD_STR = "surfacecoord";
    private const bool SHOWSURFACECOORD_DEFAULT = true;
    [FormerlySerializedAs("surfaceToggle")][SerializeField] private Toggle _surfaceToggle;
    public UnityEvent SurfaceCoordChangedEvent;

    public static bool ShowSurfaceCoordinate
    {
        get { return s_showSurfaceCoord; }
        set
        {
            s_showSurfaceCoord = value;
            PlayerPrefs.SetInt(SHOWSURFACECOORD_STR, s_showSurfaceCoord ? 1 : 0);
            Instance.SurfaceCoordChangedEvent.Invoke();
        }
    }


    // Display the in-plane slice
    private static bool s_inplane;
    private const string SHOWINPLANE_STR = "inplane";
    private const bool SHOWINPLANE_DEFAULT = true;
    [FormerlySerializedAs("inplaneToggle")][SerializeField] private Toggle _inplaneToggle;
    public UnityEvent ShowInPlaneChangedEvent;

    public static bool ShowInPlaneSlice
    {
        get { return s_inplane; }
        set
        {
            s_inplane = value;
            PlayerPrefs.SetInt(SHOWINPLANE_STR, s_inplane ? 1 : 0);
            Instance.ShowInPlaneChangedEvent.Invoke();
        }
    }

    private static bool s_ghostInactiveProbes;
    private const string GHOSTINACTIVEPROBES_STR = "ghostinactive";
    private const bool GHOSTINACTIVEPROBES_DEFAULT = true;
    [FormerlySerializedAs("ghostInactiveProbesToggle")][SerializeField] private Toggle _ghostInactiveProbesToggle;
    public UnityEvent GhostInactiveProbesChangedEvent;

    public static bool GhostInactiveProbes
    {
        get { return s_ghostInactiveProbes; }
        set
        {
            s_ghostInactiveProbes = value;
            PlayerPrefs.SetInt(GHOSTINACTIVEPROBES_STR, s_ghostInactiveProbes ? 1 : 0);
            Instance.GhostInactiveProbesChangedEvent.Invoke();
        }
    }


    private static bool s_ghostInactiveAreas;
    private const string GHOSTINACTIVEAREAS_STR = "ghostinactive_areas";
    private const bool GHOSTINACTIVEAREAS_DEFAULT = false;
    [FormerlySerializedAs("ghostInactiveAreasToggle")][SerializeField] private Toggle _ghostInactiveAreasToggle;
    public UnityEvent GhostInactiveAreasChangedEvent;

    public static bool GhostInactiveAreas
    {
        get { return s_ghostInactiveAreas; }
        set
        {
            s_ghostInactiveAreas = value;
            PlayerPrefs.SetInt(GHOSTINACTIVEAREAS_STR, s_ghostInactiveAreas ? 1 : 0);
            Instance.GhostInactiveAreasChangedEvent.Invoke();
        }
    }


    #endregion

    #region Ephys link settings
    private static string _ephysLinkServerIp;
    private static int _ephysLinkServerPort;
    [FormerlySerializedAs("ephysLinkServerIpInput")][SerializeField] private TMP_InputField _ephysLinkServerIpInput;
    [FormerlySerializedAs("ephysLinkServerPortInput")][SerializeField] private TMP_InputField _ephysLinkServerPortInput;


    /// <summary>
    ///     Return the saved Ephys Link server IP address.
    /// </summary>
    /// <returns>Saved IP address of the Ephys Link server</returns>
    public static string GetServerIp()
    {
        return _ephysLinkServerIp;
    }

    /// <summary>
    ///     Return the saved Ephys Link server port.
    /// </summary>
    /// <returns>Saved server port of the Ephys Link server</returns>
    public static int GetServerPort()
    {
        return _ephysLinkServerPort;
    }
    /// <summary>
    ///     Save Ephys Link server connection information.
    /// </summary>
    /// <param name="serverIp">Server IP address</param>
    /// <param name="serverPort">Server port number</param>
    public static void SaveEphysLinkConnectionData(string serverIp, int serverPort)
    {
        PlayerPrefs.SetString("ephys_link_ip", serverIp);
        PlayerPrefs.SetInt("ephys_link_port", serverPort);
        PlayerPrefs.Save();
    }

    /// <summary>
    ///     Save the IDs of right handed manipulators.
    /// </summary>
    /// <param name="manipulatorIds">IDs of right handed manipulators</param>
    public static void SaveRightHandedManipulatorIds(IEnumerable<string> manipulatorIds)
    {
        PlayerPrefs.SetString("right_handed_manipulator_ids", string.Join(",", manipulatorIds));
        PlayerPrefs.Save();
    }
    #endregion


    // Settings
    private static int invivoTransform;
    private static bool _axisControl;
    private static bool _showAllProbePanels;
    private static string _rightHandedManipulatorIds;
    private static bool _displayUM;
    private static Vector3 _relCoord;

    private void Awake()
    {
        // Set Singleton
        if (Instance != null)
            Debug.LogError("Make sure there is only one Settings object in the scene!");
        Instance = this;
    }

    private void Start()
    {
        // Load preferences from memory and set UI elements
        DetectCollisions = LoadBoolPref(COLLISIONS_STR, COLLISIONS_DEFAULT);
        _collisionsToggle.SetIsOnWithoutNotify(DetectCollisions);

        RecordingRegionOnly = LoadBoolPref(RECREGION_STR, RECREGION_DEFAULT);
        _recordingRegionToggle.SetIsOnWithoutNotify(RecordingRegionOnly);

        ConvertAPML2Probe = LoadBoolPref(APML2PROBE_STR, APML2PROBE_DEFAULT);
        _probeAxisToggle.SetIsOnWithoutNotify(ConvertAPML2Probe);

        UseIBLAngles = LoadBoolPref(USEIBLANGLES_STR, USEIBLANGLES_DEFAULT);
        _iblAngleToggle.SetIsOnWithoutNotify(UseIBLAngles);

        UseAcronyms = LoadBoolPref(USEACRONYMS_STR, USEACRONYMS_DEFAULT);
        _acronymToggle.SetIsOnWithoutNotify(UseAcronyms);

        Slice3DDropdownOption = LoadIntPref(SHOW3DSLICE_STR, SHOW3DSLICE_DEFAULT);
        _slice3dDropdown.SetValueWithoutNotify(Slice3DDropdownOption);

        ShowSurfaceCoordinate = LoadBoolPref(SHOWSURFACECOORD_STR, SHOWSURFACECOORD_DEFAULT);
        _surfaceToggle.SetIsOnWithoutNotify(ShowSurfaceCoordinate);

        s_inplane = LoadBoolPref(SHOWINPLANE_STR, SHOWINPLANE_DEFAULT);
        _inplaneToggle.SetIsOnWithoutNotify(s_inplane);

        s_useBeryl = LoadBoolPref(USEBERYL_STR, USEBERYL_DEFAULT);
        _useBerylToggle.SetIsOnWithoutNotify(s_useBeryl);

        s_ghostInactiveProbes = LoadBoolPref(GHOSTINACTIVEPROBES_STR, GHOSTINACTIVEPROBES_DEFAULT);
        _ghostInactiveProbesToggle.SetIsOnWithoutNotify(s_ghostInactiveProbes);

        s_ghostInactiveAreas = LoadBoolPref(GHOSTINACTIVEAREAS_STR, GHOSTINACTIVEAREAS_DEFAULT);
        _ghostInactiveAreasToggle.SetIsOnWithoutNotify(s_ghostInactiveAreas);
    }

    [FormerlySerializedAs("invivoDropdown")] [SerializeField] private TMP_Dropdown _invivoDropdown;
    [FormerlySerializedAs("axisControlToggle")] [SerializeField] private Toggle _axisControlToggle;
    [FormerlySerializedAs("showAllProbePanelsToggle")] [SerializeField] private Toggle _showAllProbePanelsToggle;
    [FormerlySerializedAs("displayUmToggle")] [SerializeField] private Toggle _displayUmToggle;


    /// <summary>
    /// On Awake() load the preferences and toggle the corresponding UI elements
    /// </summary>
    //private void Awake()
    //{



    //    invivoTransform = LoadIntPref("stereotaxic", 1);
    //    _invivoDropdown.SetValueWithoutNotify(invivoTransform);



    //    _ephysLinkServerIp = LoadStringPref("ephys_link_ip", "localhost");
    //    _ephysLinkServerIpInput.text = _ephysLinkServerIp;

    //    _ephysLinkServerPort = LoadIntPref("ephys_link_port", 8081);
    //    _ephysLinkServerPortInput.text = _ephysLinkServerPort.ToString();

    //    _axisControl = LoadBoolPref("axis_control", true);
    //    _axisControlToggle.isOn = _axisControl;

    //    _showAllProbePanels = LoadBoolPref("show_all_probe_panels", true);
    //    _showAllProbePanelsToggle.isOn = _showAllProbePanels;

    //    _rightHandedManipulatorIds = LoadStringPref("right_handed_manipulator_ids", "");


    //    _displayUM = LoadBoolPref("display_um", true);
    //    _displayUmToggle.isOn = _displayUM;

    //    _relCoord = LoadVector3Pref("rel_coord", new Vector3(5.4f, 5.7f, 0.332f));


    //}

    #region Getters/Setters

    public static void SetRelCoord(Vector3 coord)
    {
        _relCoord = coord;
        SaveVector3Pref("rel_coord", _relCoord);
    }

    public static Vector3 GetRelCoord()
    {
        return _relCoord;
    }

    public static void SetDisplayUm(bool state)
    {
        _displayUM = state;
        PlayerPrefs.SetInt("display_um", _displayUM ? 1 : 0);
    }

    public static bool GetDisplayUm()
    {
        return _displayUM;
    }

    public static void SetShowAllProbePanels(bool state)
    {
        _showAllProbePanels = state;
        PlayerPrefs.SetInt("show_all_probe_panels", _showAllProbePanels ? 1 : 0);
    }

    public static bool GetShowAllProbePanels()
    {
        return _showAllProbePanels;
    }

    public static void SetAxisControl(bool state)
    {
        _axisControl = state;
        PlayerPrefs.SetInt("axis_control", _axisControl ? 1 : 0);
    }

    public static bool GetAxisControl()
    {
        return _axisControl;
    }

    public static void SetStereotaxic(int state)
    {
        invivoTransform = state;
        PlayerPrefs.SetInt("stereotaxic", invivoTransform);
    }

    public static int GetStereotaxic()
    {
        return invivoTransform;
    }

    /// <summary>
    ///     Return the saved IDs of right handed manipulators.
    /// </summary>
    /// <returns>Saved IDs of right handed manipulators</returns>
    public static HashSet<string> GetRightHandedManipulatorIds()
    {
        return _rightHandedManipulatorIds == null
            ? new HashSet<string>()
            : _rightHandedManipulatorIds.Split(',').ToHashSet();
    }

    /// <summary>
    ///     Return if it has been more than 24 hours since the last launch.
    /// </summary>
    /// <returns>If it has been more than 24 hours since the last launch</returns>
    public static bool IsEphysLinkDataExpired()
    {
        var timestampString = PlayerPrefs.GetString("timestamp");
        if (timestampString == "") return false;

        return new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds() - long.Parse(timestampString) >= 86400;
    }

    #endregion

    #region Helper functions for booleans/integers/strings

    /// <summary>
    /// Load a boolean preference
    /// </summary>
    /// <param name="prefStr">string accessor</param>
    /// <param name="defaultValue">default value if the preference is not set</param>
    /// <returns></returns>
    private bool LoadBoolPref(string prefStr, bool defaultValue)
    {
        return PlayerPrefs.HasKey(prefStr) ? PlayerPrefs.GetInt(prefStr) == 1 : defaultValue;
    }

    /// <summary>
    /// Load an integer preference
    /// </summary>
    /// <param name="prefStr">string accessor</param>
    /// <param name="defaultValue">default value if the preference is not set</param>
    /// <returns></returns>
    private int LoadIntPref(string prefStr, int defaultValue)
    {
        return PlayerPrefs.HasKey(prefStr) ? PlayerPrefs.GetInt(prefStr) : defaultValue;
    }

    /// <summary>
    /// Load a string preference
    /// </summary>
    /// <param name="prefStr">string accessor</param>
    /// <param name="defaultValue">default value if the preference is not set</param>
    /// <returns></returns>
    private string LoadStringPref(string prefStr, string defaultValue)
    {
        return PlayerPrefs.HasKey(prefStr) ? PlayerPrefs.GetString(prefStr) : defaultValue;
    }

    private Vector3 LoadVector3Pref(string prefStr, Vector3 defaultValue)
    {
        if (PlayerPrefs.HasKey(prefStr + "_x"))
        {
            float ap = PlayerPrefs.GetFloat(prefStr + "_x");
            float ml = PlayerPrefs.GetFloat(prefStr + "_y");
            float dv = PlayerPrefs.GetFloat(prefStr + "_z");

            return new Vector3(ap, ml, dv);
        }
        else
            return defaultValue;
    }

    private static void SaveVector3Pref(string prefStr, Vector3 value)
    {
        PlayerPrefs.SetFloat(prefStr + "_x", value.x);
        PlayerPrefs.SetFloat(prefStr + "_y", value.y);
        PlayerPrefs.SetFloat(prefStr + "_z", value.z);
    }

    #endregion

    #region Probe saving/loading

    /// <summary>
    /// Return an array with information about the positions of probes that were saved from the last session
    /// </summary>
    /// <returns></returns>
    public static (Vector3 apmldv, Vector3 angles, int type, string manipulatorId, string coordinateSpaceName, string
        coordinateTransformName, Vector4 zeroCoordinateOffset, float brainSurfaceOffset, bool dropToSurfaceWithDepth,
        Color color,
        string uuid)[] LoadSavedProbeData()
    {
        int probeCount = PlayerPrefs.GetInt("probecount", 0);

        var savedProbes =
            new (Vector3 apmldv, Vector3 angles,
                int type, string manipulatorId,
                string coordinateSpaceName, string coordinateTransformName,
                Vector4 zeroCoordinateOffset, float brainSurfaceOffset, bool dropToSurfaceWithDepth,
                Color color,
                string uuid)[probeCount];

        for (int i = 0; i < probeCount; i++)
        {
            float ap = PlayerPrefs.GetFloat("ap" + i);
            float ml = PlayerPrefs.GetFloat("ml" + i);
            float dv = PlayerPrefs.GetFloat("dv" + i);
            float phi = PlayerPrefs.GetFloat("phi" + i);
            float theta = PlayerPrefs.GetFloat("theta" + i);
            float spin = PlayerPrefs.GetFloat("spin" + i);
            int type = PlayerPrefs.GetInt("type" + i);
            var manipulatorId = PlayerPrefs.GetString("manipulator_id" + i);
            string coordSpaceName = PlayerPrefs.GetString("coord_space" + i);
            string coordTransName = PlayerPrefs.GetString("coord_trans" + i);
            var x = PlayerPrefs.GetFloat("x" + i);
            var y = PlayerPrefs.GetFloat("y" + i);
            var z = PlayerPrefs.GetFloat("z" + i);
            var d = PlayerPrefs.GetFloat("d" + i);
            var brainSurfaceOffset = PlayerPrefs.GetFloat("brain_surface_offset" + i);
            var dropToSurfaceWithDepth = PlayerPrefs.GetInt("drop_to_surface_with_depth" + i) == 1;
            var color = new Color(PlayerPrefs.GetFloat("col_r" + i),
                PlayerPrefs.GetFloat("col_g" + i),
                PlayerPrefs.GetFloat("col_b" + i));
            string uuid = PlayerPrefs.GetString("uuid" + i);

            savedProbes[i] = (new Vector3(ap, ml, dv), new Vector3(phi, theta, spin),
                type, manipulatorId,
                coordSpaceName, coordTransName,
                new Vector4(x, y, z, d), brainSurfaceOffset, dropToSurfaceWithDepth,
                color,
                uuid);
        }

        return savedProbes;
    }

    /// <summary>
    /// Save the data about all of the probes passed in through allProbeData
    /// </summary>
    /// <param name="allProbeData">tip position, angles, and type for probes</param>
    public static void SaveCurrentProbeData(
        (Vector3 apmldv, Vector3 angles, int type, string manipulatorId, string coordinateSpace, string
            coordinateTransform, Vector4 zeroCoordinateOffset, float brainSurfaceOffset, bool dropToSurfaceWithDepth,
            Color color,
            string uuid)[] allProbeData)
    {
        for (int i = 0; i < allProbeData.Length; i++)
        {
            var currentProbeData = allProbeData[i];

            PlayerPrefs.SetFloat("ap" + i, currentProbeData.apmldv.x);
            PlayerPrefs.SetFloat("ml" + i, currentProbeData.apmldv.y);
            PlayerPrefs.SetFloat("dv" + i, currentProbeData.apmldv.z);
            PlayerPrefs.SetFloat("phi" + i, currentProbeData.angles.x);
            PlayerPrefs.SetFloat("theta" + i, currentProbeData.angles.y);
            PlayerPrefs.SetFloat("spin" + i, currentProbeData.angles.z);
            PlayerPrefs.SetInt("type" + i, currentProbeData.type);
            PlayerPrefs.SetString("manipulator_id" + i, currentProbeData.manipulatorId);
            PlayerPrefs.SetString("coord_space" + i, currentProbeData.coordinateSpace);
            PlayerPrefs.SetString("coord_trans" + i, currentProbeData.coordinateTransform);
            PlayerPrefs.SetFloat("x" + i, currentProbeData.zeroCoordinateOffset.x);
            PlayerPrefs.SetFloat("y" + i, currentProbeData.zeroCoordinateOffset.y);
            PlayerPrefs.SetFloat("z" + i, currentProbeData.zeroCoordinateOffset.z);
            PlayerPrefs.SetFloat("d" + i, currentProbeData.zeroCoordinateOffset.w);
            PlayerPrefs.SetFloat("brain_surface_offset" + i, allProbeData[i].brainSurfaceOffset);
            PlayerPrefs.SetInt("drop_to_surface_with_depth" + i,
                allProbeData[i].dropToSurfaceWithDepth ? 1 : 0);
            PlayerPrefs.SetFloat("col_r" + i, allProbeData[i].color.r);
            PlayerPrefs.SetFloat("col_g" + i, allProbeData[i].color.g);
            PlayerPrefs.SetFloat("col_b" + i, allProbeData[i].color.b);
            PlayerPrefs.SetString("uuid" + i, allProbeData[i].uuid);
        }

        PlayerPrefs.SetInt("probecount", allProbeData.Length);
        PlayerPrefs.SetString("timestamp",
            new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString("D16"));

        PlayerPrefs.Save();
    }

    #endregion
}