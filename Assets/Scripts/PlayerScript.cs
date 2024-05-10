using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{

	public GameObject Tral;
	public GameObject ObstacleSound;
	public GameObject Obstacles;
	public Color[] RanCol;
	public Animator Title;
	Vector3 lastpoint;
	Vector3 mousePos;
	Vector3 currentMosPos;
	Vector3 startMosPos;
	float offset;
	Transform Particles;
	int partcont;
	int con;
	bool dead;
	public Vector3[] contd;

	public List<GameObject> curentObs;
	float SnakeSpeed = 2.5f;
	Color newColor;

	// Use this for initialization
	void Start()
	{
		int ran = Random.Range(0, RanCol.Length);
		newColor = RanCol[ran];
		if (ran == 0)
		{
			gameObject.name = "Blue";
		}
		if (ran == 1)
		{
			gameObject.name = "Green";
		}
		if (ran == 2)
		{
			gameObject.name = "Red";
		}
		if (ran == 3)
		{
			gameObject.name = "Yellow";
		}

		GetComponent<SpriteRenderer>().color = newColor;

		transform.GetChild(3).GetComponent<TrailRenderer>().startColor = newColor;
		transform.GetChild(3).GetComponent<TrailRenderer>().endColor = newColor;
		transform.GetChild(2).GetComponent<SpriteRenderer>().color = newColor;
		transform.GetChild(2).parent = null;
		Particles = GameObject.Find("Particle").transform;
		InvokeRepeating("chalu", .1f, .1f);
	}
	public bool LevelComplete;
	void chalu()
	{
		if (!dead)
		{
			Particles.GetChild(Particles.childCount - 1).gameObject.SetActive(true);
			Particles.GetChild(Particles.childCount - 1).position = transform.position;
			Particles.GetChild(Particles.childCount - 1).GetComponent<ParticleSystem>().startColor = GetComponent<SpriteRenderer>().color;
			Particles.GetChild(Particles.childCount - 1).SetAsFirstSibling();
		}
	}

	void Update()
	{
		if (!LevelComplete && FindObjectOfType<GameManager>().playable)
		{
			if (Input.GetMouseButtonDown(0))
			{
				FindObjectOfType<GameManager>().MenuUI.SetActive(false);
				FindObjectOfType<GameManager>().playable = true;

				startMosPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				offset = transform.position.x - startMosPos.x;
			}
			if (FindObjectOfType<GameManager>().playable)
			{
				if (Input.GetMouseButton(0))
				{
					currentMosPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
					GetComponent<Rigidbody2D>().position = Vector3.Lerp(transform.position, new Vector3(currentMosPos.x + offset, transform.position.y, transform.position.z), Time.deltaTime * 10);
					transform.position = Vector3.Lerp(transform.position, new Vector3(currentMosPos.x + offset, transform.position.y, transform.position.z), Time.deltaTime * 10);
				}

				transform.position = new Vector3(Mathf.Clamp(transform.position.x, -2.65f, 2.65f), transform.position.y, transform.position.z);
				transform.position = new Vector3(transform.position.x, transform.position.y + Time.smoothDeltaTime * SnakeSpeed, transform.position.z);
				transform.up = -(lastpoint - transform.position).normalized;
				lastpoint = transform.position;
			}
		}
	}
	int currentlevel;
	void OnTriggerEnter2D(Collider2D col)
	{

		if (col.name.Contains("Finish"))
		{
			currentlevel++;
			LevelComplete = true;
			if (currentlevel >= PlayerPrefs.GetInt("LevelComplet"))
			{
				PlayerPrefs.SetInt("LevelNo", PlayerPrefs.GetInt("LevelNo") + 1);
			}
			FindObjectOfType<GameManager>().CompleteUI.SetActive(true);
			FindObjectOfType<GameManager>().MenuUI.SetActive(false);
			PlayerPrefs.SetInt("LevelComplete", 1);
		}
		if (col.tag == "Obstacle")
		{
			if (col.name.Contains(gameObject.name))
			{
			}
			else
			{
				Debug.Log(col.gameObject.GetComponent<SpriteRenderer>().color + " " + GetComponent<SpriteRenderer>().color);
				gameObject.SetActive(false);
				GameObject obs = Instantiate(Obstacles, transform.position, Quaternion.identity);
				obs.transform.GetComponent<ParticleSystem>().startColor = GetComponent<SpriteRenderer>().color;
				TrailRenderer[] tr = FindObjectsOfType<TrailRenderer>();
				for (int i = 0; i < tr.Length; i++)
				{
					tr[i].enabled = false;
				}
				dead = true;
				InvokeRepeating("partAct", .015f, .015f);
				Invoke("Deading", 1.5f);
			}
		}
		if (col.tag == "Red" || col.tag == "Blue" || col.tag == "Green" || col.tag == "Yellow")
		{
			col.gameObject.GetComponent<AudioSource>().Play();
			gameObject.name = col.tag;
			newColor = col.gameObject.GetComponent<SpriteRenderer>().color;
			transform.GetComponent<SpriteRenderer>().color = newColor;
			Transform lastTrail = transform.GetChild(2);
			transform.GetChild(2).parent = null;
			col.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
			col.gameObject.GetComponent<SpriteRenderer>().enabled = false;
			col.transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = false;
			GameObject ab = Instantiate(Tral, lastTrail.position, Quaternion.identity);
			ab.transform.parent = transform;
			ab.transform.SetSiblingIndex(2);
			transform.GetChild(2).GetComponent<TrailRenderer>().startColor = newColor;
			transform.GetChild(2).GetComponent<TrailRenderer>().endColor = newColor;
			if (curentObs.Count > 4)
			{
				wapas();
				print("yoyo");
			}
		}
	}

	void Deading()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	void partAct()
	{
		if (Particles.childCount > partcont)
		{
			Particles.GetChild(partcont).GetComponent<ParticleSystem>().Play();
			partcont++;
		}
	}

	void wapas()
	{
		for (int i = 0; i < curentObs[0].transform.childCount; i++)
		{
			curentObs[0].transform.GetChild(i).gameObject.SetActive(true);
			if (curentObs[0].transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>())
			{
				curentObs[0].transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>().enabled = true;
			}
			for (int j = 0; j < curentObs[0].transform.GetChild(i).childCount; j++)
			{
				curentObs[0].transform.GetChild(i).GetChild(j).gameObject.SetActive(true);
				if (curentObs[0].transform.GetChild(i).GetChild(j).gameObject.GetComponent<SpriteRenderer>())
				{
					curentObs[0].transform.GetChild(i).GetChild(j).gameObject.GetComponent<SpriteRenderer>().enabled = true;
				}
				for (int k = 0; k < curentObs[0].transform.GetChild(i).GetChild(j).childCount; k++)
				{
					curentObs[0].transform.GetChild(i).GetChild(j).GetChild(k).gameObject.SetActive(true);
					if (curentObs[0].transform.GetChild(i).GetChild(j).GetChild(k).gameObject.GetComponent<SpriteRenderer>())
					{
						curentObs[0].transform.GetChild(i).GetChild(j).GetChild(k).gameObject.GetComponent<SpriteRenderer>().enabled = true;
					}
					for (int l = 0; l < curentObs[0].transform.GetChild(i).GetChild(j).GetChild(k).childCount; l++)
					{
						curentObs[0].transform.GetChild(i).GetChild(j).GetChild(k).GetChild(l).gameObject.SetActive(true);
						if (curentObs[0].transform.GetChild(i).GetChild(j).GetChild(k).GetChild(l).gameObject.GetComponent<SpriteRenderer>())
						{
							curentObs[0].transform.GetChild(i).GetChild(j).GetChild(k).GetChild(l).gameObject.GetComponent<SpriteRenderer>().enabled = true;

						}
					}
				}
			}
		}
		GameObject curgame = curentObs[0];
		curentObs.RemoveAt(0);
	}
}
