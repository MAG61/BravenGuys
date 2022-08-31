using UnityEditor;

[CustomEditor(typeof(Obstacle))]
public class ObstacleEditor : Editor
{
    #region SerializedProperties
    SerializedProperty obstacleType;

    SerializedProperty torque;
    SerializedProperty maxTorque;
    SerializedProperty torqueVector;
    SerializedProperty Euler;

    SerializedProperty firstPos;
    SerializedProperty lastPos;
    SerializedProperty speed;
    SerializedProperty axis;
    SerializedProperty minWait;
    SerializedProperty maxWait;

    SerializedProperty muzzle;
    SerializedProperty power;
    SerializedProperty ballVector;
    #endregion

    bool turningGroup, hangingGroup, pos2PosGroup, cannonGroup = true;

    private void OnEnable()
    {
        obstacleType = serializedObject.FindProperty("obstacleType");

        torque = serializedObject.FindProperty("torque");
        maxTorque = serializedObject.FindProperty("maxTorque");
        torqueVector = serializedObject.FindProperty("torqueVector");
        Euler = serializedObject.FindProperty("Euler");

        firstPos = serializedObject.FindProperty("firstPos");
        lastPos = serializedObject.FindProperty("lastPos");
        speed = serializedObject.FindProperty("speed");
        axis = serializedObject.FindProperty("axis");
        minWait = serializedObject.FindProperty("minWait");
        maxWait = serializedObject.FindProperty("maxWait");

        muzzle = serializedObject.FindProperty("muzzle");
        power = serializedObject.FindProperty("power");
        ballVector = serializedObject.FindProperty("ballVector");
    }

    public override void OnInspectorGUI()
    {
        Obstacle _obstacle = (Obstacle)target;

        serializedObject.Update();

        EditorGUILayout.PropertyField(obstacleType);

        if (_obstacle.obstacleType == Obstacle.ObstacleType.Turning)
        {
            turningGroup = EditorGUILayout.BeginFoldoutHeaderGroup(turningGroup, "Turning Settings");
            if (turningGroup)
            {
                EditorGUILayout.PropertyField(Euler);
                EditorGUILayout.PropertyField(torque);
                EditorGUILayout.PropertyField(maxTorque);
                EditorGUILayout.PropertyField(torqueVector);
            }
        }

        if (_obstacle.obstacleType == Obstacle.ObstacleType.Hanging)
        {
            hangingGroup = EditorGUILayout.BeginFoldoutHeaderGroup(hangingGroup, "Hanging Settings");
            if (hangingGroup)
            {
            }
        }

        if (_obstacle.obstacleType == Obstacle.ObstacleType.Pos2Pos)
        {
            pos2PosGroup = EditorGUILayout.BeginFoldoutHeaderGroup(pos2PosGroup, "Pos2Pos Settings");
            if (pos2PosGroup)
            {
                EditorGUILayout.PropertyField(firstPos);
                EditorGUILayout.PropertyField(lastPos);
                EditorGUILayout.PropertyField(axis);
                EditorGUILayout.PropertyField(speed);
                EditorGUILayout.PropertyField(minWait);
                EditorGUILayout.PropertyField(maxWait);
            }
        }

        if (_obstacle.obstacleType == Obstacle.ObstacleType.Cannon)
        {
            cannonGroup = EditorGUILayout.BeginFoldoutHeaderGroup(cannonGroup, "Cannon Settings");
            if (cannonGroup)
            {
                EditorGUILayout.PropertyField(muzzle);
                EditorGUILayout.PropertyField(power);
                EditorGUILayout.PropertyField(ballVector);
            }
        }

        serializedObject.ApplyModifiedProperties();

    }
}
