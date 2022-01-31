# Neuropixels Trajectory Planner

This is a tool for planning Neuropixels recordings with up to sixteen 1.0, 2.0, or up to four 4-shank 2.0 probes. Based on the trajectory explorer by Andy Peters https://github.com/petersaj/neuropixels_trajectory_explorer. 

Download the most recent version from the releases page:
![neuropixels trajectory planner](https://github.com/dbirman/NPTrajectoryPlanner/releases)

## Known issues

### CCF -> in vivo issues

The CCF coordinates that are returned by this tool are not identical to the target you will get in vivo. The CCF brain appears to be rotated slightly (tilted backward) and squashed along the DV axis (about 95% of in vivo size). Once these issues are resolved Andy and I plan to update both tools to correctly account for this error.

## Install

Download entire folder at this link: https://drive.google.com/drive/folders/1QfUc2-Q9fWa_ncNJnP0fzvpdwQkYC3LF?usp=sharing

Run NPTrajectoryPlanner.exe the 25 um CCF 2017 atlas is included. Email Dan (dbirman@uw.edu) if you need a Mac executable.

## Setting up a probe

To set up a new probe, select the button corresponding to the probe type (NP1/NP2/NP2.4). By default the probe is set to the IBL bregma coordinate (CCF: AP 5.4f, ML 5.739f, DV 0.332f) and the AP/ML distance is relative to that position.

To target a specific brain area it's best to place your probe in that brain area by adjusting the translation first, then adjust the rotations, then drive your probe into the brain -- in the same way that you would do a rea recording. Right now, rotations go **around** the insertion position (see roadmap for details).

### Translation

Use [W/A/S/D] to move the probe to the insertion point. Hold **shift** to move faster along any axis.

Once your probe is at the insertion point, adjust the rotation angles.

### Azimuth

Azimuth is the angle of the probe manipulator relative to the brain. Use [Q/E] to control azimuth.

![Azimuth example](https://github.com/dbirman/NPTrajectoryPlanner/raw/main/Images/azimuth.gif)

### Elevation

Elevation is the angle of the probe on the manipulator, and is restricted to the range 0 (horizontal) to 90 (vertical). Use [R/F] to control elevation.

![Azimuth example](https://github.com/dbirman/NPTrajectoryPlanner/raw/main/Images/elevation.gif)

### Depth

Use [Z/X] to insert the probe.

Note that the rotation point is the insertion coordinate with depth==0 (i.e. the point the tip was at before you inserted the probe). I'll change this in a future release, but for now if you rotate the probe after inserting it will rotate around that insertion point.

### Recording region

Once the probe is at the position and angles you want, change the recording region size (in the settings) and position (using [T/G]) to match what you plan to do in your recording and adjust the insertion depth accordingly.

### Export coordinates

Clicking on the coordinates shown at the bottom of the screen copies them to the clipboard.

## Settings

**Reset active probe** - Returns the active probe to the original starting position (Bregma by default)

**Spawn IBL probes** - Places two NP1.0 probes in the configuration used by the IBL

**Probe collisions** - Prevents probe shanks and holders from intersecting when moved

**Set (0,0,0) to Bregma** - On: smpets the 0,0,0 coordinate to Bregma, or to the 0,0,0 CCF coordinate when unchecked (default: on)

**Depth from brain surface** - On: measures insertion depth from the brain surface, Off: from the DV=0 plane (default: on)

**Sagittal/Coronal slices** - On: displays slices aligned to the probe tip (default: off)

**Display area acronyms** - On: show only acronyms for brain areas (default: off)

**Areas include layers** - On: include the layers (e.g. in cortex) (default: on)

**Rig** - Dropdown with options for rigs to display

**Recording region only** - On: display only the areas within the recording region, Off: show the areas along the whole probe shank (default: on)

**Recording region size** - Slider controls the size of the recording region. Defaults to a set of options that depend on the probe.

**Display In-Plane Slice** - On: shows a slice of cortex that is "in-plane" with the probe, note that this uses a camera that is looking at the probe from the front of the brain towards the back (default: on)

**Convert AP/ML to probe** - On: when the probe manipulator is off of the 0/90 axis the AP/ML positions are not useful for calculating the insertion point relative to Bregma, turn this on to display the position along the probe forward/side axes (default: off)

## Bugs

Please report issues on the Github issues page.

## References

CCF Atlas downloaded from http://download.alleninstitute.org/informatics-archive/current-release/mouse_ccf/annotation/ 
## Version Changes

[0.1.10] Added an option that lets you switch betweeen the AP/ML axes and the forward/side axes of the probe itself (easier for planning insertions when your probe is not perpendicular to the brain)
[0.1.9] Added "in-plane" slice view

[0.1] Parity with the existing allenCCF Matlab tools (see: https://github.com/cortex-lab/allenCCF) with additional tools for multiple probes and collisions
