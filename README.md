# wrapper-jpeg-compressor
Tool from cmd line to optimize jpeg images directory.
The original project [this](https://github.com/danielgtaylor/jpeg-archive), at the moment can you can convert a photo at a 
time so i automated the job for directory.


## Debug
If you want try this code remember to add the parameters for debugging, with Visual studio: 
>Project ->  Property [Project Name] -> Debug -> command line arguments

## Syntax
>JpegOptimizationForWeb [OPTION] [SOURCE DIR] [DESTINATION DIR]

Examples:
 JpegOptimizationForWeb ssim C:\source\dir C:\destination\dir

Supported compression type (for more information visit original project):
>mpe		Mean pixel error
>ssim		Structural similarity
>ms-ssim	Multi-scale structural similarity (slow!)
>smallfry	Linear-weighted BBCQ-like
