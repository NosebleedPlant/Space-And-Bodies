using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

public class RNDR_SolitairePass : ScriptableRendererFeature
{
    private class RenderPass:ScriptableRenderPass
    {
        private readonly Material _material;
        private RenderTargetIdentifier _source;
        private RenderTargetHandle _bufferTex;
        private RenderTexture _csBuffer;
        private ComputeShader _computeShader;
        
        public RenderPass(Material material, RenderPassEvent renderEvent, ComputeShader cs)
        {
            _material = material;
            renderPassEvent = renderEvent;
            _computeShader = cs;
            _bufferTex.Init("_BufferTexture");
        }

        public void SetSource(RenderTargetIdentifier source)
        {
            _source = source;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get("Test");

            RenderTextureDescriptor cameraTextureDesc = renderingData.cameraData.cameraTargetDescriptor;
            cameraTextureDesc.depthBufferBits = 0;
            cmd.GetTemporaryRT(_bufferTex.id,cameraTextureDesc,FilterMode.Bilinear);

            #region moreTestingCS
                if(_csBuffer == null || _csBuffer.width!=Screen.width || _csBuffer.height != Screen.height) 
                {
                    if (_csBuffer != null)
                        _csBuffer.Release();

                    _csBuffer = new RenderTexture(
                        Screen.width,
                        Screen.height,
                        0,
                        RenderTextureFormat.ARGBFloat,
                        RenderTextureReadWrite.Linear);

                    _csBuffer.enableRandomWrite = true;
                    _csBuffer.Create();
                }
                _computeShader.SetTexture(0, "Result", _csBuffer);
                
                int threadGroupsX = Mathf.CeilToInt(Screen.width / 8.0f);
                int threadGroupsY = Mathf.CeilToInt(Screen.height / 8.0f);
                _computeShader.Dispatch(0, threadGroupsX, threadGroupsY, 1);
                Blit(cmd,new RenderTargetIdentifier(_csBuffer),_source);
            #endregion
            
            // Blit(cmd,_source,_bufferTex.Identifier(),_material,0);
            // Blit(cmd,_bufferTex.Identifier(),_source);
            
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(_bufferTex.id);
        }
    }

    [SerializeField] private Material material;
    [SerializeField] private ComputeShader computeShader;
    private RenderPass _renderPass;
    public override void Create()
    {
        this._renderPass = new RenderPass(
            material,
            RenderPassEvent.AfterRenderingOpaques,
            computeShader
        );
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        _renderPass.SetSource(renderer.cameraColorTarget);
        renderer.EnqueuePass(_renderPass);
    }
}
