using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Substance.Game;

public class Growing : MonoBehaviour
{
    public Substance.Game.SubstanceGraph targetGraph;

    float currentTime = -0.5f;

    void Update()
    {
        currentTime += Time.deltaTime * 0.3f;

        targetGraph.SetInputFloat("animation", currentTime);

        targetGraph.QueueForRender();
        Substance.Game.Substance.RenderSubstancesAsync();
    }
}
