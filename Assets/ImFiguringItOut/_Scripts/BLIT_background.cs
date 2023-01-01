using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BLIT_background : ScriptableRendererFeature
{
    [SerializeField] private Material material;
    [SerializeField] private Texture backgroundTexture;
    [SerializeField] private float colorMapStrength;

    private bool _isPassReady = true;
    
    private RenderPass _renderPass;
    private static readonly int MapStrength = Shader.PropertyToID("_MapStrength");

    //implements Create() function of ScriptableRendererFeature
    public override void Create()
    {
        if (material == null || backgroundTexture == null)
        {
            _isPassReady = false;
            return;
        }
        
        //construct render pass and set ready flag.
        this._renderPass = new RenderPass(
            material,
            backgroundTexture,
            colorMapStrength
        );
        _isPassReady = true;
    }
    
    //implements AddRenderPasses() function of ScriptableRendererFeature
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (!_isPassReady)
            return;
        
        _renderPass.SetCamTex(renderer.cameraColorTarget);
        renderer.EnqueuePass(_renderPass);
    }
    
    #region Render Pass
        private class RenderPass:ScriptableRenderPass
        {
            private readonly Material _material;
            private RenderTargetIdentifier _camTex;
            private RenderTargetHandle _buffer;
            private readonly Texture _texture;
            
            /// <summary>
            /// constructor for the render pass also assigns values to the material.
            /// </summary>
            /// <param name="material">material that is used to blit the background tex</param>
            /// <param name="tex">texture that will be blit as the background</param>
            /// <param name="strength">strength of color re-map applied to tex in material</param>
            public RenderPass(
                Material material,
                Texture tex,
                float strength)
            {
                _material = material;
                _texture = tex;
                
                material.SetFloat(MapStrength,strength);
                
                this.renderPassEvent = RenderPassEvent.BeforeRenderingOpaques;
            }
            
            /// <summary>
            /// sets the target texture of the render pass. Typically this is the camera's tex.
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
                
                Blit(cmd, _texture, _buffer.Identifier(), _material, 0);
                Blit(cmd, _buffer.Identifier(), _camTex);
                
                context.ExecuteCommandBuffer(cmd);
                CommandBufferPool.Release(cmd);
            }
            
            public override void FrameCleanup(CommandBuffer cmd) {
                //release buffer tex
                cmd.ReleaseTemporaryRT(_buffer.id);
            }
        }
    #endregion
}
