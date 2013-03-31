using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Pong
{
    class Barra
    {
         /// <summary>
        /// Imagem do barra
        /// </summary>
        public Texture2D Imagem { get; set; }

        /// <summary>
        /// Posição do barra na tela
        /// </summary>
        public Vector2 Posicao { get; set; }

        /// <summary>
        /// Direção de movimento da barra
        /// </summary>
        public Vector2 Direcao { get; set; }

        /// <summary>
        /// Velocidade de deslocamento da barra
        /// </summary>
        private float velocidade { get; set; }

        /// <summary>
        /// Construtor da classe barra
        /// </summary>
        /// <param name="game">Jogo que instânciou a barra</param>
        /// <param name="position">Posição de inicio da barra</param>
        public Barra(Game game, Vector2 position)
        {
            Imagem = game.Content.Load<Texture2D>(@"Imagens\barra");
            Posicao = position;
            Direcao = new Vector2(0.0f, 0.0f);
            velocidade = 150.0f;
        }

        /// <summary>
        /// Atualiza a posição da barra
        /// </summary>
        /// <param name="gameTime">Tempo de jogo</param>
        public void Update(GameTime gameTime)
        {
            Posicao += Direcao * velocidade * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        /// <summary>
        /// Desenha a barra na tela
        /// </summary>
        /// <param name="spriteBatch">Spritebatch para desenho</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Imagem, Posicao, Color.White);
        }

        /// <summary>
        /// Área de colisão retangular da sprite
        /// </summary>
        /// <returns>Retorna um retângulo de colisão</returns>
        public Rectangle GetBounding()
        {
            return new Rectangle((int)Posicao.X,   // X
                (int)Posicao.Y,                    // Y
                (int)Imagem.Width,                 // Largura
                (int)Imagem.Height);               // Altura
        }
    }
}
