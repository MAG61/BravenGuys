using UnityEditor;

[CustomEditor(typeof(Obstacle))]
public class ObstacleEditor : Editor
{
    #region SerializedProperties
    SerializedProperty obstacleType;

    SerializedProperty torque;
    SerializedProperty torqueVector;
    SerializedProperty Euler;

    SerializedProperty maxAngle;
    SerializedProperty turnSpeed;

    SerializedProperty firstPos;
    SerializedProperty lastPos;
    SerializedProperty speed;
    SerializedProperty axis;
    SerializedProperty minWait;
    SerializedProperty maxWait;

    SerializedProperty muzzle;
    SerializedProperty power;
    SerializedProperty ballVector;

    SerializedProperty bouncePower;
    #endregion

    bool turningGroup = true, hangingGroup = true, pos2PosGroup = true, cannonGroup = true, bouncerGroup = true;

    private void OnEnable()
    {
        obstacleType = serializedObject.FindProperty("obstacleType");

        torque = serializedObject.FindProperty("torque");
        torqueVector = serializedObject.FindProperty("torqueVector");
        Euler = serializedObject.FindProperty("Euler");

        maxAngle = serializedObject.FindProperty("maxAngle");
        turnSpeed = serializedObject.FindProperty("turnSpeed");

        firstPos = serializedObject.FindProperty("firstPos");
        lastPos = serializedObject.FindProperty("lastPos");
        speed = serializedObject.FindProperty("speed");
        axis = serializedObject.FindProperty("axis");
        minWait = serializedObject.FindProperty("minWait");
        maxWait = serializedObject.FindProperty("maxWait");

        muzzle = serializedObject.FindProperty("muzzle");
        power = serializedObject.FindProperty("power");
        ballVector = serializedObject.FindProperty("ballVector");

        bouncePower = serializedObject.FindProperty("bouncePower");
    }

    public override void OnInspectorGUI()
    {
        Obstacle _obstacle = (Obstacle)target;

        serializedObject.Update();

        EditorGUILayout.PropertyField(obstacleType);

        if (_obstacle.obstacleType == Obstacle.ObstacleType.Turning)
        {
            turningGroup = EditorGUILayout.BeginFoldoutHeaderGroup(turningGroup, "Turning Settings");

            EditorGUILayout.PropertyField(Euler);
            EditorGUILayout.PropertyField(torque);
            EditorGUILayout.PropertyField(torqueVector);
        }

        if (_obstacle.obstacleType == Obstacle.ObstacleType.Hanging)
        {
            hangingGroup = EditorGUILayout.BeginFoldoutHeaderGroup(hangingGroup, "Hanging Settings");

            EditorGUILayout.PropertyField(maxAngle);
            EditorGUILayout.PropertyField(turnSpeed);

        }

        if (_obstacle.obstacleType == Obstacle.ObstacleType.Pos2Pos)
        {
            pos2PosGroup = EditorGUILayout.BeginFoldoutHeaderGroup(pos2PosGroup, "Pos2Pos Settings");

            EditorGUILayout.PropertyField(firstPos);
            EditorGUILayout.PropertyField(lastPos);
            EditorGUILayout.PropertyField(axis);
            EditorGUILayout.PropertyField(speed);
            EditorGUILayout.PropertyField(minWait);
            EditorGUILayout.PropertyField(maxWait);
        }

        if (_obstacle.obstacleType == Obstacle.ObstacleType.Cannon)
        {
            cannonGroup = EditorGUILayout.BeginFoldoutHeaderGroup(cannonGroup, "Cannon Settings");

            EditorGUILayout.PropertyField(muzzle);
            EditorGUILayout.PropertyField(power);
            EditorGUILayout.PropertyField(ballVector);
        }

        if (_obstacle.obstacleType == Obstacle.ObstacleType.Bouncer)
        {
            bouncerGroup = EditorGUILayout.BeginFoldoutHeaderGroup(bouncerGroup, "Bouncer Settings");
            EditorGUILayout.PropertyField(bouncePower);
        }

        serializedObject.ApplyModifiedProperties();

    }
}
