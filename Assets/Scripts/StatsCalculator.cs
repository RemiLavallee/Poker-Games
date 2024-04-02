using System.Collections.Generic;
using UnityEngine;

public class StatsCalculator : MonoBehaviour
{
    private int flushCounter = 0;
    private int paireCounter = 0;
    private int highcardCounter = 0;
    private void Awake()
    {
        List<CarteData> pile = new List<CarteData>(52);

        for (int i = 1; i <= 13; i++)
        {
            pile.Add(new CarteData(i, Enseigne.Carreau));
            pile.Add(new CarteData(i, Enseigne.Trefle));
            pile.Add(new CarteData(i, Enseigne.Coeur));
            pile.Add(new CarteData(i, Enseigne.Pique));
        }

        List<List<CarteData>> combos = new List<List<CarteData>>();

        for (int a = 0; a < pile.Count; a++)
        {
            for (int b = a + 1; b < pile.Count; b++)
            {
                for (int c = b + 1; c < pile.Count; c++)
                {
                    for (int d = c + 1; d < pile.Count; d++)
                    {
                        for (int e = d + 1; e < pile.Count; e++)
                        {
                            var combo = new List<CarteData>
                            {
                                pile[a], pile[b], pile[c], pile[d], pile[e]
                            };
                            combo.Sort();
                            combos.Add(combo);
                        }
                    }
                }
            }
        }

        foreach (var combo in combos)
        {
            if (combo[0].enseigne == combo[1].enseigne
                && combo[0].enseigne == combo[2].enseigne
                && combo[0].enseigne == combo[3].enseigne
                && combo[0].enseigne == combo[4].enseigne)
            {
                flushCounter++;
                continue;
            }

            if (combo[0].valeur == combo[1].valeur ||
                combo[1].valeur == combo[2].valeur ||
                combo[2].valeur == combo[3].valeur ||
                combo[3].valeur == combo[4].valeur )
            {
                paireCounter++;
                continue;
            }

            highcardCounter++;
        }

        var probabilityFlush = flushCounter / (float)combos.Count;
        Debug.Log($"Prob de flush: {probabilityFlush}");
        
        var probabilityPaire = paireCounter / (float)combos.Count;
        Debug.Log($"Prob de flush: {probabilityPaire}");
        
        var probabilityHighCard = flushCounter + paireCounter / (float)combos.Count;
        Debug.Log($"Prob de flush: {probabilityHighCard}");
    }
}
