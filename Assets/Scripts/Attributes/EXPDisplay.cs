using System.Net.Mime;
using RPG.Attributes;
using UnityEngine;
using UnityEngine.UI;

public class EXPDisplay : MonoBehaviour
{
    Experience experience;

    private void Awake()
    {
        experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
    }

    // Update is called once per frame
    void Update()
    {
        float expPoints = experience.GetExperience();
        gameObject.GetComponent<Text>().text = $"{expPoints}";
    }
}
