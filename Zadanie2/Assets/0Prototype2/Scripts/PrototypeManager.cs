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

    public IEnumerator ResetGame()
    {
        yield return new WaitForSeconds(_resetTime);
        SceneManager.LoadScene(0);
    }

    public void CurrentConteiner(Conteiner.ColorType color)
    {
        List<int> randomIntList = new List<int> {0, 1, 2 ,3};      
        //selected container
        int first = (int) color;
        //remove selectedcontainer from random list
        randomIntList.RemoveAt(first);

        Queue<int> queueConteiners = new Queue<int>();

        queueConteiners.Enqueue(first);
        //randomize conatiners
        int counter = randomIntList.Count;
        for (int i = 0; i < counter; i++)
        {
            int random = Random.Range(0, randomIntList.Count);
            queueConteiners.Enqueue(randomIntList[random]);
            randomIntList.Remove(randomIntList[random]);
        }

        //set conainers on positions
        for (int i = 0; i < _containers.Length; i++)
        {
            _containers[queueConteiners.Dequeue()].transform.position = new Vector3(_containersPositions[i], -3, 0);
        }        

    }
}
