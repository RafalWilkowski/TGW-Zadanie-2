using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StoneBreakOdds : MonoBehaviour
{
    [SerializeField]
    private float[] _probabilityWeights;
    private float[] _cumulativeProbability;

    [SerializeField]
    private AudioSource _noGems;
    [SerializeField]
    private AudioSource _someGems;

    private void Start()
    {
        CalculateOdds();
    }
    private void CalculateOdds()
    {
        float totalWeight = _probabilityWeights.Sum();
        List<float> probabilityDistribution = new List<float>();
        int lenght = _probabilityWeights.Length;
        _cumulativeProbability = new float[lenght];

        // rozkład pradwopodobienstwa
        for (int i = 0; i < lenght; i++)
        {
            float someValue = _probabilityWeights[i] / totalWeight;
            probabilityDistribution.Add(someValue);
        }
        // rozklad kumultatywny
        for (int i = lenght - 1; i >= 0; --i)
        {
            float suma = probabilityDistribution.Sum();
            _cumulativeProbability[i] = suma;
            probabilityDistribution.RemoveAt(probabilityDistribution.Count - 1);
        }
    }
    public int GemQuantity()
    {
        int quantity = 0;
        float randomFloat = Random.Range(0f, 1f);

        for (int i = 0; i < _cumulativeProbability.Length; i++)
        {
            float compareFloat = _cumulativeProbability[i];
            if (randomFloat < compareFloat)
            {
                quantity = i;
                break;
            }
        }
        Debug.Log(randomFloat + " : " + quantity);
        return quantity;
    }

    public void PlayBreakSound(int quantity)
    {
        if (quantity > 0) _someGems.Play();
        else _noGems.Play();
    }

}
