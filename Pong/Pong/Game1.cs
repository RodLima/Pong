using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Pong
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        // Imagem de fundo do jogo
        Texture2D background;
        // Declara uma variavel do tipo bola (objeto)
        Bola bola;
        // Declara duas variaveis do tipo barra (objetos)
        Barra barra1, barra2;
        // Pontuação dos jogadores 
        // indice 0 = jogador 1
        // indice 1 = jogador 2
        int[] score = new int[2];
        // SpriteFont para escrever na tela
        SpriteFont fontScore;
        // Efeito sonoro da colisão da bola com a barra
        SoundEffect soundToc1 = null;
        // Efeito sonoro da colisão da bola com o campo
        SoundEffect soundToc2 = null;
        // Efeito sonoro para quando marca ponto
        SoundEffect soundPoint = null;
        // Música de fundo do jogo
        Song music = null;
        // Variável que armazena o estado do jogo
        // Iniciamos a variável na tela inicial (IntroScreen)
        PongState state = PongState.IntroScreen;
        // Variável para armazenar a imagem da tela inicial
        Texture2D intro = null;
        // Variável para armazenar a imagem da tela de game over
        Texture2D gameover = null;
        // Pontução máxima do jogo
        const int POINT_COUNT = 3;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            // Altera o vídeo para 800 pixels de largura
            graphics.PreferredBackBufferWidth = 800;
            // Altera o vídeo para 600 pixels de altura
            graphics.PreferredBackBufferHeight = 600;
            // Define o título da janela
            Window.Title = "UCPel - Pong";
        }

        protected override void Initialize()
        {
            // Inicializando a pontuação dos jogadores
            score[0] = 0; // Jogador 1
            score[1] = 0; // Jogador 2
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // Carrega a imagem de fundo do jogo
            background = Content.Load<Texture2D>(@"Imagens\gameplay");
            // Instância a bola
            bola = new Bola(this, new Vector2(386.0f, 310.0f));
            // Instância o primeiro barra
            barra1 = new Barra(this, new Vector2(10.0f, 290.0f));
            // Instância o segundo jogador
            barra2 = new Barra(this, new Vector2(765.0f, 290.0f));
            // Carrega a font para escrita
            fontScore = Content.Load<SpriteFont>(@"Fonts\score");
            // Carrega o efeito sonoro da colisão da bola com o campo
            soundToc1 = Content.Load<SoundEffect>(@"Audio\toc_1");
            // Carrega o efeito sonoro da colisão da bola com as barras
            soundToc2 = Content.Load<SoundEffect>(@"Audio\toc_2");
            // Carrega ó efeito sonoro para quando marcar pontos
            soundPoint = Content.Load<SoundEffect>(@"Audio\point");
            // Carrega a música de fundo do jogo
            music = Content.Load<Song>(@"Audio\music");
            // Carrega a textura da tela inicial
            intro = Content.Load<Texture2D>(@"Imagens\intro");
            // Carrega a textura de game over
            gameover = Content.Load<Texture2D>(@"Imagens\gameover");
            // Toca a música de fundo
            MediaPlayer.Play(music);
            // Coloca a música de fundo em loop infinito
            MediaPlayer.IsRepeating = true;
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            // Recebe o estado do teclado
            KeyboardState keyState = Keyboard.GetState();
            switch (state)
            {
                case PongState.IntroScreen:
                    // Entradas do jogador
                    if (keyState.IsKeyDown(Keys.D1))//Se pressionar a tecla 1
                    {
                        state = PongState.SinglePlayer;
                        RestartGame();
                    }
                    if (keyState.IsKeyDown(Keys.D2))//Se pressionar a tecla 2
                    {
                        state = PongState.MultiPlayer;
                        RestartGame();
                    }
                    // Se pressionar a tecla ESC encerra o jogo
                    if (keyState.IsKeyDown(Keys.Escape))
                        Exit();
                    break;

                case PongState.SinglePlayer:
                case PongState.MultiPlayer:
                    // Se pressionar a tecla ESC encerra o jogo
                    if (keyState.IsKeyDown(Keys.Escape))
                    {
                        state = PongState.IntroScreen;
                    }
                    // Entradas do jogador 1 (barra do lado esquerdo)
                    if (keyState.IsKeyDown(Keys.W))
                    {
                        if (barra1.Posicao.Y > 78.0f)
                            barra1.Direcao = new Vector2(0.0f, -1.0f);
                        else barra1.Direcao = Vector2.Zero;
                    }
                    else if (keyState.IsKeyDown(Keys.S))
                    {
                        if (barra1.Posicao.Y + barra1.Imagem.Height < 562.0f)
                            barra1.Direcao = new Vector2(0.0f, 1.0f);
                        else barra1.Direcao = Vector2.Zero;
                    }
                    else
                        barra1.Direcao = Vector2.Zero;
                    // Atualiza a posição da barra 1
                    barra1.Update(gameTime);

                    if (state == PongState.SinglePlayer)
                    {
                        // Se o jogo for para single player o computador
                        // é atualizado automáticamente
                        MoveBastaoComputador();
                    }
                    else
                    {
                        // Entradas do jogador 2 (barra do lado direito)
                        if (keyState.IsKeyDown(Keys.Up))
                        {
                            if (barra2.Posicao.Y > 78.0f)
                                barra2.Direcao = new Vector2(0.0f, -1.0f);
                            else barra2.Direcao = Vector2.Zero;
                        }
                        else if (keyState.IsKeyDown(Keys.Down))
                        {
                            if (barra2.Posicao.Y + barra1.Imagem.Height < 562.0f)
                                barra2.Direcao = new Vector2(0.0f, 1.0f);
                            else barra2.Direcao = Vector2.Zero;
                        }
                        else
                            barra2.Direcao = Vector2.Zero;
                    }
                    // Atualiza a posição da barra 2
                    barra2.Update(gameTime);

                    // Checa colisões da bola com as paredes do campo
                    // Verifica se a bola colidiu em baixo
                    if (bola.Posicao.Y + bola.Imagem.Height > 570.0f)
                    {
                        // Inverte a direção em Y do vetor
                        bola.Direcao *= new Vector2(1.0f, -1.0f);
                        // Toca o efeito sonoro de colisão com o campo
                        soundToc1.Play();
                    }
                    // Verifica se a bola colidiu em baixo
                    if (bola.Posicao.Y < 70.0f)
                    {
                        // Inverte a direção em Y do vetor
                        bola.Direcao *= new Vector2(1.0f, -1.0f);
                        // Toca o efeito sonoro de colisão com o campo
                        soundToc1.Play();
                    }
                    // Atualiza a posiação da bola
                    bola.Update(gameTime);
                    // Verifica se alguem marcou ponto
                    // Se a bola passar pela direita da tela
                    if (bola.Posicao.X + bola.Imagem.Width > 800.0f)
                    {
                        // Toca o efeito sonoro para marcar pontos
                        soundPoint.Play();
                        score[0] += 1; // aumenta um ponto ao score do jogador 1
                        // Coloca a bola no centro do campo novamente
                        bola.Posicao = new Vector2(386.0f, 310.0f);
                        // Muda a direção de disparo da bola em X
                        bola.Direcao *= new Vector2(-1.0f, 1.0f);
                    }
                    // Se a bola passar pela esquerda da tela
                    else if (bola.Posicao.X < 0.0f)
                    {
                        // Toca o efeito sonoro para marcar pontos
                        soundPoint.Play();
                        score[1] += 1; // aumenta um ponto ao score do jogador 2
                        // Coloca a bola no centro do campo novamente
                        bola.Posicao = new Vector2(386.0f, 310.0f);
                        // Muda a direção de disparo da bola
                        bola.Direcao *= new Vector2(-1.0f, 1.0f);
                    }

                    // Checa a colisão da bola com as raquetes dos jogadores
                    // Jogador 1 (Raquete da esquerda)
                    if (bola.GetBounding().Intersects(barra1.GetBounding()))
                    {
                        // Centro da bola
                        Vector2 cBall = new Vector2(bola.GetBounding().Center.X, bola.GetBounding().Center.Y);
                        cBall.Normalize();
                        // Centro da barra a esquerda (jogador 1)
                        Vector2 cBat = new Vector2(barra1.GetBounding().Center.X, barra1.GetBounding().Center.Y);
                        cBat.Normalize();
                        // Angulo de direção
                        double angDir = Math.Atan2(cBall.Y - cBat.Y, cBall.X - cBat.X);
                        // Inverte a direção X da bola
                        bola.Direcao = new Vector2((float)Math.Cos(angDir), (float)Math.Sin(angDir));
                        // Toca o efeito sonoro de colisão com a bola
                        soundToc2.Play();
                    }
                    // Jogador 2 (Raquete da direita)
                    if (bola.GetBounding().Intersects(barra2.GetBounding()))
                    {
                        // Centro da bola
                        Vector2 cBall = new Vector2(bola.GetBounding().Center.X, bola.GetBounding().Center.Y);
                        cBall.Normalize();
                        // Centro da barra a esquerda (jogador 2)
                        Vector2 cBat = new Vector2(barra2.GetBounding().Center.X, barra2.GetBounding().Center.Y);
                        cBat.Normalize();
                        // Angulo de direção
                        double angDir = Math.Atan2(cBall.Y - cBat.Y, cBall.X - cBat.X);
                        // Inverte a direção X da bola
                        bola.Direcao *= new Vector2(-1.0f, 1.0f);
                        // Toca o efeito sonoro de colisão com a bola
                        soundToc2.Play();
                    }
                    // Verifica se algum jogador chegou ao limite de pontos da partida
                    // Se o jogador 1 ganhou
                    if (score[0] >= POINT_COUNT)
                    {
                        state = PongState.GameOver;
                    }
                    // Se o jogador 2 ganhou
                    if (score[1] >= POINT_COUNT)
                    {
                        state = PongState.GameOver;
                    }
                    break;

                case PongState.GameOver:
                    // Entradas do usuario
                    if (keyState.IsKeyDown(Keys.Enter)) // Se pressionar ENTER
                    {
                        state = PongState.IntroScreen;
                    }
                    break;
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            switch (state)
            {
                case PongState.IntroScreen:
                    // Desenha a tela de apresentação (tela inicial do jogo)
                    spriteBatch.Draw(intro, Vector2.Zero, Color.White);
                    break;

                case PongState.SinglePlayer:
                case PongState.MultiPlayer:
                    // Desenha a imagem de fundo
                    spriteBatch.Draw(background, Vector2.Zero, Color.White);
                    // Desenha a bola
                    bola.Draw(spriteBatch);
                    // Desenha a barra do jogador 1
                    barra1.Draw(spriteBatch);
                    // Desenha a barra do jogador 2
                    barra2.Draw(spriteBatch);
                    // Desenhando o score do jogador 1 no centro do quadrado
                    Vector2 textSize = fontScore.MeasureString(score[0].ToString("000"));
                    spriteBatch.DrawString(fontScore,
                        score[0].ToString("000"),
                        new Vector2(300, 35) - textSize / 2,
                        Color.Black);
                    // Desenhando o score do jogador 2 no centro do quadrado
                    textSize = fontScore.MeasureString(score[1].ToString("000"));
                    spriteBatch.DrawString(fontScore,
                        score[1].ToString("000"),
                        new Vector2(500, 35) - textSize / 2,
                        Color.Black);
                    break;
                case PongState.GameOver:
                    // Desenha a tela de game over
                    spriteBatch.Draw(gameover, Vector2.Zero, Color.White);
                    break;
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public void MoveBastaoComputador()
        {
            // Bola indo para direita
            if (bola.Direcao.X > 0.0f)
            {
                if ((barra2.GetBounding().Center.Y < bola.GetBounding().Center.Y) && (barra2.Posicao.Y + barra1.Imagem.Height < 562.0f))
                    barra2.Direcao = new Vector2(0.0f, 1.0f);
                else if ((barra2.GetBounding().Center.Y > bola.GetBounding().Center.Y) && (barra2.Posicao.Y > 78.0f))
                    barra2.Direcao = new Vector2(0.0f, -1.0f);
                else
                    barra2.Direcao = new Vector2(0.0f, 0.0f);
            }
            else
            {
                barra2.Direcao = new Vector2(0.0f, 0.0f);
            }
        }

        public void RestartGame()
        {
            // Inicializando o objeto bola
            bola.Posicao = new Vector2(393, 313);
            // Inicializando o objeto barra 1
            barra1.Posicao = new Vector2(10, 290);
            // Inicializando o objeto barra 2
            barra2.Posicao = new Vector2(765, 290);
            // Inicializando o score dos jogadores
            score[0] = 0; // Jogador 1
            score[1] = 0; // Jogador 2
        }
    }
}
