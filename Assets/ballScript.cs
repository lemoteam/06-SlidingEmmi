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
	private GameObject player;
	
	// Use this for initialization
	void Start ()
	{
		speed = 50f;
		amplitude = 1f;
		// debug whithout ARCAM
		// game = GameObject.Find("game");
		// game = GameObject.FindGameObjectWithTag("game");
		catiche = GameObject.FindGameObjectWithTag("catiche");
		obstacles = GameObject.FindGameObjectsWithTag("obstacle");
		isUpdate = true;
		dlight = GameObject.FindWithTag("dlight");
		slight = GameObject.FindWithTag("slight");
		player = GameObject.FindWithTag("Player");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		var camera = Camera.main.transform.position;
		Vector3 dir = camera - game.transform.position;
		var test = Quaternion.FromToRotation(game.transform.position, dir) * game.transform.rotation;
		
		Debug.Log("<color=green>"+ "Position" + test +"</color>");
		//Debug.Log("wesh c la rotation les minettes : "+ game.transform.rotation);
		//Debug.Log("eulerAngles : " + game.transform.transform.eulerAngles);
		
		if (game && isUpdate)
		{
			var moveHorizontal = test.y * 1f;
			var moveVertical = test.x * amplitude;
			
			/*var moveHorizontal = game.transform.rotation.x * amplitude;
			var moveVertical = -1 * (game.transform.rotation.z * amplitude);*/
			var moveVector = new Vector3(moveVertical, 0, moveHorizontal) * (Time.deltaTime * speed);
			transform.LookAt(catiche.transform);
			catiche.transform.LookAt(transform);
			transform.Translate(moveVector,Space.World);
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

	}

	private void HideObject()
	{
		var index = 0;
		var obstacleL = obstacles.Length;
		foreach (var obstacle in obstacles)
		{
			index++;
			StartCoroutine(hideObstacle(obstacle, index,obstacleL));
		}
	}

	private IEnumerator hideObstacle(GameObject obstacle,float index, float obstacleL)
	{
		yield return new WaitForSeconds(.5f * (index / 2f));
		var timeToStart = Time.time;
		var currenTPosY = obstacle.transform.localPosition.y;
		var currenTPosX = obstacle.transform.localPosition.x;
		var currenTPosZ = obstacle.transform.localPosition.z;
		var currenTScaleY = obstacle.transform.localScale.y;
		var currenTScaleX = obstacle.transform.localScale.x;
		var currenTScaleZ = obstacle.transform.localScale.z;
		var speed = 2f;
		while(obstacle.transform.localScale.y != 0) 
		{
			var tempTime = Mathf.Lerp(currenTScaleY, 0, (Time.time - timeToStart ) * speed);
			var tempTimePosY = Mathf.Lerp(currenTPosY, -1, (Time.time - timeToStart ) * speed);
			obstacle.transform.localScale = new Vector3 (currenTScaleX , tempTime, currenTScaleZ);
			obstacle.transform.localPosition = new Vector3 (currenTPosX , tempTimePosY, currenTPosZ);
			yield return null;
		}
		if (index == obstacleL)
		{
			OnStaggerComplete();
		}
	}

	private void OnStaggerComplete()
	{
		isUpdate = false;
		player.GetComponent<Rigidbody>().isKinematic = true;
		player.GetComponent<Rigidbody>().useGravity = false;
		catiche.GetComponent<Rigidbody>().useGravity = false;
		catiche.GetComponent<Rigidbody>().isKinematic = true;
		
		var j = 0;
		var playerL = persos.Length;
		foreach (var perso in persos)
		{
			j++;
			StartCoroutine(HidePlayers(perso, j, playerL));
		}
		StartCoroutine(FadeDlight());
		StartCoroutine(FadeSlight());
		StartCoroutine(ShowCatiche(catiche));
		StartCoroutine(ShowEmmi(player));
	}

	private IEnumerator HidePlayers(GameObject perso,float j, float playerL)
	{	
		yield return new WaitForSeconds(.5f * j);
		var timeToStart = Time.time;
		var currenTPosY = perso.transform.localPosition.y;
		var currenTPosX = perso.transform.localPosition.x;
		var currenTPosZ = perso.transform.localPosition.z;
		var currenTScaleY = perso.transform.localScale.y;
		var currenTScaleX = perso.transform.localScale.x;
		var currenTScaleZ = perso.transform.localScale.z;
		var speed = 12f;
		
		while(perso.transform.localScale.y != 0) 
		{
			var tempTime = Mathf.Lerp(currenTScaleY, 0, (Time.time - timeToStart ) * speed);
			var tempTimePosY = Mathf.Lerp(currenTPosY, -1, (Time.time - timeToStart ) * speed);
			perso.transform.localScale = new Vector3 (currenTScaleX , tempTime, currenTScaleZ);
			// perso.transform.localPosition = new Vector3 (currenTPosX , tempTimePosY, currenTPosZ);
			yield return null;
		}
	}
	
	private IEnumerator FadeDlight()
	{
		yield return new WaitForSeconds(1f);		
		var timeToStart = Time.time;
		var instensityValue = .4f;
		var speed = 2f;
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
		var speed = 2f;
		while(slight.GetComponent<Light>().intensity != instensityValue) 
		{
			var tempTime = Mathf.Lerp(0, instensityValue, (Time.time - timeToStart ) * speed);
			slight.GetComponent<Light>().intensity = tempTime;
			yield return null;
		}
	}
	
	private IEnumerator ShowCatiche(GameObject catiche)
	{
		yield return new WaitForSeconds(1.5f);
		catiche.transform.localPosition = new Vector3(0f,0.057f, 0.043f);
		catiche.transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);

		yield return new WaitForSeconds(.5f);
		var timeToStart = Time.time;
		var currenTPosY = catiche.transform.localPosition.y;
		var currenTPosX = catiche.transform.localPosition.x;
		var currenTPosZ = catiche.transform.localPosition.z;
		var currenTScaleY = catiche.transform.localScale.y;
		var currenTScaleX = catiche.transform.localScale.x;
		var currenTScaleZ = catiche.transform.localScale.z;
		var speed = 9f;

		while(catiche.transform.localScale.y != 0.08f) 
		{
			var tempTime = Mathf.Lerp(0, 0.08f, (Time.time - timeToStart ) * speed);
			// var tempTimePosY = Mathf.Lerp(currenTPosY, -1, (Time.time - timeToStart ) * speed);
			catiche.transform.localScale = new Vector3 (currenTScaleX , tempTime, currenTScaleZ);
			// perso.transform.localPosition = new Vector3 (currenTPosX , tempTimePosY, currenTPosZ);
			yield return null;
		}
	}

	private IEnumerator ShowEmmi(GameObject emmi)
	{
		yield return new WaitForSeconds(1.5f);
		emmi.transform.localPosition = new Vector3(0f,0.057f,-0.031f);
		emmi.transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);

		yield return new WaitForSeconds(1f);
		var timeToStart = Time.time;
		var currenTPosY = catiche.transform.localPosition.y;
		var currenTPosX = catiche.transform.localPosition.x;
		var currenTPosZ = catiche.transform.localPosition.z;
		var currenTScaleY = catiche.transform.localScale.y;
		var currenTScaleX = catiche.transform.localScale.x;
		var currenTScaleZ = catiche.transform.localScale.z;
		var speed = 9f;

		while(emmi.transform.localScale.y != 0.08f) 
		{
			var tempTime = Mathf.Lerp(0, 0.08f, (Time.time - timeToStart ) * speed);
			// var tempTimePosY = Mathf.Lerp(currenTPosY, -1, (Time.time - timeToStart ) * speed);
			emmi.transform.localScale = new Vector3 (currenTScaleX , tempTime, currenTScaleZ);
			// perso.transform.localPosition = new Vector3 (currenTPosX , tempTimePosY, currenTPosZ);
			yield return null;
		}
	}
}

