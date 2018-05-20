using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballScript : MonoBehaviour
{
	public GameObject respwan;
	public GameObject respwanZone;
	public GameObject game;
	private GameObject catiche;
	public float amplitude;
	public float speed;
	public string movementX;
	public string movementZ;
	private GameObject[] obstacles;
	public GameObject[] persos;
	private bool isUpdate;
	private GameObject dlight;
	private GameObject slight;
	
	// Use this for initialization
	void Start ()
	{
		speed = 50f;
		amplitude = 80f;
		game = GameObject.Find("game");
		catiche = GameObject.FindGameObjectWithTag("catiche");
		obstacles = GameObject.FindGameObjectsWithTag("obstacle");
		isUpdate = true;
		dlight = GameObject.FindWithTag("dlight");
		slight = GameObject.FindWithTag("slight");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		/* if (ground)
		{
			Debug.Log(ground.transform.rotation);
		}
		

		var moveHorizontal = ground.transform.rotation.z;
		var moveVertical = ground.transform.rotation.x;
		var movement = new Vector3(moveHorizontal, 0, moveVertical);

		//rb.AddForce(movement * speed);

		if (transform.position.y < respwanZone.transform.position.y)
		{
			transform.position = respwan.transform.position;
		} */

		// game.transform.rotation
		if (game && isUpdate)
		{
			// var moveX = Input.GetAxis(movementX);
			// var moveZ  = Input.GetAxis(movementZ);
			// Debug.Log(game.transform.rotation.x * amplitude);
			var moveHorizontal = game.transform.rotation.x * amplitude;
			var moveVertical = -1 * (game.transform.rotation.z * amplitude);
			//var movement = new Vector3(moveHorizontal, 0, moveVertical);
			var moveVector = new Vector3(moveVertical, 0, moveHorizontal) * (Time.deltaTime * speed);
			//var moveVector = new Vector3(moveHorizontal, 0, moveVertical) * (Time.deltaTime * speed);
			transform.LookAt(catiche.transform);
			catiche.transform.LookAt(transform);
			transform.Translate(moveVector,Space.World);
			
			//rb.AddForce(movement * speed);
		}
	

	}

	void OnTriggerEnter(Collider other)
	{
		Debug.Log(other);
		if (other.transform.gameObject.name == "catiche")
		{
			Debug.Log("touchhhé");
			HideObject();
		}
		// Debug.Log(other.transform.gameObject.name);
		//transform.position = respwan.transform.position;
	}

	private void HideObject()
	{
		var index = 0;
		Debug.Log(obstacles.Length);
		var obstacleL = obstacles.Length;
		foreach (var obstacle in obstacles)
		{
			index++;
			StartCoroutine(hideObstacle(obstacle, index,obstacleL));
		}
	}

	private IEnumerator hideObstacle(GameObject obstacle,float index, float obstacleL)
	{
		yield return new WaitForSeconds(.5f * index);
		var timeToStart = Time.time;
		var currenTPosY = obstacle.transform.localPosition.y;
		var currenTPosX = obstacle.transform.localPosition.x;
		var currenTPosZ = obstacle.transform.localPosition.z;
		var currenTScaleY = obstacle.transform.localScale.y;
		var currenTScaleX = obstacle.transform.localScale.x;
		var currenTScaleZ = obstacle.transform.localScale.z;
		var speed = 6f;
		// Debug.Log("current scale y : "+currenTScaleY);
		while(obstacle.transform.localScale.y != 0) 
		{
			var tempTime = Mathf.Lerp(currenTScaleY, 0, (Time.time - timeToStart ) * speed);
			var tempTimePosY = Mathf.Lerp(currenTPosY, -1, (Time.time - timeToStart ) * speed);
			obstacle.transform.localScale = new Vector3 (currenTScaleX , tempTime, currenTScaleZ);
			obstacle.transform.localPosition = new Vector3 (currenTPosX , tempTimePosY, currenTPosZ);
			yield return null;
			//obstacle.SetActive(false);
		}
		if (index == obstacleL)
		{
			OnStaggerComplete();
		}
	}


	private void OnStaggerComplete()
	{
		isUpdate = false;
		StartCoroutine(FadeDlight());
		StartCoroutine(FadeSlight());
		Debug.Log("c fini mon pote");
	}
	private IEnumerator FadeDlight()
	{
		var timeToStart = Time.time;
		var instensityValue = .4f;
		var speed = 1f;
		while(dlight.GetComponent<Light>().intensity != instensityValue) 
		{
			var tempTime = Mathf.Lerp(1f, instensityValue, (Time.time - timeToStart ) * speed);
			dlight.GetComponent<Light>().intensity = tempTime;
			yield return null;
		}
	}
	
	private IEnumerator FadeSlight()
	{
		yield return new WaitForSeconds(1f);		
		var timeToStart = Time.time;
		var instensityValue = 28f;
		var speed = 1f;
		while(slight.GetComponent<Light>().intensity != instensityValue) 
		{
			var tempTime = Mathf.Lerp(0, instensityValue, (Time.time - timeToStart ) * speed);
			slight.GetComponent<Light>().intensity = tempTime;
			yield return null;
		}
	}
}
