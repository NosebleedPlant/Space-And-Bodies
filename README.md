# [Re:]Space-And-Bodies

### About
Space and Bodies was a collection of art works that I made from Sep-Dec 2022. They are an exploration of various anxieties and an attempt at reconciliation with different facets that define me. It was meant for display at the UAlberta DSC therefore [Re;] provides a version of the original collection that has been adapted to a more accessable format and also provides demo scenes showing off enhanced versions of the various shaders and render features I developed for this project. [Re:] is split into three sections
  1. Fractal Demo Scene
  2. Shader Demo Scene
  3. Gallery Scnee (Waiting on the recorded footage from the DSC to make this available, sorry.).

https://user-images.githubusercontent.com/42461443/210529297-732b5697-3539-4547-a032-cc7917f59056.mp4

## Fractal Demo Scene
This fractal explorere originally was made in unity BRP as part of a interactive art work for the 4k touch table at the DSC in the University of Alberta. The work, titled "I contain Multitudes", supported multi touch controls and a video of it in action is available in the gallery scene. For [Re:] I ported re-implemented the code in unity's Universal Render Pipeline

### File/Folder Structure
![test-Page-1 (1)](https://user-images.githubusercontent.com/42461443/210488398-5b72b231-42ec-4873-99a5-866e64296b8b.svg)

### Approach and References
The rendering of the fractal and errant objects is handled through a single compute shader (FractalShader). Orignally the project also served as an excuse to familiarize myself with compute shaders and raymarching. The shader has exposed lighting color and fractal parameters allowing the look of the scene to be adjusted from the inspector. A custom render feature was written in RayMarcher.cs to actually render the output of the FractalShader to screen. By shifting to URP I could now also specify the injection point at which the ouput of the FractalShader is sent to the camera. This means I could also URP's post processing stack by specifying and injection point that was before the post processing is done, which let me get some nice bloom and depth of feild effects. 

The fundamental structure of the FractalShader is based on the raytracing implementation discussed by three-eyed games [[1](http://blog.three-eyed-games.com/2018/05/03/gpu-ray-tracing-in-unity-part-1/)]. The code for the ray marcher is based on this tutorial by Jamie Wong [[2](https://jamie-wong.com/2016/07/15/ray-marching-signed-distance-functions/#the-raymarching-algorithm)] and distance estimator used to render the fractal was Mikael Christensen [[3](http://blog.hvidtfeldts.net/index.php/2011/09/distance-estimated-3d-fractals-v-the-mandelbulb-different-de-approximations/)] on their blog. The smooth min function used for blending shapes in the scene was from Inigo Quilez [[4](https://www.iquilezles.org/www/articles/smin/smin.htm)]. Inspiration for this project came from this video [[5](https://youtu.be/Cp5WWtMoeKg)] by Sebastian Lague 

### Preview
https://user-images.githubusercontent.com/42461443/210475242-356a1617-8385-4263-85bb-193954fa77ae.mp4


## Shader Demo Scene
Alot of these shaders were developed initally during exprementation for a video art peice titled "I'm figuring it out". In [Re;] I refined the the shaders and explored ways to improve their effectiveness by utalizing custom render features. A very high-level discussion on each effect is avilable under the file/folder structure section. In the project I both utalized the BLIT_ render features that I wrote as well as Cyanilux's Blit feature [[6](https://github.com/Cyanilux/URP_BlitRenderFeature)]. The reason for this is, during development I wanted to be able to apply multiple materials at the same enjection point or manipulate certain properties which is not supported by Cyan's blit. Where custom funcationality was not needed I opted to simply use Cyan's blit. 

All the scenes utalize the BLIT_background feature along with materials in the MT_Enviorment folder to apply a texture to the screen before opaqes are rendered. I mention this here to avoid repeating it in the other sections. Variations of these material use shaders that either distort the base texture to create interesting patterns or apply masks for layering different textures.

All scenes also use the post processing stack to improve appearnce and apply provided effects such as noise and color grading.

### File/Folder Structure
![test-Page-2](https://user-images.githubusercontent.com/42461443/210497577-81372db4-0d39-4166-8259-597de543c253.svg)

### Scene 00: Abberation, Downsampling and localized distortion
https://user-images.githubusercontent.com/42461443/210475250-82ee6abe-5461-4069-8cc3-c3233d866e8d.mp4

This scene utalizes three shaders: NoiseDistort, RefractionDistort, DownscaleAbberation. The NoiseDistort utalizes procedural noise to generate a noisy black texture. Noise is also used to distort the vertices of the sphere along the normals to give a glitchy effect. The vertex distortion is masked using a band that oscillates from top to bottom. RefractionDistort is a very simple shader that applies a simple refraction effect to the scene texture. There are multiple methods that can be used to apply these materials but I chose to utalize the RenderObjects feature provided by unity. First the sphere is rendered using the NoiseDistort and then rendered again using the RenderObject pass with NoiseDistort set as a material override. Finally a custom post process is applied using a blit pass and the DownscaleAbberation material. The DownscaleAbberation takes the camera texture and samples it with offset UVs to create the chromatic abberation. It also posterizes the UVs to sample a lower resolution version of the texture. Lastly it combines these sampled images through masks generated from the FakeAutomata subshaders. The FakeAutomata subshader is a simple little shader that generates a mask that looks similar to cellular automata moving across the screen.

### Scene 01: Fake Pixel Sorting
https://user-images.githubusercontent.com/42461443/210475254-8f36ef62-1b38-440a-a306-bb8d9fec8fec.mp4

This scene utalizes two shaders and two renderers. The scene contains two cameras. The first camera uses a blit featuere to write distort the image using the Cam2Tex material in FauxSorting Materials folder, onto a render texture. The shader of that material takes the y-componenet of the UVs and uses that to genearte a noise texture that is streached along the y-axis. This value is then scaled based on some manipulated value of sine time (paramters are exposed to allow tuning of this sine time). The scaled noise value is then used to posterize the UVs and sample the texture. The renderer used by this camera does not blit a background texture.
The second camear blits the background textures before rendering opaques, and after rendering them it similarly distorts the image using slightly different sine time based noise. The render texture from the first camera is also sample and combined with the second camera's opaque texture based on scene depth to create the illusion of different layers of distortion.

### Scene 02: CRT effect
https://user-images.githubusercontent.com/42461443/210475262-272fc772-e74e-4e4a-9453-35d40717032d.mp4

The shader used in this custom post processing effect is based largely off the godot shader shared by Pend00 [[7](https://godotshaders.com/shader/vhs-and-crt-monitor-effect/)]. My contribution to it was converting the code from glsl to shadergraph and creating the tearing effect. For a breakdown of the effect I would highly recommend reading through the orignal glsls code and comments. They do an excellent job of explaining how the effect works.

### Scene 03: Stylized Solitaire Glitch
https://user-images.githubusercontent.com/42461443/210475271-d06acbab-bbcb-4ee8-b149-c40af4deee0d.mp4

The scene uses a similar approach to scene 01. Two cameras using two sepreate renderers are utalized to create a solitaire glitch isolated to asaro head bust. First camera only renders the bust onto a render texture but before doing this a custom render feature first renders the content of the render texture on to the screen creating a feedback loop. I learned of this approach from this aritcle by Alan Zucconi [[8](https://www.alanzucconi.com/2016/03/09/simulate-smoke-with-shaders/)] and this effect breakdown by Sorb [[9](https://twitter.com/SoerbGames/status/1570773880444448773?s=20&t=ifWw_myvdOQhEnu-xWt3eQ)]. The render texture containing the resutls of this feedback loop are fed to the second camera. This camera initally renders everything except the best, then renders the content of the render texture onto the screen creating the trails. Once this is done the bust is rendered ontop using the RenderObject feature. The mateiral used when rendering the the trails also fades the image a bit and applies chromatic abberation. Since this effect stacks each frame the result is fading of the trails similar to sorb's exampe. I opted to use a different appraoch for the fading, instead of simply using an alpha clip I decided to darken the image and use the darkness to decide the cipping. This results in a slightly more sylized look along with the chromatic abberation.

## References and Resources
[GPU Ray Tracing in Unity – Part 1](http://blog.three-eyed-games.com/2018/05/03/gpu-ray-tracing-in-unity-part-1/)

[Ray Marching and Signed Distance Functions](https://jamie-wong.com/2016/07/15/ray-marching-signed-distance-functions/#the-raymarching-algorithm)

[Distance Estimated 3D Fractals (V): The Mandelbulb & Different DE Approximations](http://blog.hvidtfeldts.net/index.php/2011/09/distance-estimated-3d-fractals-v-the-mandelbulb-different-de-approximations/)

[smooth minimum](https://www.iquilezles.org/www/articles/smin/smin.htm)

[Coding Adventure: Ray Marching](https://youtu.be/Cp5WWtMoeKg)

[Cyanilux's URP_BlitRenderFeature](https://github.com/Cyanilux/URP_BlitRenderFeature)

[VHS and CRT monitor effect](https://godotshaders.com/shader/vhs-and-crt-monitor-effect/)

[How to Simulate Smoke with Shaders](https://www.alanzucconi.com/2016/03/09/simulate-smoke-with-shaders/)

[Sorb's effect breakdown](https://twitter.com/SoerbGames/status/1570773880444448773?s=20&t=ifWw_myvdOQhEnu-xWt3eQ)

### Unity RP resources
[Intro Renderer Feature in Unity URP - Desaturate Post Process Effect](https://www.youtube.com/watch?v=MLl4yzaYMBY&t=1s&ab_channel=NedMakesGames)

[Example: How to create a custom rendering effect using the Render Objects Renderer Feature](https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@12.1/manual/renderer-features/how-to-custom-effect-render-objects.html)

[How to create a custom Renderer Feature](https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@12.1/manual/containers/create-custom-renderer-feature-1.html)

messing with cyan's blit is a great way to learn
