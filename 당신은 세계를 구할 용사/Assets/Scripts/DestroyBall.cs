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
        if (other.gameObject.tag == "Player")
            return;

        if(other.gameObject.tag == "Crystal")
        {
            Material material = Resources.Load("Blue", typeof(Material)) as Material;

            other.gameObject.GetComponent<Renderer>().material = material;
        }

        Destroy(gameObject);
    }
}
