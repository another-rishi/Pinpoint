using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TrajectoryPlanner.UI.EphysCopilot
{
    public class EphysCopilotHandler : MonoBehaviour
    {
        #region Internal UI Functions

        #region Step 1

        private void AddResetZeroCoordinatePanel(ProbeManager probeManager)
        {
            // Instantiate
            var resetZeroCoordinatePanelGameObject = Instantiate(
                _zeroCoordinatePanel.ResetZeroCoordinatePanelPrefab,
                _zeroCoordinatePanel.PanelScrollViewContent.transform);
            var resetZeroCoordinatePanelHandler =
                resetZeroCoordinatePanelGameObject.GetComponent<ResetZeroCoordinatePanelHandler>();
            _panels.Add(resetZeroCoordinatePanelGameObject);

            // Setup
            resetZeroCoordinatePanelHandler.ProbeManager = probeManager;
        }

        #endregion

        #region Step 2

        private void AddInsertionSelectionPanel(ProbeManager probeManager)
        {
            // Instantiate
            var insertionSelectionPanelGameObject = Instantiate(_gotoPanel.InsertionSelectionPanelPrefab,
                _gotoPanel.PanelScrollViewContent.transform);
            var insertionSelectionPanelHandler =
                insertionSelectionPanelGameObject.GetComponent<InsertionSelectionPanelHandler>();
            _panels.Add(insertionSelectionPanelGameObject);

            // Setup
            insertionSelectionPanelHandler.ProbeManager = probeManager;
        }

        #endregion

        #region Step 3

        private void AddResetDuraOffsetPanel(ProbeManager probeManager)
        {
            // Instantiate
            var resetDuraPanelGameObject = Instantiate(_duraOffsetPanel.ResetDuraOffsetPanelPrefab,
                _duraOffsetPanel.PanelScrollViewContent.transform);
            var resetDuraPanelHandler = resetDuraPanelGameObject.GetComponent<ResetDuraOffsetPanelHandler>();


            _panels.Add(resetDuraPanelGameObject);


            // Setup
            resetDuraPanelHandler.ProbeManager = probeManager;
        }

        #endregion

        #region Step 4

        private void AddDrivePanel(ProbeManager probeManager)
        {
            var addDrivePanelGameObject =
                Instantiate(_drivePanel.DrivePanelPrefab, _drivePanel.PanelScrollViewContent.transform);
            var drivePanelHandler = addDrivePanelGameObject.GetComponent<DrivePanelHandler>();
            _panels.Add(addDrivePanelGameObject);

            // Setup
            drivePanelHandler.ProbeManager = probeManager;
        }

        #endregion

        #endregion

        #region Components

        #region Step 1

        [Serializable]
        private class ZeroCoordinatePanelComponents
        {
            public GameObject ResetZeroCoordinatePanelPrefab;
            public GameObject PanelScrollViewContent;
        }

        [SerializeField] private ZeroCoordinatePanelComponents _zeroCoordinatePanel;

        #endregion

        #region Step 2

        [Serializable]
        private class GotoPanelComponents
        {
            public GameObject InsertionSelectionPanelPrefab;
            public GameObject PanelScrollViewContent;
        }

        [SerializeField] private GotoPanelComponents _gotoPanel;

        #endregion

        #region Step 3

        [Serializable]
        private class DuraOffsetPanelComponents
        {
            public GameObject ResetDuraOffsetPanelPrefab;
            public GameObject PanelScrollViewContent;
        }

        [SerializeField] private DuraOffsetPanelComponents _duraOffsetPanel;

        #endregion

        #region Step 4

        [Serializable]
        private class DrivePanelComponents
        {
            public GameObject DrivePanelPrefab;
            public GameObject PanelScrollViewContent;
        }

        [SerializeField] private DrivePanelComponents _drivePanel;

        #endregion

        private readonly HashSet<GameObject> _panels = new();

        #endregion

        #region Properties

        public List<ProbeManager> ProbeManagers { private get; set; }
        public CCFAnnotationDataset AnnotationDataset { private get; set; }

        #endregion

        #region Unity

        private void OnEnable()
        {
            // Populate properties
            ProbeManagers = ProbeManager.Instances.Where(manager => manager.IsEphysLinkControlled).OrderBy(manager => manager.ManipulatorBehaviorController.ManipulatorID).ToList();
            AnnotationDataset = VolumeDatasetManager.AnnotationDataset;

            // Setup shared resources for panels
            InsertionSelectionPanelHandler.AnnotationDataset = AnnotationDataset;


            // Spawn panels
            foreach (var probeManager in ProbeManagers)
            {
                // Step 1
                AddResetZeroCoordinatePanel(probeManager);

                // Step 2
                AddInsertionSelectionPanel(probeManager);

                // Step 3
                AddResetDuraOffsetPanel(probeManager);

                // Step 4
                AddDrivePanel(probeManager);
            }
        }

        private void OnDisable()
        {
            foreach (var panel in _panels) Destroy(panel);
        }

        #endregion
    }
}