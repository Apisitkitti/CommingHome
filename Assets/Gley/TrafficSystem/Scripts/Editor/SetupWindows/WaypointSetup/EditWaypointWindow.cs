using Gley.TrafficSystem.Internal;
using Gley.UrbanAssets.Editor;
using Gley.UrbanAssets.Internal;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Gley.TrafficSystem.Editor
{
    public class EditWaypointWindow : EditWaypointWindowBase<WaypointSettings>
    {
        private TrafficSettingsLoader settingsLoader;
        private int maxSpeed;
        private int priority;
        private int penalty;
        private WaypointDrawer waypointDrawer;


        public override ISetupWindow Initialize(WindowProperties windowProperties, SettingsWindowBase window)
        {
            base.Initialize(windowProperties, window);
            maxSpeed = selectedWaypoint.maxSpeed;
            priority = selectedWaypoint.priority;
            penalty = selectedWaypoint.penalty;
            settingsLoader = SetSettingsLoader();
            roadColors = settingsLoader.LoadRoadColors();
            return this;
        }


        protected override void TopPart()
        {
            base.TopPart();
            EditorGUI.BeginChangeCheck();
            roadColors.selectedWaypointColor = EditorGUILayout.ColorField("Selected Color ", roadColors.selectedWaypointColor);
            roadColors.waypointColor = EditorGUILayout.ColorField("Neighbor Color ", roadColors.waypointColor);
            roadColors.laneChangeColor = EditorGUILayout.ColorField("Lane Change Color ", roadColors.laneChangeColor);
            roadColors.prevWaypointColor = EditorGUILayout.ColorField("Previous Color ", roadColors.prevWaypointColor);
            roadColors.complexGiveWayColor = EditorGUILayout.ColorField("Required Free Waypoints ", roadColors.complexGiveWayColor);

            EditorGUI.EndChangeCheck();
            if (GUI.changed)
            {
                SceneView.RepaintAll();
            }

            if (GUILayout.Button("Select Waypoint"))
            {
                Selection.activeGameObject = selectedWaypoint.gameObject;
            }

            base.TopPart();
        }


        protected override void SetCars()
        {
            List<VehicleTypes> result = new List<VehicleTypes>();
            for (int i = 0; i < carDisplay.Length; i++)
            {
                if (carDisplay[i].active)
                {
                    result.Add((VehicleTypes)carDisplay[i].car);
                }
            }
            selectedWaypoint.allowedCars = result;
            if (result.Count > 0)
            {
                selectedWaypoint.carsLocked = true;
            }
            else
            {
                selectedWaypoint.carsLocked = false;
            }
            List<WaypointSettings> waypointList = new List<WaypointSettings>();
            SetCarType(waypointList, selectedWaypoint.allowedCars, selectedWaypoint.neighbors);
        }


        private void SetCarType(List<WaypointSettings> waypointList, List<VehicleTypes> carTypes, List<WaypointSettingsBase> neighbors)
        {
            if (carTypes == null || carTypes.Count == 0)
            {
                return;
            }

            for (int i = 0; i < neighbors.Count; i++)
            {
                WaypointSettings neighbor = (WaypointSettings)neighbors[i];
                if (!waypointList.Contains(neighbor))
                {
                    if (!neighbor.carsLocked)
                    {
                        waypointList.Add(neighbor);
                        neighbor.allowedCars = carTypes;
                        EditorUtility.SetDirty(neighbors[i]);
                        SetCarType(waypointList, carTypes, neighbors[i].neighbors);
                    }
                }
            }
        }


        public override void DrawInScene()
        {
            base.DrawInScene();

            if (selectedList != ListToAdd.None)
            {
                waypointDrawer.DrawWaypointsForLink(selectedWaypoint, selectedWaypoint.neighbors, selectedWaypoint.otherLanes, roadColors.waypointColor, roadColors.speedColor);
            }

            waypointDrawer.DrawCurrentWaypoint(selectedWaypoint, roadColors.selectedWaypointColor, roadColors.waypointColor, roadColors.laneChangeColor, roadColors.prevWaypointColor, roadColors.complexGiveWayColor);

            for (int i = 0; i < carDisplay.Length; i++)
            {
                if (carDisplay[i].view)
                {
                    waypointDrawer.DrawWaypointsForCar(carDisplay[i].car, carDisplay[i].color);
                }
            }

            if (clickedWaypoint)
            {
                waypointDrawer.DrawSelectedWaypoint(clickedWaypoint, roadColors.selectedRoadConnectorColor);
            }
        }


        internal override void DrawCarSettings()
        {
            selectedWaypoint.giveWay = EditorGUILayout.Toggle(new GUIContent("Give Way", "Vehicle will stop when reaching this waypoint and check if next waypoint is free before continuing"), selectedWaypoint.giveWay);

            EditorGUILayout.BeginHorizontal();
            selectedWaypoint.complexGiveWay = EditorGUILayout.Toggle(new GUIContent("Complex Give Way", "Vehicle will stop when reaching this waypoint check if all selected waypoints are free before continue"), selectedWaypoint.complexGiveWay);
            if (selectedWaypoint.complexGiveWay)
            {
                if (GUILayout.Button("Pick Required Free Waypoints"))
                {
                    //PickFreeWaypoints();
                    selectedList = ListToAdd.GiveWayWaypoints;
                }
            }
            EditorGUILayout.EndHorizontal();

            selectedWaypoint.zipperGiveWay = EditorGUILayout.Toggle(new GUIContent("Zipper Give Way", "Vehicles will stop before reaching this waypoint and continue randomly one at the time"), selectedWaypoint.zipperGiveWay);
            selectedWaypoint.triggerEvent = EditorGUILayout.Toggle(new GUIContent("Trigger Event", "If a vehicle reaches this, it will trigger an event"), selectedWaypoint.triggerEvent);
            if(selectedWaypoint.triggerEvent==true)
            {
                selectedWaypoint.eventData = EditorGUILayout.TextField(new GUIContent("Event Data", "This string will be sent as a parameter for the event"), selectedWaypoint.eventData);
            }

            EditorGUILayout.BeginHorizontal();
            maxSpeed = EditorGUILayout.IntField(new GUIContent("Max speed", "The maximum speed allowed in this waypoint"), maxSpeed);
            if (GUILayout.Button("Set Speed"))
            {
                if (maxSpeed != 0)
                {
                    selectedWaypoint.speedLocked = true;
                }
                else
                {
                    selectedWaypoint.speedLocked = false;
                }
                SetSpeed();
            }
            EditorGUILayout.EndHorizontal();

           


            EditorGUILayout.BeginHorizontal();
            priority = EditorGUILayout.IntField(new GUIContent("Spawn priority", "If the priority is higher, the vehicles will have higher chances to spawn on this waypoint"), priority);
            if (GUILayout.Button("Set Priority"))
            {
                if (priority != 0)
                {
                    selectedWaypoint.priorityLocked = true;
                }
                else
                {
                    selectedWaypoint.priorityLocked = false;
                }
                SetPriority();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            penalty = EditorGUILayout.IntField(new GUIContent("Waypoint penalty", "Used for path finding. If penalty is higher vehicles are less likely to pick this route"), penalty);
            if (GUILayout.Button("Set Penalty "))
            {
                if (penalty != 0)
                {
                    selectedWaypoint.penaltyLocked = true;
                }
                else
                {
                    selectedWaypoint.penaltyLocked = false;
                }
                SetPenalty();
            }
            EditorGUILayout.EndHorizontal();
        }


        protected override CarDisplay[] SetCarDisplay()
        {
            nrOfCars = System.Enum.GetValues(typeof(VehicleTypes)).Length;
            CarDisplay[] carDisplay = new CarDisplay[nrOfCars];
            for (int i = 0; i < nrOfCars; i++)
            {
                carDisplay[i] = new CarDisplay(selectedWaypoint.allowedCars.Contains((VehicleTypes)i), i, Color.white);
            }
            return carDisplay;
        }


        internal override string SetLabels(int i)
        {
            return ((VehicleTypes)i).ToString();
        }


        internal override WaypointSettings SetSelectedWaypoint()
        {
            return SettingsWindow.GetSelectedWaypoint();
        }


        internal TrafficSettingsLoader SetSettingsLoader()
        {
            return new TrafficSettingsLoader(Internal.Constants.windowSettingsPath);
        }


        internal override WaypointDrawerBase<WaypointSettings> SetWaypointsDrawer()
        {
            waypointDrawer = CreateInstance<WaypointDrawer>();
            return waypointDrawer;
        }


        internal override void ViewWaypoint(WaypointSettingsBase waypoint)
        {
            clickedWaypoint = (WaypointSettings)waypoint;
            GleyUtilities.TeleportSceneCamera(waypoint.transform.position);
        }


        private void SetSpeed()
        {
            List<WaypointSettings> waypointList = new List<WaypointSettings>();
            selectedWaypoint.maxSpeed = maxSpeed;
            SetSpeed(waypointList, selectedWaypoint.maxSpeed, selectedWaypoint.neighbors.Cast<WaypointSettings>().ToList());
        }


        private void SetSpeed(List<WaypointSettings> waypointList, int speed, List<WaypointSettings> neighbors)
        {
            if (speed == 0)
            {
                return;
            }

            for (int i = 0; i < neighbors.Count; i++)
            {
                if (!waypointList.Contains(neighbors[i]))
                {
                    if (!neighbors[i].speedLocked)
                    {
                        waypointList.Add(neighbors[i]);
                        neighbors[i].maxSpeed = speed;
                        EditorUtility.SetDirty(neighbors[i]);
                        SetSpeed(waypointList, speed, neighbors[i].neighbors.Cast<WaypointSettings>().ToList());
                    }
                }
            }
        }


        private void SetPriority()
        {
            List<WaypointSettings> waypointList = new List<WaypointSettings>();
            selectedWaypoint.priority = priority;
            SetPriority(waypointList, selectedWaypoint.priority, selectedWaypoint.neighbors.Cast<WaypointSettings>().ToList());
        }


        private void SetPriority(List<WaypointSettings> waypointList, int priority, List<WaypointSettings> neighbors)
        {
            if (priority == 0)
            {
                return;
            }

            for (int i = 0; i < neighbors.Count; i++)
            {
                if (!waypointList.Contains(neighbors[i]))
                {
                    if (!neighbors[i].priorityLocked)
                    {
                        waypointList.Add(neighbors[i]);
                        neighbors[i].priority = priority;
                        EditorUtility.SetDirty(neighbors[i]);
                        SetPriority(waypointList, priority, neighbors[i].neighbors.Cast<WaypointSettings>().ToList());
                    }
                }
            }
        }


        private void SetPenalty()
        {
            List<WaypointSettings> waypointList = new List<WaypointSettings>();
            selectedWaypoint.penalty = penalty;
            SetPenalty(waypointList, selectedWaypoint.penalty, selectedWaypoint.neighbors.Cast<WaypointSettings>().ToList());
        }


        private void SetPenalty(List<WaypointSettings> waypointList, int penalty, List<WaypointSettings> neighbors)
        {
            for (int i = 0; i < neighbors.Count; i++)
            {
                if (!waypointList.Contains(neighbors[i]))
                {
                    if (!neighbors[i].penaltyLocked)
                    {  
                        waypointList.Add(neighbors[i]);
                        neighbors[i].penalty = penalty;
                        EditorUtility.SetDirty(neighbors[i]);
                        SetPenalty(waypointList, penalty, neighbors[i].neighbors.Cast<WaypointSettings>().ToList());
                    }
                    else
                    {
                        Debug.Log(neighbors[i].name + " is penalty locked");
                    }
                }
            }
        }


        protected override void OpenEditWaypointWindow()
        {
            window.SetActiveWindow(typeof(EditWaypointWindow), false);
        }


        protected override GUIContent SetAllowedAgentsText()
        {
            return new GUIContent("Allowed vehicles: ", "Only the following vehicles can pass through this waypoint");
        }


        protected override void ShowOtherLanes()
        {
            EditorGUILayout.Space();
            MakeListOperations("Other Lanes", "Connections to other lanes, used for overtaking", selectedWaypoint.otherLanes, ListToAdd.OtherLanes);
        }


        protected override void PickFreeWaypoints()
        {
            EditorGUILayout.Space();
            MakeListOperations("Pick Required Free Waypoints", "Waypoints required to be free for Complex Give Way", selectedWaypoint.giveWayList, ListToAdd.GiveWayWaypoints);
        }


        public override void DestroyWindow()
        {
            settingsLoader.SaveRoadColors(roadColors);
            base.DestroyWindow();
        }
    }
}
