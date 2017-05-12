using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour {


	GameObject[] nearestReturnPoints = new GameObject[5];
	List<GameObject> currentWorkers = new List<GameObject>();
	//List<GameObject> nearestReturnPoint = new List<GameObject>();
	GameObject activeWorker;
	float workTime = 2.0f;
	float currentWorkTime = 0.0f;
	float closestReturnPointDistance = 9999.0f;
	GameObject currentClosest = null;
	bool workerWorking = false;

	// Use this for initialization
	void Start () {
		//nearestReturnPoints = new GameObject[5];
		foreach(GameObject returnPoint in GameObject.FindGameObjectsWithTag("SupportStructure")){
			if(returnPoint.name.Contains("Hall")||returnPoint.name.Contains("Trading")){
				if(Vector3.Distance(returnPoint.transform.position,this.transform.position)<closestReturnPointDistance){
					closestReturnPointDistance = Vector3.Distance(returnPoint.transform.position,this.transform.position);
					currentClosest = returnPoint;
				}
			}
		}
		nearestReturnPoints[currentClosest.GetComponent<Stats>().team] = currentClosest;
	}

	// Update is called once per frame
	void Update () {
		if(!currentWorkers.Contains(activeWorker)){
			activeWorker = null;
		}
		if(currentWorkers.Count>0){
			if(!workerWorking){
				foreach(GameObject worker in currentWorkers){
					if(worker!=activeWorker){
						if(Vector3.Distance(worker.transform.position,this.transform.position)<0.5f){
							activeWorker = worker;
							activeWorker.SetActive(false);
							workerWorking = true;
						}
					}
					if(Vector3.Distance(worker.transform.position,nearestReturnPoints[worker.GetComponent<Stats>().team].transform.position)<0.5f){
						if(activeWorker = worker){
							activeWorker = null;
						}
						if(worker.GetComponent<Worker>().goldCarried == true){
							worker.GetComponent<Worker>().goldCarried = false;
							GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameGUI>().gold[worker.GetComponent<Stats>().team]+=15;
							worker.GetComponent<Stats>().attackTarget = this.gameObject;
						}
					}
				}
			}
			if(workerWorking){
				currentWorkTime+=Time.deltaTime;
				if(currentWorkTime>=workTime){
					activeWorker.GetComponent<Stats>().attackTarget = nearestReturnPoints[activeWorker.GetComponent<Stats>().team];
					activeWorker.GetComponent<Worker>().goldCarried = true;
					activeWorker.SetActive(true);
					workerWorking = false;
					currentWorkTime = 0.0f;
				}
			}
		}
	}

	public void assignWorker(GameObject workerUnit){
		currentWorkers.Add(workerUnit);
	}
	public void removeWorker(GameObject workerUnit){
		currentWorkers.Remove(workerUnit);
	}

	public void addReturnPoint(GameObject returnPoint){
		if(nearestReturnPoints[returnPoint.GetComponent<Stats>().team]==null){
			nearestReturnPoints[returnPoint.GetComponent<Stats>().team] = returnPoint;
		} else if(Vector3.Distance(returnPoint.transform.position,this.transform.position)<Vector3.Distance(nearestReturnPoints[returnPoint.GetComponent<Stats>().team].transform.position,this.transform.position)){
			nearestReturnPoints[returnPoint.GetComponent<Stats>().team] = returnPoint;
		}
	}
}
