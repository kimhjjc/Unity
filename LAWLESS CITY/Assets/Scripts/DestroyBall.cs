using UnityEngine;
using System.Collections;

public class DestroyBall : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Destroy(gameObject, 5.0f);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Ball" || other.gameObject.tag == "Ball1")
            return;
        

        Destroy(gameObject);
    }
}
