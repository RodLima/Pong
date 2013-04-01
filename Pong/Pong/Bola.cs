using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Pong
{
    class Bola
    {
        /// <summary>
        /// Imagem da bola
        /// </summary>
        public Texture2D Imagem { get; set; }

        /// <summary>
        /// Posição da bola na tela
        /// </summary>
        public Vector2 Posicao { get; set; }

        /// <summary>
        /// Direção da bola
        /// </summary>
        public Vector2 Direcao { get; set; }

        /// <summary>
        /// Velocidade de deslocamento da bola
        /// </summary>
        private float velocidade;

        /// <summary>
        /// Construtor da classe bola
        /// </summary>
        /// <param name="game">Jogo que instânciou a bola</param>
        /// <param name="position">Posição de inicio da bola</param>
        public Bola(Game game, Vector2 position)
        {
            Imagem = game.Content.Load<Texture2D>(@"Imagens\bola");
            Posicao = position;
            Direcao = new Vector2(1.0f, 1.0f);
            velocidade = 250.0f;
        }

        /// <summary>
        /// Atualiza a posição da bola
        /// </summary>
        /// <param name="gameTime">Tempo de jogo</param>
        public void Update(GameTime gameTime)
        {
            Posicao += Direcao * velocidade * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        /// <summary>
        /// Desenha a bola na tela
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
            return new Rectangle
                ((int)Posicao.X,                    // X 
                (int)Posicao.Y,                     // Y
                (int)Imagem.Width,                 // Largura
                (int)Imagem.Height);               // Altura
        }
    }
}
