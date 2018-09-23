using UnityEngine;

public class ProjectileRotator : MonoBehaviour {

    private float TimeToFullyRotate = 0.5f;
    private float TimeElapsed = 0.0f;
	
	// Update is called once per frame
	void Update () {
        TimeElapsed = TimeElapsed += Time.deltaTime;
        var percentComplete  = TimeElapsed / TimeToFullyRotate;
        this.gameObject.transform.eulerAngles = Vector3.LerpUnclamped(Vector3.zero, new Vector3(0, 0, 360), percentComplete);
        if(TimeElapsed > TimeToFullyRotate)
        {
            TimeElapsed = 0.0f;
        }
    }
}
