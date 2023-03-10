using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class RayMarcher : ScriptableRendererFeature
{
    /// <summary>
    /// This class holds parameters for the compute shader
    /// </summary>
    [System.Serializable]
    public class Settings
    {
        [Header("Rendering")]
        public RenderPassEvent renderPassEvent;
        public ComputeShader rayMarchShader;
        [Range(1,16)] public float fractalPower= 4.44f;
        public bool animate = false;
        
        [Header("Light")]
        public Vector3 lightPos;
        public Vector3 lightForward;
        public float intensity;
        public float shininess = 10.0f;
        
        [Header("Background Colors")]
        public Color bgColorA;
        public Color bgColorB;

        [Header("Fractal Colors")]
        public Color specColor;
        public Color ambientColor;
        public Color diffuseColor;
        public Color bloomColor;
    }
    
    public Settings settings;
    
    //objects
    public static int ObjectCount = 15;
    public static Vector4[] ObjectPositions = new Vector4[ObjectCount];
        
    private RenderPass _renderPass;
    
    //implements Create() function of ScriptableRendererFeature
    public override void Create()
    {
        //ensure that fractal animates when in build
#if !UNITY_EDITOR
        settings.animate = true;
        settings.fractalPower = 4.44f;
#endif
        this._renderPass = new RenderPass(settings);
    }

    //implements AddRenderPasses() function of ScriptableRendererFeature
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        _renderPass.SetTarget(renderer.cameraColorTarget);
        renderer.EnqueuePass(_renderPass);
        
    }
    
    private class RenderPass:ScriptableRenderPass
    {
        private Settings _settings;
        private readonly ComputeShader _computeShader;
        private RenderTargetIdentifier _target;
        private RenderTexture _bufferTex;
        
        //animation
        private float _reset = 20;
        private float _timer = 0;
        private float _startVal = 4.44f;
        private float _endVal = 16;
        
        //camara
        private Camera _camera;
        
        /// <summary>
        /// constructor for the render pass also assigns values to the material.
        /// </summary>
        /// <param name="settings"></param>
        public RenderPass(Settings settings)
        {
            renderPassEvent = settings.renderPassEvent;
            _computeShader = settings.rayMarchShader;
            _settings = settings;
        }

        /// <summary>
        /// sets the target texture of the render pass. Typically this is the camera's tex.
        /// </summary>
        /// <param name="target"></param>
        public void SetTarget(RenderTargetIdentifier target)
        {
            _target = target;
        }

        //implements the Execute function of ScriptableRenderPass called before render
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get("Ray march Fractal");
            _camera = Camera.main;
            
            //create the buffer render tex to hold info from the compute shader
            if(_bufferTex == null || _bufferTex.width!=Screen.width || _bufferTex.height != Screen.height) 
            {
                if (_bufferTex != null)
                    _bufferTex.Release();
        
                _bufferTex = new RenderTexture(
                    Screen.width,
                    Screen.height,
                    0,
                    RenderTextureFormat.ARGBFloat,
                    RenderTextureReadWrite.Linear);
        
                _bufferTex.enableRandomWrite = true;
                _bufferTex.Create();
            }
            //assing buffer to compute shader
            _computeShader.SetTexture(0, "Result", _bufferTex);

            //animate the fractal by manipulating the fractal power
            if(_settings.animate)
                AnimateFractal();
            
            //set parameters of the compute shader
            SetSceneParams();
            SetLightingParams();
            SetCameraParams();
            
            //dispatch compute shader
            int threadGroupsX = Mathf.CeilToInt(Screen.width / 8.0f);
            int threadGroupsY = Mathf.CeilToInt(Screen.height / 8.0f);
            _computeShader.Dispatch(0, threadGroupsX, threadGroupsY, 1);
            
            //blit buffer to the target, in this case the camera's tex
            Blit(cmd,new RenderTargetIdentifier(_bufferTex),_target);

            //excute
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
        
        private void AnimateFractal() 
        {
            _timer += Time.deltaTime;
            _settings.fractalPower = Mathf.Lerp(_startVal, _endVal, _timer / _reset);
            if(_timer > _reset) 
            {
                _timer = 0;
                _startVal = _endVal;
                _endVal = Random.Range(2, 16);
            }
        }
        

        //paramets setting helpers:
        private void SetSceneParams() 
        {
            _computeShader.SetVector("_BGColorA", new Vector4(_settings.bgColorA.r, _settings.bgColorA.g, _settings.bgColorA.b, _settings.bgColorA.a));
            _computeShader.SetVector("_BGColorB", new Vector4(_settings.bgColorB.r, _settings.bgColorB.g, _settings.bgColorB.b, _settings.bgColorB.a));

            _computeShader.SetFloat("_FractalPower", _settings.fractalPower);

            _computeShader.SetFloat("_ObjectCount", ObjectCount);
            _computeShader.SetVectorArray("_ObjectPositions", ObjectPositions);

            _computeShader.SetVector("_SpecColor", new Vector3(_settings.specColor.r, _settings.specColor.g, _settings.specColor.b));
            _computeShader.SetVector("_AmbientColor", new Vector3(_settings.ambientColor.r, _settings.ambientColor.g, _settings.ambientColor.b));
            _computeShader.SetVector("_DiffuseColor", new Vector3(_settings.diffuseColor.r, _settings.diffuseColor.g, _settings.diffuseColor.b));
            _computeShader.SetVector("_BloomColor", new Vector3(_settings.bloomColor.r, _settings.bloomColor.g, _settings.bloomColor.b));
            _computeShader.SetFloat("_Shininess", _settings.shininess);
        }

        private void SetLightingParams()
        {
            _computeShader.SetVector("_LightIntensity", new Vector3(_settings.intensity, _settings.intensity, _settings.intensity));
            
            _computeShader.SetVector("_LightPos", _settings.lightPos);
            _computeShader.SetVector("_LightDirection", _settings.lightForward);
        }

        private void SetCameraParams() 
        {
            _computeShader.SetMatrix("_CameraToWorld", _camera.cameraToWorldMatrix);
            _computeShader.SetMatrix("_CameraInverseProjection", _camera.projectionMatrix.inverse);
        } 
    }
}
