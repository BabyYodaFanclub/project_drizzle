using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BrunoMikoski.TextJuicer;
using BrunoMikoski.TextJuicer.Modifiers;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(TMP_TextJuicer))]
public class TextJuicerCustomModifier : TextJuicerVertexModifier
{
    /*private void Awake()
    {
        AddAnimation(new TextAnimationConfig()
        {
            ChangeColor = true,
            Color = Color.green,
            DoWobble = true,
            WobbleIntensity = Vector3.up * 5,
            MinCharIndex = 8,
            MaxCharIndex = 11,
        });
        AddAnimation(new TextAnimationConfig()
        {
            ChangeColor = true,
            Color = Color.red,
            DoWobble = false,
            MinCharIndex = 0,
            MaxCharIndex = 5,
        });
    }*/

    public override bool ModifyGeometry { get; } = true;
    public override bool ModifyVertex { get; } = true;

    public AnimationCurve WobbleCurve = new AnimationCurve(new Keyframe(0, 0),
        new Keyframe(0.25f, 2.0f),
        new Keyframe(0.5f, 0), new Keyframe(0.75f, 2.0f),
        new Keyframe(1, 0f));

    [SerializeField] public List<TextAnimationConfig> ConfiguredEffects = new List<TextAnimationConfig>();
    
    public override void ModifyCharacter(CharacterData characterData, TMP_Text textComponent, TMP_TextInfo textInfo,
        float progress,
        TMP_MeshInfo[] meshInfo)
    {
            int materialIndex = characterData.MaterialIndex;

            int vertexIndex = characterData.VertexIndex;

            Vector3[] sourceVertices = meshInfo[materialIndex].vertices;

            Vector2 charMidBaseline =
                (sourceVertices[vertexIndex + 0] + sourceVertices[vertexIndex + 2]) / 2;

            Vector3 offset = charMidBaseline;

            Vector3[] destinationVertices = textInfo.meshInfo[materialIndex].vertices;

            destinationVertices[vertexIndex + 0] = sourceVertices[vertexIndex + 0] - offset;
            destinationVertices[vertexIndex + 1] = sourceVertices[vertexIndex + 1] - offset;
            destinationVertices[vertexIndex + 2] = sourceVertices[vertexIndex + 2] - offset;
            destinationVertices[vertexIndex + 3] = sourceVertices[vertexIndex + 3] - offset;

            var configs = ConfiguredEffects.Where(conf =>
                conf.MinCharIndex <= characterData.Index && conf.MaxCharIndex >= characterData.Index);

            var finalPosition = Vector3.zero;
            var finalRotation = Quaternion.identity;
            var finalScale = Vector3.one;

            foreach (var config in configs)
            {
                if (config.DoWobble)
                {
                    // Randomly offsetting the wobble for each char
                    finalPosition += config.WobbleIntensity 
                                     * WobbleCurve.Evaluate((characterData.Progress + config.RandomCharOffsets[characterData.Index % 10]) % 1);
                }

                if (config.ChangeColor)
                {
                    var color = textInfo.meshInfo[materialIndex].colors32;

                    color[vertexIndex + 0] = config.Color;
                    color[vertexIndex + 1] = config.Color;
                    color[vertexIndex + 2] = config.Color;
                    color[vertexIndex + 3] = config.Color;
                }
            }
            
            Matrix4x4 matrix = Matrix4x4.TRS(finalPosition, finalRotation, finalScale);

            destinationVertices[vertexIndex + 0] =
                matrix.MultiplyPoint3x4(destinationVertices[vertexIndex + 0]);
            destinationVertices[vertexIndex + 1] =
                matrix.MultiplyPoint3x4(destinationVertices[vertexIndex + 1]);
            destinationVertices[vertexIndex + 2] =
                matrix.MultiplyPoint3x4(destinationVertices[vertexIndex + 2]);
            destinationVertices[vertexIndex + 3] =
                matrix.MultiplyPoint3x4(destinationVertices[vertexIndex + 3]);

            destinationVertices[vertexIndex + 0] += offset;
            destinationVertices[vertexIndex + 1] += offset;
            destinationVertices[vertexIndex + 2] += offset;
            destinationVertices[vertexIndex + 3] += offset;
    }
}

[Serializable]
public class TextAnimationConfig
{
    public int MinCharIndex = 0;
    public int MaxCharIndex = int.MaxValue;
    public bool DoWobble;
    public Vector3 WobbleIntensity = Vector3.up;
    public bool ChangeColor;
    public Color Color;
    
    [NonSerialized]
    public float[] _randomCharOffsets;

    public float[] RandomCharOffsets
    {
        get
        {
            if (_randomCharOffsets == null)
            {
                _randomCharOffsets = new float[10];
                for (int i = 0; i < _randomCharOffsets.Length; i++)
                {
                    _randomCharOffsets[i] = Random.value;
                }
            }
            return _randomCharOffsets;
        }
    }
}