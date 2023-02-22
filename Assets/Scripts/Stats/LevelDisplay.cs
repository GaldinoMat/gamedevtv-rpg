using RPG.Stats;
using UnityEngine;
using UnityEngine.UI;

public class LevelDisplay : MonoBehaviour
{
    BaseStats stats;
    // Start is called before the first frame update
    private void Awake()
    {
        stats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<Text>().text = $"{stats.GetLevel()}";
    }
}
