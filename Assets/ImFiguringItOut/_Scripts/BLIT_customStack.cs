using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BLIT_customStack : ScriptableRendererFeature
{
    [SerializeField] private RenderPassEvent injectionEvent = RenderPassEvent.AfterRenderingOpaques;
    [SerializeField] private List<Material> materials = new List<Material>();
    
    private RenderPass _renderPass;
    private static readonly int MapStrength = Shader.PropertyToID("_MapStrength");

    //implements Create() function of ScriptableRendererFeature
    public override void Create()
    {
        //construct render pass
        this._renderPass = new RenderPass(materials,injectionEvent);
    }
    
    //implements AddRenderPasses() function of ScriptableRendererFeature
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        _renderPass.SetCamTex(renderer.cameraColorTarget);
        renderer.EnqueuePass(_renderPass);
    }
    
    private class RenderPass:ScriptableRenderPass
    {
        private readonly List<Material> _materials;
        private RenderTargetIdentifier _camTex;
        private RenderTargetHandle _buffer;
        
        /// <summary>
        /// constructor for the render pass also assigns values to the material.
        /// </summary>
        /// <param name="material">material that is used to blit the background tex</param>
        /// <param name="injectionEvent">injection point for the render pass</param>
        public RenderPass(List<Material> material,RenderPassEvent injectionEvent)
        {
            _materials = material;
            this.renderPassEvent = injectionEvent;
        }
        
        /// <summary>
        /// sets the target texture of the render pass. Typically this is the camera's tex since were doing a post-process.
        /// </summary>
        /// <param name="target">identifier for the target texture</param>
        public void SetCamTex(RenderTargetIdentifier target)
        {
            _camTex = target;
        }
        
        //implements the Execute function of ScriptableRenderPass
        public override void Execute(
            ScriptableRenderContext context,
            ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get("Blit background");
            
            var texDesc = renderingData.cameraData.cameraTargetDescriptor;
            texDesc.depthBufferBits = 0;
            cmd.GetTemporaryRT(_buffer.id, texDesc, FilterMode.Bilinear);
            
            foreach (var mat in _materials)
            {
                Blit(cmd, _camTex, _buffer.Identifier(), mat, 0);
                Blit(cmd, _buffer.Identifier(), _camTex);    
            }
            
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
        
        public override void FrameCleanup(CommandBuffer cmd) {
            //release buffer tex
            cmd.ReleaseTemporaryRT(_buffer.id);
        }
    }
}
