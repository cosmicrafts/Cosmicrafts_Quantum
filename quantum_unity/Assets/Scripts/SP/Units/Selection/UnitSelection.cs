using UnityEngine;
using System.Collections.Generic;

namespace CosmicraftsSP
{
    public class UnitSelection : MonoBehaviour
    {
        private Unit selectedUnit;
        private List<Unit> selectedUnits = new List<Unit>(); // List of selected units

        [Header("Selection Visuals")]
        public GameObject selectionEffectPrefab; // Drag your selection effect (e.g., an outline prefab) here
        public RectTransform selectionBox; // UI element for the selection box

        private GameObject currentSelectionEffect;
        private List<GameObject> currentSelectionEffects = new List<GameObject>(); // List to store current selection effects

        [Header("Raycast Settings")]
        public LayerMask unitLayerMask; // Assign the 3D objects layer here in the Inspector
        public LayerMask groundLayerMask; // Layer mask for the ground or walkable area

        private Vector3 startMousePosition;
        private Vector3 endMousePosition;
        private bool isDragging = false;

        void Update()
        {
            HandleMouseInput();

            // Log if selection is active and which unit is selected
            if (selectedUnit != null)
            {
                Debug.Log($"Selected Unit: {selectedUnit.name} (ID: {selectedUnit.getId()})");
            }
        }

        void HandleMouseInput()
        {
            // Start selection when the mouse button is pressed
            if (Input.GetMouseButtonDown(0))
            {
                startMousePosition = Input.mousePosition;
                isDragging = true;

                // Show selection box UI
                if (selectionBox != null)
                {
                    selectionBox.gameObject.SetActive(true);
                }
            }

            // Update the selection box as the mouse is dragged
            if (isDragging)
            {
                endMousePosition = Input.mousePosition;
                UpdateSelectionBox();
            }

            // Finish selection when the mouse button is released
            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
                SelectUnitsInArea();
                // Hide the selection box
                if (selectionBox != null)
                {
                    selectionBox.gameObject.SetActive(false);
                }
            }

            // Right-click to issue a move or attack command
            if (Input.GetMouseButtonDown(1) && selectedUnits.Count > 0)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, unitLayerMask | groundLayerMask))
                {
                    Unit targetUnit = hit.collider.GetComponent<Unit>();

                    if (targetUnit != null && targetUnit.MyTeam != selectedUnit.MyTeam)
                    {
                        // Command to attack
                        foreach (Unit unit in selectedUnits)
                        {
                            CommandAttack(unit, targetUnit);
                        }
                    }
                    else
                    {
                        // Command to move
                        foreach (Unit unit in selectedUnits)
                        {
                            CommandMove(unit, hit.point);
                        }
                    }
                }
            }
        }

        void UpdateSelectionBox()
        {
            if (selectionBox == null) return;

            // Calculate the width and height of the selection box
            float width = endMousePosition.x - startMousePosition.x;
            float height = endMousePosition.y - startMousePosition.y;

            // Set the position and size of the selection box UI
            selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
            selectionBox.anchoredPosition = startMousePosition + new Vector3(width / 2, height / 2);
        }

        void SelectUnitsInArea()
        {
            Vector3 startScreenPosition = startMousePosition;
            Vector3 endScreenPosition = endMousePosition;

            // Calculate the bounding box from the mouse drag positions
            Rect selectionRect = new Rect(
                Mathf.Min(startScreenPosition.x, endScreenPosition.x),
                Mathf.Min(startScreenPosition.y, endScreenPosition.y),
                Mathf.Abs(startScreenPosition.x - endScreenPosition.x),
                Mathf.Abs(startScreenPosition.y - endScreenPosition.y)
            );

            // Clear previously selected units
            DeselectAllUnits();

            // Find all units within the selection area
            foreach (Unit unit in FindObjectsOfType<Unit>())
            {
                Vector3 unitScreenPosition = Camera.main.WorldToScreenPoint(unit.transform.position);

                if (selectionRect.Contains(unitScreenPosition) && unit.MyTeam == Team.Blue)
                {
                    selectedUnits.Add(unit);
                    if (selectionEffectPrefab != null)
                    {
                        GameObject effectInstance = Instantiate(selectionEffectPrefab, unit.transform.position, Quaternion.identity);
                        effectInstance.transform.SetParent(unit.transform); // Attach the effect to the unit
                        currentSelectionEffects.Add(effectInstance); // Store the effect instance
                    }

                    Debug.Log($"Unit Selected: {unit.name} (ID: {unit.getId()})");
                }
            }
        }

        void DeselectAllUnits()
        {
            // Destroy all current selection effects
            foreach (GameObject effect in currentSelectionEffects)
            {
                Destroy(effect);
            }
            currentSelectionEffects.Clear();

            selectedUnits.Clear();
        }

        void CommandMove(Unit unit, Vector3 destination)
        {
            Ship ship = unit.GetComponent<Ship>();
            if (ship != null)
            {
                ship.SetDestination(destination, ship.StoppingDistance);

                // Log the move command
                Debug.Log($"Commanded {unit.name} (ID: {unit.getId()}) to move to {destination}");
            }
        }

        void CommandAttack(Unit attacker, Unit target)
        {
            Shooter shooter = attacker.GetComponent<Shooter>();
            if (shooter != null)
            {
                shooter.SetTarget(target);

                // Log the attack command
                Debug.Log($"Commanded {attacker.name} (ID: {attacker.getId()}) to attack {target.name} (ID: {target.getId()})");
            }
        }

        public Unit GetSelectedUnit()
        {
            return selectedUnit;
        }
    }
}
