using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test01 : MonoBehaviour {

	// Use this for initialization
	void Start () {
#if UNITY_EDITOR
		// AnimatorControllerを生成
		CreateAnimatorControllerInheritance createAnimCtrl = new CreateAnimatorControllerInheritance();
		createAnimCtrl.Init();
#endif
	}
}

