# Virtual Deskcat
 Your feline virtual desktop companion that you can talk to.
 Software used: Unity2D

 # Disclaimer⚠️
 The project is still under development and contains a myriad of bugs. Currently all features listed in the readme below has already been implemented to a working extent.

 # About
 Virtual Deskcat aims to be the modernized version of Virtual Assistants like BonziBuddy and Microsoft Cortana. However Deskcat is unique in the way that it utilizes **Machine Learning technology** to create a more seamless and personalized interaction with the host.

> Woah, machine learning technology... so fancy. trust me bro, its really self explanatory

# Launch Instructions and features
Just pull the project, open in Unity3D, and build. Open the exe and you should be greeted with a cat!
Or you could just download from releases. :)

## Features
1. Talk to VD by speaking into the microphone
2. Ask VD to generate images by saying stuff like "I want a picture of ____" or "You know what would be nice? an image of a ____"

 ## How VD utilizes Machine Learning Technology
 VD has 3 primary ML Models:
 - OpenAIs Whisper-Tiny Model, used primarily for speech recgonition
 - GPT-sw3-1.3b, a collection of large decoder-only pretrained transformer language models trained off of over 1.3b parameters, primarily used for conversations and sentence transforming
 - stable diffusion v1-5, a lightweight checkpoint used for Text to image generation.

These models are all baked into the software, using Interface endpoints via cURL to the models, cloud hosted on HuggingFace.

> Aka it uses an API hehe


 
