using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class EnemyTextureGenerator : MonoBehaviour
{
    [Header("Texture Settings")]
    [Range(2, 2048)] public int texWidth  = 256;
    [Range(2, 2048)] public int texHeight = 256;

    [Header("Perlin Noise Parameters")]
    [Range(0.001f, 0.1f)] public float noiseScale      = 0.05f;
    [Range(1, 10)]         public int   octaves         = 7;
    [Range(0f, 0.5f)]        public float persistence     = 0.1f;
    [Range(0.1f, 8f)]      public float lacunarity      = 4f;
    [Range(1, 20)]         public int   posterizeLevels = 6;
                            public Vector2 noiseOffset     = Vector2.zero;

    [Header("Color Mapping")]
    public Color lowColor  = Color.red;
    public Color highColor = Color.blue;

    [Header("Physical Material")]
    [Range(0f, 1f)] public float metallic   = 0f;
    [Range(0f, 1f)] public float smoothness = 0.5f;

    [Header("Relief (Bump) Mapping")]
    public bool   useBump       = false;
    [Range(0f, 2f)] public float bumpStrength = 1f;

    private Texture2D _texture;
    private Material  _material;

    void Awake()
    {
        if (Application.isPlaying)
        {
            var rend = GetComponent<Renderer>();
            if (rend != null)
                // per-instance material at runtime
                _material = rend.material;
        }
    }

    void Start()
    {
        if (_material == null)
        {
            Debug.LogError($"[PlanetTextureGenerator] No material on '{name}'. Disabling.");
            enabled = false;
            return;
        }
        InitTexture(texWidth, texHeight);
        GenerateTexture();
    }

    // (re)create the Texture2D and assign it
    public void InitTexture(int width, int height)
    {
        var rend = GetComponent<Renderer>();
        if (rend == null) return;

        _material = Application.isPlaying
            ? rend.material
            : rend.sharedMaterial;

        if (_texture != null &&
            _texture.width  == width &&
            _texture.height == height)
            return;

        _texture = new Texture2D(width, height, TextureFormat.RGBA32, false)
        {
            filterMode = FilterMode.Bilinear,
            wrapMode   = TextureWrapMode.Repeat
        };
        _material.mainTexture = _texture;
    }

    // Generate both the color map and, if enabled, a bump map, and apply all material properties.
    public void GenerateTexture()
    {
        if (_texture == null) return;

        // build the color map 
        for (int y = 0; y < texHeight; y++)
            for (int x = 0; x < texWidth; x++)
            {
                float n = ComputeNoiseAt(x, y);
                _texture.SetPixel(x, y, Color.Lerp(lowColor, highColor, n));
            }
        _texture.Apply();

        // apply Standard shader properties
        _material.SetFloat("_Metallic",   metallic);
        _material.SetFloat("_Glossiness", smoothness);

        // optional bump (normal) map
        if (useBump)
        {
            var bump = BuildNormalMap(bumpStrength);
            _material.SetTexture("_BumpMap", bump);
            _material.EnableKeyword("_NORMALMAP");
            _material.SetFloat("_BumpScale", bumpStrength);
        }
        else
        {
            _material.DisableKeyword("_NORMALMAP");
        }
    }

    private float ComputeNoiseAt(int x, int y)
    {
        float u = x / (float)(texWidth - 1);
        float t = Mathf.PingPong(u, 0.5f) * 2f;

        float value = 0f, amp = 1f, freq = 1f;
        for (int i = 0; i < octaves; i++)
        {
            float period  = texWidth * noiseScale * freq;
            float sampleX = t * period + noiseOffset.x;
            float sampleY = (y * noiseScale * freq) + noiseOffset.y;
            value += Mathf.PerlinNoise(sampleX, sampleY) * amp;
            amp  *= persistence;
            freq *= lacunarity;
        }

        value = Mathf.Clamp01(value);
        int levels = Mathf.Max(1, posterizeLevels);
        if (levels > 1)
        {
            float step = 1f / (levels - 1);
            value = Mathf.Round(value / step) * step;
        }
        return value;
    }

    // Turn float‐noise into a normal map via a simple Sobel filter.
    private Texture2D BuildNormalMap(float strength)
    {
        int w = _texture.width, h = _texture.height;
        var normalMap = new Texture2D(w, h, TextureFormat.RGBA32, false)
        {
            filterMode = FilterMode.Bilinear,
            wrapMode   = TextureWrapMode.Repeat
        };

        for (int y = 0; y < h; y++)
        for (int x = 0; x < w; x++)
        {
            float hl = ComputeNoiseAt((x - 1 + w) % w, y);
            float hr = ComputeNoiseAt((x + 1) % w, y);
            float hd = ComputeNoiseAt(x, (y - 1 + h) % h);
            float hu = ComputeNoiseAt(x, (y + 1) % h);

            Vector3 dx = new Vector3(1, 0, (hl - hr) * strength);
            Vector3 dy = new Vector3(0, 1, (hd - hu) * strength);
            Vector3 norm = Vector3.Cross(dy, dx).normalized;

            Color c = new Color(
                norm.x * 0.5f + 0.5f,
                norm.y * 0.5f + 0.5f,
                norm.z * 0.5f + 0.5f
            );
            normalMap.SetPixel(x, y, c);
        }
        normalMap.Apply();
        return normalMap;
    }


    // Batch‐set noise parameters and immediately rebuild everything.
    public void SetNoiseParameters(
        float newScale,
        int newOctaves,
        float newPersistence,
        float newLacunarity,
        Vector2 newOffset,
        int newPosterizeLevels
    )
    {
        noiseScale      = newScale;
        octaves         = newOctaves;
        persistence     = newPersistence;
        lacunarity      = newLacunarity;
        noiseOffset     = newOffset;
        posterizeLevels = newPosterizeLevels;

        InitTexture(texWidth, texHeight);
        GenerateTexture();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        var rend = GetComponent<Renderer>();
        if (rend == null || rend.sharedMaterial == null) return;
        InitTexture(texWidth, texHeight);
        GenerateTexture();
    }
#endif
}