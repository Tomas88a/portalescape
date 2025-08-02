using UnityEngine;

public class NailHighlightOnProximity : MonoBehaviour
{
    public float highlightDistance = 2.2f;          // 与玩家交互距离
    public Color highlightColor = Color.yellow;     // 高亮颜色
    public bool useEmission = false;                // 可选：是否用Emission高亮
    public float emissionIntensity = 1.8f;

    private Renderer[] renderers;
    private Color[][] originalColors;
    private bool isHighlighted = false;

    private void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();
        originalColors = new Color[renderers.Length][];
        for (int i = 0; i < renderers.Length; i++)
        {
            var mats = renderers[i].materials;
            originalColors[i] = new Color[mats.Length];
            for (int j = 0; j < mats.Length; j++)
                originalColors[i][j] = mats[j].color;
        }
    }

    void Update()
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        float dist = Vector3.Distance(transform.position, cam.transform.position);
        if (dist < highlightDistance)
        {
            if (!isHighlighted)
                SetHighlight(true);
        }
        else
        {
            if (isHighlighted)
                SetHighlight(false);
        }
    }

    void SetHighlight(bool state)
    {
        isHighlighted = state;
        for (int i = 0; i < renderers.Length; i++)
        {
            var mats = renderers[i].materials;
            for (int j = 0; j < mats.Length; j++)
            {
                if (state)
                {
                    mats[j].color = highlightColor;
                    if (useEmission)
                    {
                        mats[j].EnableKeyword("_EMISSION");
                        mats[j].SetColor("_EmissionColor", highlightColor * emissionIntensity);
                    }
                }
                else
                {
                    mats[j].color = originalColors[i][j];
                    if (useEmission)
                    {
                        mats[j].SetColor("_EmissionColor", Color.black);
                    }
                }
            }
        }
    }
}
