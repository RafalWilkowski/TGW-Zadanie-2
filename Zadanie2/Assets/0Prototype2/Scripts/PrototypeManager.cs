using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System.Linq;

public class PrototypeManager : MonoBehaviour
{
    public static PrototypeManager Instance;

    [SerializeField]
    private float[] _containersPositions;
    [SerializeField]
    private GameObject[] _containers;

    [SerializeField]
    private float _resetTime;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CurrentConteiner(Conteiner.ColorType.YELLOW);
    }
    public IEnumerator ResetGame()
    {
        yield return new WaitForSeconds(_resetTime);
        SceneManager.LoadScene(0);
    }

    public void CurrentConteiner(Conteiner.ColorType color)
    {
        GameObject first = _containers[0];
        switch (color)
        {
            case Conteiner.ColorType.RED:
                first = _containers[0];
                break;
            case Conteiner.ColorType.YELLOW:
                first = _containers[1];
                break;
            case Conteiner.ColorType.BLUE:
                first = _containers[2];
                break;
            case Conteiner.ColorType.GREEN:
                first = _containers[3];
                break;
        }

        float positionY = first.transform.position.y;
        float positionZ = first.transform.position.z;
        //first container change position
        first.transform.position = new Vector3(_containersPositions[0], positionY, positionZ);
        //rest of containers
        List<int> randomIntList = new List<int> { 1, 2, 3 };
        int[] randomArray = new int[randomIntList.Count];
        
        //randomize containers order
        for(int i = 0; i < randomArray.Length; i++)
        {          
                int random = Random.Range(0, randomIntList.Count);
                randomArray[i] = randomIntList[random];
                randomIntList.Remove(randomIntList[random]);          
        }
        
        //set conainers on positions
        for(int i = 1; i < randomArray.Length; i++)
        {
            _containers[i].transform.position = new Vector3(_containersPositions[randomArray[i-1]], positionY, positionZ);
        }

    }
}
