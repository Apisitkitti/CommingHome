using Gley.UrbanAssets.Internal;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Gley.UrbanAssets.Editor
{
    public abstract class EditWaypointWindowBase<T2> : SetupWindowBase where T2 : WaypointSettingsBase
    {
        protected struct CarDisplay
        {
            public Color color;
            public int car;
            public bool active;
            public bool view;

            public CarDisplay(bool active, int car, Color color)
            {
                this.active = active;
                this.car = car;
                this.color = color;
                view = false;
            }
        }


        protected enum ListToAdd
        {
            None,
            Neighbors,
            OtherLanes,
            GiveWayWaypoints
        }


        protected CarDisplay[] carDisplay;
        protected T2 selectedWaypoint;
        protected T2 clickedWaypoint;
        protected ListToAdd selectedList;
        protected int nrOfCars;
        protected RoadColors roadColors;

        private WaypointDrawerBase<T2> baseWaypointDrawer;
        private float scrollAdjustment = 223;

        protected abstract CarDisplay[] SetCarDisplay();
        internal abstract WaypointDrawerBase<T2> SetWaypointsDrawer();
        internal abstract T2 SetSelectedWaypoint();
        protected abstract void ShowOtherLanes();
        protected abstract void PickFreeWaypoints();
        protected abstract GUIContent SetAllowedAgentsText();
        internal abstract string SetLabels(int i);
        internal abstract void DrawCarSettings();
        protected abstract void SetCars();
        protected abstract void OpenEditWaypointWindow();
        internal abstract void ViewWaypoint(WaypointSettingsBase waypoint);


        public override ISetupWindow Initialize(WindowProperties windowProperties, SettingsWindowBase window)
        {
            base.Initialize(windowProperties, window);
            baseWaypointDrawer = SetWaypointsDrawer();
            baseWaypointDrawer.onWaypointClicked += WaypointClicked;
            baseWaypointDrawer.Initialize();

            selectedWaypoint = SetSelectedWaypoint();
          

            carDisplay = SetCarDisplay();
            return this;
        }


        protected override void ScrollPart(float width, float height)
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, false, GUILayout.Width(width - SCROLL_SPACE), GUILayout.Height(height - scrollAdjustment));
            EditorGUI.BeginChangeCheck();
            if (selectedList == ListToAdd.None)
            {
                DrawCarSettings();

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.LabelField(SetAllowedAgentsText(), EditorStyles.boldLabel);
                EditorGUILayout.Space();

                for (int i = 0; i < nrOfCars; i++)
                {
                    EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                    carDisplay[i].active = EditorGUILayout.Toggle(carDisplay[i].active, GUILayout.MaxWidth(20));
                    EditorGUILayout.LabelField(SetLabels(i));
                    carDisplay[i].color = EditorGUILayout.ColorField(carDisplay[i].color, GUILayout.MaxWidth(80));
                    Color oldColor = GUI.backgroundColor;
                    if (carDisplay[i].view)
                    {
                        GUI.backgroundColor = Color.green;
                    }
                    if (GUILayout.Button("View", GUILayout.MaxWidth(64)))
                    {
                        carDisplay[i].view = !carDisplay[i].view;
                    }
                    GUI.backgroundColor = oldColor;

                    EditorGUILayout.EndHorizontal();
                }
                if (GUILayout.Button("Set"))
                {
                    SetCars();
                }

                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();
            }

            EditorGUILayout.Space();
            MakeListOperations("Neighbors", "From this waypoint a moving agent can continue to the following ones", selectedWaypoint.neighbors, ListToAdd.Neighbors);
            ShowOtherLanes();
            if (selectedList == ListToAdd.GiveWayWaypoints)
            {
                PickFreeWaypoints();
            }
            EditorGUI.EndChangeCheck();
            if (GUI.changed)
            {
                SceneView.RepaintAll();
            }

            base.ScrollPart(width, height);
            GUILayout.EndScrollView();
        }


        public override void DestroyWindow()
        {
            if (selectedWaypoint)
            {
                EditorUtility.SetDirty(selectedWaypoint);
            }
           
            baseWaypointDrawer.onWaypointClicked -= WaypointClicked;
            base.DestroyWindow();
        }


        protected void MakeListOperations(string title, string description, List<WaypointSettingsBase> listToEdit, ListToAdd listType)
        {
            //if (listType == ListToAdd.GiveWayWaypoints)
            //    return;
            if (selectedList == listType || selectedList == ListToAdd.None)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.LabelField(new GUIContent(title, description), EditorStyles.boldLabel);
                EditorGUILayout.Space();
                for (int i = 0; i < listToEdit.Count; i++)
                {
                    if (listToEdit[i] == null)
                    {
                        listToEdit.RemoveAt(i);
                        i--;
                        continue;
                    }
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label(listToEdit[i].name);
                    Color oldColor = GUI.backgroundColor;
                    if (listToEdit[i] == clickedWaypoint)
                    {
                        GUI.backgroundColor = Color.green;
                    }
                    if (GUILayout.Button("View", GUILayout.MaxWidth(64)))
                    {
                        if (listToEdit[i] == clickedWaypoint)
                        {
                            clickedWaypoint = null;
                        }
                        else
                        {
                            ViewWaypoint(listToEdit[i]);
                        }
                    }
                    GUI.backgroundColor = oldColor;
                    if (GUILayout.Button("Delete", GUILayout.MaxWidth(64)))
                    {
                        DeleteWaypoint(listToEdit[i], listType);
                    }

                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.Space();
                if (selectedList == ListToAdd.None)
                {
                    if (GUILayout.Button("Add/Remove " + title))
                    {
                        baseWaypointDrawer.Initialize();
                        selectedList = listType;
                    }
                }
                else
                {
                    if (GUILayout.Button("Done"))
                    {
                        selectedList = ListToAdd.None;
                        SceneView.RepaintAll();
                    }
                }

                EditorGUILayout.Space();
                EditorGUILayout.EndVertical();
            }
        }


        private void WaypointClicked(WaypointSettingsBase clickedWaypoint, bool leftClick)
        {
            if (leftClick)
            {
                if (selectedList == ListToAdd.Neighbors)
                {
                    AddNeighbor(clickedWaypoint);
                }

                if (selectedList == ListToAdd.OtherLanes)
                {
                    AddOtherLanes(clickedWaypoint);
                }

                if (selectedList == ListToAdd.GiveWayWaypoints)
                {
                    AddGiveWayWaypoints(clickedWaypoint);
                }

                if (selectedList == ListToAdd.None)
                {
                    OpenEditWaypointWindow();
                }
            }
            SettingsWindowBase.TriggerRefreshWindowEvent();
        }


        private void DeleteWaypoint(WaypointSettingsBase waypoint, ListToAdd list)
        {
            switch (list)
            {
                case ListToAdd.Neighbors:
                    waypoint.prev.Remove(selectedWaypoint);
                    selectedWaypoint.neighbors.Remove(waypoint);
                    break;
                case ListToAdd.OtherLanes:
                    selectedWaypoint.otherLanes.Remove(waypoint);
                    break;
                case ListToAdd.GiveWayWaypoints:
                    selectedWaypoint.giveWayList.Remove(waypoint);
                    break;
            }
            clickedWaypoint = null;
            SceneView.RepaintAll();
        }


        private void AddNeighbor(WaypointSettingsBase neighbor)
        {
            if (!selectedWaypoint.neighbors.Contains(neighbor))
            {
                selectedWaypoint.neighbors.Add(neighbor);
                neighbor.prev.Add(selectedWaypoint);
            }
            else
            {
                neighbor.prev.Remove(selectedWaypoint);
                selectedWaypoint.neighbors.Remove(neighbor);
            }
        }


        private void AddOtherLanes(WaypointSettingsBase waypoint)
        {
            if (!selectedWaypoint.otherLanes.Contains(waypoint))
            {
                selectedWaypoint.otherLanes.Add(waypoint);
            }
            else
            {
                selectedWaypoint.otherLanes.Remove(waypoint);
            }
        }


        private void AddGiveWayWaypoints(WaypointSettingsBase waypoint)
        {
            if (!selectedWaypoint.giveWayList.Contains(waypoint))
            {
                selectedWaypoint.giveWayList.Add(waypoint);
            }
            else
            {
                selectedWaypoint.giveWayList.Remove(waypoint);
            }
        }
    }
}