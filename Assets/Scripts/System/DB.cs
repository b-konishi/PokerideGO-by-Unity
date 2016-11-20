using UnityEngine;
using System.Collections;

public class DB : MonoBehaviour {
	public GameObject[] players ;
	public int[] ranking = {1,1,1,1};

	private Status[] status = new Status[4];
	// Use this for initialization
	void Start () {
		for (int i = 0; i < 4; i++) {
			status[i] = players[i].GetComponent<Status>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		updateRanking ();
	}

	void updateRanking(){
		int[] kinomi_num = new int[4];
		for (int i = 0; i < 4; i++) {
			kinomi_num[i] = status[i].kinomi_num;
			ranking[i] = 1;
		}
		for (int i = 0; i < 4; i++) {
			for(int j = 0; j < 4; j++){
				if(kinomi_num[i] < kinomi_num[j]){
					ranking[i] += 1;
				}
			}
		}
	}
}
