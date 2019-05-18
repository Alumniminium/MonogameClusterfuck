using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameClusterFuck;

namespace One.Primitives
{
   class ConvexHull
    {
        static BasicEffect drawingEffect;
        static VertexDeclaration vertexDecl;

        public static void InitializeStaticMembers(GraphicsDevice device)
        {
            //by making these variables static between objects,
            //we save time and memory
            drawingEffect = new BasicEffect(device);
            drawingEffect.TextureEnabled = false;
            drawingEffect.VertexColorEnabled = true;
            drawingEffect.LightingEnabled = false;

            vertexDecl = new VertexDeclaration(VertexPositionColor.VertexDeclaration.GetVertexElements());
            PresentationParameters pp = device.PresentationParameters;
            Matrix proj = Matrix.CreateOrthographicOffCenter(0, pp.BackBufferWidth, pp.BackBufferHeight, 0, 1, 50);
            Matrix viewMatrix = Matrix.CreateLookAt(new Vector3(0, 0, 5), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
            drawingEffect.World = Matrix.Identity;
            drawingEffect.Projection = proj;
            drawingEffect.View = viewMatrix;
        }


        private Game game;
        private VertexPositionColor[] vertices;
        private int[] indices;
        int vertexCount;

        bool[] backFacing;
        VertexPositionColor[] shadowVertices;


        private Vector2 position = Vector2.Zero;
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        
        public ConvexHull(Vector2[] points, Color color, Vector2 position)
        {
            this.game = Engine.Instance;
            this.position = position;

            vertexCount = points.Length;
            vertices = new VertexPositionColor[vertexCount + 1];
            Vector2 center = Vector2.Zero;
            
            for (int i = 0; i < vertexCount; i++)
            {
                vertices[i] = new VertexPositionColor();
                vertices[i].Position =  new Vector3(points[i], 0);
                vertices[i].Color = color;

                center += points[i];
            }
            
            center /= points.Length;

            vertices[vertexCount] = new VertexPositionColor();
            vertices[vertexCount].Position = new Vector3(center, 0);
            vertices[vertexCount].Color = color;
            
            indices = new int[vertexCount + 2];

            indices[0] = vertexCount;
            for (int i = 0; i < vertices.Length; i++)
            {
                indices[i+1] = i;
            }
            indices[vertexCount + 1] = 0;

            backFacing = new bool[vertexCount];
        }

        public void Draw()
        {
            GraphicsDevice device = game.GraphicsDevice;
            //device.RasterizerState.AlphaBlendEnable = false;

            //device.VertexDeclaration = vertexDecl;

            drawingEffect.World = Matrix.CreateTranslation(position.X, position.Y, 0);
            //drawingEffect.Begin();

            foreach (EffectPass pass in drawingEffect.CurrentTechnique.Passes)
            {
                //pass.Begin();
                #warning PrimitiveType was TriangleFan
                device.DrawUserIndexedPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, vertices, 0, vertices.Length, indices, 0, vertexCount);
                //pass.End();
                
            }
            //drawingEffect.End();
        }

        public void DrawShadows(LightSource lightSource)
        {
            //compute facing of each edge, using N*L
            for (int i = 0; i < vertexCount; i++)
            {
                Vector2 firstVertex = new Vector2(vertices[i].Position.X, vertices[i].Position.Y) + position;
                int secondIndex = (i + 1) % vertexCount;
                Vector2 secondVertex = new Vector2(vertices[secondIndex].Position.X, vertices[secondIndex].Position.Y) + position;
                Vector2 middle = (firstVertex + secondVertex) / 2;

                Vector2 L = lightSource.Position - middle;

                Vector2 N = new Vector2();
                N.X = - (secondVertex.Y - firstVertex.Y);
                N.Y = secondVertex.X - firstVertex.X;
                
                if (Vector2.Dot(N, L) > 0)
                    backFacing[i] = false;
                else
                    backFacing[i] = true;
            }
            
            //find beginning and ending vertices which
            //belong to the shadow
            int startingIndex=0;
            int endingIndex=0;
            for (int i = 0; i < vertexCount; i++)
            {
                int currentEdge = i;
                int nextEdge = (i + 1) % vertexCount;

                if (backFacing[currentEdge] && !backFacing[nextEdge])
                    endingIndex = nextEdge;

                if (!backFacing[currentEdge] && backFacing[nextEdge])
                    startingIndex = nextEdge;
            }

            int shadowVertexCount;

            //nr of vertices that are in the shadow

            if (endingIndex > startingIndex)
                shadowVertexCount = endingIndex - startingIndex+1;
            else
                shadowVertexCount = vertexCount + 1 - startingIndex + endingIndex ;

            shadowVertices = new VertexPositionColor[shadowVertexCount * 2];

            //create a triangle strip that has the shape of the shadow
            int currentIndex = startingIndex;
            int svCount = 0;
            while (svCount != shadowVertexCount*2)
            {
                Vector3 vertexPos = vertices[currentIndex].Position + new Vector3(position,0);
                
                //one vertex on the hull
                shadowVertices[svCount] = new VertexPositionColor();
                shadowVertices[svCount].Color = Color.TransparentBlack;
                shadowVertices[svCount].Position = vertexPos;

                //one extruded by the light direction
                shadowVertices[svCount+1] = new VertexPositionColor();
                shadowVertices[svCount + 1].Color = Color.TransparentBlack;
                Vector3 L2P = vertexPos - new Vector3(lightSource.Position,0);
                L2P.Normalize();
                shadowVertices[svCount + 1].Position = new Vector3(lightSource.Position,0) + L2P *9000;
                
                svCount+=2;
                currentIndex = (currentIndex + 1) % vertexCount;
            }


            //draw the shadow geometry
            //game.GraphicsDevice.VertexDeclaration = vertexDecl;

            drawingEffect.World = Matrix.Identity;
            //drawingEffect.Begin();
            //drawingEffect.CurrentTechnique.Passes[0].Begin();
            
            game.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, shadowVertices, 0, shadowVertexCount*2-2);

            //drawingEffect.CurrentTechnique.Passes[0].End(); 
            //drawingEffect.End();

        
        }
	
        
    }
}