using UnityEngine;

/*
 * This script has some Math utils functions
 */

public static class CMath
{
    //Returns the angle between 2 vector (0 to 360)
    public static float AngleBetweenVector2(Vector2 vec1, Vector2 vec2)
    {
        Vector2 diference = vec2 - vec1;
        float sign = (vec2.y < vec1.y) ? -1.0f : 1.0f;
        float angle = Vector2.Angle(Vector2.right, diference) * sign;
        if (angle < 0) { angle += 360; }
        return angle;
    }

    //Returns the world mouse position (requires a trigger collider)
    public static Vector3 GetMouseWorldPos()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 1000f, 1 << 9))
        {
            return hit.point;
        }

        return Vector3.zero;
    }

    //Returns the Y rotation to look from vector A to vector B
    public static Vector3 LookToY(Vector3 from, Vector3 to)
    {
        return new Vector3(to.x, from.y, to.z);
    }
}
