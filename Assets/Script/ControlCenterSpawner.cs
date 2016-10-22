using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ControlCenterSpawner : NetworkBehaviour {

    public GameObject controlCenterPrefab;

    public override void OnStartServer() {
        Debug.Log(this.name + " im server");

        GameObject controlCenter = Instantiate(controlCenterPrefab) as GameObject;
        controlCenter.name = "ControlCenter";
        NetworkServer.Spawn(controlCenter);
    }
}
