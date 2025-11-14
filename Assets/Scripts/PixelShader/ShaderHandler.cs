using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple full-screen post-process handler. Assign one or more Materials that use full-screen image effect shaders.
/// If multiple materials are assigned, the active effect can be switched via EffectIndex.
/// Backward-compatible: if the list is empty, it falls back to eMat.
/// </summary>
[ExecuteInEditMode]
public class ShaderHandler : MonoBehaviour
{
    [Header("Legacy (single material) - kept for backwards compatibility")]
    [SerializeField] private Material eMat;

    [Header("Effect Pool")]
    [Tooltip("Assign Materials that use shaders under Assets/GEShader (GE/*)." )]
    [SerializeField] private List<Material> effects = new List<Material>();
    [Range(0, 16)] [SerializeField] private int effectIndex = 0;

    public int EffectIndex
    {
        get => effectIndex;
        set => effectIndex = Mathf.Clamp(value, 0, Mathf.Max(0, (effects?.Count ?? 0) - 1));
    }

    public void NextEffect()
    {
        if (effects == null || effects.Count == 0) return;
        effectIndex = (effectIndex + 1) % effects.Count;
    }

    public void PrevEffect()
    {
        if (effects == null || effects.Count == 0) return;
        effectIndex = (effectIndex - 1 + effects.Count) % effects.Count;
    }

    private Material GetActiveMaterial()
    {
        if (effects != null && effects.Count > 0)
        {
            var idx = Mathf.Clamp(effectIndex, 0, effects.Count - 1);
            return effects[idx];
        }
        return eMat;
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        var mat = GetActiveMaterial();
        if (mat != null)
        {
            Graphics.Blit(source, destination, mat);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }
}
