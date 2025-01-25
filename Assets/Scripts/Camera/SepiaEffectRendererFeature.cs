using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SepiaEffectRendererFeature : ScriptableRendererFeature
{
    class SepiaPass : ScriptableRenderPass
    {
        private Material _sepiaMaterial;

        public SepiaPass(Material material)
        {
            _sepiaMaterial = material;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (_sepiaMaterial == null)
            {
                return;
            }

            var cmd = CommandBufferPool.Get("SepiaEffect");
            var cameraColorTarget = renderingData.cameraData.renderer.cameraColorTargetHandle;

            cmd.Blit(cameraColorTarget, cameraColorTarget, _sepiaMaterial);
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }

    [SerializeField]
    private Material _sepiaMaterial;

    private SepiaPass sepiaPass;

    public override void Create()
    {
        if (_sepiaMaterial == null) return;

        sepiaPass = new SepiaPass(_sepiaMaterial)
        {
            renderPassEvent = RenderPassEvent.AfterRendering
        };
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(sepiaPass);
    }
}
