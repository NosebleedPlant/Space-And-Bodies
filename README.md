# Space-And-Bodies

### About
Space and Bodies was a collection of art works that I made from Sep-Dec 2022. They are an exploration of various anxieties and an attempt at reconciliation with different facets that define me. It was meant for display at the UAlberta DSC therefore [Re;] provides a version of the original collection that has been adapted to a more accessable format and also provides demo scenes showing off enhanced versions of the various shaders and render features I developed for this project. [Re;] is split into three sections
  1. Fractal Demo Scene
  2. Shader Demo Scene
  3. Gallery Scnee (Waiting on the recorded footage from the DSC to make this available, sorry.).

## Fractal Demo Scene
This fractal explorere originally was made in unity BRP as part of a interactive art work for the 4k touch table at the DSC in the University of Alberta. The work, titled "I contain Multitudes", supported multi touch controls and a video of it in action is available in the gallery scene. For [Re;] I ported re-implemented the code in unity's Universal Render Pipeline

### File/Folder Structure
![test-Page-1 (1)](https://user-images.githubusercontent.com/42461443/210488398-5b72b231-42ec-4873-99a5-866e64296b8b.svg)

### Approach and References
The rendering of the fractal and errant objects is handled through a single compute shader (FractalShader). Orignally the project also served as an excuse to familiarize myself with compute shaders and raymarching. The shader has exposed lighting color and fractal parameters allowing the look of the scene to be adjusted from the inspector. A custom render feature was written in RayMarcher.cs to actually render the output of the FractalShader to screen. By shifting to URP I could now also specify the injection point at which the ouput of the FractalShader is sent to the camera. This means I could also URP's post processing stack by specifying and injection point that was before the post processing is done, which let me get some nice bloom and depth of feild effects. 

The fundamental structure of the FractalShader is based on the raytracing implementation discussed by three-eyed games [[1](http://blog.three-eyed-games.com/2018/05/03/gpu-ray-tracing-in-unity-part-1/)]. The code for the ray marcher is based on this tutorial by Jamie Wong [[2](https://jamie-wong.com/2016/07/15/ray-marching-signed-distance-functions/#the-raymarching-algorithm)] and distance estimator used to render the fractal was Mikael Christensen [[3](http://blog.hvidtfeldts.net/index.php/2011/09/distance-estimated-3d-fractals-v-the-mandelbulb-different-de-approximations/)] on their blog. The smooth min function used for blending shapes in the scene was from Inigo Quilez [[4](https://www.iquilezles.org/www/articles/smin/smin.htm)]. Inspiration for this project came from this video [[5](https://youtu.be/Cp5WWtMoeKg)] by Sebastian Lague 

### Preview
https://user-images.githubusercontent.com/42461443/210475242-356a1617-8385-4263-85bb-193954fa77ae.mp4


## Shader Demo Scene
Alot of these shaders were developed initally during exprementation for a video art peice titled "I'm figuring it out". In [Re;] I refined the the shaders and explored ways to improve their effectiveness by utalizing custom render features. A short discussion on each effect is avilable under the file/folder structure section.

### File/Folder Structure
![test-Page-2](https://user-images.githubusercontent.com/42461443/210497577-81372db4-0d39-4166-8259-597de543c253.svg)

### File/Folder Structure
https://user-images.githubusercontent.com/42461443/210475250-82ee6abe-5461-4069-8cc3-c3233d866e8d.mp4

https://user-images.githubusercontent.com/42461443/210475254-8f36ef62-1b38-440a-a306-bb8d9fec8fec.mp4

https://user-images.githubusercontent.com/42461443/210475262-272fc772-e74e-4e4a-9453-35d40717032d.mp4

https://user-images.githubusercontent.com/42461443/210475271-d06acbab-bbcb-4ee8-b149-c40af4deee0d.mp4

## References and Resources
[GPU Ray Tracing in Unity – Part 1](http://blog.three-eyed-games.com/2018/05/03/gpu-ray-tracing-in-unity-part-1/)

[Ray Marching and Signed Distance Functions](https://jamie-wong.com/2016/07/15/ray-marching-signed-distance-functions/#the-raymarching-algorithm)

[Distance Estimated 3D Fractals (V): The Mandelbulb & Different DE Approximations](http://blog.hvidtfeldts.net/index.php/2011/09/distance-estimated-3d-fractals-v-the-mandelbulb-different-de-approximations/)

[smooth minimum](https://www.iquilezles.org/www/articles/smin/smin.htm)

[Coding Adventure: Ray Marching](https://youtu.be/Cp5WWtMoeKg)

