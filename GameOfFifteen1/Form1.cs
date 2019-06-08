using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using GameOfFifteen;

namespace GameOfFifteen1
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
            button15.Visible = false;
        }

        int[] RightState = new int[16]
            {
                1 , 2 , 3 , 4 , 5 , 6 , 7 , 8 , 9 , 10 , 11 , 12 , 13 , 14  , 15 , 0
            };


        // Функція нажимання на одну з клавіщ 1...15
        private void button2_Click(object sender, EventArgs e)
        {
            
            List<Button> ButtonList = new List<Button>
            {
                button0 , button1 , button2 , button3 ,
                 button4 , button5 , button6 , button7 ,
                  button8 , button9 , button10 , button11 ,
                   button12 , button13 , button14 , button15
            };
            // Отримання позиції нажатої клавіщі
            int Position = Convert.ToInt32(((Button)sender).Tag);
            int[] CurrentState = new int[16];
            for(int i = 0; i < 16; i++)
            {
                // Поточний стан 
                CurrentState[i] = Convert.ToInt32(ButtonList[i].Text);
            }

            // Для зберігання сусідніх вершин з тою на яку нажимаємо
            List<int> AdjacentIndex = new List<int>();

            // Координата клавіщі на яку ми нажали
            Coordinate localCoordinate = new Coordinate(Position/4 , Position%4);

            // Список суміжніх вершин
            List<Coordinate> Cor = new List<Coordinate>();

            // Визначення суміжніх вершин
            Cor.Add(new Coordinate(localCoordinate.x, localCoordinate.y + 1));
            Cor.Add(new Coordinate(localCoordinate.x, localCoordinate.y - 1));
            Cor.Add(new Coordinate(localCoordinate.x + 1, localCoordinate.y));
            Cor.Add(new Coordinate(localCoordinate.x - 1, localCoordinate.y));

            int j = 4;
            // Усунення неіснуючих вершин графу
            for (int i = 0; i < j; i++)
            {
                if (Cor[i].x < 0 || Cor[i].x >= 4 || Cor[i].y < 0 || Cor[i].y >= 4)
                {
                    Cor.RemoveAt(i); i--; j--;
                }
            }

            for(int i = 0; i < Cor.Count; i++)
            {
                // Перетворення координати(x,y) в позицію(1 ... 16) 
                AdjacentIndex.Add(Cor[i].x * 4 + Cor[i].y);
            }

            int localPosition = -1;
            // Визначення чи нажата клавіща межує з пустим полем
            for(int i = 0; i < AdjacentIndex.Count; i++)
            {
                if(ButtonList[AdjacentIndex[i]].Visible == false)
                {
                    localPosition = AdjacentIndex[i]; break;
                }
            }

            // Якзо нажата клавіща межує з пустим полем то свап клавіщ
            if (localPosition != -1)
            {
                ButtonList[Position].Visible = false; string locText = ButtonList[Position].Text;
                ButtonList[Position].Text = "0";
                ButtonList[localPosition].Visible = true; ButtonList[localPosition].Text = locText;

                int[] localState = new int[16];
                for(int i = 0; i < 16; i++)
                {
                    localState[i] = Convert.ToInt32(ButtonList[i].Text);
                }

                // Перевірка чи досягнуто кінцевого результату
                for(int i = 0; i < 16; i++)
                {
                    if (localState[i] != RightState[i]) break;
                    else
                    {
                        if (i == 15) MessageBox.Show("YOU WIN!");
                    }
                }

            }

        }


        // Клавіща перемішування вершин Shuffler
        private void button16_Click(object sender, EventArgs e)
        {
            List<Button> ButtonList = new List<Button>
            {
                button0 , button1 , button2 , button3 ,
                 button4 , button5 , button6 , button7 ,
                  button8 , button9 , button10 , button11 ,
                   button12 , button13 , button14 , button15
            };

            // Список клавіщ , на початку всі клавіщі видимі
            for (int i = 0; i < ButtonList.Count; i++)
            {
                if (ButtonList[i].Visible == false)
                {
                    ButtonList[i].Visible = true;
                }
            }
            // Перемішування
            int[] CurrentState = Shuffler();

            for(int i = 0; i < CurrentState.Length; i++)
            {
                if(CurrentState[i] == -1)
                {
                    ButtonList[i].Visible = false;
                    ButtonList[i].Text = "0";
                }
                else
                {
                    ButtonList[i].Text = CurrentState[i].ToString();
                }
            }

        }


        public int[,] State = new int[4, 4]
        {
        {1 , 2 , 3 , 4 },
        {5 , 6 , 7 , 8 },
        {9  ,10 , 11 , 12 },
        {13  , 14 , 15 , 0}
        };  // Кінцевий (правильний) стан

        // Кількість перемішувань по замовчуванню
        public  int NumberOfMixing = 100;

        int[] Shuffler()
        {
            int[,] GoalState = new int[4, 4]; 
            for(int i = 0; i < 4; i++)
            {
                for(int j = 0; j < 4; j++)
                {
                    GoalState[i, j] = State[i, j];
                }
            }

            if(textBox1.Text != "")
            NumberOfMixing = Convert.ToInt32(textBox1.Text);

            // Список суміжніх координат з нульовою координатою
            List<Coordinate> Adjacent = new List<Coordinate>();
            // Нульова координата на початку знаходиться за індексм [3,3]
            Coordinate ZeroCordinates = new Coordinate(3, 3);

            // Процес перемішування який виконується NumberOfMixing разів
            for (int i = 0; i < NumberOfMixing; i++)
            {
                // Пошук суміжніх вершин
                FindAdjacentCor(ref Adjacent, ZeroCordinates);
                Random rand = new Random();
                // Формування рандомного значення для перемішування суміжної вершини
                System.Threading.Thread.Sleep(10);
                int MoveDirection = rand.Next((Adjacent.Count + 1) - 1);

                // swap суміжніх вершин
                Coordinate ToSwap = Adjacent[MoveDirection];
                int temp = GoalState[ToSwap.x, ToSwap.y];
                GoalState[ToSwap.x, ToSwap.y] = 0;
                GoalState[ZeroCordinates.x, ZeroCordinates.y] = temp;

                ZeroCordinates = ToSwap;
            }

            // Функція запису у файл
            WriteToTxt(GoalState);

            int[] StateToReturn = new int[16];
            int counter = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    StateToReturn[counter] = GoalState[i, j];
                    if (StateToReturn[counter] == 0) StateToReturn[counter] = -1;
                    counter++;
                }
            }
            return StateToReturn;
        }

        // Функція знаходження суміжніх вершин
        public void FindAdjacentCor(ref List<Coordinate> Cor, Coordinate zero)
        {
            Cor.Clear();
            //Суміжні вершини будуть знаходитись за наступними індексами
            Cor.Add(new Coordinate(zero.x, zero.y + 1));
            Cor.Add(new Coordinate(zero.x, zero.y - 1));
            Cor.Add(new Coordinate(zero.x + 1, zero.y));
            Cor.Add(new Coordinate(zero.x - 1, zero.y));

            int j = 4;
            // Усунення неіснуючих вершин графу
            for (int i = 0; i < j; i++)
            {
                if (Cor[i].x < 0 || Cor[i].x >= 4 || Cor[i].y < 0 || Cor[i].y >= 4)
                {
                    Cor.RemoveAt(i); i--; j--;
                }
            }

        }

        // Створення та запоанення текстового файлу
        void WriteToTxt(int[,] State)
        {
            // Запис відбувається в текстовий файл під назвою text який знаходиться в корневій папці проекту
            using (StreamWriter str = new StreamWriter("Shuffler.txt"))
            {
                
                StringBuilder strState = new StringBuilder(8);
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        strState.Append(State[i, j].ToString() + " ");
                    }
                    str.WriteLine(strState);
                    strState.Clear();

                }
            }

          

        }


        int StepCounter = 0;
        List<string> Moves = new List<string>();
        private void SolbeButton_Click(object sender, EventArgs e)
        {
            StepCounter = 0;
           List<string> AllMoves =  Solver("Shuffler.txt");
            StringBuilder str = new StringBuilder();
            //  for(int i = 0; i < AllMoves.Count; i++)
            //  {
            //      str.Append(AllMoves[i]+ " ");
            //  }
            //  SolveBox.Text = str.ToString();
            SolveBox.Text = AllMoves[StepCounter];
            StepCounter++;
            Moves = AllMoves;

            MovesBox.Text = AllMoves.Count.ToString();
        }


        // Функція розвязування задачі
        static List<string> Solver(string path)
        {
            // Получаєм перемішаний стан з файлу 
            StreamReader str = new StreamReader(path);
            int[] CurrentState = new int[16];
            string StageStr = str.ReadToEnd();
            string newString = "";
            for (int i = 0; i < StageStr.Length; i++)
            {
                if (StageStr[i] == '\n' || StageStr[i] == '\r')
                {
                    continue;
                }
                newString += StageStr[i];
            }

            string[] strArr = newString.Split(' ');
            for (int i = 0; i < 16; i++)
            {
                if (strArr[i] == "0") { strArr[i] = "-1"; }
                CurrentState[i] = Convert.ToInt32(strArr[i]);
            }

            List<string> AllMoves = new List<string>();
            AstarAlgorithm algo = new AstarAlgorithm();
            var result = algo.Run(CurrentState, ref AllMoves);
            str.Close();

            return AllMoves;
        }

        private void NextMoveButton_Click(object sender, EventArgs e)
        {
           
            if(StepCounter <= Moves.Count) NextMoveState(Moves[StepCounter-1]);
            if (StepCounter >= Moves.Count)
            {
                SolveBox.Text = "You WIN";
                MovesBox.Text = "0";
                StepCounter++;
            }
            else
            {
                SolveBox.Text = Moves[StepCounter];
                StepCounter++;
                MovesBox.Text = (Moves.Count - StepCounter + 1).ToString();
            }
        }

        // Функція переміщення клітинки при нажиманні на Next Move button
        private void NextMoveState(string move)
        {
            List<Button> ButtonList = new List<Button>
            {
                button0 , button1 , button2 , button3 ,
                 button4 , button5 , button6 , button7 ,
                  button8 , button9 , button10 , button11 ,
                   button12 , button13 , button14 , button15
            };

            int.TryParse(string.Join("", move.Where(c => char.IsDigit(c))),  out int value);

            int localPosition = 0; // Локальна позиція клітинки
            for(int i = 0; i < 16; i++)
            {
                if (ButtonList[i].Text == value.ToString()) localPosition = Convert.ToInt32(ButtonList[i].Tag); 
            }

            int localPositionSwap = 0;
            string localText = "";
            
            switch(move[move.Length-1])
            {
                case '\u2191':
                    {
                        localPositionSwap = localPosition - 4;
                        break;
                    }
                case '\u2193':
                    {
                        localPositionSwap = localPosition + 4;
                        break;
                    }
                case '\u2192':
                    {
                        localPositionSwap = localPosition + 1;
                        break;
                    }
                case '\u2190':
                    {
                        localPositionSwap = localPosition - 1;
                        break;
                    }
            }

            if (localPositionSwap < 0 | localPositionSwap > 15 | ButtonList[localPositionSwap].Text != "0")
            {
                MessageBox.Show("Wrong!");
                return;
            }

            localText = ButtonList[localPosition].Text;
            ButtonList[localPosition].Text = ButtonList[localPositionSwap].Text;
            ButtonList[localPositionSwap].Text = localText;

            ButtonList[localPositionSwap].Visible = true;
            ButtonList[localPosition].Visible = false;

        }

        private void TimeMoveStepButton_Click(object sender, EventArgs e)
        {
            float TimeStep = Convert.ToSingle(TimeBox.Text);
            for (int i = 0; i < Moves.Count; i++)
            {
                System.Threading.Thread.Sleep((int)(TimeStep * 1000));
                
                if (StepCounter <= Moves.Count) NextMoveState(Moves[StepCounter - 1]);
                if (StepCounter >= Moves.Count)
                {
                    SolveBox.Text = "You WIN";
                    MovesBox.Text = "0";
                    StepCounter++;
                }
                else
                {
                    SolveBox.Text = Moves[StepCounter];
                    StepCounter++;
                    MovesBox.Text = (Moves.Count - StepCounter + 1).ToString();
                }
                this.Refresh();
            }
        }
    }




    // Клас який імплементує координату в двовимірному просторі
    public class Coordinate
    {
        public int x, y;
        public Coordinate(int x, int y)
        {
            this.x = x; this.y = y;
        }

    }
}
