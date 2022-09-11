using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
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
public class PlayerPrefs : MonoBehaviour
{
    // Settings
    private bool _collisions;
    private bool _recordingRegionOnly;
    private bool _useAcronyms;
    private bool _depthFromBrain;
    private bool convertAPML2probeAxis;
    private int slice3d;
    private bool inplane;
    private int invivoTransform;
    private bool useIBLAngles;
    private bool showSurfaceCoord;
    //private bool useIblBregma;
    private string _ephysLinkServerIp;
    private int _ephysLinkServerPort;
    private bool _axisControl;
    private bool _showAllProbePanels;
    private string _rightHandedManipulatorIds;
    private bool _useBeryl;

    [SerializeField] Toggle collisionsToggle;
    [SerializeField] Toggle recordingRegionToggle;
    [SerializeField] Toggle acronymToggle;
    [SerializeField] Toggle depthToggle;
    [SerializeField] Toggle probeAxisToggle;
    [SerializeField] TMP_Dropdown slice3dDropdown;
    [SerializeField] Toggle inplaneToggle;
    [SerializeField] TMP_Dropdown invivoDropdown;
    [SerializeField] Toggle iblAngleToggle;
    [SerializeField] Toggle surfaceToggle;
    //[SerializeField] Toggle bregmaToggle;
    [SerializeField] TMP_InputField ephysLinkServerIpInput;
    [SerializeField] TMP_InputField ephysLinkServerPortInput;
    [SerializeField] Toggle axisControlToggle;
    [SerializeField] Toggle showAllProbePanelsToggle;
    [SerializeField] Toggle useBerylToggle;
    

    /// <summary>
    /// On Awake() load the preferences and toggle the corresponding UI elements
    /// </summary>
    void Awake()
    {
        _collisions = LoadBoolPref("collisions", true);
        collisionsToggle.isOn = _collisions;

        //useIblBregma = LoadBoolPref("bregma", true);
        ////bregmaToggle.isOn = useIblBregma;

        _recordingRegionOnly = LoadBoolPref("recording", true);
        recordingRegionToggle.isOn = _recordingRegionOnly;

        _useAcronyms = LoadBoolPref("acronyms", true);
        acronymToggle.isOn = _useAcronyms;

        _depthFromBrain = LoadBoolPref("depth", true);
        depthToggle.isOn = _depthFromBrain;

        convertAPML2probeAxis = LoadBoolPref("probeaxis", false);
        probeAxisToggle.isOn = convertAPML2probeAxis;

        slice3d = LoadIntPref("slice3d", 0);
        slice3dDropdown.SetValueWithoutNotify(slice3d);

        inplane = LoadBoolPref("inplane", true);
        inplaneToggle.isOn = inplane;

        invivoTransform = LoadIntPref("stereotaxic", 1);
        invivoDropdown.SetValueWithoutNotify(invivoTransform);

        useIBLAngles = LoadBoolPref("iblangle", true);
        iblAngleToggle.isOn = useIBLAngles;

        showSurfaceCoord = LoadBoolPref("surface", true);
        surfaceToggle.isOn = showSurfaceCoord;
        
        _ephysLinkServerIp = LoadStringPref("ephys_link_ip", "localhost");
        ephysLinkServerIpInput.text = _ephysLinkServerIp;
        
        _ephysLinkServerPort = LoadIntPref("ephys_link_port", 8080);
        ephysLinkServerPortInput.text = _ephysLinkServerPort.ToString();

        _axisControl = LoadBoolPref("axis_control", false);
        axisControlToggle.isOn = _axisControl;

        _showAllProbePanels = LoadBoolPref("show_all_probe_panels", true);
        showAllProbePanelsToggle.isOn = _showAllProbePanels;

        _rightHandedManipulatorIds = LoadStringPref("right_handed_manipulator_ids", "");

        _useBeryl = LoadBoolPref("use_beryl", true);
        useBerylToggle.isOn = _useBeryl;
    }

    /// <summary>
    /// Return an array with information about the positions (in CCF) of probes that were saved from the last session
    /// </summary>
    /// <returns></returns>
    public (Vector3 tipPos, float depth, Vector3 angles, int type, int manipulatorId, Vector4 zeroCoordinateOffset,
        float brainSurfaceOffset, bool dropToSurfaceWithDepth)[] LoadSavedProbes()
    {
        int probeCount = UnityEngine.PlayerPrefs.GetInt("probecount", 0);

        var savedProbes =
            new (Vector3 tipPos, float depth, Vector3 angles, int type, int manipulatorId, Vector4 zeroCoordinateOffset,
                float brainSurfaceOffset, bool dropToSurfaceWithDepth)[probeCount];

        for (int i = 0; i < probeCount; i++)
        {
            float ap = UnityEngine.PlayerPrefs.GetFloat("ap" + i);
            float ml = UnityEngine.PlayerPrefs.GetFloat("ml" + i);
            float dv = UnityEngine.PlayerPrefs.GetFloat("dv" + i);
            float depth = UnityEngine.PlayerPrefs.GetFloat("depth" + i);
            float phi = UnityEngine.PlayerPrefs.GetFloat("phi" + i);
            float theta = UnityEngine.PlayerPrefs.GetFloat("theta" + i);
            float spin = UnityEngine.PlayerPrefs.GetFloat("spin" + i);
            int type = UnityEngine.PlayerPrefs.GetInt("type" + i);
            var manipulatorId = UnityEngine.PlayerPrefs.GetInt("manipulator_id" + i);
            var x = UnityEngine.PlayerPrefs.GetFloat("x" + i);
            var y = UnityEngine.PlayerPrefs.GetFloat("y" + i);
            var z = UnityEngine.PlayerPrefs.GetFloat("z" + i);
            var d = UnityEngine.PlayerPrefs.GetFloat("d" + i);
            var brainSurfaceOffset = UnityEngine.PlayerPrefs.GetFloat("brain_surface_offset" + i);
            var dropToSurfaceWithDepth = UnityEngine.PlayerPrefs.GetInt("drop_to_surface_with_depth" + i) == 1;

            savedProbes[i] = (new Vector3(ap, ml, dv), depth, new Vector3(phi, theta, spin), type, manipulatorId,
                new Vector4(x, y, z, d), brainSurfaceOffset, dropToSurfaceWithDepth);
        }

        return savedProbes;
    }

    #region Getters/Setters

    public void SetUseBeryl(bool state)
    {
        _useBeryl = state;
        UnityEngine.PlayerPrefs.SetInt("use_beryl", _useBeryl ? 1 : 0);
    }

    public bool GetUseBeryl()
    {
        return _useBeryl;
    }

    public void SetShowAllProbePanels(bool state)
    {
        _showAllProbePanels = state;
        UnityEngine.PlayerPrefs.SetInt("show_all_probe_panels", _showAllProbePanels ? 1 : 0);
    }

    public bool GetShowAllProbePanels()
    {
        return _showAllProbePanels;
    }

    public void SetAxisControl(bool state)
    {
        _axisControl = state;
        UnityEngine.PlayerPrefs.SetInt("axis_control", _axisControl ? 1 : 0);
    }

    public bool GetAxisControl()
    {
        return _axisControl;
    }

    public void SetSurfaceCoord(bool state)
    {
        showSurfaceCoord = state;
        UnityEngine.PlayerPrefs.SetInt("surface", showSurfaceCoord ? 1 : 0);
    }

    public bool GetSurfaceCoord()
    {
        return showSurfaceCoord;
    }

    public void SetUseIBLAngles(bool state)
    {
        useIBLAngles = state;
        UnityEngine.PlayerPrefs.SetInt("iblangle", useIBLAngles ? 1 : 0);
    }

    public bool GetUseIBLAngles()
    {
        return useIBLAngles;
    }

    public void SetStereotaxic(int state)
    {
        invivoTransform = state;
        UnityEngine.PlayerPrefs.SetInt("stereotaxic", invivoTransform);
    }

    public int GetStereotaxic()
    {
        return invivoTransform;
    }

    public void SetInplane(bool state)
    {
        inplane = state;
        UnityEngine.PlayerPrefs.SetInt("inplane", inplane ? 1 : 0);
    }

    public bool GetInplane()
    {
        return inplane;
    }

    public void SetSlice3D(int state)
    {
        slice3d = state;
        UnityEngine.PlayerPrefs.SetInt("slice3d", slice3d);
    }

    public int GetSlice3D()
    {
        return slice3d;
    }

    public void SetAPML2ProbeAxis(bool state)
    {
        convertAPML2probeAxis = state;
        UnityEngine.PlayerPrefs.SetInt("probeaxis", convertAPML2probeAxis ? 1 : 0);
    }

    public bool GetAPML2ProbeAxis()
    {
        return convertAPML2probeAxis;
    }

    public void SetDepthFromBrain(bool state)
    {
        _depthFromBrain = state;
        UnityEngine.PlayerPrefs.SetInt("depth", _depthFromBrain ? 1 : 0);
    }

    public bool GetDepthFromBrain()
    {
        return _depthFromBrain;
    }

    public void SetAcronyms(bool state)
    {
        _useAcronyms = state;
        UnityEngine.PlayerPrefs.SetInt("acronyms", _recordingRegionOnly ? 1 : 0);
    }

    public bool GetAcronyms()
    {
        return _useAcronyms;
    }

    public void SetRecordingRegionOnly(bool state)
    {
        _recordingRegionOnly = state;
        UnityEngine.PlayerPrefs.SetInt("recording", _recordingRegionOnly ? 1 : 0);
    }

    public bool GetRecordingRegionOnly()
    {
        return _recordingRegionOnly;
    }

    public void SetCollisions(bool toggleCollisions)
    {
        _collisions = toggleCollisions;
        UnityEngine.PlayerPrefs.SetInt("collisions", _collisions ? 1 : 0);
    }

    public bool GetCollisions()
    {
        return _collisions;
    }

    //public void SetBregma(bool useBregma)
    //{
    //    useIblBregma = useBregma;
    //    PlayerPrefs.SetInt("bregma", useIblBregma ? 1 : 0);
    //}

    public bool GetBregma()
    {
        return true;
        //return useIblBregma;
    }
    
    public string GetServerIp()
    {
        return _ephysLinkServerIp;
    }
    
    public int GetServerPort()
    {
        return _ephysLinkServerPort;
    }

    public HashSet<int> GetRightHandedManipulatorIds()
    {
        return _rightHandedManipulatorIds == "" ? new HashSet<int>(): Array.ConvertAll(_rightHandedManipulatorIds.Split(','), int.Parse).ToHashSet();
    }

    public bool IsLinkDataExpired()
    {
        var timestampString = UnityEngine.PlayerPrefs.GetString("timestamp");
        if (timestampString == "") return false;

        return new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds() - long.Parse(timestampString) >= 86400;
    }

    #endregion

    #region Helper functions for booleans/integers/strings
    private bool LoadBoolPref(string prefStr, bool defaultValue)
    {
        return UnityEngine.PlayerPrefs.HasKey(prefStr) ? UnityEngine.PlayerPrefs.GetInt(prefStr) == 1 : defaultValue;
    }

    private int LoadIntPref(string prefStr, int defaultValue)
    {
        return UnityEngine.PlayerPrefs.HasKey(prefStr) ? UnityEngine.PlayerPrefs.GetInt(prefStr) : defaultValue;
    }

    private string LoadStringPref(string prefStr, string defaultValue)
    {
        return UnityEngine.PlayerPrefs.HasKey(prefStr) ? UnityEngine.PlayerPrefs.GetString(prefStr) : defaultValue;
    }

    #endregion

    /// <summary>
    /// Save the data about all of the probes passed in through allProbeData in CCF coordinates, note that depth is ignored 
    /// </summary>
    /// <param name="allProbeData">tip position, angles, and type for probes in CCF coordinates</param>
    public void SaveCurrentProbeData(
        (float ap, float ml, float dv, float phi, float theta, float spin, int type, int manipulatorId, Vector4
            zeroCoordinateOffset, float brainSurfaceOffset, bool dropToSurfaceWithDepth)[] allProbeData)
    {
        for (int i = 0; i < allProbeData.Length; i++)
        {

            UnityEngine.PlayerPrefs.SetFloat("ap" + i, allProbeData[i].ap);
            UnityEngine.PlayerPrefs.SetFloat("ml" + i, allProbeData[i].ml);
            UnityEngine.PlayerPrefs.SetFloat("dv" + i, allProbeData[i].dv);
            UnityEngine.PlayerPrefs.SetFloat("phi" + i, allProbeData[i].phi);
            UnityEngine.PlayerPrefs.SetFloat("theta" + i, allProbeData[i].theta);
            UnityEngine.PlayerPrefs.SetFloat("spin" + i, allProbeData[i].spin);
            UnityEngine.PlayerPrefs.SetInt("type" + i, allProbeData[i].type);
            UnityEngine.PlayerPrefs.SetInt("manipulator_id" + i, allProbeData[i].manipulatorId);
            UnityEngine.PlayerPrefs.SetFloat("x" + i, allProbeData[i].zeroCoordinateOffset.x);
            UnityEngine.PlayerPrefs.SetFloat("y" + i, allProbeData[i].zeroCoordinateOffset.y);
            UnityEngine.PlayerPrefs.SetFloat("z" + i, allProbeData[i].zeroCoordinateOffset.z);
            UnityEngine.PlayerPrefs.SetFloat("d" + i, allProbeData[i].zeroCoordinateOffset.w);
            UnityEngine.PlayerPrefs.SetFloat("brain_surface_offset" + i, allProbeData[i].brainSurfaceOffset);
            UnityEngine.PlayerPrefs.SetInt("drop_to_surface_with_depth" + i,
                allProbeData[i].dropToSurfaceWithDepth ? 1 : 0);
        }
        UnityEngine.PlayerPrefs.SetInt("probecount", allProbeData.Length);
        UnityEngine.PlayerPrefs.SetString("timestamp",
            new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString("D16"));

        UnityEngine.PlayerPrefs.Save();
    }

    public void SaveEphysLinkConnectionData(string serverIp, int serverPort)
    {
        UnityEngine.PlayerPrefs.SetString("ephys_link_ip", serverIp);
        UnityEngine.PlayerPrefs.SetInt("ephys_link_port", serverPort);
        UnityEngine.PlayerPrefs.Save();
    }

    public void SaveRightHandedManipulatorIds(HashSet<int> manipulatorIds)
    {
        UnityEngine.PlayerPrefs.SetString("right_handed_manipulator_ids", string.Join(",", manipulatorIds));
        UnityEngine.PlayerPrefs.Save();
    }
}
