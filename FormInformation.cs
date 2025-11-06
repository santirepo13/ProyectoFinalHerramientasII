using System;
using System.Drawing;
using System.Windows.Forms;
using CodeQuest.Factories;
using CodeQuest.Services;

namespace CodeQuest
{
    /// <summary>
    /// Formulario informativo general que se muestra solo al inicio del juego
    /// Explica las reglas y mecánicas del juego
    /// </summary>
    public partial class FormInformation : Form
    {
        private int userId;
        private string username;
        private readonly IGameService gameService;
        
        private Label lblTitulo;
        private Label lblUsuario;
        private Label lblInformacion;
        private Button btnComenzarJuego;
        private Button btnVolver;

        /// <summary>
        /// Constructor para mostrar información general del juego al inicio
        /// </summary>
        /// <param name="userId">ID del usuario</param>
        /// <param name="username">Nombre del usuario</param>
        public FormInformation(int userId, string username)
        {
            this.userId = userId;
            this.username = username;
            gameService = ServiceFactory.GetGameService();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            // Form properties
            this.Text = "CodeQuest - Información del Juego";
            this.Size = new Size(750, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(240, 248, 255);

            // Title label
            lblTitulo = new Label();
            lblTitulo.Text = "🎮 Bienvenido a CodeQuest";
            lblTitulo.Font = new Font("Arial", 20, FontStyle.Bold);
            lblTitulo.ForeColor = Color.FromArgb(25, 25, 112);
            lblTitulo.Size = new Size(650, 40);
            lblTitulo.Location = new Point(50, 20);
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblTitulo);

            // User label
            lblUsuario = new Label();
            lblUsuario.Text = $"¡Hola {username}! 👋";
            lblUsuario.Font = new Font("Arial", 14, FontStyle.Bold);
            lblUsuario.ForeColor = Color.FromArgb(70, 70, 70);
            lblUsuario.Size = new Size(400, 30);
            lblUsuario.Location = new Point(125, 70);
            lblUsuario.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(lblUsuario);

            // Information label
            lblInformacion = new Label();
            lblInformacion.Text = "📚 CÓMO FUNCIONA EL JUEGO:\n\n" +
                                 "🎯 OBJETIVO:\n" +
                                 "Responde preguntas de programación en C# para ganar XP y subir de nivel.\n\n" +
                                 "🎮 MECÁNICA DEL JUEGO:\n" +
                                 "• Jugarás 3 rondas consecutivas\n" +
                                 "• Cada ronda tiene 3 preguntas\n" +
                                 "• La dificultad aumenta: Ronda 1 (Fácil), Ronda 2 (Medio), Ronda 3 (Difícil)\n\n" +
                                 "💰 SISTEMA DE PUNTUACIÓN:\n" +
                                 "• 10 XP por cada respuesta correcta\n" +
                                 "• Bonificación por velocidad:\n" +
                                 "  - ≤30 segundos total: +20 XP extra\n" +
                                 "  - ≤60 segundos total: +10 XP extra\n" +
                                 "  - >60 segundos: sin bonificación\n\n" +
                                 "📈 PROGRESIÓN:\n" +
                                 "• Cada 100 XP subes 1 nivel\n" +
                                 "• Tu progreso se guarda automáticamente\n" +
                                 "• Puedes ver tu posición en el ranking\n\n" +
                                 "💡 CONSEJOS:\n" +
                                 "• Lee cada pregunta cuidadosamente\n" +
                                 "• Responde rápido pero con precisión\n" +
                                 "• Las preguntas cubren conceptos básicos de C#\n\n" +
                                 "¡Estás listo para demostrar tus conocimientos! 🚀";
            
            lblInformacion.Font = new Font("Arial", 10);
            lblInformacion.ForeColor = Color.FromArgb(70, 70, 70);
            lblInformacion.Size = new Size(550, 320);
            lblInformacion.Location = new Point(50, 110);
            this.Controls.Add(lblInformacion);

            // Start game button
            btnComenzarJuego = new Button();
            btnComenzarJuego.Text = "🎯 Comenzar Juego";
            btnComenzarJuego.Font = new Font("Arial", 14, FontStyle.Bold);
            btnComenzarJuego.Size = new Size(200, 45);
            btnComenzarJuego.Location = new Point(150, 440);
            btnComenzarJuego.BackColor = Color.FromArgb(34, 139, 34);
            btnComenzarJuego.ForeColor = Color.White;
            btnComenzarJuego.FlatStyle = FlatStyle.Flat;
            btnComenzarJuego.Click += BtnComenzarJuego_Click;
            this.Controls.Add(btnComenzarJuego);

            // Back button
            btnVolver = new Button();
            btnVolver.Text = "⬅️ Volver";
            btnVolver.Font = new Font("Arial", 12, FontStyle.Bold);
            btnVolver.Size = new Size(120, 35);
            btnVolver.Location = new Point(380, 445);
            btnVolver.BackColor = Color.FromArgb(108, 117, 125);
            btnVolver.ForeColor = Color.White;
            btnVolver.FlatStyle = FlatStyle.Flat;
            btnVolver.Click += BtnVolver_Click;
            this.Controls.Add(btnVolver);

            this.ResumeLayout(false);
        }

        private void BtnComenzarJuego_Click(object sender, EventArgs e)
        {
            try
            {
                // Iniciar directamente con la primera ronda
                int roundId = gameService.StartNewRound(userId);
                var questions = gameService.GetQuestionsForRound(1); // Ronda 1
                
                if (questions.Count == 3)
                {
                    FormQuestions formQuestions = new FormQuestions(userId, username, roundId, 1, questions);
                    formQuestions.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Error: No se pudieron cargar las preguntas para la primera ronda.", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al iniciar el juego: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnVolver_Click(object sender, EventArgs e)
        {
            FormStart formInicio = new FormStart();
            formInicio.Show();
            this.Close();
        }
    }
}