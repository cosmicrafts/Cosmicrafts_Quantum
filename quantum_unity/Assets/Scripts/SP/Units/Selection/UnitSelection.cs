using UnityEngine;

namespace CosmicraftsSP
{
    public class UnitSelection : MonoBehaviour
    {
        private Unit selectedUnit;

        [Header("Selection Visuals")]
        public GameObject selectionEffectPrefab; // Drag your selection effect (e.g., an outline prefab) here

        private GameObject currentSelectionEffect;

        void Update()
        {
            // Log to confirm that Update is being called
            Debug.Log("UnitSelection Update is running.");

            // Check for touch input on mobile or mouse input for testing on PC
            if (Input.GetMouseButtonDown(0)) // This works for both taps on mobile and left-click on PC
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                // Log to confirm that a ray is being cast
                Debug.Log("Raycast initiated.");

                if (Physics.Raycast(ray, out hit))
                {
                    // Log the object that was hit
                    Debug.Log($"Raycast hit: {hit.collider.gameObject.name}");

                    Unit unit = hit.collider.GetComponent<Unit>();
                    if (unit != null && unit.MyTeam == Team.Blue) // Assuming player controls the blue team
                    {
                        SelectUnit(unit);
                    }
                    else if (unit != null && selectedUnit != null && unit.MyTeam != selectedUnit.MyTeam)
                    {
                        // If tapping an enemy unit while an allied unit is selected
                        CommandAttack(selectedUnit, unit);
                    }
                    else
                    {
                        DeselectUnit();
                    }
                }
            }
        }

        void SelectUnit(Unit unit)
        {
            DeselectUnit(); // Deselect any previously selected unit

            selectedUnit = unit;
            if (selectionEffectPrefab != null)
            {
                currentSelectionEffect = Instantiate(selectionEffectPrefab, unit.transform);
            }

            // Log the selected unit
            Debug.Log($"Unit Selected: {unit.name} (ID: {unit.getId()})");
        }

        void DeselectUnit()
        {
            if (selectedUnit != null)
            {
                Debug.Log($"Unit Deselected: {selectedUnit.name} (ID: {selectedUnit.getId()})");
            }

            selectedUnit = null;
            if (currentSelectionEffect != null)
            {
                Destroy(currentSelectionEffect);
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
