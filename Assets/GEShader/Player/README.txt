SelfQuest Player Customization Shaders

This folder contains 10 lightweight, stylized shaders designed for simple player avatars composed of primitives (cubes, spheres, pyramids). All shaders are authored for the Built-in Render Pipeline and rely only on UnityCG.cginc. Use them to create unique, readable silhouettes on mobile.

Shader list (menu path: SelfQuest/Player/*):
1) ToonColorBands.shader — Toon-lit with adjustable band steps and rim.
2) GradientFresnel.shader — Vertical gradient tinted by a soft fresnel edge.
3) CheckerObject.shader — Object-space checker pattern that ignores UVs.
4) MatcapSheen.shader — Simple matcap sheen using a provided matcap texture.
5) HologramGrid.shader — Transparent scrolling grid with fresnel and scanlines.
6) PulsingEmissive.shader — Solid color with time-based emissive pulse.
7) MarbleNoise.shader — Procedural marble veins (no textures) in object space.
8) LavaFlow.shader — Procedural flowing emissive using dual scrolling noise.
9) RetroPixelDither.shader — Palette step + 4x4 Bayer ordered dithering.
10) OutlineToon.shader — Two-pass: external outline + toon interior.

Notes:
- Transparent shaders have correct blending tags for additive or alpha.
- Procedural patterns use object-space positions so cubes/spheres/pyramids look consistent without UV work.
- For MatcapSheen provide a small matcap texture (e.g., 256x256, clamp, no mip).

Mobile tips:
- Prefer low band counts, disable real-time shadows if not needed.
- Limit emission intensity to avoid HDR bloom on mobile if post-process is active.
