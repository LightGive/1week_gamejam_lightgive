FlatFX

A pack of geometry-based effects for your 2D Unity projects.

Online manual: http://ax23w4.com/devlog/flatfx

To start using FlatFXjust click Game Object > Effects > FlatFX in Unity's main menu. Or do the same by right-clicking the Hierarchy panel. This will add a polygon with four points to the center of your scene view. This will add a new FlatFX object at 0,0,0 of your scene. You can place it whereven you want on X and Y axes - when you spawn new effect, you'll set the coordinates for them in Vector2 form.

When you have a reference to your FlatFX component, adding the effect is easy:

flatfx.AddEffect(Vector2.zero,0);

First parameter is a coordinates of new effect and second is number of effect type from 0 to 4.

My twitter: @ax23w4
Email: andrii.sudyn@gmail.com