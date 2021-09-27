# Endless Freeride
<img src="https://github.com/Kfollen93/Endless-Freeride/blob/main/Gif/snowboardCoverGif.gif" width="640" height="360"/>

You can download the Android .APK file from my Itch.io page here: <a href="https://kfollen.itch.io/endless-freeride">Endless Freeride</a> <br>
## Description
<i>*The private repository containing all project files and commit messages may be available upon request.</i> <br>
<br>
Drop in and shred down the mountain while getting some air and performing tricks all while trying to line up for a perfect landing in this snowboarding version of a casual infinite-runner.<br>
<br>
There is also a built-in average FPS counter that you can toggle off/on for your convenience.

## Development
This project was my first step into mobile game development, while simultaneously providing me with an opportunity to experiment more with <a href="https://docs.unity3d.com/ScriptReference/Rigidbody.html">Rigidbodies</a>. My prior projects were all based around using Unity's built-in Character Controller. I had used some Rigidbodies before for some enemies and collision checks, but I felt like a game based around motion would be a fun way to experience building a Rigidbody character controller, and applying physics to move the player.

## Learning Outcome
Making a mobile game taught me a lot about optimization. Upon my first test of the game on an old 2013 Android phone, I was shocked to see my game only getting around 12-15fps; there was hardly anything in the scene! I then blindly started cutting everything out: post processing, lighting, shadows, etc. Although this brute-force/blind approach got me up to ~25fps, it also tanked the quality of the game. I then slowed down to try and actually understand what the Unity Profiler is showing me, along with analyzing the "Stats" window. With a few more lighting changes, removing extraneous quality settings, and cleaning up some code I was able to get the game around ~45fps. It was better, but I really had 60fps in mind; which for a brand new Android phone, it probably wouldn't have been too difficult, but I wanted this game to run at 60fps no matter how old the Android device was.<br>

## Unity Profiler
I spent some time reading documentation and tutorials surrounding the Profiler and the Stats window. I needed to look into whether I was GPU-bound or CPU-bound, what was happening in terms of garbage collection, and why I was seeing some pretty huge spikes being displayed. <br>
<br>
Going back to the stats window, I noticed that I had an absurdly large polygon count. I hardly had anything in the scene, so I was really confused where this was coming from. After turning a few objects off and on, I quickly narrowed it down to my character model, and the fences that I made in Blender. I fixed the fence by using a transparent chain-link texture on a plane instead of a .fbx model, and for the character I imported it into Blender and used the decimate tool to bring down the polygon count to about 4.5k. <br>
<br>
While this had brought the frame rate up, my 2013 Android test phone was still struggling to maintain a 60fps, especially as the game was played for longer periods of time. After spending some more time reviewing Unity's Profiler and documentation, I knew I had a few performance upgrades that I could now make through code. One of those optimizations were removing all of my instantiate/destroy calls. I was originally instantiating and destroying the infinite grounds, each time placing one ahead of the other. However, I realized this was a great time to implement an object pooler for all the ground platforms. Using a pooler would prevent any garbage issues that may have arose by instead of creating/destroying the platforms, being able to simply active/deactivate them; to further this, for respawning the player I ended up just moving the position, rather than the (horrible) decision I first had of destroying and instantiating which also relied on several `FindWithTag()` calls. These changes increased performance while simultaneously reducing the size of the spikes I was seeing in the Profiler, which appeared to come from the instantiating platform calls. <br>

## Performance Outcome
I was able to increase performance by over 400%. This is referring to my initial test of the game running at around ~12fps, to my final test with all optimizations in place running at an average of ~60fps (which is the target cap). With all of these optimization changes I now actually had extra room for performance and I could tweak the settings a bit that made the game look better and provide the player with some visual feedback (things like shadows as the player is coming down from a jump).

## Additional Information
<ul>
  <li>Made with: Unity 2020.3.11f1</li>
</ul>
