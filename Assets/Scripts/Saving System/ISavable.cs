using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Saving
{
    public interface ISavable
    {
        object CaptureState();

        void RestoreState(object state);
    }
}