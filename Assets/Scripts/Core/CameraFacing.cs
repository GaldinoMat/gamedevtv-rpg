using System.Runtime.InteropServices;
using UnityEngine;

namespace RPG.Core
{
    public class CameraFacing : MonoBehaviour
    {

        // Update is called once per frame
        void Update()
        {
            transform.forward = Camera.main.transform.forward;
        }
    }
}