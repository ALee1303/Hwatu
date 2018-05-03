using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MG_Library
{
    // Inheritable Singleton
    // http://www.devartplus.com/singleton-inheritance-in-c-net-part-1/
    public abstract class ScreenManager2D<DerivedType>
        where DerivedType : new()
    {
        private static DerivedType instance;
        public static DerivedType Instance
        {
            get
            {
                if (instance == null)
                    instance = new DerivedType();
                return instance;
            }
        }

        public Vector2 Dimension { get; private set; }
        public ContentManager Content { get; private set; }
        public GraphicsDevice GraphicsDevice { get; private set; }
        public SpriteBatch SpriteBatch { get; private set; }

        protected GameScreen currentScreen;

        public virtual void Initialize(GraphicsDevice graphicsDevice)
        {
            this.GraphicsDevice = graphicsDevice;
            
        }

        public virtual void LoadContent(ContentManager Content, SpriteBatch spriteBatch)
        {
            this.Content = new ContentManager(Content.ServiceProvider, "Content");
            this.SpriteBatch = spriteBatch;
        }

        public abstract void UnloadContent();

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(SpriteBatch spriteBatch);

        protected void ValidateSingletonCreation()
        {
            if (instance != null)
                throw new ApplicationException("Singleton Already Exist");
        }
    }
}
