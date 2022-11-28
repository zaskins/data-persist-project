using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameManager : MonoBehaviour
{
    public static NameManager Instance;

    public string name;
    public string bestName;
    public int bestScore;

    // Start is called before the first frame update
    private void Awake() {

        if(Instance != null){
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
