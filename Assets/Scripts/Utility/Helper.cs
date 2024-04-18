using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Helper Class with Static Methods or Extension Methods
/// </summary>
public static class Helper
{

    #region Extension Methods

    /// <summary>
    /// Detaches the Particle System from any GameObject and Plays it
    /// </summary>
    /// <param name="particleSystem">ParticleSystem to detach and play</param>
    public static void DetachPlay(this ParticleSystem particleSystem)
    {
        particleSystem.transform.parent = null;
        particleSystem.Play();
    }

    #endregion

}
