using Impression;
using Impression.Graphics;
using Impression.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Example06Model
{
    public class Example06ModelGame : Impression.Game
    {
		GraphicsDeviceManager graphics;

		Camera camera;

		List<Model> models;
		List<Vector3> modelScales;

		int currentModelIndex;

		Matrix modelTransform;
        float rotationSpeed = 1;

		MouseState currentMouseState;
		MouseState lastMouseState;

        public Example06ModelGame()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);

			switch (FrameworkContext.Platform)
			{
				case PlatformType.Windows:
				case PlatformType.Mac:
				case PlatformType.Linux:
					{
						graphics.PreferredBackBufferWidth = 1280;
						graphics.PreferredBackBufferHeight = 720;

						this.IsMouseVisible = true;
					}
					break;
                case PlatformType.WindowsStore:
                case PlatformType.WindowsMobile:
					{
						graphics.PreferredBackBufferWidth = 1280;
						graphics.PreferredBackBufferHeight = 720;

						graphics.IsFullScreen = true;

						// Frame rate is 30 fps by default for mobile device
						TargetElapsedTime = TimeSpan.FromTicks(TimeSpan.TicksPerSecond / 30L);
					}
					break;
				case PlatformType.Android:
				case PlatformType.iOS:
					{
						graphics.PreferredBackBufferWidth = 1280;
						graphics.PreferredBackBufferHeight = 720;

						graphics.IsFullScreen = true;

						// Frame rate is 30 fps by default for mobile device
						TargetElapsedTime = TimeSpan.FromTicks(TimeSpan.TicksPerSecond / 30L);
					}
					break;
			}

            this.View.Title = "Example06Model";
        }

        protected override void Initialize()
        { 
            base.Initialize();

            camera = new Camera(graphics.GraphicsDevice.Viewport);
			camera.Transform = Matrix.CreateTranslation(0, 2f, 10);
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            // Load all models
			models = new List<Model>
			{
				this.Content.Load<Model>("Models/Tank/tank"),
			};

			// Make fixed scale to easy visibility
			modelScales = new List<Vector3>
			{
				Vector3.One,
			};
        }

        protected override void UnloadContent()
        {
            // do something

            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
			switch (FrameworkContext.Platform)
			{
				case PlatformType.Windows:
				case PlatformType.Mac:
				case PlatformType.Linux:
					{
						if (Keyboard.GetState().IsKeyDown(Keys.Escape))
							this.Exit();
					}
					break;
				default:
					{
						// do nothings
					}
					break;
			}

			lastMouseState = currentMouseState;
			currentMouseState = Mouse.GetState();

			// Allow models to sequenced when left button was just pressed
			if (lastMouseState.LeftButton == ButtonState.Released &&
				currentMouseState.LeftButton == ButtonState.Pressed)
			{
				currentModelIndex++;

				// When current model index is out of range, reset index to zero
				if (!(currentModelIndex < models.Count))
				{
					currentModelIndex = 0;
				}
			}

			modelTransform = Matrix.CreateScale(modelScales[currentModelIndex]) * 
			                       Matrix.CreateRotationY((float)gameTime.TotalGameTime.TotalSeconds * rotationSpeed) * 
			                       Matrix.CreateTranslation(Vector3.Zero);
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
			graphics.GraphicsDevice.Clear(Color.BurlyWood);

			// Draw model
			models[currentModelIndex].Draw(modelTransform, camera.View, camera.Projection);
            
            base.Draw(gameTime);
        }
    }
}