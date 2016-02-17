using UnityEngine;
using System.Collections;

public class LaserTurretController : MonoBehaviour {

	public GameObject laserPrefab;
	public Transform[] laserSpawns;
	public float fireRate;
	public float laserSpeed;

	private GameObject[] laserPool;
	private float shootTimer;
	private System.Random rand;
	private Vector3 laserVector;

	void Awake() {
		laserPool = new GameObject[100];
	}

	// Use this for initialization
	void Start () {
		for (int i = 0; i < laserPool.Length; i++) {
			laserPool[i] = Instantiate(laserPrefab);
			laserPool[i].SetActive(false);
		}
		rand = new System.Random ();
		laserVector = new Vector3 (0, 0, laserSpeed);
	}

	IEnumerator ResetLaser(GameObject laser) {
		yield return new WaitForSeconds (1.0f);
		laser.SetActive(false);
		laser.transform.position = laserSpawns[0].position;

	}

	// Update is called once per frame
	void Update () {
		shootTimer += Time.deltaTime;

		if (shootTimer > fireRate) {
			foreach(GameObject go in laserPool) {
				if(!go.activeSelf) {
					go.transform.position = laserSpawns[rand.Next(0, laserSpawns.Length)].position;
					go.transform.rotation = laserSpawns[rand.Next(0, laserSpawns.Length)].rotation;
					go.transform.SetParent(laserSpawns[rand.Next(0, laserSpawns.Length)]);
					go.SetActive(true);
					break;
				}
			}

			shootTimer = 0;
		}

		foreach (GameObject go in laserPool) {
			if(go.activeSelf) {
				go.transform.localPosition += (go.transform.forward * laserSpeed);

				if(go.transform.parent != null) {
					if(Vector3.Distance(go.transform.position, go.transform.parent.position) > 2) {
						go.transform.SetParent(null);
					}
				}
				else {
					StartCoroutine("ResetLaser", go);
				}
			}
		}

	}
}
