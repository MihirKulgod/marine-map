import numpy as np
from noise import pnoise2
from PIL import Image
import argparse

def apply_tint(tilemap):
    tint = np.array([-25, -15, 30], dtype=np.int16)
    tilemap_rgb = tilemap.astype(np.int16)
    tilemap_rgb = np.clip(tilemap_rgb + tint, 0, 255).astype(np.uint8)
    return tilemap_rgb

def generate_tilemap(width, height, noise_scale, seed):
    tilemap = np.zeros((height, width))

    for y in range(height):
        for x in range(width):
            nx = ((x + seed) / width) * noise_scale
            ny = ((y + seed) / height) * noise_scale

            terrain_height = pnoise2(nx, ny, octaves=4)
            tilemap[y, x] = terrain_height

    # Normalize to 0–255
    tilemap = (tilemap - tilemap.min()) / (tilemap.max() - tilemap.min())
    tilemap = (tilemap * 255).astype(np.uint8)

    return tilemap

def generate_coral(tilemap, terrain_heightmap, coral_noise_scale, seed):
    height, width, _ = tilemap.shape

    for y in range(height):
        for x in range(width):
            depth = terrain_heightmap[y, x] / 255.0

            # Only place coral at suitable depths
            if 0.4 < depth < 0.6:
                nx = ((x + seed) / width) * coral_noise_scale
                ny = ((y + seed) / height) * coral_noise_scale

                coral_noise = pnoise2(nx, ny, octaves=4)

                # Noise threshold for placing coral
                if coral_noise > 0.2:
                    # Coral appears bright red
                    tilemap[y, x] = [255, 0, 0]

    return tilemap
            

def save_image(tilemap, filename):
    img = Image.fromarray(tilemap, mode="RGB")
    img.save(filename)

if __name__ == "__main__":
    # CLI Arguments
    parser = argparse.ArgumentParser(description="Generate a procedural tilemap.")

    parser.add_argument("--size", type=int, default=32)
    parser.add_argument("--seed", type=int, default=42)
    parser.add_argument("--terrain-scale", type=float, default=3)
    parser.add_argument("--coral-scale", type=float, default=12)
    parser.add_argument("--output", type=str, default="output.png")

    args = parser.parse_args()

    terrain_heightmap = generate_tilemap(args.size, args.size, args.terrain_scale, args.seed)
    tilemap = np.stack([terrain_heightmap, terrain_heightmap, terrain_heightmap], axis=-1)
    tilemap = generate_coral(tilemap, terrain_heightmap, args.coral_scale, args.seed)
    tilemap = apply_tint(tilemap)
    save_image(tilemap, args.output)