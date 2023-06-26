using UnityEngine;

[CreateAssetMenu(fileName = "Config", menuName = "Slice", order = 0)]
public class LevelConfig : ScriptableObject
{
        public float height = 3;
        public float heightOffset = 0;

        public float minAngle = 180;
        public float maxAngle = 360;

        public Sliceble prefab;

        public float moveSpeed;
        public float maxSliceWidth;
}