using UnityEngine;
using System;
using System.Collections;

public class HeadTracking : MonoBehaviour {
	
	public SkeletonWrapper sw;
	public GameObject Head;
	
	public int player;
	
	public float scale = 1.0f;
    /* Début ajout F. Davesne */
    public bool isTracked;
	/* Fin ajout F. Davesne */

	void Start () {
        isTracked = false;
	}
	
	void Update () {
        if (player == -1) {
            isTracked = false;
            return;
        }

        if (sw.pollSkeleton()) {
            Debug.Log("After pollSkeleton() ");
            isTracked = false;

            transform.localPosition = new Vector3(
                    sw.bonePos[player, (int)Kinect.NuiSkeletonPositionIndex.Head].x * scale,
                    -sw.bonePos[player, (int)Kinect.NuiSkeletonPositionIndex.Head].y * scale,
                    sw.bonePos[player, (int)Kinect.NuiSkeletonPositionIndex.Head].z * scale - 11);

            Debug.Log(sw.bonePos[player, (int)Kinect.NuiSkeletonPositionIndex.Head].x * scale);

            /* Ajout F. Davesne pour repérer si les éléments du squelette intéressants sont trackés => isTracked */
            isTracked = isTracked | (sw.boneState[player, (int)Kinect.NuiSkeletonPositionIndex.Head] == Kinect.NuiSkeletonPositionTrackingState.Tracked);
            /* Fin Ajout F. Davesne */
        }
    }
}
